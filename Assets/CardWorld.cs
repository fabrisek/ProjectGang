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
    // Start is called before the first frame update

    public void ChangeInformation(Sprite levelImage, float timerSave, string name, string numberLevel, int star,int indexScene)
    {
        index = indexScene;
        image.sprite = levelImage;
        timer.text = Timer.FormatTime(timerSave);
        levelName.text = name;
        levelNumber.text = numberLevel;
        starPlayer.text = star.ToString() + " / 5";
    }

    public void ClickButton()
    {
        LevelLoader.Instance.LoadLevel(index);
    }
}
