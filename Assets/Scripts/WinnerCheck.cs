using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WinnerCheck : MonoBehaviour
{
    public TMP_Text mission1;
    public TMP_Text mission2;
    public TMP_Text mission3;
    public Image starMission1;
    public Image starMission2;
    public Image starMission3;

    
    public GameUIManager gameUIManager;
    public SceneLoadManager loadManager;

    public void UpdateWinnersWindow(bool m1, bool m2, bool m3){
        int stars = 0;
        if(m1){
            mission1.color = Color.yellow;
            starMission1.color = Color.yellow;
            gameUIManager.SetGameUIState(3);
            stars++;
        } else {
            return;
        }
        if(m2){
            mission2.color = Color.yellow;
            starMission2.color = Color.yellow;
            stars++;
        }
        if(m3){
            mission3.color = Color.yellow;
            starMission3.color = Color.yellow;
            stars++;
        }
        loadManager.stars = stars;
    }
}
