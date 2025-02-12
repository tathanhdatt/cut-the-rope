using System;
using System.Collections.Generic;

[Serializable]
public class LevelDatabase
{
    public int currentBoxId;
    public int boxTopId;

    public List<BoxDatabase> boxes = new List<BoxDatabase>();
    public event Action OnUpdateBox;

    public LevelDatabase(string data)
    {
        string[] splitData = data.Split('\n');
        for (int i = 0; i < splitData.Length; i++)
        {
            if (!splitData[i].Contains("Box_")) continue;
            int numberLevelsInBox = 0;
            while (!splitData[i + numberLevelsInBox + 1].Contains("Box_"))
            {
                numberLevelsInBox++;
                if (i + numberLevelsInBox + 1 >= splitData.Length) break;
            }

            int starToUnlock = int.Parse(splitData[i].Split(',')[1]);
            BoxDatabase boxDatabase = new BoxDatabase(numberLevelsInBox, starToUnlock);
            this.boxes.Add(boxDatabase);
        }
    }

    public int GetLevelStar(int boxId, int levelId)
    {
        return this.boxes[boxId].GetStar(levelId);
    }

    public void SetLevelStar(int boxId, int levelId, int star)
    {
        this.boxes[boxId].SetStar(levelId, star);
    }

    public BoxDatabase GetCurrentBox()
    {
        return this.boxes[this.currentBoxId];
    }

    public int GetTotalStars()
    {
        int totalStars = 0;
        for (int i = 0; i <= this.boxTopId; i++)
        {
            totalStars += this.boxes[i].GetTotalStars();
        }

        return totalStars;
    }

    public bool IsBoxUnlocked(int boxId)
    {
        return boxId <= this.boxTopId;
    }

    public void UpdateUnlockedBox()
    {
        int totalStars = GetTotalStars();
        for (int i = this.boxTopId + 1; i < this.boxes.Count; i++)
        {
            if (totalStars >= this.boxes[i].starToUnlocked)
            {
                this.boxTopId += 1;
            }
        }

        OnUpdateBox?.Invoke();
    }
}