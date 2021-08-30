using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    public LevelSelectorManager.Levels levels;
    public int totalStars = 0;
    public LevelManager[] levelManagers;
    public int[] limits = new int[] {0, 1, 4};


    public void SetLevels(LevelSelectorManager.Levels levels){
        this.levels = levels;
        foreach (LevelSelectorManager.Level level in levels.levels){
            totalStars += level.stars;
        }

        for (int i = 0; i < 3; i++){
            if (totalStars >= limits[i]){
                levelManagers[i].SetLevel(levels.levels[i]);
            } else {
                return;
            }
        }
    }
}
