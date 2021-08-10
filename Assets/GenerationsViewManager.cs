using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GenerationsViewManager : MonoBehaviour
{
    public static int populationSize = 50;
    public GameObject individualCardPrefab;
    public Transform newParent;
    public List<IndividualStatsManager> ismList = new List<IndividualStatsManager>();
    public static bool uiNeedUpdate = false;
    public TextMeshProUGUI bestFitnessMesh;
    public TextMeshProUGUI title;
    public TextMeshProUGUI avgFitness; 
    public TextMeshProUGUI noPenalty;

    private void Start() {
        for(int i = 0; i < populationSize; i++){
            IndividualStatsManager ism = Instantiate(individualCardPrefab, this.transform.position, Quaternion.identity).GetComponent<IndividualStatsManager>();
            ism.transform.SetParent(newParent, false);
            ismList.Add(ism);
        }
    }

    public void UpdateUi(int index = -2){
        if(index <= -2) index = GenerationsStats.instance.maxIndex;
        if(index == -1) return;
        
        int i = 0;
        foreach(IndividualStatsManager ism in ismList){
            ism.UpdateUi(index, i);
            i++;
        }
        GenerationsStats.GenerationStats gs = GenerationsStats.instance.generations[index];
        title.text = "Geração " + gs.generation;
        noPenalty.text = "" + gs.noPenalty;
        avgFitness.text = "" + gs.fitnessMean;
        bestFitnessMesh.text = "" + gs.individuals[0].fitness;
    }

    public void NextIndex(){
        UpdateUi(GenerationsStats.instance.nextIndex);
    }

    public void PreviousIndex(){
        UpdateUi(GenerationsStats.instance.previousIndex);
    }

    private void Update() {
        if(uiNeedUpdate){
            UpdateUi(GenerationsStats.instance.index);
            uiNeedUpdate = false;
        }
    }
}
