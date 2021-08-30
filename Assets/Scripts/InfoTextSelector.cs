using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InfoTextSelector : MonoBehaviour
{
    public TMP_Text infoTxt;
    //TextAreaAttribute(int minLines, int maxLines);
    [TextArea(3,10)]
    public string text;

    public void SetThisText(){
        infoTxt.text = text;
    }
}
