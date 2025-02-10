﻿using System;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Dt.Attribute;
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

    [SerializeField]
    private float velocity;

    [Title("Levels")]
    [SerializeField, Required]
    private GameObject levelContent;

    [SerializeField]
    private LevelUI[] levels;

    [SerializeField]
    private Sprite[] stars;

    private Tweener snappingTweener;

    public event Action<int> OnSelectedBox;
    public event Action OnClickBack;

    public override async UniTask Initialize()
    {
        await base.Initialize();
        this.boxScrollRect.onValueChanged.AddListener(OnValueChangedHandler);
        this.backButton.onClick.AddListener(() => OnClickBack?.Invoke());
    }

    [SerializeField]
    private Vector2 a;

    private void Update()
    {
        this.a = this.boxScrollRect.normalizedPosition;
    }

    private void OnValueChangedHandler(Vector2 pos)
    {
        this.velocity = this.boxScrollRect.velocity.x;
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
        float rangePerBox = 100f / (this.boxScrollRect.content.childCount - 1);
        Debug.Log(rangePerBox);
        float remainder = pos.x % rangePerBox;
        pos.x -= remainder;
        Debug.Log("pos"+pos.x);
        if (remainder >= rangePerBox / 2)
        {
            remainder = rangePerBox;
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
        HideAllLevels();
        this.boxScrollRect.horizontalNormalizedPosition = 0;
        await base.Show();
        await FadeOut();
    }

    private async UniTask FadeOut()
    {
        await this.fadeBg.DOFade(0f, 0.6f)
            .SetEase(Ease.OutQuad).AsyncWaitForCompletion();
    }

    public void AddBox(int id, Sprite coverSprite)
    {
        BoxUI box = Instantiate(this.boxPrefab, this.boxScrollRect.content);
        box.Initialize(id, coverSprite);
        box.OnClicked += OnClickedBoxHandler;
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
}