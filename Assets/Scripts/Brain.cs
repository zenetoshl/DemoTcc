using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {
    public static int dnaLength = 6;
    public float timeAlive = 0.0f;
    public float distanceWalked = 0.0f;
    public static int fitnessOpt = 3;
    public DNA dna;
    public GameObject eyes;
    public MeshRenderer mesh;
    bool alive = true;
    public bool winner = false;
    int i = 0;
    Vector3 initPos;

    public void Init () {
        //DNA Sequence
        //0 - frente
        //1 - direita
        //2 - tras
        //3 - esquerda
        initPos = transform.position;
        dna = new DNA (dnaLength, 4);
        timeAlive = 0.0f;
        i = 1;
        distanceWalked = 0.0f;
        SetColors();
        alive = true;
        Debug.Log(initPos);
    }

    public void Init (DNA Elitedna) {
        //DNA Sequence
        //0 - frente
        //1 - direita
        //2 - tras
        //3 - esquerda
        initPos = transform.position;
        dnaLength = 100;
        dna = new DNA(Elitedna);
        timeAlive = 0.0f;
        i = 1;
        distanceWalked = 0.0f;
        SetColors();
        alive = true;
        Debug.Log(initPos);
    }

    public void SetColors(){
        Color color =  new Color(dna.r, dna.g, dna.b, 1);
        mesh.material.color = color;
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "dead") {
            alive = false;
            mesh.enabled = false;
            PopulationManager.populationCount--;
            winner = false;
        } else
        if(other.gameObject.tag == "win") {
            alive = false;
            mesh.enabled = false;
            PopulationManager.populationCount--;
            winner = true;
        } else
        if(other.gameObject.tag != "decision") return;
        Debug.Log(other.gameObject.tag);
        float turn = 0;
        switch (dna.GetGene (i)) {
            case 0:
                turn = 0;
                break;
            case 1:
                turn = 90;
                break;
            case 2:
                turn = 180;
                break;
            case 3:
                turn = 270;
                break;
        }
        if(i >= dnaLength - 2){
            i = 0;
        }
        i++;
        this.transform.Rotate (0, turn, 0);
    }

    private void FixedUpdate () {
        if (!alive) return;
        timeAlive = PopulationManager.elapsed;
        this.transform.Translate (0, 0, 0.1f);
        distanceWalked = transform.position.z - initPos.z;
    }

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

    public float CalculateFitness1(){
        return (distanceWalked * timeAlive);
    }

    public float CalculateFitness2(){
        if(winner){
            return (distanceWalked + timeAlive);
        } else{
            return (-(distanceWalked + timeAlive));
        }
    }

    public float CalculateFitness3(){
        return ((distanceWalked / timeAlive) + (winner ? 8 : 0));
    }
}