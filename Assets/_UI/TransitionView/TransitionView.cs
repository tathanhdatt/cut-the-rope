using System.Threading.Tasks;
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

    public override async UniTask Show()
    {
        ResetPosition();
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
        this.leftCover.DOLocalMove(this.startLeftCoverPosition, this.coverDuration)
            .SetEase(Ease.OutQuart);
        this.rightCover.DOLocalMove(this.startRightCoverPosition, this.coverDuration)
            .SetEase(Ease.OutQuart);
        await UniTask.WaitForSeconds(this.coverDuration);
    }

    private async UniTask ReverseCovers()
    {
        this.leftCover.DOLocalMove(this.endLeftCoverPosition, this.coverDuration)
            .SetEase(Ease.OutQuart);
        this.rightCover.DOLocalMove(this.endRightCoverPosition, this.coverDuration)
            .SetEase(Ease.OutQuart);
        await UniTask.WaitForSeconds(this.coverDuration);
    }

    public override async UniTask Hide()
    {
        await ReverseCovers();
        await base.Hide();
    }
}