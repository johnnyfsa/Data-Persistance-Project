using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreTable : MonoBehaviour
{
    private List<PlayerData> highScores;
    [SerializeField] Transform highScoreTable;
    [SerializeField] GameObject highScoreElementPrefab;
    private List<GameObject> highScoreElementList;

    // Start is called before the first frame update
    void Start()
    {
        highScoreElementList = new List<GameObject>();
        highScores = DatastreamManager.Instance.highScores;
        for (int i = 0; i < highScores.Count; i++)
        {
            var inst = Instantiate(highScoreElementPrefab, highScoreTable);
            highScoreElementList.Add(inst);

            var texts = highScoreElementList[i].GetComponentsInChildren<Text>();
            texts[0].text = highScores[i].name;
            texts[1].text = highScores[i].score.ToString();

            inst.SetActive(true);
        }
    }

    public void HideHighScoreTable() 
    {
        highScoreTable.gameObject.SetActive(false);
    }

    public void ShowHighScoreTable()
    {
        highScoreTable.gameObject.SetActive(true);
    }

}
