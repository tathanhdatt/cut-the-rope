using Dt.Attribute;
using UnityEngine;

public class RopeConnector : MonoBehaviour
{
    [SerializeField, ValueDropdown(ValueDropdownField.Tag)]
    private string ropeTag;

    [SerializeField, Required]
    private HingeJoint2D joint2D;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(this.ropeTag))
        {
            this.joint2D.connectedBody = other.attachedRigidbody;
            this.joint2D.enabled = true;
        }
    }
}