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

    [Header("Buttons")]
    [SerializeField, Required]
    private Button backButton;

    [SerializeField, Required]
    private Button replayButton;

    [Line]
    [SerializeField]
    private Transform[] collectedStars;

    public event Action OnClickBack;
    public event Action OnClickReplay;

    public override async UniTask Initialize()
    {
        await base.Initialize();
        this.backButton.onClick.AddListener(() => OnClickBack?.Invoke());
        this.replayButton.onClick.AddListener(() => OnClickReplay?.Invoke());
    }

    public override async UniTask Show()
    {
        HideAllCollectedStar();
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

    public void HideAllCollectedStar()
    {
        foreach (Transform star in this.collectedStars)
        {
            star.DOKill();
            star.localScale = Vector3.zero;
        }
    }

    public void ShowCollectedStarsAt(int index)
    {
        if (index >= this.collectedStars.Length) return;
        this.collectedStars[index]
            .DOScale(Vector3.one, 0.2f)
            .SetEase(Ease.OutBack);
    }
}