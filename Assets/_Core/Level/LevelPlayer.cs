using Cysharp.Threading.Tasks;
using Dt.Attribute;
using Dt.BehaviourTree;
using UnityEngine;

public class LevelPlayer : MonoBehaviour
{
    [SerializeField, Required]
    private VisualNode startingNode;
    
    [SerializeField]
    private Rope[] ropes;

    public async UniTask Initialize()
    {
        ResetGravity();
        await this.startingNode.Initialize();
        InitializeRope();
    }

    private void InitializeRope()
    {
        foreach (Rope rope in this.ropes)
        {
            rope.Initialize();
        }
    }

    private void ResetGravity()
    {
        Physics2D.gravity = -9.81f * Vector2.up;
    }

    public void Play()
    {
        this.startingNode.Run();
    }
}