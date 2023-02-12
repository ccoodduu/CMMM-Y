import { CloseIcon } from "./icons/CloseIcon";
// import { SettingsIcon } from "./icons/SettingsIcon";

export const TopContent = ({ setSettingsOpen }) => (
  <div className="top-container">
    <div className="title-container">
      <h1 className="title">Cell Machine</h1>
      <h1 className="secondary-title">Mystic Mod Launcher</h1>
    </div>
    <div className="flex-spacer"></div>
    <div className="btn-container">
      <CloseIcon className="iconBtn" onClick={() => window.electron.exit()} />
      {/* <SettingsIcon className="iconBtn" onClick={() => setSettingsOpen(true)} /> */}
    </div>
  </div>
);
