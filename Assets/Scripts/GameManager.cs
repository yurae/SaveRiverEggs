using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const float TIMERVAL = 31f;
    //private const float TIMERVAL = 10f;
    float GameTimer = TIMERVAL;
    int Score = 0;

    public Text ScoreText;
    public Text TimerText;
    public Text EndGameScoreText;

    public GameObject StartGameScreen;
    public GameObject EndGameScreen;

    public GameObject EggObj;

    public Button BtnStartGame;
    public Button BtnRestartGame;

    private bool inGameCore = false;

    public delegate void OnClickEgg();
    private List<GameObject> eggs = new List<GameObject>();

    public static string[] EggNames = { "vory", "grid", "kong", "kori", "pat", "penguin", "ryu", "summer", "tomato", "yuza" };
    public static float[] EggPos = { -0.75f, -2.35f, -4.2f };

    void Update()
    {
        if (!inGameCore) {
            return;
		}

        GameTimer -= Time.deltaTime;
        UpdateTimerText();
        if (GameTimer <= 0f) {
            EndGame();
        }
    }

    private IEnumerator InGameCoroutine() {
        if (inGameCore)
            SpawnEgg();
        while (inGameCore) {
            yield return new WaitForSeconds(RandomWaitSpawn());
            if (inGameCore) {
                SpawnEgg();
            }
        }
	}

    private void SpawnEgg() {
        int eggIdx = (int)Random.Range(0, EggNames.Length);
        string eggName = EggNames[eggIdx];

        int posIdx = (int)Random.Range(0, EggPos.Length);

        Egg egg = Instantiate(EggObj).GetComponent<Egg>();
        float speed = RandomSpeed();
        egg.Init(posIdx, speed, eggName);
        egg.EventOnClickedEgg += HandleClickedEgg;

        eggs.Add(egg.gameObject);
    }

    private void HandleClickedEgg() {
        Score++;
        UpdateScoreText();
    }

    private float RandomWaitSpawn() {
        float min = 0.5f, max = 1.6f, mean = 0.8f, sigma = 1.2f;
        switch (GetGameLevel()) {
            case 1: {
                    break;
                }
            case 2: {
                    min = 0.5f;
                    max = 1.4f;
                    mean = 0.7f;
                    sigma = 1f;
                    break;
                }
            case 3: {
                    min = 0.4f;
                    max = 1.2f;
                    mean = 0.6f;
                    sigma = 2f;
                    break;
                }
            case 4: {
                    min = 0.3f;
                    max = 1.0f;
                    mean = 0.5f;
                    sigma = 1.5f;
                    break;
                }
            case 5: {
                    min = 0.2f;
                    max = 0.8f;
                    mean = 0.4f;
                    sigma = 1.5f;
                    break;
                }
            case 6: {
                    min = 0.2f;
                    max = 0.7f;
                    mean = 0.3f;
                    sigma = 1.2f;
                    break;
                }
            case 7: {
                    min = 0.1f;
                    max = 0.6f;
                    mean = 0.2f;
                    sigma = 1.2f;
                    break;
                }
            default: {
                    break;
                }
        }
        return GaussianRandom(mean, sigma, min, max);
	}

    private float RandomSpeed() {
        float min = 1f, max = 3f, mean = 4f, sigma = 2f;
        switch (GetGameLevel()) {
            case 1: {
                    break;
                }
            case 2: {
                    min = 2f;
                    max = 10f;
                    mean = 5f;
                    break;
                }
            case 3: {
                    min = 2f;
                    max = 12f;
                    mean = 6f;
                    break;
                }
            case 4: {
                    min = 4f;
                    max = 15f;
                    mean = 7.5f;
                    break;
                }
            case 5: {
                    min = 6f;
                    max = 15f;
                    mean = 9f;
                    break;
                }
            case 6: {
                    min = 9f;
                    max = 20f;
                    mean = 12f;
                    sigma = 1.5f;
                    break;
                }
            case 7: {
                    min = 15f;
                    max = 30f;
                    mean = 15f;
                    sigma = 1.5f;
                    break;
                }
            default: {
                    break;
                }
        }
        return GaussianRandom(mean, sigma, min, max);
    }

    private int GetGameLevel() {
        if (GameTimer <= 3f) {
            return 7;
        }
        else if (GameTimer <= 5f) {
            return 6;
        }
        else if (GameTimer <= 10f) {
            return 5;
        }
        else if (GameTimer <= 15f) {
            return 4;
        }
        else if (GameTimer <= 20f) {
            return 3;
        }
        else if (GameTimer <= 25f) {
            return 2;
        }
        else {
            return 1;
        }
    }

    private float GaussianRandom(float mean, float sigma, float min, float max) {
        float rand1 = Random.Range(0.0f, 1.0f);
        float rand2 = Random.Range(0.0f, 1.0f);

        float u, v, S;

        do {
            u = 2.0f * UnityEngine.Random.value - 1.0f;
            v = 2.0f * UnityEngine.Random.value - 1.0f;
            S = u * u + v * v;
        }
        while (S >= 1.0f);

        // Standard Normal Distribution
        float std = u * Mathf.Sqrt(-2.0f * Mathf.Log(S) / S);
        return Mathf.Clamp(std * sigma + mean, min, max);
    }

    private void DestroyAllObj() {
        foreach(GameObject obj in eggs) {
            if (obj != null)
                Destroy(obj);
		}
        eggs.Clear();
    }

    private void StartGame() {
        GameTimer = TIMERVAL;
        Score = 0;
        UpdateTimerText();
        UpdateScoreText();
        StartGameScreen.SetActive(false);
        inGameCore = true;

        StartCoroutine(InGameCoroutine());
    }

	private void EndGame() {
        EndGameScoreText.text = Score.ToString() + " Á¡";
        EndGameScreen.SetActive(true);
        DestroyAllObj();
        inGameCore = false;

    }

    private void RestartGame() {
        EndGameScreen.SetActive(false);
        StartGame();
    }

    private void UpdateTimerText() {
        TimerText.text = Mathf.Max(0, (int)GameTimer).ToString();
    }

    private void UpdateScoreText() {
        ScoreText.text = Score.ToString();
    }


    void Start() {
        BtnStartGame.onClick.AddListener(() => {
            StartGame();
        });
        BtnRestartGame.onClick.AddListener(() => {
            RestartGame();
        });
    }

}
