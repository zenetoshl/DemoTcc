using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brain : MonoBehaviour {
    public int dnaLength = 2;
    public float timeAlive = 0.0f;
    public float distanceWalked = 0.0f;
    public DNA dna;
    public GameObject eyes;
    public MeshRenderer mesh;
    bool alive = true;
    bool seeGround = true;

    private void OnCollisionEnter (Collision other) {
        if (other.gameObject.tag == "dead") {
            alive = false;
            mesh.enabled = false;
            PopulationManager.populationCount--;
        }
    }

    public void Init () {
        //DNA Sequence
        //0 - frente
        //1 - esquerda
        //2 - direita
        dna = new DNA (dnaLength, 3);
        timeAlive = 0.0f;
        distanceWalked = 0.0f;
        SetColors();
        alive = true;
    }

    public void SetColors(){
        Color color =  new Color(dna.r, dna.g, dna.b, 1);
        mesh.material.color = color;
    }

    private void FixedUpdate () {
        if (!alive) return;
        Debug.DrawRay (eyes.transform.position, eyes.transform.forward * 10, Color.red, 10);
        seeGround = false;
        RaycastHit hit;
        if (Physics.Raycast (eyes.transform.position, eyes.transform.forward * 10, out hit)) {
            if (hit.collider.gameObject.tag == "platform") {
                seeGround = true;
            }
        }
        timeAlive = PopulationManager.elapsed;
        float turn = 0;
        float move = 0;
        if (seeGround) {
            switch (dna.GetGene (0)) {
                case 0:
                    move = 1;
                    break;
                case 1:
                    turn = -90;
                    break;
                case 2:
                    turn = 90;
                    break;
            }
        } else {
            switch (dna.GetGene (1)) {
                case 0:
                    move = 1;
                    break;
                case 1:
                    turn = -90;
                    break;
                case 2:
                    turn = 90;
                    break;
            }
        }
        this.transform.Translate (0, 0, move * 0.1f);
        this.transform.Rotate (0, turn, 0);
        distanceWalked += move * 0.1f;
    }
}