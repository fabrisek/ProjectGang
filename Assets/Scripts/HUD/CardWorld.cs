using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class CardWorld : MonoBehaviour
{
    [SerializeField] Color colorImageLock;
    [SerializeField] Sprite imageLock;
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
        levelName.text = name;
        levelNumber.text = "";

        unlock = isUnlock;
        if (!isUnlock)
        {
            image.sprite = levelImage;
            image.color = colorImageLock;
            timer.text = "";
            starPlayer.text = "";
        }
        else
        {
            
            image.sprite = levelImage;
            timer.text = Timer.FormatTime(timerSave);
            
            
            starPlayer.text = star.ToString() + " / 5";
        }
    }

    public void ClickButton()
    {
        if (unlock)
        {
            LevelLoader.Instance.LoadLevel(index);
        }
        else
        {
            AudioManager.instance.playSoundEffect(5, 1);
            //Debug.Log("je suis bloquer");
            Debug.Log("Ajouter anim bloquer");
        }
    }
}
