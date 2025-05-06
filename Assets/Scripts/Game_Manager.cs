using UnityEngine;
using TMPro;

public class Game_Manager : MonoBehaviour
{
    public static Game_Manager instance;
    public Chicken_movement chicken;
    public TMP_Text score_text;
    public TMP_Text FinalScore;
    public GameObject ScoreCanvas;
     public ChunkGenerator ChunkGenerator;
    public GameObject EndCanvas;
    private bool triggeredEnd = false;

    [Header("Distance Scoring")]
    private bool distanceScoring = false;
    public float ScoreAdjustment = 5;
    private int scoreCounter = 0;

    [Header("Double Points Power‑Up")]
    public float doublePointsDuration = 5f;
    private bool doublePointsActive = false;
    private float doublePointsTimer = 0f;

    [Header("Slow Time Power‑Up")]
    public float slowTimeDuration = 5f;
    private bool slowTimeActive = false;
    private float slowTimeTimer = 0f;

    [Header("Obstacle Penalty")]
    public int penalty = 1000;

    int score = 0;
    public int targetScore = 30000;

    public GameObject countdownCanvas;
    public TMP_Text countdownText;
    private float countdown = 3f;
    private bool countdownActive = false;

    public int lives = 2;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        EndCanvas.SetActive(false);
        countdownCanvas.SetActive(false);
    }

    public void StartGame()
    {
        ScoreCanvas.SetActive(true);
        countdownCanvas.SetActive(true);
        countdownActive = true;
        countdownText.text = Mathf.CeilToInt(countdown).ToString();
        UpdateScoreText();
    }

    void Update()
    {
        if(lives == 0)
        {
            Loss();
            return;
        }
       
        if(score >= targetScore)
        {
            Win();
        }
        if (countdownActive)
        {
            countdown -= Time.deltaTime;
            if (countdown > 0)
                countdownText.text = Mathf.CeilToInt(countdown).ToString();
            else
            {
                countdownText.text = "Go!";
                countdownActive = false;
                Invoke(nameof(EndCountdown), 1f);
            }
        }

        if (scoreCounter == ScoreAdjustment)
        {
            if (distanceScoring)
            {
                int amt = doublePointsActive ? 2 : 1;
                score += amt;
                UpdateScoreText();
            }
            scoreCounter = 0;
        }
        scoreCounter++;

        if (doublePointsActive)
        {
            doublePointsTimer -= Time.deltaTime;
            if (doublePointsTimer <= 0f)
                EndDoublePoints();
        }

        if (slowTimeActive)
        {
            slowTimeTimer -= Time.unscaledDeltaTime;
            if (slowTimeTimer <= 0f)
                EndSlowTime();
        }
    }

    public void AddPoints(int amount = 1)
    {
        score += doublePointsActive ? amount * 2 : amount;
        UpdateScoreText();
    }

 /*   public void DeductPoints()
    {
        score -= penalty;
        if (score < 0) score = 0;
        UpdateScoreText();
    }
 */
    private void UpdateScoreText()
    {
        score_text.text = $"{score} Points";
    }

    public void ActivateDoublePoints()
    {
        doublePointsActive = true;
        doublePointsTimer = doublePointsDuration;
    }

    private void EndDoublePoints()
    {
        doublePointsActive = false;
    }

    public void ActivateSlowTime()
    {
        if (slowTimeActive) return;           
        slowTimeActive = true;
        slowTimeTimer = slowTimeDuration;
        Time.timeScale = 0.5f;               
        Time.fixedDeltaTime = 0.02f * 0.5f;   
    }

    private void EndSlowTime()
    {
        slowTimeActive = false;
        Time.timeScale = 1f;
        Time.fixedDeltaTime = 0.02f;
    }
    void EndCountdown()
    {
        countdownCanvas.SetActive(false);
        distanceScoring = true;
        chicken.isPlaying = true;
    }
    public void ObstacleCollision()
    {
        lives -= 1;
    }
    public void LifeUp()
    {
        if (lives != 2)
        {
            lives = 2;
        }
    }
    private void Loss()
    {
        End();
        FinalScore.text = $"You Lost! Final Score: {score} Points";

    }
    
    private void Win()
    {
        if(!triggeredEnd)
        {
            ChunkGenerator.TriggerEndChunk();
            triggeredEnd = true;
        }
        FinalScore.text = $"You Won! Final Score: {score} Points";
        if (chicken.moveSpeed == 0)
        {
            End();
        }
    }
    private void End()
    {
        distanceScoring = false;
        chicken.isPlaying = false;
        EndCanvas.SetActive(true);
        ScoreCanvas.SetActive(false);
    }

    public void Easy()
    {
        chicken.moveSpeed = 10f;
        StartGame();
    }
    public void Medium()
    {
        chicken.moveSpeed = 15f;
        StartGame();
    }
    public void Hard()
    {
        chicken.moveSpeed = 20f;
        StartGame();
    }
}

