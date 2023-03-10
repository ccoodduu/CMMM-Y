using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PopulateLevelGrid : MonoBehaviour
{
    public GameObject prefab;
    public Color completedColor;

    void Start()
    {
        Populate();
    }

    void Populate()
    {
        GameObject newObj;

        for (int i = 0; i < GameLevels.levels.Length; i++)
        {
            newObj = Instantiate(prefab, transform);
            TMP_Text text = newObj.GetComponentInChildren<TMP_Text>();
            text.text = (i + 1) + "";
            if (PlayerPrefs.GetInt("Level" + i, 0) == 1)
            {
                text.color = completedColor;
            }

            int levelToLoad = i;

            newObj.GetComponent<Button>().onClick.AddListener(delegate ()
            {
                GridManager.loadString = GameLevels.levels[levelToLoad];
                GridManager.currentLevel = levelToLoad;
                GridManager.mode = Mode_e.LEVEL;
                SceneManager.LoadScene("LevelScreen");
            });
        }
    }
}
