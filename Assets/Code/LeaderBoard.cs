using UnityEngine;

public class LeaderBoard : MonoBehaviour
{
    public Transform ContentParent;
    public GameObject LeaderBoardEntryPrefab;

    void Start()
    {
        for(int i=0; i<=20; ++i)
        {
           var entry = Instantiate(LeaderBoardEntryPrefab, ContentParent).GetComponent<LeaderBoardEntryUI>();
            entry._entryName.text = $"iteration{i}";
        }
    }
}
