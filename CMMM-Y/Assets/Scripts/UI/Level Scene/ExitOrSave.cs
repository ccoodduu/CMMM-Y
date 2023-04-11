using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitOrSave : MonoBehaviour
{
    public PopupWindow confirmationPopup;

    public void Clicked() {
        if (GridManager.hasSaved || GridManager.mode == Mode_e.LEVEL)
        {
            SceneManager.LoadScene(0);
        }
        else {
            confirmationPopup.Open();
        }
    }
}
