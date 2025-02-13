using System;
using System.Threading;
using Core.AudioService;
using Core.Service;
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

    private CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();

    private async void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(this.ballTag))
        {
            try
            {
                this.animator.Play(this.biteAnim);
                await UniTask.WaitForSeconds(this.delayDestroyBall,
                    cancellationToken: this.cancellationTokenSource.Token);
                PlaySound();
                if (other == null) return;
                Destroy(other.gameObject);
                await UniTask.WaitForSeconds(this.delay,
                    cancellationToken: this.cancellationTokenSource.Token);
                Messenger.Broadcast(Message.LevelWin);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
        }
    }

    private async void PlaySound()
    {
        ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.MountOpen);
        await UniTask.WaitForSeconds(0.3f);
        ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.MountClose);
    }

    private void OnDestroy()
    {
        this.cancellationTokenSource.Cancel();
    }

#if UNITY_EDITOR
    private string[] GetAnims()
    {
        return this.animator.GetAnimations();
    }
#endif
}