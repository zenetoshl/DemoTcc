using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    public Dropdown mutationOption;
    public Dropdown breedOption;
    public Dropdown fitnessOption;
    public Text populationSize;
    public Text timeLimit;
    public Text eliteSize;
    public Text mutationPercent;
    public Text dnaSize;

    public void SetMutationOption(){
        PopulationManager.instance.SetMutationOpt(mutationOption.value + 1);
    }
    public void SetBreedOption(){
        PopulationManager.instance.SetBreedOpt(breedOption.value + 1);
    }
    public void SetFitnessOption(){
        PopulationManager.instance.SetFitnessOpt(fitnessOption.value + 1);
    }

    private void Start() {
        UpdateUi();
        SetMutationOption();
        SetBreedOption();
        SetFitnessOption();
    }

    public void UpdateUi(){
        populationSize.text = "" + PopulationManager.populationSize;
        timeLimit.text = "" + PopulationManager.trialTime;
        eliteSize.text = "" + PopulationManager.elite;
        mutationPercent.text = "" + PopulationManager.mutationPercentage;
        dnaSize.text = "" + Brain.dnaLength;
    }
}
