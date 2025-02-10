using System;
using Dt.Attribute;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUI : MonoBehaviour
{
    [SerializeField, Required]
    private TMP_Text levelText;

    [SerializeField, Required]
    private GameObject lockIcon;

    [SerializeField, Required]
    private Image starImage;

    [SerializeField, Required]
    private Button playButton;

    public event Action<int> OnClickPlay;

    public void Show(int levelId, bool isLocked, Sprite icon)
    {
        this.playButton.onClick.RemoveAllListeners();
        this.playButton.onClick.AddListener(() => OnClickPlay?.Invoke(levelId));
        if (isLocked)
        {
            this.lockIcon.SetActive(true);
            this.levelText.gameObject.SetActive(false);
            this.starImage.gameObject.SetActive(false);
        }
        else
        {
            this.lockIcon.SetActive(false);
            this.levelText.gameObject.SetActive(true);
            this.starImage.gameObject.SetActive(true);
            this.levelText.SetText($"{levelId + 1}");
            this.starImage.sprite = icon;
        }
        gameObject.SetActive(true);
    }
}