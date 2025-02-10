using System;
using System.Collections.Generic;

[Serializable]
public class BoxDatabase
{
    public int currentLevelId;
    public int levelTopId;
    public List<int> stars = new List<int>();
    private const string LevelPath = "Assets/_Level/Box_";

    public BoxDatabase(int id)
    {
        string[] levels =
            System.IO.Directory.GetFiles($"{LevelPath}{id + 1}/");
        foreach (string level in levels)
        {
            if (level.EndsWith(".meta")) continue;
            this.stars.Add(0);
        }
    }

    public int GetStar(int levelId)
    {
        return this.stars[levelId];
    }

    public void SetStar(int levelId, int star)
    {
        this.stars[levelId] = star;
    }

    public int GetCurrentLevelStar()
    {
        return this.stars[this.currentLevelId];
    }

    public void SetCurrentLevelStar(int star)
    {
        this.stars[this.currentLevelId] = star;
    }
}