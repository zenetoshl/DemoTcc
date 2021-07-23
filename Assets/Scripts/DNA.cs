using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DNA {
    public List<int> genes = new List<int> ();
    int dnaLength = 0;
    int maxValues = 0;
    public float r = 1;
    public float g = 0;
    public float b = 0;
    public static int breedOpt = 3;
    public static int mutationOpt = 3;
    private bool isShip = false;

    public DNA (int length, int mValues) {
        dnaLength = length;
        maxValues = mValues;
        r = Random.Range (0.0f, 1.0f);
        g = Random.Range (0.0f, 1.0f);
        b = Random.Range (0.0f, 1.0f);
        SetRandom ();
    }

    public DNA (int length, int mValues, int maxSpeed, int maxFrameLimit) {
        dnaLength = length;
        maxValues = mValues;
        r = Random.Range (0.0f, 1.0f);
        g = Random.Range (0.0f, 1.0f);
        b = Random.Range (0.0f, 1.0f);
        isShip = true;
        SetRandom ();
    }

    public DNA (DNA original){
        dnaLength = original.dnaLength;
        maxValues = original.maxValues;
        r = original.r;
        g = original.g;
        b = original.b;
        foreach (int item in original.genes)
        {
            genes.Add (item);
        }
    }

    private void SetRandom () {
        genes.Clear ();
        for (int i = 0; i < dnaLength + (isShip ? 2 : 0) ; i++) {
            genes.Add (Random.Range (1, maxValues + 1));
        }
    }

    public void SetGene (int pos, int value) {
        genes[pos] = value;
    }

    public int GetGene (int pos) {
        return genes[pos];
    }

    public void Combine (DNA parent1, DNA parent2) {
        switch (breedOpt)
        {
            case 1:
                GenericBreed(parent1, parent2);
                break;
            case 2:
                AlternatedBreed(parent1, parent2);
                break;
            case 3:
                RandomBreed(parent1, parent2);
                break;
            default:
                GenericBreed(parent1, parent2);
                break;
        }
        r = parent2.r;
        g = parent1.g;
        b = parent2.b;
    }

    public void Mutate () {
        switch (mutationOpt)
        {
            case 1:
                FixedSizeMutation(2);
                break;
            case 2:
                RandomNumberMutation();
                break;
            case 3:
                FullMutation();
                break;
            default:
                FixedSizeMutation(2);
                break;
        }
    }

    public void FullMutation(){
        SetRandom();
    }

    public void RandomNumberMutation(){
        int mutationSize = Random.Range (0, dnaLength + (isShip ? 2 : 0));
        for (int i = 0; i < mutationSize; i++){
            genes[Random.Range (0, dnaLength + (isShip ? 2 : 0))] = Random.Range (0, maxValues);
        }
    }

    public void FixedSizeMutation(int mutationSize){
        for (int i = 0; i < mutationSize; i++){
            genes[Random.Range (0, dnaLength + (isShip ? 2 : 0))] = Random.Range (0, maxValues);
        }
    }

    public void RandomBreed(DNA parent1, DNA parent2){
        for (int i = 0; i < dnaLength + (isShip ? 2 : 0); i++) {
            genes[i] = (Random.Range (0, 100) <= 50) ? parent1.genes[i] : parent2.genes[i];
        }
    }

    public void AlternatedBreed(DNA parent1, DNA parent2){
        for (int i = 0; i < dnaLength + (isShip ? 2 : 0); i++) {
            genes[i] = (i % 2 == 0) ? parent1.genes[i] : parent2.genes[i];
        }
    }

    public void GenericBreed(DNA parent1, DNA parent2){
        for (int i = 0; i < dnaLength + (isShip ? 2 : 0); i++) {
            genes[i] = (i <(dnaLength + (isShip ? 2 : 0)) / 2) ? parent1.genes[i] : parent2.genes[i];
        }
    }
}