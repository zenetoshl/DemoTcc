using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
public class UIManager : MonoBehaviour
{
    public TMP_Dropdown mutationOption;
    public TMP_Dropdown breedOption;
    public TMP_Dropdown selectionOption;
    public TMP_Text populationSize;
    public TMP_Text eliteSize;
    public TMP_Text mutationPercent;

    public void SetMutationOption(){
        PopulationManager.instance.SetMutationOpt(mutationOption.value + 1);
    }
    public void SetBreedOption(){
        PopulationManager.instance.SetBreedOpt(breedOption.value + 1);
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
    public void SetDinoSelectionOption(){
        PopulationManagerDino.instance.SetSelectionOpt(selectionOption.value + 1);
    }

    public void SetShipsMutationOption(){
        PopulationManagerShip.instance.SetMutationOpt(mutationOption.value + 1);
    }
    public void SetShipsBreedOption(){
        PopulationManagerShip.instance.SetBreedOpt(breedOption.value + 1);
    }
    public void SetShipsSelectionOption(){
        PopulationManagerShip.instance.SetSelectionOpt(selectionOption.value + 1);
    }

    private void Start() {
        if(SceneManager.GetActiveScene ().name == "ships" || SceneManager.GetActiveScene ().name == "ships 1" || SceneManager.GetActiveScene ().name == "ships 2"){
                SetShipsSelectionOption();
                SetShipsMutationOption();
                SetShipsBreedOption();
                UpdateShipUi();
        
        } else {
            if (SceneManager.GetActiveScene ().name == "dinoRun" || SceneManager.GetActiveScene ().name == "dinoRun 1" || SceneManager.GetActiveScene ().name == "dinoRun 2"){
                SetDinoMutationOption();
                SetDinoBreedOption();
                SetDinoSelectionOption();
                UpdateDinoUi();
            } else {
                SetMutationOption();
                SetBreedOption();
                SetSelectionOption();
                UpdateUi();
            }
        }
    }

    public void UpdateUi(){
        populationSize.text = "" + PopulationManager.populationSize;
        eliteSize.text = "" + PopulationManager.elite;
        mutationPercent.text = "" + PopulationManager.mutationPercentage;
    }

    public void UpdateDinoUi(){
        eliteSize.text = "" + PopulationManagerDino.elite;
        mutationPercent.text = "" + PopulationManagerDino.mutationPercentage;
        populationSize.text = "" + PopulationManagerDino.populationSize;
    }

    public void UpdateShipUi(){
        eliteSize.text = "" + PopulationManagerShip.elite;
        mutationPercent.text = "" + PopulationManagerShip.mutationPercentage;
        populationSize.text = "" + PopulationManagerShip.populationSize;
    }
}
