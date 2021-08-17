using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    public GameObject gameUI;
    public GameObject gameDetails;
    public GameObject endWindow;
    public GameObject gameMenu;
    public GameObject gameInfo;
    public GameObject showInfoButton;
    public bool showInfo = false;
    
    public void SetGameUIState(int state) {
        switch (state)
        {
            case 0: 
                gameUI.SetActive(false);
                gameDetails.SetActive(false);
                endWindow.SetActive(false);
                gameMenu.SetActive(false);
                gameInfo.SetActive(false);
                break;
            case 2: 
                gameUI.SetActive(false);
                gameDetails.SetActive(true);
                endWindow.SetActive(false);
                gameMenu.SetActive(false);
                gameInfo.SetActive(false);
                break;
            case 3: 
                gameUI.SetActive(false);
                gameDetails.SetActive(false);
                endWindow.SetActive(true);
                gameMenu.SetActive(false);
                gameInfo.SetActive(false);
                break;
            case 4: 
                gameUI.SetActive(false);
                gameDetails.SetActive(false);
                endWindow.SetActive(false);
                gameMenu.SetActive(true);
                gameInfo.SetActive(false);
                break;
            case 5: 
                gameUI.SetActive(true);
                gameDetails.SetActive(false);
                endWindow.SetActive(false);
                gameMenu.SetActive(false);
                showInfo = !showInfo;
                gameInfo.SetActive(showInfo);
                showInfoButton.SetActive(!showInfo);
                break;
            default:
                gameUI.SetActive(true);
                gameDetails.SetActive(false);
                endWindow.SetActive(false);
                gameMenu.SetActive(false);
                gameInfo.SetActive(showInfo);
                showInfoButton.SetActive(!showInfo);
                break;
        }
    }
}
