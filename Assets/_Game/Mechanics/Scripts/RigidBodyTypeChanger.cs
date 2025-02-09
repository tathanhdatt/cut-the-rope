using Cysharp.Threading.Tasks;
using Dt.Attribute;
using Dt.BehaviourTree;
using Dt.BehaviourTree.Leaf;
using UnityEngine;

public class RigidBodyTypeChanger : VisualNode, ILeafNode
{
    [SerializeField, Required]
    private Rigidbody2D rb;
    [SerializeField]
    private RigidbodyType2D type2D;
    protected override async UniTask OnRunning()
    {
        this.rb.bodyType = this.type2D;
        await UniTask.CompletedTask;
    }
}