using UnityEngine;

public class LevelAutoPlayer : MonoBehaviour
{
    private async void Start()
    {
        await FindAnyObjectByType<LevelPlayer>().Initialize();
        FindAnyObjectByType<LevelPlayer>().Play();
    }
}