using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dt.Attribute;
using UnityEngine;
using UnityEngine.UI;

public class WinView : BaseView
{
    [SerializeField, Required]
    private CanvasGroup fadeGroup;

    [SerializeField, Required]
    private Button nextButton;

    [SerializeField, Required]
    private Button replayButton;

    [SerializeField]
    private GameObject[] stars;

    public event Action OnClickNext;
    public event Action OnClickReplay;

    public override async UniTask Initialize()
    {
        await base.Initialize();
        this.nextButton.onClick.AddListener(() => OnClickNext?.Invoke());
        this.replayButton.onClick.AddListener(() => OnClickReplay?.Invoke());
    }

    public override async UniTask Show()
    {
        HideAllStars();
        await base.Show();
        await ShowCanvasGroup();
    }

    private async UniTask ShowCanvasGroup()
    {
        await this.fadeGroup.DOFade(1, 0.5f)
            .SetEase(Ease.OutQuart).AsyncWaitForCompletion();
    }

    public override async UniTask Hide()
    {
        await HideCanvasGroup();
        await base.Hide();
    }

    private async UniTask HideCanvasGroup()
    {
        await this.fadeGroup.DOFade(0, 0.5f)
            .SetEase(Ease.OutQuart).AsyncWaitForCompletion();
    }

    public void ShowStars(int numStars)
    {
        for (int i = 0; i < numStars; i++)
        {
            this.stars[i].transform.localScale = Vector3.zero;
        }

        for (int i = 0; i < numStars; i++)
        {
            this.stars[i].SetActive(true);
            this.stars[i].transform.DOScale(Vector3.one, 0.2f)
                .SetEase(Ease.OutBack);
        }
    }

    public void HideAllStars()
    {
        HideStars(this.stars.Length);
    }

    public void HideStars(int numberStars)
    {
        for (int i = 0; i < numberStars; i++)
        {
            this.stars[i].SetActive(false);
        }
    }
}