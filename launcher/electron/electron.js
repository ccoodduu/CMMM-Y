const electron = require("electron");
require("dotenv").config();
const { download } = require("electron-dl");
const { join } = require("path");
const extract = require("extract-zip");
const { exec } = require("child_process");
const path = require("path");
const fs = require("fs");
const { format } = require("url");
const os = require("os");

const app = electron.app;
const BrowserWindow = electron.BrowserWindow;
let mainWindow;

const createWindow = () => {
  mainWindow = new BrowserWindow({
    width: 300,
    height: 300,
    frame: false,
    webPreferences: {
      preload: path.join(__dirname, "preload.js"), // use a preload script
      // nodeIntegration: false,
      contextIsolation: true,
    },
  });

  // and load the index.html of the app.
  const startUrl =
    process.env.NODE_ENV === "development"
      ? "http://localhost:3000"
      : format({
          pathname: path.join(__dirname, "/../build/index.html"),
          protocol: "file:",
          slashes: true,
        });

  mainWindow.loadURL(startUrl);

  // Emitted when the window is closed.
  mainWindow.on("closed", () => (mainWindow = null));
};
app.on("ready", createWindow);
app.on("window-all-closed", () => process.platform !== "darwin" && app.quit());
app.on("activate", () => mainWindow === null && createWindow());

// In this file you can include the rest of your app's specific main process
// code. You can also put them in separate files and require them here.

electron.ipcMain.on("resize", (e, args) =>
  mainWindow.setSize(args.width, args.height)
);

electron.ipcMain.on("exit", () => app.quit());
electron.ipcMain.on("reload", () => mainWindow && mainWindow.reload());
electron.ipcMain.handle("getPlatform", () => os.platform());

const error = (e) => {
  if (mainWindow) {
    mainWindow.webContents.send("log", e);
    mainWindow.webContents.send(
      "error",
      `There was an error trying to execute that version (Check the log for more info).`
    );
  }
};
const progress = (stage, extra) =>
  mainWindow.webContents.send("progress", { stage, ...extra });

const execCallback = (e, d) => e && e.errno !== -2 && error("LaunchError");

electron.ipcMain.on("launchGame", async (event, { data, version }) => {
  const platform = os.platform();
  const repo =
    process.env.CUSTOM_REPO || "Sequitur-Studios/Cell-Machine-Mystic-Mod";

  const userDir = join(electron.app.getPath("userData"), "game");
  const dlPath = join(userDir, "/dlTemp/");
  const extractPath = join(userDir, `/versions/${version}/`);
  const assetUrl = version.startsWith("#")
    ? `https://api.github.com/repos/${repo}/actions/artifacts/${
        version.split("#")[1]
      }/zip`
    : `https://github.com/${repo}/releases/download/${version}/${data.launcherData.packNames[platform]}`;

  const runPath = join(
    extractPath,
    data.launcherData.executableNames[platform]
  );

  try {
    if (!fs.existsSync(userDir)) fs.mkdirSync(userDir);
    if (!fs.existsSync(dlPath)) fs.mkdirSync(dlPath);

    if (!fs.existsSync(runPath)) {
      await download(mainWindow, assetUrl, {
        directory: dlPath,
        filename: "pack.zip",
        onProgress: (p) => progress("downloading", p),
      });

      progress("extracting", { percent: 1 });

      await extract(join(dlPath, "pack.zip"), { dir: extractPath });
    }

    progress("launching", { percent: 1 });

    if (platform === "darwin" || platform === "linux")
      exec(`chmod -R 755 "${runPath}"`, execCallback);

    if (platform === "darwin") exec(`open -a "${runPath}";exit`, execCallback);
    if (platform === "windows") exec(`start "l" "${runPath}"`, execCallback);
    if (platform === "linux") exec(`nohup "${runPath}" &`, execCallback);

    app.quit();
  } catch (err) {
    error(err);
  }
});
