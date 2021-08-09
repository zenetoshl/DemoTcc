using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenerationsViewManager : MonoBehaviour
{
    public static int populationSize = 50;
    public GameObject individualCardPrefab;
    public Transform newParent;
    public List<IndividualStatsManager> ismList = new List<IndividualStatsManager>();
    public static bool uiNeedUpdate = false;

    private void Start() {
        for(int i = 0; i < populationSize; i++){
            IndividualStatsManager ism = Instantiate(individualCardPrefab, this.transform.position, Quaternion.identity).GetComponent<IndividualStatsManager>();
            ism.transform.SetParent(newParent, false);
            ismList.Add(ism);
        }
    }

    public void UpdateUi(){
        int i = 0;
        foreach(IndividualStatsManager ism in ismList){
            ism.UpdateUi(GenerationsStats.instance.maxIndex, i);
            i++;
        }
    }

    private void Update() {
        if(uiNeedUpdate){
            UpdateUi();
            uiNeedUpdate = false;
        }
    }
}
