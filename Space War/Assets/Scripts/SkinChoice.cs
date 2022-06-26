using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkinChoice : MonoBehaviour
{
    public Image skinPreview;
    public Sprite[] skinList;
    public int[] pontuationList;

    public Text hpValue;
    public Text coolDownValue;
    public Text speedValue;

    public GameObject button;
    public Image buttonIcon;
    public Sprite lockIcon;
    public Sprite okIcon;

    public GameObject scoreToUnlock;
    public Text score;
    private string scoreLine = " TO UNLOCK";

    private int index = 0;
    private int selectedIndex;

    private Color iconColor;
    private Color buttonColor;

    private float bestScore;
    private string topScoreKey = "topScore";
    private string skinPrefsKey = "Skin Prefs";

    private void Start()
    {
        if(PlayerPrefs.HasKey(topScoreKey))
            bestScore = PlayerPrefs.GetFloat(topScoreKey);

        iconColor = buttonIcon.color;
        buttonColor = button.GetComponent<Image>().color;

        if(PlayerPrefs.HasKey(skinPrefsKey))
        {
            string[] data = PlayerPrefs.GetString(skinPrefsKey).Split('|');
            selectedIndex = int.Parse(data[0]);
        }

        UpdateAttributes();
        OnIndexChange();
    }

    public void ChangeSkin(bool isLeft)
    {
        if (index > 0 && isLeft)
            index--;

        else if (index < skinList.Length-1 && !isLeft)
            index++;
            
        skinPreview.sprite = skinList[index];
        OnIndexChange();
    }

    private void OnIndexChange()
    {
        bool isInteractable = false;
        ChangeSelectionButton(0.5f);

        if (bestScore >= pontuationList[index])
        {
            isInteractable = true;
            buttonIcon.sprite = okIcon;
            scoreToUnlock.SetActive(false);
        }
        else
        {
            scoreToUnlock.SetActive(true);
            score.text = pontuationList[index].ToString() + scoreLine; 
            isInteractable = false;
            buttonIcon.sprite = lockIcon;
        }

        button.GetComponent<Button>().interactable = isInteractable;

        if(selectedIndex == index)
        {
            ChangeSelectionButton(1);
        }

        UpdateAttributes();
    }

    private void UpdateAttributes()
    {
        switch(index)
        {
            case 0:
                hpValue.text = "3";
                coolDownValue.text = "1.0";
                speedValue.text = "1.0";
                break;

            case 1:
                hpValue.text = "4";
                coolDownValue.text = "1.2";
                speedValue.text = "1.3";
                break;

            case 2:
                hpValue.text = "3";
                coolDownValue.text = "0.8";
                speedValue.text = "0.9";
                break;

            case 3:
                hpValue.text = "5";
                coolDownValue.text = "1.5";
                speedValue.text = "0.9";
                break;

            case 4:
                hpValue.text = "2";
                coolDownValue.text = "0.5";
                speedValue.text = "1.5";
                break;
        }
    }

    public void OnSelectButton()
    {
        selectedIndex = index;
        ChangeSelectionButton(1f);

        string v = "";

        v += selectedIndex.ToString() + "|";
        v += hpValue.text + "|";
        v += coolDownValue.text + "|";
        v += speedValue.text + "|";

        PlayerPrefs.SetString(skinPrefsKey, v);
    }

    private void ChangeSelectionButton(float value)
    {
        buttonColor.a = value;
        iconColor.a = value;
        button.GetComponent<Image>().color = buttonColor;
        buttonIcon.color = iconColor;
    }
}
