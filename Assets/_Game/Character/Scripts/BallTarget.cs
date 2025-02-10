using Cysharp.Threading.Tasks;
using Dt.Attribute;
using Dt.Extension;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class BallTarget : MonoBehaviour
{
    [SerializeField, ValueDropdown(ValueDropdownField.Tag)]
    private string ballTag;

    [SerializeField, ValueDropdown("GetAnims")]
    private string biteAnim;

    [SerializeField, Required]
    private Animator animator;

    [SerializeField]
    private float delay;

    [SerializeField]
    private float delayDestroyBall;

    private async void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(this.ballTag))
        {
            this.animator.Play(this.biteAnim);
            await UniTask.WaitForSeconds(this.delayDestroyBall);
            Destroy(other.gameObject);
            await UniTask.WaitForSeconds(this.delay);
            Messenger.Broadcast(Message.LevelWin);
        }
    }

#if UNITY_EDITOR
    private string[] GetAnims()
    {
        return this.animator.GetAnimations();
    }
#endif
}