using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Image lockImage;
    public TMP_Text text;
    public Image[] stars;
    public LevelSelectorManager.Level level;

    public void SetLevel(LevelSelectorManager.Level level){
        this.level = level;
        UpdateUI();
    }

    public void UpdateUI(){
        lockImage.gameObject.SetActive(false);
        text.gameObject.SetActive(true);
        int loopLimit = level.stars > 3 ? 3 : level.stars;
        for (int i = 0; i < loopLimit; i++){
            stars[i].color = Color.yellow;
        }
        for (int i = 0; i <3; i++){
            stars[i].gameObject.SetActive(true);
        }
    }
}
