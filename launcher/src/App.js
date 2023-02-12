import { useState, useEffect } from "react";
import { LoadingPage } from "./components/LoadingPage";
import { ProgressBar } from "./components/ProgressBar";
import { ErrorPage } from "./components/ErrorPage";
import { TopContent } from "./components/TopContent";
import { AlertIcon } from "./components/icons/AlertIcon";
import { Modal } from "./components/Modal";

export const App = () => {
  const electron = window.electron;
  const [data, setData] = useState(null);
  const [loadingData, setLoadingData] = useState(true);
  const [launching, setLaunching] = useState(false);
  const [error, setError] = useState(false);
  const [artifacts, setArtifacts] = useState([]);
  const [settingsOpen, setSettingsOpen] = useState(false);

  useEffect(() => {
    (async () => {
      if (!loadingData) return;
      try {
        setLoadingData(true);
        const tempData = await electron.getData();
        setData(tempData);
        setArtifacts(await electron.getArtifacts(tempData));
        setLoadingData(false);
      } catch (error) {
        // add error handling here
        setLoadingData(false);
      }
    })();
    // eslint-disable-next-line react-hooks/exhaustive-deps
  }, []);

  useEffect(() => {
    const onError = (err) => {
      setLaunching(false);
      setLoadingData(false);
      setError(err.toString());
    };

    const onLog = (l) => console.log("[INTERNAL]", l);

    const onProgress = (p) => {
      if (document) {
        document.querySelector(".launch-pg").style["width"] = `${
          p.percent * 100
        }%`;

        const label = document.querySelector(".launch-pg-text");

        if (p.stage === "downloading")
          label.innerText = `Downloading (${Math.trunc(p.percent * 100)}%)...`;
        else if (p.stage === "extracting") label.innerText = "Extracting...";
        else if (p.stage === "launching") label.innerText = "Launching...";
      }
    };

    electron.receive("progress", onProgress);
    electron.receive("error", onError);
    electron.receive("log", onLog);

    return () => {
      electron.removeListener("progress", onProgress);
      electron.removeListener("error", onError);
      electron.removeListener("log", onLog);
    };
  });

  if (loadingData) return <LoadingPage />;

  if (!data)
    return <ErrorPage errCode="There was an error loading the launcher." />;

  if (error)
    return (
      <Modal setOpen={setError}>
        <AlertIcon />
        <span className="loader-text-err">{error}</span>
      </Modal>
    );

  if (settingsOpen)
    return (
      <Modal setOpen={setSettingsOpen}>
        <div className="settings-container">
          <h3>Settings</h3>

          <div className="setting">
            <input
              type="checkbox"
              onChange={(e) =>
                localStorage.setItem("enableArtifacts", e.target.checked)
              }
              defaultChecked={
                localStorage.getItem("enableArtifacts") === "true"
              }
            />
            <span>Enable launching versions built from artifacts.</span>
          </div>
        </div>
      </Modal>
    );
  return (
    <div className="content">
      <TopContent setSettingsOpen={setSettingsOpen} />
      <div className="flex-spacer"></div>
      <div className="bottom-container">
        {launching ? (
          <ProgressBar id="launch-pg" />
        ) : (
          <BottomContainerIdle
            data={data}
            setLaunching={setLaunching}
            electron={electron}
            artifacts={artifacts}
          />
        )}
      </div>
    </div>
  );
};

const BottomContainerIdle = ({ data, setLaunching, electron, artifacts }) => (
  <>
    <div className="input-group">
      <label htmlFor="version">Version</label>
      <select name="version" id="version">
        {data.releaseData.map((e, i) => (
          <option value={e.tag_name} key={e.tag_name}>
            {i === 0 ? `Latest release (${e.tag_name})` : e.tag_name}
          </option>
        ))}
        {localStorage.getItem("enableArtifacts") === "true" &&
          artifacts.map((e, i) => (
            <option value={"#" + e.id} key={"#" + e.id}>
              {i === 0 ? `Latest Artifact (#${e.id})` : `Artifact #${e.id}`}
            </option>
          ))}
      </select>
    </div>
    <div className="flex-spacer"></div>
    <div
      className="playBtn"
      onClick={() => {
        setLaunching(true);
        const tag = document.querySelector("#version").value;
        electron.launchGame(data, tag);
      }}
    >
      Launch
    </div>
  </>
);
