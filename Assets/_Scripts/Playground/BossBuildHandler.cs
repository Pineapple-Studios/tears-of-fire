using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class BossBuildHandler : MonoBehaviour
{
    [Header("Game props")]
    [SerializeField]
    private TucanoRexProps _trxp;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private PlayerProps _pp;
    [SerializeField]
    private PlayerController _pc;
    [SerializeField]
    private TMP_Text _playerTxt;
    [SerializeField]
    private Button _btnEnd;

    [Header("Panel")]
    [SerializeField]
    private GameObject _startPanel;
    [SerializeField]
    private GameObject _timerPanel;
    [SerializeField]
    private GameObject _endPanel;

    [Header("Research fields")]
    [SerializeField]
    private Button _btnStartGame;
    [SerializeField]
    private Transform _initialPosition;
    [SerializeField]
    private string _researchLink;

    private string _name;
    private string _email;
    private int _playedTimes = 0;
    private int _killBoss = 0;
    private float _timeSpend = 0;
    private int _dead = 0;

    private bool _isStartingGame = false;

    private void OnEnable()
    {
        PlayerProps.onPlayerDead += OnPlayerDead;
        TucanoRexProps.onTucanoRexDead += OnTucanoRexDead;
    }

    private void OnDisable()
    {
        PlayerProps.onPlayerDead -= OnPlayerDead;
        TucanoRexProps.onTucanoRexDead -= OnTucanoRexDead;
    }

    private void Start()
    {
        _startPanel.SetActive(true);
        _timerPanel.SetActive(false);
        _endPanel.SetActive(false);
        Time.timeScale = 0;
    }

    private void Update()
    {
        if (_isStartingGame) _timeSpend += Time.deltaTime;

        if (_playerTxt.text != _pp.GetLife().ToString())
            _playerTxt.text = _pp.GetLife().ToString();

        if (_btnStartGame.interactable) return;
        if (_name == null || _email == null) _btnStartGame.interactable = false;
        else _btnStartGame.interactable = true;
    }

    private void OnTucanoRexDead(GameObject obj)
    {
        _playedTimes++;
        BossKilled();
        StartEndGame();
    }

    private void OnPlayerDead(GameObject obj)
    {
        _playedTimes++;
        PlayerDead();
        StartEndGame();
    }

    private IEnumerator SaveResearch(string name, string email, int playedTimes, int killBoss, float timeSpend, int dead)
    {
        int seconds = (int)timeSpend;
        string URL = $"https://ps-research.onrender.com/api/v1/tears-of-fire/boss-room?name=V2-{name}&email={email}&playedTimes={playedTimes}&killBoss={killBoss}&timeSpend={seconds}&dead={dead}";
        UnityWebRequest researchAPI = UnityWebRequest.Get(URL);
        yield return researchAPI.SendWebRequest();

        if (researchAPI.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(researchAPI.error);
        }
        else
        {
            // Show results as text
            Debug.Log(">>> SUCCESS");
            Debug.Log(researchAPI.downloadHandler.text);
            Application.OpenURL(_researchLink);
            Application.Quit();
        }
    }

    private void StartEndGame()
    {
        Time.timeScale = 0;
        _endPanel.SetActive(true);
    }

    public void SaveName(string name) { _name = name; }
    public void SaveEmail(string email) { _email = email; }
    public void StartGame() {

        // Time.timeScale = 1;
        _startPanel.SetActive(false);
        _timerPanel.SetActive(true);
        _isStartingGame = true; 
    }
    public void BossKilled() { _killBoss++; }
    public void PlayerDead() { _dead++; }
    public void Restart()
    {
        _pp.FullHeal();
        _endPanel.SetActive(false);
        _pc.EnableInput();
        _pc.ToggleRespawning();
        _trxp.RestartTucanoRex();
        _player.gameObject.transform.position = _initialPosition.position;
        _timerPanel.SetActive(true);
    }
    public void EndGame()
    {
        _btnEnd.interactable = false;
        _btnEnd.GetComponentInChildren<TMP_Text>().text = "Buscando link da pesquisa...";
        Debug.Log($"({_name}, {_email}, {_playedTimes}, {_killBoss}, {_timeSpend}, {_dead})");
        StartCoroutine(SaveResearch(_name, _email, _playedTimes, _killBoss, _timeSpend, _dead));
    }

}
