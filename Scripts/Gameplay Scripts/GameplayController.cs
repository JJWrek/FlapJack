using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameplayController : MonoBehaviour
{

    public PlayerScript player;
    private int numberOfAttempts;

    public AdManager adManager;
    public int maxNumberOfAttemptsBeforeAd;
    public int maxScoreBeforeAd;
    public Text scoreText, scoreBoardText, highScoreBoardText;
    public GameObject getReady, tapToStart, scoreBoardPanel;
    public Sprite redMedal, yellowMedal, blueMedal, lilacMedal, greenMedal, aquaMedal, pinkMedal, limeMedal, purpleMedal, silverMedal, goldMedal;
    public Image medalImage; // Represents all of the medals. Simply change out the source image

    private int score;

    void Awake()
    {
        PlayerPrefs.SetInt("Score", 0);

        UpdateAttemptsCounter();
    }

    void Update()
    {
        /// if the game has started, then the Ready! picture and Tap To Start Picture goes away
        if (player.hasGameStarted)
        {
            getReady.SetActive(false);
            tapToStart.SetActive(false);
        }

        ScoreUpdate();
        
    }

    /// <summary>
    /// Shows the scoreboard at the end of the game.
    /// </summary>
    public void ShowScoreBoard()
	{
        SetTheMedal();
        scoreBoardPanel.SetActive(true);
    }

    /// <summary>
    /// Updates the Attempts counter stored in the PlayerPrefs.
    /// </summary>
    void UpdateAttemptsCounter()
    {
        numberOfAttempts = PlayerPrefs.GetInt("Attempts", 0);
        numberOfAttempts++;
        PlayerPrefs.SetInt("Attempts", numberOfAttempts);
    }

    /// <summary>
    /// Sets the medal to the score that the player gets
    /// </summary>
    void SetTheMedal()
    {
        if (score >= 10 && score < 50)
        {
            medalImage.sprite = redMedal;
        }
        else if (score >= 50 && score < 100)
        {
            medalImage.sprite = yellowMedal;
        }
        else if (score >= 100 && score < 150)
        {
            medalImage.sprite = blueMedal;
        }
        else if (score >= 150 && score < 200)
        {
            medalImage.sprite = lilacMedal;
        }
        else if (score >= 200 && score < 250)
        {
            medalImage.sprite = greenMedal;
        }
        else if (score >= 250 && score < 300)
        {
            medalImage.sprite = aquaMedal;
        }
        else if (score >= 300 && score < 350)
        {
            medalImage.sprite = pinkMedal;
        }
        else if (score >= 350 && score < 400)
        {
            medalImage.sprite = limeMedal;
        }
        else if (score >= 400 && score < 450)
        {
            medalImage.sprite = purpleMedal;
        }
        else if (score >= 450 && score < 500)
        {
            medalImage.sprite = silverMedal;
        }
        else if (score >= 500)
        {
            medalImage.sprite = goldMedal;
        }
        else
        {
            medalImage.gameObject.SetActive(false);
        }

    }

    /// <summary>
    /// Keeps the players high score
    /// </summary>
    void SetNewHighScore()
    {
        if(score > PlayerPrefs.GetInt("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", score);
        }
    }

    /// <summary>
    /// Updates the score and the scoreboard
    /// </summary>
    void ScoreUpdate()
    {
        score = PlayerPrefs.GetInt("Score", 0);
        scoreText.text = score.ToString();

        scoreBoardText.text = score.ToString();
        highScoreBoardText.text = PlayerPrefs.GetInt("HighScore", 0).ToString();

        SetNewHighScore();
    }

    public void PlayAgain()
    {
        var playingAd = TryPlayAd();

        if (!playingAd) // Try to play an ad, else move on as normal
            ReloadCurrentScene();
        else
            // Add listener to OnAdClosed so that we know when to move on
            adManager.OnAdClosed.AddListener(ReloadCurrentScene);
    }

    public void GoToMenu()
    {
        var playingAd = TryPlayAd();

        if (!playingAd) // Try to play an ad, else move on as normal
            LoadMainMenu();
        else
            // Add listener to OnAdClosed so that we know when to move on
            adManager.OnAdClosed.AddListener(LoadMainMenu);
    }

    /// <summary>
    /// Checks to see if an interstitial ad should play.
    /// </summary>
    /// <returns>Is the ad playing or not</returns>
    public bool TryPlayAd()
	{
        if ((numberOfAttempts >= maxNumberOfAttemptsBeforeAd || score >= maxScoreBeforeAd) && adManager.IsInterstitialAdReady())
        {
            PlayerPrefs.SetInt("Attempts", 0); // Reset Attempt Counter
            adManager.ShowInterstitialAd();
            return true;
        }
        else if (!adManager.IsInterstitialAdReady())
		{
            Debug.LogError("Ad is not ready to play right now.");
		}
		else
		{
            Debug.Log("No need to play ad right now.");
        }

        return false;
    } 
    
    /// <summary>
    /// Reloads the current scene. Used for retrying after dying.
    /// </summary>
    public void ReloadCurrentScene()
	{
        /// loads the scene that is currently on
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// <summary>
    /// Loads the main menu scene. Used for leaving the game after dying.
    /// </summary>
    public void LoadMainMenu()
	{
        /// loads the the main menu
        SceneManager.LoadScene("MainMenu");
    }
}
