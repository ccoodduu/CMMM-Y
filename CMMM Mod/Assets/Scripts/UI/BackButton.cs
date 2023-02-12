using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackButton : MonoBehaviour
{
    public GameObject menu;
    public void clicked() {
        menu.SetActive(false);
    }
}
