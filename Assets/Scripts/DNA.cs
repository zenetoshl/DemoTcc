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

    public DNA (int length, int mValues) {
        dnaLength = length;
        maxValues = mValues;
        r = Random.Range (0.0f, 1.0f);
        g = Random.Range (0.0f, 1.0f);
        b = Random.Range (0.0f, 1.0f);
        SetRandom ();
    }

    private void SetRandom () {
        genes.Clear ();
        for (int i = 0; i < dnaLength; i++) {
            genes.Add (Random.Range (0, maxValues));
        }
    }

    public void SetGene (int pos, int value) {
        genes[pos] = value;
    }

    public int GetGene (int pos) {
        return genes[pos];
    }

    public void Combine (DNA dad, DNA mother) {
        for (int i = 0; i < dnaLength; i++) {
            genes[i] = (i < dnaLength / 2) ? dad.genes[i] : mother.genes[i];
        }
        r = (dad.r + mother.r) % 1;
        g = (dad.g + mother.g) % 1;
        b = (dad.b + mother.b) % 1;
    }

    public void Mutate () {
        genes[Random.Range (0, dnaLength)] = Random.Range (0, maxValues);
    }
}