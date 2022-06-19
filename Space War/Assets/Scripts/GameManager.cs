using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    #region "Properties"

    public static GameManager instance;
    public bool isGamePaused;
    public Text lifePointsUI;
    public UIManager uiManager;

    private float score = 0;
    private string scoreKey = "scoreKey";
    private AudioSource audioSource;

    [Header("Background Properties")]
    public float destroyPosition;
    public float backgroundSize;
    public float backgroundSpeed;
    public GameObject backgroundPrefab;

    [Header("Player Properties")]
    public Player player;
    public float protectionTime;
    public int playerHP = 3;

    [Header("Obstacles Properties")]
    public GameObject[] listObstacles;
    public Transform[] spawnObstacles = new Transform[3];
    public float coolDown;

    [Header("Power Ups")]
    public PowerUp powerManager;
    public GameObject[] powerUps;

    [Header("Dificult Properties")]
    private float[] scoreList = { 120, 75, 35, 10 };
    private int difficultyLevel = -1;

    [Header("Boss Properties")]
    public float bossCoolDown;
    public GameObject[] bossList;

    [Header("Audio Properties")]
    public AudioMixer audioMixer;
    private string sfxValueKey = "SFX Value";

    [HideInInspector]
    public bool isBossOn = false;

    #endregion

    private void Start()
    {
        StartCoroutine(SpawnNewObstacle());
        lifePointsUI.text = "x " + playerHP.ToString();
        StartCoroutine(SpawnBoss());

        audioSource = GetComponent<AudioSource>();
        SetSfxValue();
    }

    private void Awake()
    {
        if(GameManager.instance != null)
        {
            Destroy(gameObject);
        }

        instance = this;
        player.transform.position = player.lines[1].position;
    }

    private void UpdateLifeUI()
    {
        lifePointsUI.text = "x " + playerHP.ToString();
    }

    #region "Spawns Methods"

    private IEnumerator SpawnNewObstacle()
    {
        if(!isGamePaused && !isBossOn)
        {
            GameObject obstacle;

            Transform spawnPoint = spawnObstacles[Random.Range(0, spawnObstacles.Length)];
            if(score <= 70)
            {
                obstacle = Instantiate(listObstacles[Random.Range(0, 2)]);
            }
            else
            {
                obstacle = Instantiate(listObstacles[Random.Range(0, spawnObstacles.Length)]);
            }

            obstacle.transform.position = spawnPoint.position;

            int index = Random.Range(0, 9);
            if (index == 1)
                SpawnPowerUp();
        }

        yield return new WaitForSeconds(coolDown);

        StartCoroutine(SpawnNewObstacle());
    }

    private void SpawnPowerUp()
    {
        Transform spawnPoint = spawnObstacles[Random.Range(0, spawnObstacles.Length)];
        GameObject powerUp = Instantiate(powerUps[Random.Range(0, powerUps.Length)]);
        powerUp.transform.position = spawnPoint.position;
    }

    public IEnumerator SpawnBoss()
    {
        yield return new WaitForSeconds(bossCoolDown);
        StartCoroutine(uiManager.newBoss());

        yield return new WaitForSeconds(1.2f);

        isBossOn = true;
        GameObject boss = Instantiate(bossList[Random.Range(0, bossList.Length)]);
        boss.transform.position = spawnObstacles[1].position;
    }

    #endregion

    #region "Game Over"
    private IEnumerator GameOver()
    {
        StartCoroutine(player.Die());

        yield return new WaitForSeconds(0.6f);

        PlayerPrefs.SetFloat(scoreKey, score);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Game Over");
    }

    #endregion

    #region "Score Method"
    public void Score(float newScore)
    {
        UpdateScore(newScore);
        CheckScore();
    }

    private void UpdateScore(float newScore)
    {
        score += newScore;
        uiManager.UpdScore(score);
    }

    private void CheckScore()
    {
        for (int i = 0; i < scoreList.Length; i++)
        {
            if (score >= scoreList[i])
            {
                if(difficultyLevel != i)
                    RaiseDifficulty(i);

                break;
            }
        }
    }

    private void RaiseDifficulty(int difficulty)
    {
        switch (difficulty)
        {
            case 0:
                AlterSpeed(1f, 1f, 1f);
                break;

            case 1:
                AlterSpeed(1.05f, 1f, 0.9f);
                break;

            case 2:
                AlterSpeed(1.05f, 0.9f, 1f);
                break;

            case 3:
                AlterSpeed(1.02f, 0.9f, 0.9f);
                break;
        }

        difficultyLevel = difficulty;
    }

    private void AlterSpeed(float bgSpeed, float spawnCoolDown, float shotCoolDown)
    {
        backgroundSpeed *= bgSpeed;
        coolDown *= spawnCoolDown;
        player.coolDown *= shotCoolDown;
    }

    #endregion

    #region "Player Methods"

    public void TakeDamage()
    {
        if (!player.isInvencible)
        {
            playerHP--;
            UpdateLifeUI();

            if (playerHP <= 0)
            {
                StartCoroutine(GameOver());
            }
            else
            {
               StartCoroutine(player.GetProtection(protectionTime));
                player.PlayAudio(1);
            }
        }
    }

    public void HealPlayer(int plusLife)
    {
        if (playerHP < 3)
        {
            playerHP += plusLife;
            UpdateLifeUI();
        }

    }

    #endregion

    #region "Power Up Method"

    public void CallPowerUpText(Color color, Color shadowColor, string txt)
    {
        StartCoroutine(uiManager.newPowerUp(color, shadowColor, txt));
    }

    public void GivePowerUp(int index)
    {
        powerManager.ActivePowerUp(index);
    }

    public void DestroyAllObstacles()
    {
        GameObject[] allObstacles = GameObject.FindGameObjectsWithTag("Obstacles");

        foreach(GameObject obstacle in allObstacles)
        {
            Obstacles ob = obstacle.GetComponent<Obstacles>();
            ob.Destroy();
            Destroy(obstacle);
        }
    }

    #endregion

    #region "Boss Methods"

    public void CallUpgPanel()
    {
        StartCoroutine(uiManager.ShowUpgradePanel());
    }

    public void FullLifeUpgrade()
    {
        playerHP = 3;
        uiManager.HideUpgradePanel();
        UpdateLifeUI();
    }

    public void PlusShotSpeed()
    {
        float shotSpeed = player.shotSpeed + 0.8f;
        player.NewShotSpeed(shotSpeed);
        uiManager.HideUpgradePanel();
    }

    public void PlusDamage()
    {
        int newDamage = PlayerPrefs.GetInt("damageKey") + 1;
        player.NewDamage(newDamage);
        uiManager.HideUpgradePanel();
    }

    #endregion

    #region "Audio"

    public void PlayDamageAudio()
    {
        audioSource.Play();
    }

    public void SetSfxValue()
    {
        float value = 1.0f;

        if (PlayerPrefs.HasKey(sfxValueKey))
            value = PlayerPrefs.GetFloat(sfxValueKey);
        
        audioMixer.SetFloat("SfxVolumeParameter", value);
    }

    #endregion
}
