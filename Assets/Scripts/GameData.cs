using System.Collections.Generic;

[System.Serializable]
public class LevelData
{
    public string levelName;
    public bool isUnlocked;
    public float highscore;
}

[System.Serializable]
public class GameData
{
    public List<LevelData> levels = new();
}