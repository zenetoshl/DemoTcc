using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class GameInfoManager : MonoBehaviour
{
    public TextMeshProUGUI bestFitnessMesh;
    public TextMeshProUGUI aliveCount;
    public TextMeshProUGUI currentTime;
    public TextMeshProUGUI noPenalty;
    public Text title;

    // Update is called once per frame
    public void UpdateUI(int populationCount, float elapsed, int noPenalty)
    {
        aliveCount.text = "- " + populationCount;
        currentTime.text = "- " + Math.Round(elapsed, 1) + "s";
        this.noPenalty.text = "- " + noPenalty;
    }

    public void updateNewGeneration(float bestFitness, int newGeneration){
        title.text = "Geração - " + newGeneration;
        bestFitnessMesh.text = "- " + bestFitness;
    }
}
