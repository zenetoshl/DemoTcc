using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoTextSelector : MonoBehaviour
{
    public TMP_Text infoTxt;
    public string text;

    public void SetThisText(){
        infoTxt.text = text;
    }
}
