using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class LevelSelectorManager : MonoBehaviour
{   
    [System.Serializable]
    public struct Level{
        public int stars;
        //public float avgFitness;

        public Level(int stars/*, float avgFitness*/){
            this.stars = stars;
            //this.avgFitness = avgFitness;
        }
    }

    [System.Serializable]
    public class Levels{
        public List<Level> levels;

        public Levels(List<Level> levels){
            this.levels = levels;
        }
    }

    [System.Serializable]
    public class GameInfo {
        public List<Levels> gameInfo;

        public GameInfo(List<Levels> gameInfo){
            this.gameInfo = gameInfo;
        }
    }

    public GameInfo gameinfo;

    public LevelsManager dino;
    public LevelsManager labirint;
    public LevelsManager ships;

    private void Start() {
        LoadGame();
    }

    public void GetGameInfo(){
        dino.SetLevels(gameinfo.gameInfo[0]);
        labirint.SetLevels(gameinfo.gameInfo[1]);
        ships.SetLevels(gameinfo.gameInfo[2]);
    }

    public void SaveGame() {
        BinaryFormatter binary = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savedGameData.mtg";
        FileStream fileStream = new FileStream(path, FileMode.Create);
        var json = JsonUtility.ToJson(gameinfo);
        Debug.Log("SALVANDO ---" + json);
        binary.Serialize(fileStream, json);
        fileStream.Close();
    }

    public void LoadGame(){
        if (File.Exists (Application.persistentDataPath + "/savedGameData.mtg")) {
            FileStream file = new FileStream(Application.persistentDataPath + "/savedGameData.mtg", FileMode.Open);
            BinaryFormatter binary = new BinaryFormatter ();
            var saveInfo = binary.Deserialize(file);
            GameInfo gameInfoLoaded = JsonUtility.FromJson<GameInfo>("" + saveInfo);
            Debug.Log("LOADING ------" + saveInfo);
            file.Close();
            gameinfo = gameInfoLoaded;
        } else {
            Debug.Log("erro no load: arquivo não encontrado, resetando...");
            ResetGame();
        }
    }

    public void ResetGame(){
        gameinfo = new GameInfo(new List<Levels> {
            new Levels( new List<Level> {new Level(0), new Level(0), new Level(0)}),
            new Levels( new List<Level> {new Level(0), new Level(0), new Level(0)}),
            new Levels( new List<Level> {new Level(0), new Level(0), new Level(0)})
         });
        SaveGame();
    }
}
