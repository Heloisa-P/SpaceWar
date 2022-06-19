using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverScreenManager : MonoBehaviour
{
    private string gameOverScoreKey = "scoreKey";
    private string topScoreKey = "topScore";

    private float topScorePoints;
    private float gameOverScorePoints;

    [Header("References")]
    public Text scoreText;
    public Text topScore;
    public Text topScoreTitle;
    public Image topScoreImage;
    public Image scoreSprite;
    public Text[] gameOverText = new Text[2];

    [Header("Properties")]
    public Sprite[] images = new Sprite[3];
    public string[] sentences = new string[3];
    public Color[] topScoreColors = new Color[5];

    [Header("Scenes Names")]
    public string homeScreen;
    public string gameScreen;

    private void Awake()
    {
        StartCoroutine(changeTopScoreColor());

        if (!PlayerPrefs.HasKey(topScoreKey))
            PlayerPrefs.SetFloat(topScoreKey, gameOverScorePoints);

        gameOverScorePoints = PlayerPrefs.GetFloat(gameOverScoreKey);
        topScorePoints = PlayerPrefs.GetFloat(topScoreKey);

        scoreText.text = gameOverScorePoints.ToString();
        topScore.text = topScorePoints.ToString();

        if(gameOverScorePoints <= topScorePoints * 0.75)
            setImageAndText(0);
 
        else if (gameOverScorePoints > topScorePoints * 0.75 && gameOverScorePoints <= topScorePoints)
            setImageAndText(1);

        else if (gameOverScorePoints > topScorePoints)
        {
            setImageAndText(2);
            PlayerPrefs.SetFloat(topScoreKey, gameOverScorePoints);
            topScore.text = gameOverScorePoints.ToString();
        }
    }

    private IEnumerator changeTopScoreColor()
    {
        Color color = topScoreColors[Random.Range(0, topScoreColors.Length)];
        topScoreTitle.color = color;
        topScore.color = color;
        topScoreImage.color = color;

        yield return new WaitForSeconds(1f);

        StartCoroutine(changeTopScoreColor());
    }

    private void setImageAndText(int index)
    {
        scoreSprite.sprite = images[index];
        gameOverText[0].text = sentences[index];
        gameOverText[1].text = sentences[index];
    }

    public void homeButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(homeScreen);
    }

    public void restartButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameScreen);
    }
}
