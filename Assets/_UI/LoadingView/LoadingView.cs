using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dt.Attribute;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : BaseView
{
    [SerializeField, Required]
    private SliceImageFillBar loadingBar;

    [SerializeField, Required]
    private Image fadeBg;

    public override async UniTask Show()
    {
        await base.Show();
        await this.loadingBar.FillTo(0.3f, 0.8f);
        await UniTask.WaitForSeconds(0.4f);
        await this.loadingBar.FillTo(1f, 0.6f);
        await Hide();
    }

    public override async UniTask Hide()
    {
        await FadeIn();
        await base.Hide();
    }

    private async UniTask FadeIn()
    {
        await this.fadeBg.DOFade(1f, 0.5f)
            .SetEase(Ease.OutQuad).AsyncWaitForCompletion();
    }
}