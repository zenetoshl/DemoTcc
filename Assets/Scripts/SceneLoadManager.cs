using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SceneLoadManager : MonoBehaviour
{
    public int gameId;
    public int levelIndex;
    public LevelSelectorManager.GameInfo gameInfo;
    public int stars = 0;

    private void Start() {
        LoadGame();
    }

    public void ReloadScene(){
        Application.LoadLevel(Application.loadedLevel);
    }

    public void BackToMenu(){
        gameInfo.gameInfo[gameId].levels[levelIndex] = new LevelSelectorManager.Level(stars);
        SaveGame();
        Application.LoadLevel("Start Menu");
    }

    public void SaveGame() {
        BinaryFormatter binary = new BinaryFormatter();
        string path = Application.persistentDataPath + "/savedGameData.mtg";
        FileStream fileStream = new FileStream(path, FileMode.Create);
        var json = JsonUtility.ToJson(gameInfo);
        Debug.Log("SALVANDO ---" + json);
        binary.Serialize(fileStream, json);
        fileStream.Close();
    }

    public void LoadGame(){
        if (File.Exists (Application.persistentDataPath + "/savedGameData.mtg")) {
            FileStream file = new FileStream(Application.persistentDataPath + "/savedGameData.mtg", FileMode.Open);
            BinaryFormatter binary = new BinaryFormatter ();
            var saveInfo = binary.Deserialize(file);
            LevelSelectorManager.GameInfo gameInfoLoaded = JsonUtility.FromJson<LevelSelectorManager.GameInfo>("" + saveInfo);
            Debug.Log("LOADING ------" + saveInfo);
            file.Close();
            gameInfo = gameInfoLoaded;
        } else {
            Debug.Log("erro no load: arquivo não encontrado, resetando...");
            ResetGame();
        }
    }

    public void ResetGame(){
        gameInfo = new LevelSelectorManager.GameInfo(new List<LevelSelectorManager.Levels> {
            new LevelSelectorManager.Levels( new List<LevelSelectorManager.Level> {new LevelSelectorManager.Level(0), new LevelSelectorManager.Level(0), new LevelSelectorManager.Level(0)}),
            new LevelSelectorManager.Levels( new List<LevelSelectorManager.Level> {new LevelSelectorManager.Level(0), new LevelSelectorManager.Level(0), new LevelSelectorManager.Level(0)}),
            new LevelSelectorManager.Levels( new List<LevelSelectorManager.Level> {new LevelSelectorManager.Level(0), new LevelSelectorManager.Level(0), new LevelSelectorManager.Level(0)})
         });
        SaveGame();
    }
}
