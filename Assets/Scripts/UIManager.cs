using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class UIManager : MonoBehaviour
{
    public Dropdown mutationOption;
    public Dropdown breedOption;
    public Dropdown fitnessOption;
    public Dropdown selectionOption;
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
    public void SetSelectionOption(){
        PopulationManager.instance.SetSelectionOpt(selectionOption.value + 1);
    }

    public void SetDinoMutationOption(){
        PopulationManagerDino.instance.SetMutationOpt(mutationOption.value + 1);
    }
    public void SetDinoBreedOption(){
        PopulationManagerDino.instance.SetBreedOpt(breedOption.value + 1);
    }
    public void SetDinoFitnessOption(){
        PopulationManagerDino.instance.SetFitnessOpt(fitnessOption.value + 1);
    }
    public void SetDinoSelectionOption(){
        PopulationManagerDino.instance.SetSelectionOpt(selectionOption.value + 1);
    }

    private void Start() {
        if (SceneManager.GetActiveScene ().name == "Platform"){
            UpdateDinoUi();
            SetDinoMutationOption();
            SetDinoBreedOption();
            SetDinoFitnessOption();
        } else {
            UpdateUi();
            SetMutationOption();
            SetBreedOption();
            SetFitnessOption();
        }
    }

    public void UpdateUi(){
        if (SceneManager.GetActiveScene ().name == "Platform"){
            UpdateDinoUi();
            return;
        }
        populationSize.text = "" + PopulationManager.populationSize;
        timeLimit.text = "" + PopulationManager.trialTime;
        eliteSize.text = "" + PopulationManager.elite;
        mutationPercent.text = "" + PopulationManager.mutationPercentage;
        dnaSize.text = "" + Brain.dnaLength;
    }

    public void UpdateDinoUi(){
        populationSize.text = "" + PopulationManagerDino.populationSize;
        timeLimit.text = "" + PopulationManagerDino.trialTime;
        eliteSize.text = "" + PopulationManagerDino.elite;
        mutationPercent.text = "" + PopulationManagerDino.mutationPercentage;
        dnaSize.text = "" + DinoBrain.dnaLength;
    }
}
