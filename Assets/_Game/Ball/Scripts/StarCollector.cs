using Dt.Attribute;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class StarCollector : MonoBehaviour
{
    [SerializeField, ValueDropdown(ValueDropdownField.Tag)]
    private string starTag;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(this.starTag))
        {
            other.gameObject.SetActive(false);
            Messenger.Broadcast(Message.CollectStar);
        }
    }
}