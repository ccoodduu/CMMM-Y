using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;


public class CameraSettings : MonoBehaviour
{

    private void Start()
    {
        GetComponent<UniversalAdditionalCameraData>().antialiasing = 2 - (AntialiasingMode)PlayerPrefs.GetInt("Anti Aliasing", 1);
        GetComponent<UniversalAdditionalCameraData>().volumeLayerMask = PlayerPrefs.GetInt("Bloom", 1) == 0 ? 0 : (1 << 0);
	}
}