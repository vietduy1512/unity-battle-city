using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GamePlayManager : MonoBehaviour
{
    public static int stageNumber;

    public static int lastStage = 2;

    public static int playerLives = 2;

    public static bool stageCleared = false;

    [SerializeField]
    Image topCurtain, bottomCurtain, blackCurtain;

    [SerializeField]
    Text stageNumberText, gameOverText;

    GameObject[] spawnPoints, spawnPlayerPoints;

    bool stageStart = false;

    bool tankReserveEmpty = false;

    [SerializeField]
    Transform tankReservePanel;

    [SerializeField]
    Text playerLivesText, stageText;

    GameObject tankImage;

    void Start()
    {
        stageNumberText.text = "STAGE " + stageNumber.ToString();
        spawnPoints = GameObject.FindGameObjectsWithTag(Constants.Tag.ENEMY_SPAWN_POINT);
        spawnPlayerPoints = GameObject.FindGameObjectsWithTag(Constants.Tag.PLAYER_SPAWN_POINT);
        UpdateTankReserve();
        UpdatePlayerLives();
        UpdateStageNumber();
        StartCoroutine(StartStage());
        stageStart = true;
    }

    void Update()
    {
        if (tankReserveEmpty && GameObject.FindWithTag(Constants.Tag.SMALL) == null && GameObject.FindWithTag(Constants.Tag.FAST) == null && GameObject.FindWithTag(Constants.Tag.BIG) == null && GameObject.FindWithTag(Constants.Tag.ARMORED) == null)
        {
            stageCleared = true;
            StartCoroutine(Victory());
        }
    }
    private IEnumerator Victory()
    {
        tankReserveEmpty = false;
        var victory = transform.Find("Canvas/VictoryImage").gameObject;
        victory.SetActive(true);
        yield return new WaitForSeconds(3);
        if (stageNumber == lastStage)
        {
            SceneManager.LoadScene(Constants.Scene.MAIN_MENU);
        }
        else
        {
            SceneManager.LoadScene(Constants.Scene.STAGE + (stageNumber + 1));
        }
    }

    IEnumerator StartStage()
    {
        StartCoroutine(RevealStageNumber());
        yield return new WaitForSeconds(5);
        StartCoroutine(RevealTopStage());
        StartCoroutine(RevealBottomStage());
        yield return null;
        InvokeRepeating("SpawnEnemy", LevelManager.spawnRate, LevelManager.spawnRate);
        SpawnPlayer();
    }

    IEnumerator RevealStageNumber()
    {
        while (blackCurtain.rectTransform.localScale.y > 0)
        {
            blackCurtain.rectTransform.localScale = new Vector3(1, Mathf.Clamp(blackCurtain.rectTransform.localScale.y - Time.deltaTime, 0, 1), 1);
            yield return null;
        }
    }

    IEnumerator RevealTopStage()
    {
        stageNumberText.enabled = false;
        while (topCurtain.rectTransform.position.y < 1250)
        {
            topCurtain.rectTransform.Translate(new Vector3(0, 500 * Time.deltaTime, 0));
            yield return null;
        }
    }

    IEnumerator RevealBottomStage()
    {
        while (bottomCurtain.rectTransform.position.y > -400)
        {
            bottomCurtain.rectTransform.Translate(new Vector3(0, -500 * Time.deltaTime, 0));
            yield return null;
        }
    }

    public IEnumerator GameOver()
    {
        while (gameOverText.rectTransform.localPosition.y < 0)
        {
            gameOverText.rectTransform.localPosition = new Vector3(gameOverText.rectTransform.localPosition.x, gameOverText.rectTransform.localPosition.y + 120f * Time.deltaTime, gameOverText.rectTransform.localPosition.z);
            yield return null;
        }
        yield return new WaitForSeconds(2);
        stageCleared = false;
        SceneManager.LoadScene(Constants.Scene.MAIN_MENU);

        //LevelCompleted();
    }

    public void SpawnEnemy()
    {
        if (LevelManager.smallTanks + LevelManager.fastTanks + LevelManager.bigTanks + LevelManager.armoredTanks > 0)
        {
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            Animator anime = spawnPoints[spawnPointIndex].GetComponent<Animator>();
            anime.SetTrigger(Constants.Trigger.SPAWNING);
        }
        else
        {
            CancelInvoke();
            tankReserveEmpty = true;
        }
    }

    public void SpawnPlayer()
    {
        if (playerLives > 0)
        {
            if (!stageStart)
            {
                playerLives--;
                UpdatePlayerLives();
            }
            stageStart = false;
            Animator anime = spawnPlayerPoints[0].GetComponent<Animator>();
            anime.SetTrigger(Constants.Trigger.SPAWNING);
        }
        else
        {
            StartCoroutine(GameOver());
        }
    }

    void UpdateTankReserve()
    {
        int j;
        int numberOfTanks = LevelManager.smallTanks + LevelManager.fastTanks + LevelManager.bigTanks + LevelManager.armoredTanks;
        for (j = 0; j < numberOfTanks; j++)
        {
            tankImage = tankReservePanel.transform.GetChild(j).gameObject;
            tankImage.SetActive(true);
        }
    }
    public void RemoveTankReserve()
    {
        int numberOfTanks = LevelManager.smallTanks + LevelManager.fastTanks + LevelManager.bigTanks + LevelManager.armoredTanks;
        tankImage = tankReservePanel.transform.GetChild(numberOfTanks).gameObject;
        tankImage.SetActive(false);
    }
    public void UpdatePlayerLives()
    {
        playerLivesText.text = playerLives.ToString();
    }

    void UpdateStageNumber()
    {
        stageText.text = stageNumber.ToString();
    }
}