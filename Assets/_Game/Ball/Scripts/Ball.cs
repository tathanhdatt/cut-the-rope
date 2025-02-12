using Dt.Attribute;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField, Required]
    private GameObject bubbleGraphic;

    [SerializeField, Required]
    private SpriteRenderer graphic;

    [Title("Padding")]
    [SerializeField]
    private int leftPadding;

    [SerializeField]
    private int rightPadding;

    [SerializeField]
    private int topPadding;

    [SerializeField]
    private int bottomPadding;

    private Camera cam;
    private bool isCalled;

    private void Awake()
    {
        this.cam = Camera.main;
    }

    public void SetActiveBubbleGraphic(bool active)
    {
        this.bubbleGraphic.gameObject.SetActive(active);
    }

    private void Update()
    {
        if (IsOutScreen() && !this.isCalled)
        {
            this.isCalled = true;
            Messenger.Broadcast(Message.LevelLose);
        }
    }

    private bool IsOutScreen()
    {
        if (this.cam == null) return true;
        Vector2 screenPosition = this.cam.WorldToScreenPoint(transform.position);
        if (screenPosition.x < -this.leftPadding || 
            screenPosition.y < -this.bottomPadding)
        {
            return true;
        }

        if (screenPosition.x > Screen.width + this.rightPadding ||
            screenPosition.y > Screen.height + this.topPadding)
        {
            return true;
        }
        return false;
    }
}