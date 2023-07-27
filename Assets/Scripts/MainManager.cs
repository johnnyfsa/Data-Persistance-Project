using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public Text BestScoreText;

    public GameObject GameOverPanel;
    
    private bool m_Started = false;
    private int m_Points;
    
    //private bool m_GameOver = false;

    private PlayerData currentPlayerData;
    private List<PlayerData> m_Players;
    [SerializeField] HighScoreTable m_HighScoreTable;
    [SerializeField] GameObject m_EnterNamePanel;
    [SerializeField] InputField m_EnterNameInputField;

    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};

        currentPlayerData = new PlayerData();
        m_Players = DatastreamManager.Instance.highScores;
        if (DatastreamManager.Instance.highScores.Count > 0)
        {
            BestScoreText.text = "Best Score: " + DatastreamManager.Instance.highScores[0].name + ": " + DatastreamManager.Instance.highScores[0].score;
        }
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
    }

    public void RestartGame() 
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame() 
    {
        SceneManager.LoadScene("menu");
    }

    
    void AddPoint(int point)
    {
        m_Points += point;
        currentPlayerData.score = m_Points;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        if (IsHighScore(currentPlayerData))
        {
            m_HighScoreTable.HideHighScoreTable();
            m_EnterNamePanel.SetActive(true);
        }
        //m_GameOver = true;
        GameOverPanel.SetActive(true);
    }

    public void AssignName() 
    {
        currentPlayerData.name = m_EnterNameInputField.text;
        m_EnterNamePanel.SetActive(false);
        m_HighScoreTable.ShowHighScoreTable();
        DatastreamManager.Instance.AddNewHighScore(currentPlayerData);
    }


    bool IsHighScore(PlayerData player) 
    {
        if (m_Players.Count == 0) 
        {
            return true;
        }
        for (int i = 0; i < m_Players.Count; i++)
        {
            if (player.score > m_Players[i].score) 
            {
                return true;
            }
        }
        return false;
    }
}
