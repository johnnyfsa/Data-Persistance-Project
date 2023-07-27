using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DatastreamManager : MonoBehaviour
{
    public static DatastreamManager Instance { get; private set; }
    public List<PlayerData> highScores;
    [SerializeField] int maxCount;
    [SerializeField] string path = "Assets/HighScores/High_Scores.json";

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance) 
        { 
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        LoadHighScores();
    }

    private void SaveHighScores() 
    {
        string highScorestoJSON = JsonHelper.ToJson(highScores, true);
        File.WriteAllText(path, highScorestoJSON);
    }

    private void LoadHighScores() 
    {
        if (File.Exists(path)) 
        { 
            string json = File.ReadAllText(path);
            if (!string.IsNullOrEmpty(json)) 
            {
                highScores = JsonHelper.FromJsonToList<PlayerData>(json);
            }         
        }
    }

    public void AddNewHighScore(PlayerData p) 
    {
        for (int i =0; i< maxCount; i++) 
        {
            if (i>=highScores.Count || p.score > highScores[i].score) 
            {
                //add a new high score
                highScores.Insert(i,p);
                while (highScores.Count > maxCount) 
                {
                    highScores.RemoveAt(maxCount);
                }
                //save high score
                SaveHighScores();
                break;
            }
        }
    }

}
