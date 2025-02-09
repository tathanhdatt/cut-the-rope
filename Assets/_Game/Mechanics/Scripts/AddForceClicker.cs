using Cysharp.Threading.Tasks;
using Dt.Attribute;
using Dt.BehaviourTree;
using Dt.BehaviourTree.Leaf;
using Lean.Touch;
using UnityEngine;

public class AddForceClicker : VisualNode, ILeafNode
{
    [SerializeField, Required]
    private Collider2D clickedArea;

    [SerializeField, Required]
    private Collider2D triggerCollider2D;

    [SerializeField, ValueDropdown(ValueDropdownField.Tag)]
    private string addForceTag;

    [SerializeField, Required]
    private Transform direction;

    [SerializeField]
    private float force;

    [SerializeField, ReadOnly]
    private bool canAddForce;

    public override async UniTask Initialize()
    {
        await base.Initialize();
        LeanTouch.OnFingerDown += OnFingerDownHandler;
        LeanTouch.OnFingerUp += OnFingerUpHandler;
    }


    private void OnFingerDownHandler(LeanFinger finger)
    {
        Vector3 worldPos = finger.GetWorldPosition(10);
        if (this.clickedArea.bounds.Contains(worldPos))
        {
            this.canAddForce = true;
            this.triggerCollider2D.enabled = true;
        }
    }

    private void OnFingerUpHandler(LeanFinger finger)
    {
        this.canAddForce = false;
        this.triggerCollider2D.enabled = false;
    }

    protected override async UniTask OnRunning()
    {
        await UniTask.CompletedTask;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!this.canAddForce) return;
        if (other.CompareTag(this.addForceTag))
        {
            other.attachedRigidbody.AddForce(this.direction.up * this.force);
            this.canAddForce = false;
        }
    }

    private void OnDrawGizmos()
    {
        Debug.DrawRay(this.direction.position, this.direction.up, Color.red);
    }

    private void OnDestroy()
    {
        LeanTouch.OnFingerDown -= OnFingerDownHandler;
        LeanTouch.OnFingerUp -= OnFingerUpHandler;
    }
}