import { CloseIcon } from "./icons/CloseIcon";
import { AlertIcon } from "./icons/AlertIcon";
import { RefreshIcon } from "./icons/RefreshIcon";

export const ErrorPage = ({ errCode }) => (
  <div className="fs-container">
    <div className="top-align">
      <RefreshIcon
        className="iconBtn"
        onClick={() => window.electron.reload()}
      />
      <CloseIcon className="iconBtn" onClick={() => window.electron.exit()} />
    </div>
    <AlertIcon />
    <span className="loader-text-err">{errCode}</span>
  </div>
);
