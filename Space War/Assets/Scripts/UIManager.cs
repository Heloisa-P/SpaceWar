using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public GameObject pauseScreen;

    [Header("Control UI")]
    public GameObject buttonUp;
    public GameObject buttonDown;
    public GameObject swipeDetection;

    [Header("UI Properties")]
    public Text scoreUI;
    public GameObject powerUpPanel;
    public Text powerUpText;
    public Text powerUpShadowText;

    [Header("Boss UI")]
    public GameObject bossTextPrefab;
    public GameObject bossCanvas;
    public GameObject upgradePanel;
    public Button[] buttonList;


    private void Awake()
    {
        CheckControls();
    }

    #region "Controls"

    private void CheckControls()
    {
        string key = "isSwipeMode";
        if (PlayerPrefs.HasKey(key))
        {
            if (bool.Parse(PlayerPrefs.GetString(key)))
                ActiveSwipeControl();
            else
                ActiveButtonControl();
        }
        else
        {
            ActiveSwipeControl();
        }
    }

    private void ActiveButtonControl()
    {
        buttonUp.SetActive(true);
        buttonDown.SetActive(true);
        swipeDetection.SetActive(false);
    }

    private void ActiveSwipeControl()
    {
        buttonUp.SetActive(false);
        buttonDown.SetActive(false);
        swipeDetection.SetActive(true);

    }

    #endregion

    #region "Pause Game"

    public void PauseGame()
    {
        GameManager.instance.isGamePaused = true;
        pauseScreen.SetActive(true);
    }

    public void UnPauseGame()
    {
        GameManager.instance.isGamePaused = false;
        pauseScreen.SetActive(false);
    }

    public void homeButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Start");
    }

    public void restartButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("Main");
    }

    #endregion

    #region "Score"

    public void UpdScore(float score)
    {
        scoreUI.text = score.ToString();
    }

    #endregion

    #region "PowerUp"

    public IEnumerator newPowerUp(Color color, Color shadowColor, string pwrText)
    {
        powerUpPanel.SetActive(true);
        
        powerUpText.text = pwrText;
        powerUpText.color = color;
        powerUpShadowText.text = pwrText;
        powerUpShadowText.color = shadowColor;

        yield return new WaitForSeconds(1f);

        powerUpPanel.SetActive(false);
    }

    #endregion

    #region "Boss"

    public IEnumerator newBoss()
    {
        GameObject text = Instantiate(bossTextPrefab, bossCanvas.transform);
        yield return new WaitForSeconds(1f);
        Destroy(text);

    }

    public IEnumerator ShowUpgradePanel()
    {
        GameManager.instance.isGamePaused = true;
        upgradePanel.SetActive(true);

        yield return new WaitForSeconds(0.6f);

        foreach (Button button in buttonList)
            button.interactable = true;
    }

    public void HideUpgradePanel()
    {
        upgradePanel.SetActive(false);
        GameManager.instance.isGamePaused = false;
        StartCoroutine(GameManager.instance.SpawnBoss());
    }

    #endregion
}
