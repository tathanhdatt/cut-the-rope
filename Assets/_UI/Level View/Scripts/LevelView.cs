using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dt.Attribute;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : BaseView
{
    [SerializeField, Required]
    private Image fadeBg;

    [SerializeField, Required]
    private ScrollRect boxScrollRect;

    [SerializeField, Required]
    private BoxUI boxPrefab;

    [SerializeField, Required]
    private Button backButton;

    [SerializeField]
    private float snappingVelocity;

    [SerializeField]
    private float snappingDuration;

    [Line]
    [SerializeField, Required]
    private TMP_Text totalCollectedStar;

    [Title("Levels")]
    [SerializeField, Required]
    private GameObject levelContent;

    [SerializeField]
    private LevelUI[] levels;

    [SerializeField]
    private Sprite[] stars;

    [SerializeField, ReadOnly]
    private float rangePerBox;

    [Title("Footer")]
    [SerializeField, Required]
    private IndicatorFillBar footer;

    [SerializeField, Required]
    private GameObject dotPrefab;

    [SerializeField, Required]
    private Transform footerContent;

    private Tweener snappingTweener;

    private readonly List<BoxUI> boxes = new List<BoxUI>(2);

    public event Action<int> OnSelectedBox;
    public event Action OnClickBack;

    public override async UniTask Initialize()
    {
        await base.Initialize();
        this.boxScrollRect.onValueChanged.AddListener(OnValueChangedHandler);
        this.backButton.onClick.AddListener(() => OnClickBack?.Invoke());
    }

    private void OnValueChangedHandler(Vector2 pos)
    {
        this.footer.SetIndicator(this.boxScrollRect.horizontalNormalizedPosition);
        if (Mathf.Abs(this.boxScrollRect.velocity.x) > this.snappingVelocity)
        {
            this.snappingTweener?.Kill();
            return;
        }

        if (this.snappingTweener.IsActive())
        {
            return;
        }

        pos = this.boxScrollRect.normalizedPosition;
        pos *= 100;
        this.rangePerBox = 100f / (this.boxScrollRect.content.childCount - 1);
        float remainder = pos.x % this.rangePerBox;
        pos.x -= remainder;
        if (remainder >= this.rangePerBox / 2)
        {
            remainder = this.rangePerBox;
        }
        else
        {
            remainder = 0;
        }

        pos.x += remainder;
        pos /= 100f;
        this.snappingTweener = this.boxScrollRect
            .DONormalizedPos(pos, this.snappingDuration)
            .SetEase(Ease.OutQuad);
    }

    public override async UniTask Show()
    {
        SetActionBoxScrollRect(true);
        SetActiveFooter(true);
        HideAllLevels();
        this.boxScrollRect.horizontalNormalizedPosition = 0;
        this.footer.SetIndicator(0);
        await base.Show();
        await FadeOut();
    }

    private async UniTask FadeOut()
    {
        await this.fadeBg.DOFade(0f, 0.6f)
            .SetEase(Ease.OutQuad).AsyncWaitForCompletion();
    }

    public void AddBox(int id, Sprite coverSprite, int starToUnlock)
    {
        BoxUI box = Instantiate(this.boxPrefab, this.boxScrollRect.content);
        box.Initialize(id, coverSprite, starToUnlock);
        box.OnClicked += OnClickedBoxHandler;
        this.boxes.Add(box);
    }

    public void AddFooterDot()
    {
        Instantiate(this.dotPrefab, this.footerContent);
    }

    private void OnClickedBoxHandler(int boxId)
    {
        OnSelectedBox?.Invoke(boxId);
    }

    public LevelUI ShowLevel(int levelId, bool isLocked, int star)
    {
        this.levels[levelId].Show(levelId, isLocked, this.stars[star]);
        return this.levels[levelId];
    }

    public void SetActiveLevelContent(bool isActive)
    {
        this.levelContent.SetActive(isActive);
    }

    public void HideAllLevels()
    {
        SetActiveLevelContent(false);
        foreach (LevelUI level in this.levels)
        {
            level.gameObject.SetActive(false);
        }
    }

    public void SetActionBoxScrollRect(bool isActive)
    {
        this.boxScrollRect.gameObject.SetActive(isActive);
    }

    public async UniTask ScrollToBox(int boxId)
    {
        this.snappingTweener = this.boxScrollRect
            .DOHorizontalNormalizedPos(this.rangePerBox / 100 * boxId, 1.4f)
            .SetEase(Ease.OutQuad);
        await this.snappingTweener.AsyncWaitForCompletion();
    }

    public void UpdateBox(int boxId, bool isUnlocked)
    {
        this.boxes[boxId].UpdateStatus(isUnlocked);
    }

    public void SetTotalCollectedStar(int star)
    {
        this.totalCollectedStar.SetText(star.ToString());
    }

    public void SetActiveFooter(bool active)
    {
        this.footer.gameObject.SetActive(active);
    }
}