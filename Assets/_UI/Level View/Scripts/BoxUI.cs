using System;
using DG.Tweening;
using Dt.Attribute;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoxUI : MonoBehaviour
{
    [SerializeField, Required]
    private Button selectButton;

    [SerializeField, ReadOnly]
    private int id;

    [SerializeField, Required]
    private Image leftCover;

    [SerializeField, Required]
    private Image rightCover;

    [SerializeField, Required]
    private GameObject lockObject;

    [SerializeField, Required]
    private GameObject mask;

    [SerializeField, Required]
    private TMP_Text starToUnlock;

    public event Action<int> OnClicked;

    public void Initialize(int id, Sprite cover, int starToUnlock)
    {
        this.id = id;
        this.selectButton.onClick.AddListener(OnClickHandler);
        this.leftCover.sprite = cover;
        this.rightCover.sprite = cover;
        this.starToUnlock.SetText(starToUnlock.ToString());
    }

    public void UpdateStatus(bool isUnlocked)
    {
        this.lockObject.SetActive(!isUnlocked);
        this.mask.SetActive(isUnlocked);
    }

    private void OnClickHandler()
    {
        OnClicked?.Invoke(this.id);
    }
}