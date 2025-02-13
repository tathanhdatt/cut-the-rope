using System.Collections.Generic;
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

    public async UniTask Load(UniTask initManagerTask, UniTask initPresenterTask)
    {
        await this.loadingBar.FillTo(0.3f, 0.8f);
        await UniTask.WhenAny(initManagerTask);
        await this.loadingBar.FillTo(0.8f, 0.6f);
        await UniTask.WhenAny(initPresenterTask);
        await this.loadingBar.FillTo(1f, 0.4f);
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