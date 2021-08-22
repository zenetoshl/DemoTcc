using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class PopulationManagerDino : MonoBehaviour {
    static PopulationManagerDino _instance;
    public static PopulationManagerDino instance {
        get {
            if (!_instance) {
                //first try to find one in the scene
                _instance = FindObjectOfType<PopulationManagerDino> ();

                if (!_instance) {
                    //if that fails, make a new one
                    GameObject go = new GameObject ("_VariablesManager");
                    _instance = go.AddComponent<PopulationManagerDino> ();

                    if (!_instance) {
                        //if that still fails, we have a big problem;
                        Debug.LogError ("Fatal Error: could not create PopulationManagerDino");
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
    public static int populationDead = 0;
    public static int populationAlive = 0;
    public static int populationSize = 50;
    public static int elite = 5;
    private static int _id = 1;
    public static int id {
        get {
            _id += 1;
            return _id;
        }
    }
    public static int mutationPercentage = 1;
    public static int complexityPoints = 0;
    public static float trialTime = 15.0f;
    public static int selectionOpt = 1;
    public static float spawnTime = .15f;
    public static int noPenalty = 0;
    public static float bestFitness;
    public GameObject prefab;
    public GameInfoManager gameInfoManager;
    public List<GameObject> population = new List<GameObject> ();
    private static int currentGeneration = 0;
    private void Update() {
        if (Input.GetKeyDown("space"))
        {
            StartGame();
        }
    }

    private void CheckWin(List<GameObject> pop){
        int winners = 0;
        foreach (GameObject o in pop){
            if(o.GetComponent<DinoBrain>().winner){
                winners++;
            }
        }
        bool m1 = winners >= (populationSize / 2);
        bool m2 = currentGeneration < 10;
        bool m3 = complexityPoints < 700;
        winner.UpdateWinnersWindow(m1, m2, m3);
    }

    private void GetNewPointsCount(){
        complexityPoints = (mutationPercentage * 5) + (selectionOpt * 10) + (elite * 20) + (populationSize * 3) + (DNA.mutationOpt * 10) + (DNA.breedOpt * 10);
        pointsText.text = complexityPoints + " PTS";
    }

    public void StartGame () {
        Vector3 startPos;
        GameObject created;
        populationDead = 0;
        populationAlive = 0;
        for (int i = 0; i < populationSize; i++) {
            startPos = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
            created = Instantiate (prefab, startPos, this.transform.rotation);
            DinoBrain brain = created.GetComponent<DinoBrain> ();
            brain.Init ();
            population.Add (created);
        }
        started = true;
        StartCoroutine("SpawnNewGeneration"); 
    }

    private GameObject Breed (GameObject father, GameObject mother) {
        Vector3 startPos = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
        GameObject created = Instantiate (prefab, startPos, this.transform.rotation);
        DinoBrain brain = created.GetComponent<DinoBrain> ();
        brain.Init ();
        brain.dna.Combine (father.transform.gameObject.GetComponent<DinoBrain> ().dna, mother.transform.gameObject.GetComponent<DinoBrain> ().dna);
        if (Random.Range (1, 100) <= mutationPercentage) brain.dna.Mutate (); //mutate in a certain percentage
        return created;
    }

    private GameObject BreedElite(GameObject elite){
        Vector3 startPos = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
        GameObject created = Instantiate (prefab, startPos, this.transform.rotation);
        DinoBrain brain = created.GetComponent<DinoBrain> ();
        brain.Init (elite.transform.gameObject.GetComponent<DinoBrain> ().dna);
        return created;
    }

    private void BreedNewPopulation () {
        populationDead = 0;
        populationAlive = 0;
        List<GameObject> sortedList = population.OrderBy (o => -o.GetComponent<DinoBrain>().CalculateFitness()).ToList ();
        float genBestFitness = sortedList[0].GetComponent<DinoBrain>().CalculateFitness();
        if(currentGeneration == 0){
            bestFitness = genBestFitness;
        } else if (bestFitness <= genBestFitness){
            bestFitness = genBestFitness;
        }
        int index = GenerationsStats.instance.CreateNewGeneration(noPenalty);
        foreach (GameObject o in sortedList){
            DinoBrain brain = o.GetComponent<DinoBrain>();
            GenerationsStats.instance.generations[index].AddIndividual(brain.dna, brain.CalculateFitness(), brain.timeAlive);
        }
        GenerationsViewManager.uiNeedUpdate = true;
        population.Clear ();
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
            obj.GetComponent<DinoBrain>().ToggleBrain(true);
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
                CheckWin(population);
                BreedNewPopulation ();
                GenerationsViewManager.uiNeedUpdate = true;
                elapsed = 0.0f;
                noPenalty = 0;
                gameInfoManager.updateNewGeneration( bestFitness, currentGeneration);
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
        DinoBrain.fitnessOpt = opt;
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
        DinoBrain.dnaLength += opt;
        if(DinoBrain.dnaLength <= 2){
            DinoBrain.dnaLength = 2;
        } else if (DinoBrain.dnaLength >= 100){
            DinoBrain.dnaLength = 100;
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
    }
}