using Core.AudioService;
using Core.Service;
using UnityEngine;

public class WinViewPresenter : BaseViewPresenter
{
    private WinView view;
    private readonly LevelDatabase levelDatabase;
    private readonly IAdsAdapter adsAdapter;

    public WinViewPresenter(GamePresenter presenter, Transform transform, 
        LevelDatabase levelDatabase, IAdsAdapter adsAdapter) : base(presenter, transform)
    {
        this.levelDatabase = levelDatabase;
        this.adsAdapter = adsAdapter;
    }

    protected override void AddViews()
    {
        this.view = AddView<WinView>();
    }

    protected override void OnShow()
    {
        this.adsAdapter.InterstitialAdapter.Show();
        ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.Win);
        this.view.ShowStars(this.levelDatabase
            .GetCurrentBox().GetCurrentLevelStar());
        base.OnShow();
        this.view.OnClickNext += OnClickNextHandler;
        this.view.OnClickReplay += OnClickReplayHandler;
        this.view.OnClickHome += OnClickHomeHandler;
    }


    private async void OnClickNextHandler()
    {
        ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.Tap);
        if (this.levelDatabase.GetCurrentBox().IsCurrentLevelLastLevel())
        {
            await Hide();
            this.levelDatabase.currentBoxId += 1;
            this.levelDatabase.currentBoxId %= this.levelDatabase.boxes.Count;
            await Presenter.GetViewPresenter<LevelViewPresenter>().Show();
            await Presenter.GetViewPresenter<TransitionViewPresenter>().Hide();
            await Presenter.GetViewPresenter<LevelViewPresenter>()
                .ScrollToBox(this.levelDatabase.currentBoxId);
        }
        else
        {
            await Presenter.GetViewPresenter<GameplayViewPresenter>().Show();
            Messenger.Broadcast(Message.PlayNextLevel);
            await Hide();
            await Presenter.GetViewPresenter<TransitionViewPresenter>().Hide();
        }
    }

    private async void OnClickReplayHandler()
    {
        ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.Tap);
        Messenger.Broadcast(Message.Replay);
        Presenter.GetViewPresenter<GameplayViewPresenter>().Show();
        await Hide();
        await Presenter.GetViewPresenter<TransitionViewPresenter>().Hide();
    }

    protected override void OnHide()
    {
        base.OnHide();
        this.view.OnClickNext -= OnClickNextHandler;
        this.view.OnClickReplay -= OnClickReplayHandler;
        this.view.OnClickHome -= OnClickHomeHandler;
    }

    private async void OnClickHomeHandler()
    {
        ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.Tap);
        Messenger.Broadcast(Message.ClearLevel);
        await Hide();
        Presenter.GetViewPresenter<HomeViewPresenter>().Show();
        Presenter.GetViewPresenter<TransitionViewPresenter>().Hide();
    }
}