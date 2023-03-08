using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideEditorButtons : MonoBehaviour
{
    public void Start()
    {
        if (!GridManager.mode.IsEditor())
            this.gameObject.SetActive(false);
    }
}
