using System;
using System.Collections.Generic;

[Serializable]
public class LevelDatabase
{
    public int currentBoxId;

    public List<BoxDatabase> boxes = new List<BoxDatabase>();

    private const string BoxPath = "Assets/_Level/";

    public LevelDatabase()
    {
        int numBoxes = System.IO.Directory.GetDirectories(BoxPath).Length;
        for (int i = 0; i < numBoxes; i++)
        {
            BoxDatabase boxDatabase = new BoxDatabase(i);
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