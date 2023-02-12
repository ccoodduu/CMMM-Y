import { CloseIcon } from "./icons/CloseIcon";

export const Modal = ({ setOpen, children }) => {
  return (
    <div className="modal-container">
      <div className="inner-modal">
        <CloseIcon
          className="iconBtn modalErrBtn"
          onClick={() => setOpen(false)}
        />
        {children}
      </div>
    </div>
  );
};
