using Beamable;
using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public static UIController Instance { get; private set; }

    public Canvas canvas;
    public TextMeshProUGUI _textbox;

    //private GameJamMicroserviceClient _msclient;
    [Space]
    public GameObject healthUIPrefab;
    public Transform healthUIParent;
    public HealthUI mainCharacterHealth;

    [Header("Game Over")]
    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverText;

    private IBeamableAPI _api;

    private void Awake()
    {
        Instance = this;
    }

    async void Start()
    {
        _api = await API.Instance;
        //_msclient = new GameJamMicroserviceClient();
    }

    public void CallMicroService()
    {
        var player1Id = Guid.NewGuid();
        var player2Id = Guid.NewGuid();
        //StartCoroutine(StartLongPoling());

        //playerInfo[0] = new PlayerInfo
        //{
            
        //};
        //playerInfo[1] = new PlayerInfo
        //{

        //};
        //playerInfo[2] = new PlayerInfo
        //{

        //};

       // _msclient.JoinGame(player1Id, "player 1");
       // _msclient.JoinGame(player2Id, "player 2");

       //var otherPlayersString = _msclient.RefreshGameState(JsonUtility.ToJson(myPlayers))
       //     .Then((result) =>
       //     {
       //         var otherPlayersConcrete = JsonUtility.FromJson<PlayerInfo[]>(result);
       //        _textbox.text = result;
       //     });
        
    }

    //IEnumerator StartLongPoling()
    //{
    //    var random = new System.Random();
    //    while (true)
    //    {
    //        try
    //        {
    //            //_msClient
    //            //    .Multiply(random.Next(), 5)
    //            //    .Then((x) =>
    //            //    {
    //            //        _textbox.text = x.ToString();
    //            //    });
                
    //        }
    //        catch(Exception e)
    //        {
    //            _textbox.text = e.ToString();
    //        }
    //        yield return new WaitForSeconds(1f);
    //    }
    //}

    public HealthUI InstantiateHealthUI()
    {
        return Instantiate(healthUIPrefab, healthUIParent).GetComponent<HealthUI>();
    }

    public void GameOver(string text)
    {
        Cursor.lockState = CursorLockMode.None;
        gameOverPanel.SetActive(true);
        gameOverText.text = text;
    }
}
