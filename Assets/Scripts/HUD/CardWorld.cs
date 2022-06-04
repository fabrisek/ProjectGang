using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CardWorld : MonoBehaviour
{
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI timer;
    [SerializeField] TextMeshProUGUI levelName;
    [SerializeField] TextMeshProUGUI levelNumber;
    [SerializeField] TextMeshProUGUI posPlayer;
    [SerializeField] TextMeshProUGUI starPlayer;
    int index;
    bool unlock;

    // Start is called before the first frame update

    public void ChangeInformation(Sprite levelImage, float timerSave, string name, string numberLevel, int star,int indexScene, bool isUnlock)
    {
        index = indexScene;
        image.sprite = levelImage;
        timer.text = Timer.FormatTime(timerSave);
        levelName.text = name;
        levelNumber.text = numberLevel;
        starPlayer.text = star.ToString() + " / 5";
        unlock = isUnlock;
        if (!isUnlock)
        {
            image.color = Color.black;
        }
    }

    public void ClickButton()
    {
        if (unlock)
            LevelLoader.Instance.LoadLevel(index);
    }
}
