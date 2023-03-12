using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideIfMode : MonoBehaviour
{
    public Mode_e mode;
    void Start()
    {
        if (GridManager.mode == mode)
        {
            gameObject.SetActive(false);
        }
    }
}
