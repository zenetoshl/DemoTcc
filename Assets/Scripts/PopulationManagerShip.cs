using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class PopulationManagerShip : MonoBehaviour{
    static PopulationManagerShip _instance;
    public static PopulationManagerShip instance {
        get {
            if (!_instance) {
                //first try to find one in the scene
                _instance = FindObjectOfType<PopulationManagerShip> ();

                if (!_instance) {
                    //if that fails, make a new one
                    GameObject go = new GameObject ("_VariablesManager");
                    _instance = go.AddComponent<PopulationManagerShip> ();

                    if (!_instance) {
                        //if that still fails, we have a big problem;
                        Debug.LogError ("Fatal Error: could not create PopulationManagerShip");
                    }
                }
            }

            return _instance;
        }
    }
    public WinnerCheck winner;
    public TMP_Text pointsText;
    public bool started = false;
    public static float elapsed = 0.0f;
    public static int populationCount {
        get => populationAlive - populationDead;
    }
    private static int _id = 1;
    public static int id {
        get {
            _id += 1;
            return _id;
        }
    }
    public static int populationDead = 0;
    public static int complexityPoints = 0;
    public static int noPenalty = 0;
    public static int populationAlive = 0;
    public static int populationSize = 50;
    public static int elite = 10;
    public static int mutationPercentage = 1;
    public static float trialTime = 15.0f;
    public static int selectionOpt = 1;
    public static float spawnTime = .05f;
    public static float bestFitness;

    public GameObject prefab;
    public GameInfoManager gameInfoManager;
    public GameObject rankButton;
    public List<GameObject> population = new List<GameObject> ();
    private static int currentGeneration = 0;

    public int maxGenStar = 10;
    public int maxComplexityStar = 600;

    private void GetNewPointsCount(){
        complexityPoints = (mutationPercentage * 5) + (selectionOpt * 10) + (elite * 20) + (populationSize * 3) + (DNA.mutationOpt * 10) + (DNA.breedOpt * 10);
        pointsText.text = complexityPoints + " PTS";
    }

    private void CheckWin(List<GameObject> pop){
        int winners = 0;
        foreach (GameObject o in pop){
            if(o.GetComponent<ShipController>().winner){
                winners++;
            }
        }
        bool m1 = winners >= (populationSize / 50);
        bool m2 = currentGeneration < maxGenStar;
        bool m3 = complexityPoints < maxComplexityStar;
        winner.UpdateWinnersWindow(m1, m2, m3);
    }

    public void StartGame () {
        Vector3 startPos;
        GameObject created;
        populationDead = 0;
        populationAlive = 0;
        for (int i = 0; i < populationSize; i++) {
            startPos = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
            created = Instantiate (prefab, startPos, this.transform.rotation);
            ShipController brain = created.GetComponent<ShipController> ();
            brain.Init ();
            population.Add (created);
        }
        StartCoroutine("SpawnNewGeneration");
        started = true;
    }

    private GameObject Breed (GameObject father, GameObject mother) {
        Vector3 startPos = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
        GameObject created = Instantiate (prefab, startPos, this.transform.rotation);
        ShipController brain = created.GetComponent<ShipController> ();
        brain.Init ();
        brain.dna.Combine (father.transform.gameObject.GetComponent<ShipController> ().dna, mother.transform.gameObject.GetComponent<ShipController> ().dna);
        if (Random.Range (1, 100) <= mutationPercentage) brain.dna.Mutate (); //mutate in a certain percentage
        return created;
    }

    private GameObject BreedElite(GameObject elite){
        Vector3 startPos = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
        GameObject created = Instantiate (prefab, startPos, this.transform.rotation);
        ShipController brain = created.GetComponent<ShipController> ();
        brain.Init (elite.transform.gameObject.GetComponent<ShipController> ().dna, elite.transform.gameObject.GetComponent<ShipController> ().fitness);
        return created;
    }

    private void BreedNewPopulation () {
        Debug.Log("acima de 0 pontos:" + noPenalty);
        noPenalty = 0;
        populationDead = 0;
        populationAlive = 0;
        List<GameObject> sortedList = population.OrderBy (o => -o.GetComponent<ShipController>().fitness).ToList ();
        float genBestFitness = sortedList[0].GetComponent<ShipController>().CalculateFitness();
        if(currentGeneration == 0){
            bestFitness = genBestFitness;
        } else if (bestFitness <= genBestFitness){
            bestFitness = genBestFitness;
        }
        int index = GenerationsStats.instance.CreateNewGeneration(noPenalty);
        foreach (GameObject o in sortedList){
            ShipController brain = o.GetComponent<ShipController>();
            GenerationsStats.instance.generations[index].AddIndividual(brain.dna, brain.CalculateFitness(), brain.timeAlive);
        }
        GenerationsViewManager.uiNeedUpdate = true;
        population.Clear ();
        rankButton.SetActive (true);
        if(selectionOpt == 1){
            SelectionByFittest(sortedList);
        } else {
            RandomSelection(sortedList);
        }

        foreach (GameObject obj in sortedList) {
            Destroy (obj);
        }
        StartCoroutine("SpawnNewGeneration");
        currentGeneration++;
    }

    IEnumerator SpawnNewGeneration(){
        foreach (GameObject obj in population){
            obj.GetComponent<ShipController>().ToggleBrain(true);
            populationAlive++;
            yield return new WaitForSeconds(spawnTime);
        }
        yield return null;
    }

    private void RandomSelection(List<GameObject> sorted){
        int i = 0;
        while(population.Count < populationSize){
            if(i < elite){
                population.Add (Breed (sorted[i], sorted[i]));
            }
            else {
                population.Add (Breed (sorted[Random.Range (0, sorted.Count - 1)], sorted[Random.Range (0, sorted.Count - 1)]));
            }
            i++;
        }
    }

    private void SelectionByFittest(List<GameObject> sorted){
        int i = 0;
        while(population.Count < populationSize){
            if(i < elite){
                population.Add (Breed (sorted[i], sorted[i]));
            }
            else {
                population.Add (Breed (sorted[i - elite], sorted[(i - elite + 1) % sorted.Count]));
            }
            i++;
        }
    }

    private void FixedUpdate () {
        if(started){
            elapsed += Time.deltaTime;
            if (populationDead >= populationSize) {
                BreedNewPopulation ();
                elapsed = 0.0f;
                noPenalty = 0;
                gameInfoManager.updateNewGeneration(bestFitness, currentGeneration);
            }
            gameInfoManager.UpdateUI(populationCount, elapsed, noPenalty);
        }
    }

    public void SetBreedOpt(int opt){
        if(opt > 3 || opt < 1){
            opt = 1;
        }
        DNA.breedOpt = opt;
        GetNewPointsCount();
    }

    public void SetFitnessOpt(int opt){
        if(opt > 3 || opt < 1){
            opt = 1;
        }
        ShipController.fitnessOpt = opt;
        GetNewPointsCount();
    }

    public void SetMutationOpt(int opt){
        if(opt > 3 || opt < 1){
            opt = 1;
        }
        DNA.mutationOpt = opt;
        GetNewPointsCount();
    }

    public void SetSelectionOpt(int opt){
        if(opt > 2 || opt < 1){
            opt = 1;
        }
        selectionOpt = opt;
        GetNewPointsCount();
    }

    public void SetMutation(int opt){
        mutationPercentage += opt;
        if(mutationPercentage <= 0){
            mutationPercentage = 0;
        } else if (mutationPercentage >= 100){
            mutationPercentage = 100;
        }
        GetNewPointsCount();
    }

    public void SetPopulation(int opt){
        populationSize += opt;
        if(populationSize <= 10){
            populationSize = 10;
        } else if (populationSize >= 100){
            populationSize = 100;
        }
        GetNewPointsCount();
    }

    public void SetGenesSize(int opt){
        ShipController.dnaLength += opt;
        if(ShipController.dnaLength <= 2){
            ShipController.dnaLength = 2;
        } else if (ShipController.dnaLength >= 100){
            ShipController.dnaLength = 100;
        }
        GetNewPointsCount();
    }

    public void SetElite(int opt){
        elite += opt;
        if(elite <= 0){
            elite = 0;
        } else if (elite >= populationSize - 1){
            elite = populationSize - 1;
        }
        GetNewPointsCount();
    }

    public void SetTrialTime(int opt){
        trialTime += opt;
        if(trialTime <= 5){
            trialTime = 5;
        } else if (trialTime >= 20){
            trialTime = 20;
        }
        GetNewPointsCount();
    }
}
