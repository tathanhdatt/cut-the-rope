using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dt.Attribute;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class SliceImageFillBar : MonoBehaviour
{
    [SerializeField, Required]
    private Image fillImage;

    [SerializeField]
    private float fillDuration;

    [SerializeField]
    private Ease ease;

    private Tweener fillTweener;

    public async UniTask FillTo(float percentage)
    {
        await FillTo(percentage, this.fillDuration);
    }

    public async UniTask FillTo(float percentage, float duration)
    {
        this.fillTweener?.Complete();
        this.fillTweener = this.fillImage.DOFillAmount(percentage, duration);
        this.fillTweener.SetEase(this.ease);
        await this.fillTweener.AsyncWaitForCompletion();
    }

    public void SetFillAmount(float percentage)
    {
        this.fillTweener?.Complete();
        this.fillImage.fillAmount = percentage;
    }
}