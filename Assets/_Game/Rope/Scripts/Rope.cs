using System.Collections.Generic;
using Dt.Attribute;
using UnityEngine;

public class Rope : MonoBehaviour
{
    [SerializeField]
    private List<Chunk> chunks;

    public void Initialize()
    {
        foreach (Chunk chunk in this.chunks)
        {
            chunk.OnCut += OnCutHandler;
        }

        Messenger.AddListener(Message.LevelWin, LevelWinHandler);
    }

    private void LevelWinHandler()
    {
        Messenger.RemoveListener(Message.LevelWin, LevelWinHandler);
        FadeChucks();
    }

    private void OnCutHandler()
    {
        foreach (Chunk chunk in this.chunks)
        {
            chunk.OnCut -= OnCutHandler;
            chunk.IsCut = true;
        }
        FadeChucks();
    }

    private void FadeChucks()
    {
        foreach (Chunk chunk in this.chunks)
        {
            chunk.FadeGraphic();
        }
    }

    public void AddChunk(Chunk chunk)
    {
        this.chunks.Add(chunk);
    }

    public void CleanChucks()
    {
        this.chunks.Clear();
    }

    [Button]
    public void Enable()
    {
        foreach (Chunk chunk in this.chunks)
        {
            chunk.gameObject.SetActive(true);
        }

        foreach (Chunk chunk in this.chunks)
        {
            chunk.GetComponent<Joint2D>().enabled = true;
        }
    }

    [Button]
    public void Disable()
    {
        foreach (Chunk chunk in this.chunks)
        {
            chunk.gameObject.SetActive(false);
        }

        foreach (Chunk chunk in this.chunks)
        {
            chunk.GetComponent<Joint2D>().enabled = false;
        }
    }
}