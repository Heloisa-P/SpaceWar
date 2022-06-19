using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    private string powerUpName;
    private Player player;

    [Header("Power Up Properties")]
    public float timeControlDuration;
    public float protectionDuration;
    public float fastShotDuration;
    public float buffShotDuration;
    private int newDamage;

    public Color[] colors;
    public Color[] shadowColors;
    private Color color;
    private Color shadowColor;

    private AudioSource audioSource;

    private void Start()
    {
        player = GameManager.instance.player;
        newDamage = PlayerPrefs.GetInt("damageKey") + 1;

        audioSource = GetComponent<AudioSource>();
    }

    public void ActivePowerUp(int index)
    {
        audioSource.Play();

        switch (index)
        {
            case 0:
                GameManager.instance.HealPlayer(1);

                powerUpName = "+1 HP";
                color = colors[0];
                shadowColor = shadowColors[0];
                break;

            case 1:
                StartCoroutine(player.GetProtection(protectionDuration));

                powerUpName = "Protection!!";
                color = colors[1];
                shadowColor = shadowColors[1];
                break;

            case 2:
                StartCoroutine(ControlTime());

                powerUpName = "Slow... Time...";
                color = colors[2];
                shadowColor = shadowColors[2];
                break;

            case 3:
                StartCoroutine(player.ShotBuff(newDamage, buffShotDuration));
                powerUpName = "Super Shot!!!";
                color = colors[3];
                shadowColor = shadowColors[3];
                break;

            case 4:
                StartCoroutine(player.FastShot(fastShotDuration));

                powerUpName = "Fast Shot!";
                color = colors[4];
                shadowColor = shadowColors[4];
                break;

            case 5:
                GameManager.instance.DestroyAllObstacles();

                powerUpName = "BOOM!";
                color = colors[5];
                shadowColor = shadowColors[5];
                break;
        }

        GameManager.instance.CallPowerUpText(color, shadowColor, powerUpName);
    }

    public IEnumerator ControlTime()
    {
        Time.timeScale = Time.timeScale * 0.5f;

        yield return new WaitForSeconds(timeControlDuration);

        Time.timeScale = Time.timeScale * 2f;
    }

}
