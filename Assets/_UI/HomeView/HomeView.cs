using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dt.Attribute;
using UnityEngine;
using UnityEngine.UI;

public class HomeView : BaseView
{
    [SerializeField, Required]
    private Image fadeBg;
    
    [SerializeField, Required]
    private Button playButton;
    
    public event Action OnClickPlay;

    public override async UniTask Initialize()
    {
        await base.Initialize();
        this.playButton.onClick.AddListener(() => this.OnClickPlay?.Invoke());
    }

    public override async UniTask Show()
    {
        await base.Show();
        await FadeOut();
    }

    private async UniTask FadeOut()
    {
        await this.fadeBg.DOFade(0f, 0.6f)
            .SetEase(Ease.InQuad).AsyncWaitForCompletion();
    }
}