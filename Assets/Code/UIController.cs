using Beamable;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    public Animator crossHairAnim;
    public AudioSource hitSource;
    public GameObject pauseMenu;
    public HurtFx hurtFx;
    public Slider mouseSensitivitySlider;

    [Space]
    public List<CharacterUI> myCharacters;
    public List<CharacterUI> enemyCharacters;

    [Space]
    public TextMeshProUGUI timer;
    public GameObject switchCharacterInfo;

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

    public void ShowHitEffect()
    {
        crossHairAnim.CrossFadeInFixedTime("hit", 0f);
        hitSource.Play();
    }

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


[Serializable]
public class CharacterUI
{
    public Image image;
    public Animator animator;
    public TextMeshProUGUI index;

    public void PlayAnimAndChangeColor()
    {
        image.color = new Color(0.4f, 0.4f, 0.4f);
        animator.CrossFadeInFixedTime("died", 0f);
    }
}