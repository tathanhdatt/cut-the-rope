using Dt.Attribute;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BallTarget : MonoBehaviour
{
    [SerializeField, ValueDropdown(ValueDropdownField.Tag)]
    private string ballTag;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(this.ballTag))
        {
            Destroy(other.gameObject);
            Messenger.Broadcast(Message.LevelWin);
        }
    }
}