using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class ScaneManager : MonoBehaviour
{
    public static ScaneManager Instance;
    public TMP_InputField NameField;
    GameObject HighscoreText;

    public string p_Name;
    public string p_bestName;
    public int High_Score;
    public int BestHighScore;
    // Start is called before the first frame update
    public void StartGame()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.LoadScene(1);
        LoadGame();
        HighscoreText = GameObject.Find("HighScoreText");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name == "main")
        {
            SceneManager.LoadScene(0);
            SaveGame(p_bestName, BestHighScore);

            if (Instance != null)
            {
                Destroy(gameObject);
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void SaveName()
    {
        p_Name = NameField.text;
        print(p_Name);
    }

    public void SaveHighscore(int Highscore)
    {
        High_Score = Highscore;
    }

    public class Savedata
    {
        public string p_bestName;
        public int BestHighScore;
    }

    public void SaveGame(string bestname, int bestscore)
    {
        Savedata data = new Savedata();
        data.p_bestName = bestname;
        data.BestHighScore = bestscore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savedata.json", json);
    }

    public void LoadGame()
    {
        string path = Application.persistentDataPath + "/savedata.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Savedata data = JsonUtility.FromJson<Savedata>(json);

            p_bestName = data.p_bestName;
            BestHighScore = data.BestHighScore;
        }
    }
}
