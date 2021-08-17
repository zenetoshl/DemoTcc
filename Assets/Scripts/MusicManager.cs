using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public AudioSource music;
    public Image icon;
    private bool muted = false;

    public void ToggleMusic(){
        if(muted) {
            muted = !muted;
            icon.color = new Color(0.08995318f,1f,0f,1f);
            music.UnPause();
        } else {
            muted = !muted;
            icon.color = new Color(0.5660f,0.5660f,0.5660f,1f);
            music.Pause();
        }
    }
}
