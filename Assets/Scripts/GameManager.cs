using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class GameManager : MonoBehaviour
{
    public static bool gameOver;
    public static bool levelCompleted;
    public static bool mute;
    public static bool isGameStarted;
    public static int score = 0;

    public GameObject gameObjectPanel;
    public GameObject levelCompletedPanel;
    public GameObject gamePlayPanel;
    public GameObject startMenuPanel;

    public static int currentLevelIndex;
    public TextMeshProUGUI currentLevelText;
    public TextMeshProUGUI nextLevelText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI highScoreText;
    public Slider gameProgressSlider;

    public static int numberOfPassedRings;

    public bool connectedToGooglePlay = false;

    private void Awake()
    {
        currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 1);

        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    void Start()
    { 
        Time.timeScale = 1;
        numberOfPassedRings = 0;
        isGameStarted = gameOver = levelCompleted = mute = false;
        highScoreText.text = $"Best Score\n{PlayerPrefs.GetInt("HighScore", 0)}";

        AdsManager.instance.RequestInterstitial();
    }

    void Update()
    {
        currentLevelText.text = currentLevelIndex.ToString();
        nextLevelText.text = (currentLevelIndex+1).ToString();

        float progress = numberOfPassedRings / GameObject.FindObjectOfType<HelixManager>().numberOfRings * 100;
        gameProgressSlider.value = progress;
        scoreText.text = score.ToString();

        if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        if (currentLevelIndex > 1)
        {
            isGameStarted = true;
            gamePlayPanel.SetActive(true);
            startMenuPanel.SetActive(false);
        }

        //if (Input.GetMouseButton(0) && !isGameStarted)
        //{
        //    //if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
        //        //return;

        //    isGameStarted = true;
        //    gamePlayPanel.SetActive(true);
        //    startMenuPanel.SetActive(false);
        //}

        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved && !isGameStarted)
        {
            if (EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))
                return;

            isGameStarted = true;
            gamePlayPanel.SetActive(true);
            startMenuPanel.SetActive(false);
        }

        if (gameOver)
        {
            Time.timeScale = 0;
            gameObjectPanel.SetActive(true);

            if (Input.GetButtonDown("Fire1"))
            {
                if (score > PlayerPrefs.GetInt("HighScore", 0))
                {
                    PlayerPrefs.SetInt("HighScore", score);
                }

                if (connectedToGooglePlay)
                {
                    Social.ReportScore(score, GPGSIds.leaderboard_highscore, LeaderboardUpdate);
                }

                score = 0;
                PlayerPrefs.SetInt("CurrentLevelIndex", 1);
                SceneManager.LoadScene("Level");
            }

            if (Random.Range(0, 3) == 0)
            {
                AdsManager.instance.ShowInterstatial();
            }
        }

        if (levelCompleted)
        {
            Time.timeScale = 0;
            levelCompletedPanel.SetActive(true);

            if (Input.GetButtonDown("Fire1"))
            {
                PlayerPrefs.SetInt("CurrentLevelIndex", currentLevelIndex + 1);
                SceneManager.LoadScene("Level");
            }
        }
    }

    public void ShowLeaderBoard()
    {
        Debug.Log("Connecting...");
        if (!connectedToGooglePlay) LogInToGooglePlay();
        Social.ShowLeaderboardUI();
    }

    private void LogInToGooglePlay()
    {
        PlayGamesPlatform.Instance.Authenticate(ProcessAuthentication);
    }

    private void ProcessAuthentication(SignInStatus status)
    {
        Debug.Log(status.ToString());
        connectedToGooglePlay = status == SignInStatus.Success;
    }

    private void LeaderboardUpdate(bool success)
    {
        if (success)
        {
            Debug.Log("Updated leaderboard");
        }
        else
        {
            Debug.Log("Unable to update leaderboard");
        }
    }
}
