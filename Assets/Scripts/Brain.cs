using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Brain : MonoBehaviour {
    public SpriteRenderer sprite;
    public static int dnaLength = 6;
    public float timeAlive = 0.0f;
    public float bornTime = 0.0f;
    public float distanceWalked = 0.0f;
    public static int fitnessOpt = 3;
    public float timeLimit = 15f;
    public DNA dna;
    public bool alive = true;
    public bool winner = false;
    public int i = 0;
    public Vector3 initPos;

    public abstract void Init ();

    public abstract void Init (DNA Elitedna, float fitness = 0f);

    public abstract void ToggleBrain(bool b);

    public abstract void Die();

    public abstract void SetColors();

    public float CalculateFitness(){
        switch (fitnessOpt)
        {
            case 1:
                return CalculateFitness1();
            case 2:
                return CalculateFitness2();
            case 3:
                return CalculateFitness3();
            default:
                return CalculateFitness3();
        }
    }

    public abstract float CalculateFitness1();

    public abstract float CalculateFitness2();

    public abstract float CalculateFitness3();
}