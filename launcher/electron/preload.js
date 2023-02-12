const { contextBridge, ipcRenderer } = require("electron");
contextBridge.exposeInMainWorld("electron", {
  exit: () => ipcRenderer.send("exit"),
  reload: () => ipcRenderer.send("reload"),

  getData: async () => {
    ipcRenderer.send("resize", { width: 300, height: 300 });
    const repo =
      process.env.CUSTOM_REPO || "Sequitur-Studios/Cell-Machine-Mystic-Mod";
    const branch = process.env.CUSTOM_BRANCH || "master";
    const launcherData = await get(
      process.env.CUSTOM_LAUNCHER_CONFIG ||
        `https://raw.githubusercontent.com/${repo}/${branch}/launcher_config.json`
    );
    const releaseData = await get(
      `https://api.github.com/repos/${repo}/releases`
    );

    const artifactData = await get(
      `https://api.github.com/repos/${repo}/actions/artifacts`
    );

    ipcRenderer.send("resize", {
      width: 600,
      height: 320,
    });
    return {
      releaseData: releaseData,
      launcherData: launcherData,
      artifactData: artifactData,
    };
  },

  receive: (channel, func) =>
    ipcRenderer.on(channel, (event, ...args) => func(...args)),

  removeListener: (channel, func) =>
    ipcRenderer.removeListener(channel, (event, ...args) => func(...args)),

  launchGame: (data, version) =>
    ipcRenderer.send("launchGame", { data, version }),

  getArtifacts: async (data) => {
    const platform = await ipcRenderer.invoke("getPlatform");
    const artiName = data.launcherData.packNames[platform].split(".")[0];
    return data.artifactData.artifacts.filter((e) => e.name === artiName);
  },
});

const get = async (u) =>
  await fetch(u).then((r) => {
    if (!r.ok) throw Error("failed to fetch");
    return r.json();
  });
