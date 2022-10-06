using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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

    private void Awake()
    {
        currentLevelIndex = PlayerPrefs.GetInt("CurrentLevelIndex", 1);
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
}
