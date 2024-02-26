using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    GameObject GameManager;

    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text HighscoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    public int m_highscore;
    int m_currentHighscore;
    string p_name;
    string p_curentName;

    private bool m_GameOver = false;

    
    // Start is called before the first frame update
    void Start()
    {
        GameManager = GameObject.Find("GameManager");
        p_name = GameManager.GetComponent<ScaneManager>().p_bestName;
        p_curentName = GameManager.GetComponent<ScaneManager>().p_Name;
        m_highscore = GameManager.GetComponent<ScaneManager>().BestHighScore;
        HighscoreText.text = $"Best Score : {p_name} : {m_highscore}";


        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
        if (m_Points >= m_highscore)
        {
            UpdateHighscore(m_Points);
        }

    }

    void UpdateHighscore(int CurrentScore)
    {
        m_currentHighscore = CurrentScore;

        if (m_currentHighscore >= GameManager.GetComponent<ScaneManager>().BestHighScore)
        {
           GameManager.GetComponent<ScaneManager>().BestHighScore = m_currentHighscore;
           GameManager.GetComponent<ScaneManager>().p_bestName = p_curentName;
           HighscoreText.text = $"Best Score : {p_curentName} : {m_currentHighscore}";
        }
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
    }
}
