using System;
using Dt.Attribute;
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

    public event Action<int> OnClicked;

    public void Initialize(int id, Sprite cover)
    {
        this.id = id;
        this.selectButton.onClick.AddListener(() => OnClicked?.Invoke(this.id));
        this.leftCover.sprite = cover;
        this.rightCover.sprite = cover;
    }
}