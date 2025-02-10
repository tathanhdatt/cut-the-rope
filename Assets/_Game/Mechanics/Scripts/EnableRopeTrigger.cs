using Dt.Attribute;
using UnityEngine;

public class EnableRopeTrigger : MonoBehaviour
{
    [SerializeField, ValueDropdown(ValueDropdownField.Tag)]
    private string triggerTag;
    
    [SerializeField, Required]
    private Rope rope;

    [SerializeField, Required]
    private GameObject graphic;
    
    [SerializeField, Required]
    private Collider2D detectCollider;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(this.triggerTag))
        {
            this.rope.Enable();
            this.detectCollider.enabled = false;
            gameObject.SetActive(false);
        }
    }
}