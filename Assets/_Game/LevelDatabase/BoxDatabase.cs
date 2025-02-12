using System;
using System.Collections.Generic;

[Serializable]
public class BoxDatabase
{
    public int currentLevelId;
    public int levelTopId;
    public List<int> stars = new List<int>();
    public int starToUnlocked;

    public BoxDatabase(int numberLevels, int starToUnlocked)
    {
        for(int i = 0; i < numberLevels; i++)
        {
            this.stars.Add(0);
        }
        this.starToUnlocked = starToUnlocked;
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

    public bool IsCurrentLevelLastLevel()
    {
        return this.currentLevelId >= this.stars.Count - 1;
    }

    public void UpdateLevelTop()
    {
        if (this.currentLevelId + 1 > this.levelTopId)
        {
            this.levelTopId = this.currentLevelId + 1;
            this.levelTopId = Math.Clamp(this.levelTopId, 0, this.stars.Count - 1);
        }
    }

    public int GetTotalStars()
    {
        int totalStars = 0;
        for (int i = 0; i <= this.levelTopId; i++)
        {
            totalStars += this.stars[i];
        }
        return totalStars;
    }
}