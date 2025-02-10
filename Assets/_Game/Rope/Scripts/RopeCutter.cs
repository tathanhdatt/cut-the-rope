using Cysharp.Threading.Tasks;
using Dt.Attribute;
using Dt.BehaviourTree;
using Dt.BehaviourTree.Leaf;
using Dt.Extension;
using Lean.Touch;
using UnityEngine;

public class RopeCutter : VisualNode, ILeafNode
{
    [SerializeField, Required]
    private Transform graphic;

    [SerializeField]
    private int cutTimes;

    [SerializeField, ReadOnly]
    private Vector3 lastWorldPosition;

    [SerializeField, ReadOnly]
    private Vector3 worldPosition;

    [SerializeField, ReadOnly]
    private bool isLevelWin;
    
    private readonly RaycastHit2D[] hits = new RaycastHit2D[2];

    private readonly ContactFilter2D filter2D = new ContactFilter2D()
    {
        useTriggers = true
    };

    public override async UniTask Initialize()
    {
        await base.Initialize();
        Messenger.AddListener(Message.LevelWin, TurnOffCutting);
        this.graphic.gameObject.SetActive(false);
    }

    protected override async UniTask OnStartRunning()
    {
        await base.OnStartRunning();
        LeanTouch.OnFingerDown += OnFingerDownHandler;
        LeanTouch.OnFingerUp += OnFingerUpHandler;
    }


    private void OnFingerDownHandler(LeanFinger finger)
    {
        this.lastWorldPosition = finger.GetWorldPosition(10);
        this.worldPosition = finger.GetWorldPosition(10);
        this.graphic.position = this.worldPosition;
        this.graphic.gameObject.SetActive(true);
        LeanTouch.OnFingerUpdate += OnFingerUpdateHandler;
    }

    private void OnFingerUpHandler(LeanFinger finger)
    {
        this.graphic.gameObject.SetActive(false);
        LeanTouch.OnFingerUpdate -= OnFingerUpdateHandler;
    }

    private void TurnOffCutting()
    {
        Messenger.RemoveListener(Message.LevelWin, TurnOffCutting);
        this.isLevelWin = true;
    }

    protected override async UniTask OnRunning()
    {
        await UniTask.WaitUntil(() => this.isLevelWin);
    }

    protected override async UniTask OnEndRunning()
    {
        await base.OnEndRunning();
        this.graphic.gameObject.SetActive(false);
        LeanTouch.OnFingerUpdate -= OnFingerUpdateHandler;
        LeanTouch.OnFingerDown -= OnFingerDownHandler;
        LeanTouch.OnFingerUp -= OnFingerUpHandler;
    }


    private void OnFingerUpdateHandler(LeanFinger finger)
    {
        if (finger.ScreenDelta == Vector2.zero) return;
        this.lastWorldPosition = finger.GetLastWorldPosition(10);
        this.worldPosition = finger.GetWorldPosition(10);
        this.graphic.transform.position = this.worldPosition;
        this.hits.Clear();
        int numberHit = Physics2D.Linecast(
            this.lastWorldPosition,
            this.worldPosition,
            this.filter2D,
            this.hits);
        if (numberHit == 0) return;

        foreach (RaycastHit2D hit in this.hits)
        {
            if (hit == default) continue;
            ICuttable cuttable = hit.collider.GetComponent<ICuttable>();
            if (cuttable == null) continue;
            if (cuttable.IsCut) continue;
            cuttable.Cut();
            this.cutTimes--;
            if (this.cutTimes <= 0)
            {
                TurnOffCutting();
            }
            break;
        }
    }

    private void OnDestroy()
    {
        LeanTouch.OnFingerUpdate -= OnFingerUpdateHandler;
        LeanTouch.OnFingerDown -= OnFingerDownHandler;
        LeanTouch.OnFingerUp -= OnFingerUpHandler;
    }

    private void OnDrawGizmosSelected()
    {
        Debug.DrawRay(this.lastWorldPosition,
            this.worldPosition - this.lastWorldPosition,
            Color.green);
    }
}