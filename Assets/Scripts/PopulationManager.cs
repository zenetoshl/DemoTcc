using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PopulationManager : MonoBehaviour {
    public static float elapsed = 0.0f;
    public static int populationCount;
    public int populationSize = 50;
    public GameObject prefab;
    public float trialTime = 20.0f;
    public List<GameObject> population = new List<GameObject> ();
    public int mutationPercentage = 1;

    private int currentGeneration = 1;

    GUIStyle guiStyle = new GUIStyle ();

    private void OnGUI () {
        guiStyle.fontSize = 25;
        guiStyle.normal.textColor = Color.white;
        GUI.BeginGroup (new Rect (10, 10, 250, 150));
        GUI.Box (new Rect (0, 0, 140, 140), "stats", guiStyle);
        GUI.Label (new Rect (10, 25, 200, 30), "Generation: " + currentGeneration, guiStyle);
        GUI.Label (new Rect (10, 50, 200, 30), string.Format ("Time: {0:0.00}", elapsed), guiStyle);
        GUI.Label (new Rect (10, 75, 200, 30), "Population size: " + populationCount, guiStyle);
        GUI.EndGroup ();
    }

    private void Start () {
        Vector3 startPos;
        GameObject created;
        for (int i = 0; i < populationSize; i++) {
            startPos = new Vector3 (this.transform.position.x + Random.Range (-5, 5), this.transform.position.y, this.transform.position.z + Random.Range (-5, 5));
            created = Instantiate (prefab, startPos, this.transform.rotation);
            Brain brain = created.GetComponent<Brain> ();
            brain.Init ();
            population.Add (created);
        }
        populationCount = population.Count;
    }

    private GameObject Breed (GameObject father, GameObject mother) {
        Vector3 startPos = new Vector3 (this.transform.position.x + Random.Range (-5, 5), this.transform.position.y, this.transform.position.z + Random.Range (-5, 5));
        GameObject created = Instantiate (prefab, startPos, this.transform.rotation);
        Brain brain = created.GetComponent<Brain> ();
        brain.Init ();
        brain.dna.Combine (father.transform.gameObject.GetComponent<Brain> ().dna, mother.transform.gameObject.GetComponent<Brain> ().dna);
        if (Random.Range (1, 100) <= mutationPercentage) brain.dna.Mutate (); //mutate in a certain percentage
        brain.SetColors ();
        return created;
    }

    private void BreedNewPopulation () {
        List<GameObject> sortedList = population.OrderBy (o => o.GetComponent<Brain> ().timeAlive).ToList ().OrderBy (o => o.GetComponent<Brain> ().distanceWalked).ToList ();
        population.Clear ();
        for (int i = (int) (sortedList.Count / 2.0f); i < sortedList.Count; i++) {
            int j = (i + 1) % sortedList.Count;
            population.Add (Breed (sortedList[i], sortedList[j]));
            population.Add (Breed (sortedList[j], sortedList[i]));
        }
        populationCount = population.Count;

        foreach (GameObject obj in sortedList) {
            Destroy (obj);
        }

        currentGeneration++;
    }

    private void Update () {
        elapsed += Time.deltaTime;
        if (elapsed >= trialTime) {
            BreedNewPopulation ();
            elapsed = 0.0f;
        }
    }

}