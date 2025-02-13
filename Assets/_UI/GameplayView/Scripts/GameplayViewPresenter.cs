using Core.AudioService;
using Core.Service;
using UnityEngine;

public class GameplayViewPresenter : BaseViewPresenter
{
    private GameplayView view;
    private int collectedStar;

    public GameplayViewPresenter(GamePresenter presenter, Transform transform) : base(
        presenter, transform)
    {
    }

    protected override void AddViews()
    {
        this.view = AddView<GameplayView>();
        Messenger.AddListener(Message.CollectStar, CollectStarHandler);
    }

    private void CollectStarHandler()
    {
        this.view.ShowCollectedStarsAt(this.collectedStar);
        this.collectedStar++;
    }

    protected override void OnShow()
    {
        ServiceLocator.GetService<IAudioService>().PlayMusic(AudioName.Play);
        this.collectedStar = 0;
        base.OnShow();
        this.view.OnClickBack += OnClickBackHandler;
        this.view.OnClickReplay += OnClickReplayHandler;
    }


    protected override void OnHide()
    {
        ServiceLocator.GetService<IAudioService>().StopMusic(AudioName.Play);
        base.OnHide();
        this.view.OnClickBack -= OnClickBackHandler;
        this.view.OnClickReplay -= OnClickReplayHandler;
    }

    private async void OnClickBackHandler()
    {
        ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.Tap);
        Time.timeScale = 0;
        await Presenter.GetViewPresenter<TransitionViewPresenter>().Show();
        Messenger.Broadcast(Message.ClearLevel);
        Time.timeScale = 1;
        await Hide();
        await Presenter.GetViewPresenter<HomeViewPresenter>().Show();
        await Presenter.GetViewPresenter<TransitionViewPresenter>().Hide();
    }
    
    private async void OnClickReplayHandler()
    {
        ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.Tap);
        Time.timeScale = 0;
        this.collectedStar = 0;
        this.view.HideAllCollectedStar();
        await Presenter.GetViewPresenter<TransitionViewPresenter>().Show();
        Messenger.Broadcast(Message.Replay);
        Time.timeScale = 1;
        await Presenter.GetViewPresenter<TransitionViewPresenter>().Hide();
    }
}