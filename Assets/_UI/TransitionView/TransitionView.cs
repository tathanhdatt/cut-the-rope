using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dt.Attribute;
using UnityEngine;

public class TransitionView : BaseView
{
    [SerializeField, Required]
    private Transform leftCover;

    [SerializeField, Required]
    private Transform rightCover;

    [SerializeField, Required]
    private Transform knife;

    [Title("Start Position")]
    [SerializeField]
    private Vector3 startLeftCoverPosition;

    [SerializeField]
    private Vector3 startRightCoverPosition;

    [SerializeField]
    private Vector3 startKnifePosition;

    [Title("End Position")]
    [SerializeField]
    private Vector3 endLeftCoverPosition;

    [SerializeField]
    private Vector3 endRightCoverPosition;

    [SerializeField]
    private Vector3 endKnifePosition;

    [Title("Durations")]
    [SerializeField]
    private float coverDuration;

    [SerializeField]
    private float knifeDuration;

    [Line]
    [SerializeField, Required]
    private GameObject preventClickOverObject;

    public override async UniTask Show()
    {
        ResetPosition();
        this.preventClickOverObject.SetActive(true);
        await base.Show();
        await MoveCoversToCenter();
    }

    private void ResetPosition()
    {
        this.leftCover.localPosition = this.endLeftCoverPosition;
        this.rightCover.localPosition = this.endRightCoverPosition;
        // this.knife.position = this.startKnifePosition;
    }

    private async UniTask MoveCoversToCenter()
    {
        this.leftCover.DOKill(true);
        this.rightCover.DOKill(true);
        this.leftCover.DOLocalMove(this.startLeftCoverPosition, this.coverDuration)
            .SetEase(Ease.OutQuart).SetUpdate(true);
        this.rightCover.DOLocalMove(this.startRightCoverPosition, this.coverDuration)
            .SetEase(Ease.OutQuart).SetUpdate(true);
        await UniTask.WaitForSeconds(this.coverDuration, true);
    }

    private async UniTask ReverseCovers()
    {
        this.leftCover.DOKill(true);
        this.rightCover.DOKill(true);
        this.leftCover.DOLocalMove(this.endLeftCoverPosition, this.coverDuration)
            .SetEase(Ease.OutQuart).SetUpdate(true);
        this.rightCover.DOLocalMove(this.endRightCoverPosition, this.coverDuration)
            .SetEase(Ease.OutQuart).SetUpdate(true);
        this.preventClickOverObject.SetActive(false);
        await UniTask.WaitForSeconds(this.coverDuration, true);
    }

    public override async UniTask Hide()
    {
        await ReverseCovers();
        await base.Hide();
    }
}