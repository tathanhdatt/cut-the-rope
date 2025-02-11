using System;
using System.Collections.Generic;

[Serializable]
public class LevelDatabase
{
    public int currentBoxId;

    public List<BoxDatabase> boxes = new List<BoxDatabase>();

    public LevelDatabase(string data)
    {
        string[] splitData = data.Split(',');
        for (int i = 0; i < splitData.Length; i++)
        {
            if (!splitData[i].Contains("Box_")) continue;
            int numberLevelsInBox = 0;
            while (!splitData[i + numberLevelsInBox + 1].Contains("Box_"))
            {
                numberLevelsInBox++;
                if (i + numberLevelsInBox + 1 >= splitData.Length) break;
            }

            BoxDatabase boxDatabase = new BoxDatabase(numberLevelsInBox);
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
}