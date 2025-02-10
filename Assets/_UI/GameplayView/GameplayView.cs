using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dt.Attribute;
using UnityEngine;
using UnityEngine.UI;

public class GameplayView : BaseView
{
    [SerializeField, Required]
    private Image fadeBg;
    
    [SerializeField, Required]
    private Button backButton;
    
    public event Action OnClickBack;

    public override async UniTask Initialize()
    {
        await base.Initialize();
        this.backButton.onClick.AddListener(() => OnClickBack?.Invoke());
    }

    public override async UniTask Show()
    {
        await base.Show();
        await FadeOut();
    }

    private async UniTask FadeOut()
    {
        await this.fadeBg.DOFade(0f, 0.6f)
            .SetEase(Ease.OutQuad).AsyncWaitForCompletion();
    }

    public override async UniTask Hide()
    {
        await FadeIn();
        await base.Hide();
    }

    private async UniTask FadeIn()
    {
        await this.fadeBg.DOFade(1f, 0.6f)
            .SetEase(Ease.InQuad).AsyncWaitForCompletion();
    }
}