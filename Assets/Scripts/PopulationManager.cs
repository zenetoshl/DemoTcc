using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopulationManager : MonoBehaviour {
    static PopulationManager _instance;
    public static PopulationManager instance {
        get {
            if (!_instance) {
                //first try to find one in the scene
                _instance = FindObjectOfType<PopulationManager> ();

                if (!_instance) {
                    //if that fails, make a new one
                    GameObject go = new GameObject ("_VariablesManager");
                    _instance = go.AddComponent<PopulationManager> ();

                    if (!_instance) {
                        //if that still fails, we have a big problem;
                        Debug.LogError ("Fatal Error: could not create PopulationManager");
                    }
                }
            }

            return _instance;
        }
    }
    public bool started = false;
    public static float elapsed = 0.0f;
    public static int populationCount;
    public static int populationSize = 50;
    public static int elite = 2;
    public static int mutationPercentage = 1;
    public static float trialTime = 20.0f;
    public GameObject prefab;
    public List<GameObject> population = new List<GameObject> ();

    private int currentGeneration = 1;

    GUIStyle guiStyle = new GUIStyle ();

    private void OnGUI () {
        if(started){
            guiStyle.fontSize = 25;
            guiStyle.normal.textColor = Color.white;
            GUI.BeginGroup (new Rect (10, 10, 250, 150));
            GUI.Box (new Rect (0, 0, 140, 140), "stats", guiStyle);
            GUI.Label (new Rect (10, 25, 200, 30), "Generation: " + currentGeneration, guiStyle);
            GUI.Label (new Rect (10, 50, 200, 30), string.Format ("Time: {0:0.00}", elapsed), guiStyle);
            GUI.Label (new Rect (10, 75, 200, 30), "Population size: " + populationCount, guiStyle);
            GUI.EndGroup ();
        }
    }

    public void StartGame () {
        Vector3 startPos;
        GameObject created;
        for (int i = 0; i < populationSize; i++) {
            startPos = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
            created = Instantiate (prefab, startPos, this.transform.rotation);
            Brain brain = created.GetComponent<Brain> ();
            brain.Init ();
            population.Add (created);
        }
        started = true;
        populationCount = population.Count;
    }

    private GameObject Breed (GameObject father, GameObject mother) {
        Vector3 startPos = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
        GameObject created = Instantiate (prefab, startPos, this.transform.rotation);
        Brain brain = created.GetComponent<Brain> ();
        brain.Init ();
        brain.dna.Combine (father.transform.gameObject.GetComponent<Brain> ().dna, mother.transform.gameObject.GetComponent<Brain> ().dna);
        if (Random.Range (1, 100) <= mutationPercentage) brain.dna.Mutate (); //mutate in a certain percentage
        brain.SetColors ();
        return created;
    }

    private GameObject BreedElite(GameObject elite){
        Vector3 startPos = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z);
        GameObject created = Instantiate (prefab, startPos, this.transform.rotation);
        Brain brain = created.GetComponent<Brain> ();
        brain.Init (elite.transform.gameObject.GetComponent<Brain> ().dna);
        return created;
    }

    private void BreedNewPopulation () {
        List<GameObject> sortedList = population.OrderBy (o => -o.GetComponent<Brain>().CalculateFitness()).ToList ();
        population.Clear ();
        for (int i = 0; i < (int) (sortedList.Count / 2.0f) -(elite / 2); i++) {
            int j = (i + 1) % sortedList.Count;
            population.Add (Breed (sortedList[i], sortedList[j]));
            population.Add (Breed (sortedList[j], sortedList[i]));
        }
        for (int i = 0; i < elite; i++){
            population.Add(BreedElite(sortedList[i]));
        }
        populationCount = population.Count;

        foreach (GameObject obj in sortedList) {
            Destroy (obj);
        }

        currentGeneration++;
    }

    private void Update () {
        if(started){
            elapsed += Time.deltaTime;
            if (elapsed >= trialTime) {
                BreedNewPopulation ();
                elapsed = 0.0f;
            }
        }
    }

    public void SetBreedOpt(int opt){
        if(opt > 3 || opt < 1){
            opt = 1;
        }
        DNA.breedOpt = opt;
    }

    public void SetFitnessOpt(int opt){
        if(opt > 3 || opt < 1){
            opt = 1;
        }
        Brain.fitnessOpt = opt;
    }

    public void SetMutationOpt(int opt){
        if(opt > 3 || opt < 1){
            opt = 1;
        }
        DNA.mutationOpt = opt;
    }

    public void SetMutation(int opt){
        mutationPercentage += opt;
        if(mutationPercentage <= 0){
            mutationPercentage = 0;
        } else if (mutationPercentage >= 100){
            mutationPercentage = 100;
        }
    }

    public void SetPopulation(int opt){
        populationSize += opt;
        if(populationSize <= 10){
            populationSize = 10;
        } else if (populationSize >= 100){
            populationSize = 100;
        }
    }

    public void SetGenesSize(int opt){
        Brain.dnaLength += opt;
        if(Brain.dnaLength <= 2){
            Brain.dnaLength = 2;
        } else if (Brain.dnaLength >= 100){
            Brain.dnaLength = 100;
        }
    }

    public void SetElite(int opt){
        elite += opt;
        if(elite <= 0){
            elite = 0;
        } else if (elite >= populationSize - 1){
            elite = populationSize - 1;
        }
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