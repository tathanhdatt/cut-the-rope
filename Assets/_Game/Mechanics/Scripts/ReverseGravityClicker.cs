using Cysharp.Threading.Tasks;
using Dt.Attribute;
using Dt.BehaviourTree;
using Dt.BehaviourTree.Leaf;
using Dt.Extension;
using Lean.Touch;
using UnityEngine;

public class ReverseGravityClicker : VisualNode, ILeafNode
{
    [SerializeField, ValueDropdown(ValueDropdownField.Tag)]
    private string reverseTag;

    [SerializeField]
    private float normalGravity = -9.81f;

    [SerializeField]
    private float reversedGravity = 9.81f;

    [SerializeField, ReadOnly]
    private bool isCompleted;

    private readonly RaycastHit2D[] hits = new RaycastHit2D[1];

    private readonly ContactFilter2D filter2D = new ContactFilter2D()
    {
        useTriggers = true
    };

    private bool TryReverse(Collider2D other)
    {
        if (other.CompareTag(this.reverseTag))
        {
            this.isCompleted = true;
            if (Physics2D.gravity.y >= 0)
            {
                Physics2D.gravity = Vector2.up * this.normalGravity;
            }
            else
            {
                Physics2D.gravity = Vector2.up * this.reversedGravity;
            }

            other.GetComponent<Ball>().SetActiveBubbleGraphic(false);
            return true;
        }

        return false;
    }

    protected override async UniTask OnStartRunning()
    {
        await base.OnStartRunning();
        LeanTouch.OnFingerDown += OnFingerDownHandler;
    }

    private void OnFingerDownHandler(LeanFinger finger)
    {
        Vector3 worldPos = finger.GetWorldPosition(10);
        this.hits.Clear();
        int numberHits = Physics2D.Raycast(
            worldPos,
            Vector2.zero,
            this.filter2D,
            this.hits);
        if (numberHits == 0) return;
        foreach (RaycastHit2D hit in this.hits)
        {
            if (hit == default(RaycastHit2D)) continue;
            if (TryReverse(hit.collider))
            {
                break;
            }
        }
    }

    protected override async UniTask OnRunning()
    {
        await UniTask.WaitUntil(() => this.isCompleted);
    }

    protected override async UniTask OnEndRunning()
    {
        await base.OnEndRunning();
        LeanTouch.OnFingerDown -= OnFingerDownHandler;
    }
}