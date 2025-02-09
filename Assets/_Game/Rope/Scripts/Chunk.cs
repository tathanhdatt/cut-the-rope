using System;
using DG.Tweening;
using Dt.Attribute;
using UnityEngine;

public class Chunk : MonoBehaviour, ICuttable
{
    [SerializeField, Required]
    private Joint2D joint2D;

    [SerializeField, Required]
    private SpriteRenderer graphic;

    public event Action OnCut;

    public void Cut()
    {
        this.joint2D.enabled = false;
        OnCut?.Invoke();
    }

    public async void FadeGraphic()
    {
        await this.graphic.DOFade(0, 0.5f).AsyncWaitForCompletion();
        gameObject.SetActive(false);
    }
}