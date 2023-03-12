using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeleteAllPlayerdata : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DeleteAll()
    {
        PlayerPrefs.DeleteAll();

        // Reload scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
