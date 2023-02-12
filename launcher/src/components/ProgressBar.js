export const ProgressBar = ({ id }) => (
  <div className="pg-bar-container">
    <div className={`pg-bar-text ${id}-text`}>Downloading (0%)...</div>
    <div className="pg-bar-inner">
      <div className={`pg-bar ${id}`}></div>
    </div>
  </div>
);
