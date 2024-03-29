﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public GameObject menu;
    public GameObject selectionGO;
    public LevelSelectorManager selection;

    public void ToSelection(){
        menu.SetActive(false);
        selectionGO.SetActive(true);
        selection.GetGameInfo();
    }

    public void Continue(string SceneToLoad)
    {
        SceneManager.LoadScene(SceneToLoad);
    }

    public void QuitGame()
    {
        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
