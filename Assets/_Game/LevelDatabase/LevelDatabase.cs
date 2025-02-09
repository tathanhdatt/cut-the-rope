using System;
using System.Collections.Generic;

[Serializable]
public class LevelDatabase
{
    public int currentLevel = 1;


    public int levelTop = 1;


    private int currentBox = 1;

    public List<BoxDatabase> boxes = new List<BoxDatabase>();

    public int GetCurrentLevelStar()
    {
        return this.boxes[this.currentBox - 1].stars[this.currentLevel - 1];
    }

    public void SetCurrentLevelStar(int star)
    {
        this.boxes[this.currentBox - 1].stars[this.currentLevel - 1] = star;
    }

    public BoxDatabase GetCurrentBox()
    {
        return this.boxes[this.currentBox - 1];
    }

    public void AddNewLevels()
    {
        this.boxes[this.currentBox - 1].stars.Add(0);
    }
}