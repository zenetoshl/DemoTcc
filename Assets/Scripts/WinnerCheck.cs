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

    public void UpdateWinnersWindow(bool m1, bool m2, bool m3){
        if(m1){
            mission1.color = Color.yellow;
            starMission1.color = Color.yellow;
        }
        if(m2){
            mission1.color = Color.yellow;
            starMission1.color = Color.yellow;
        }
        if(m3){
            mission1.color = Color.yellow;
            starMission1.color = Color.yellow;
        }
    }
}
