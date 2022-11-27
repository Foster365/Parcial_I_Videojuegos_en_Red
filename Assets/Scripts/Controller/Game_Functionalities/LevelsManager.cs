using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsManager : MonoBehaviour
{
    public enum Levels { mainMenuScreen, gameScreen, winScreen, gameOverScreen};
    public enum LevelsValues { Main_Menu, Game, Win, Game_Over };

    Dictionary<Levels, LevelsValues> levelsDictionary = new Dictionary<Levels, LevelsValues>();

    public Dictionary<Levels, LevelsValues> LevelsDictionary { get => levelsDictionary; set => levelsDictionary = value; }

    private void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        HandleLevelsDictionary();
    }

    public LevelsValues GetDictionaryValue(Levels level, LevelsValues val)
    {
        levelsDictionary.TryGetValue(level, out val); 
        return val;
    }


    void HandleLevelsDictionary()
    {
        levelsDictionary.Add(Levels.mainMenuScreen, LevelsValues.Main_Menu);
        levelsDictionary.Add(Levels.gameScreen, LevelsValues.Game);
        levelsDictionary.Add(Levels.winScreen, LevelsValues.Win);
        levelsDictionary.Add(Levels.gameOverScreen, LevelsValues.Game_Over);
    }

}
