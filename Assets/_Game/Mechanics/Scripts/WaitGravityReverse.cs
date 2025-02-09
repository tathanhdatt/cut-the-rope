using Cysharp.Threading.Tasks;
using Dt.BehaviourTree;
using Dt.BehaviourTree.Leaf;

public class WaitGravityReverse : VisualNode, ILeafNode
{
    protected override async UniTask OnRunning()
    {
        await UniTask.CompletedTask;
    }
}