using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneLoadManager : MonoBehaviour
{
    public void ReloadScene(){
        Application.LoadLevel(Application.loadedLevel);
    }
    public void BackToMenu(){
        //Save
        //BackToMenu
        Application.LoadLevel(Application.loadedLevel);
    }
}
