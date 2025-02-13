using Core.AudioService;
using Core.Service;
using Cysharp.Threading.Tasks;
using Dt.Attribute;
using Dt.BehaviourTree;
using Dt.BehaviourTree.Leaf;
using UnityEngine;

public class ReverseGravityTrigger : VisualNode, ILeafNode
{
    [SerializeField, ValueDropdown(ValueDropdownField.Tag)]
    private string triggerTag;

    [SerializeField]
    private float normalGravity;

    [SerializeField]
    private float reversedGravity;

    [SerializeField, ReadOnly]
    private bool isReversed;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(this.triggerTag))
        {
            if (Physics2D.gravity.y >= 0)
            {
                ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.BubbleBreak);
                Physics2D.gravity = Vector2.up * this.normalGravity;
            }
            else
            {
                ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.Bubble);
                Physics2D.gravity = Vector2.up * this.reversedGravity;
            }

            gameObject.SetActive(false);
            this.isReversed = true;
            other.GetComponent<Ball>().SetActiveBubbleGraphic(true);
        }
    }

    protected override async UniTask OnStartRunning()
    {
        await base.OnStartRunning();
        this.isReversed = false;
    }

    protected override async UniTask OnRunning()
    {
        await UniTask.WaitUntil(() => this.isReversed);
    }
}