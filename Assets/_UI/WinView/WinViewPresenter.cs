using UnityEngine;

public class WinViewPresenter : BaseViewPresenter
{
    private WinView view;
    private LevelDatabase levelDatabase;

    public WinViewPresenter(GamePresenter presenter,
        Transform transform, LevelDatabase levelDatabase) : base(presenter, transform)
    {
        this.levelDatabase = levelDatabase;
    }

    protected override void AddViews()
    {
        this.view = AddView<WinView>();
    }

    protected override void OnShow()
    {
        this.view.ShowStars(this.levelDatabase
            .GetCurrentBox().GetCurrentLevelStar());
        base.OnShow();
        this.view.OnClickNext += OnClickNextHandler;
        this.view.OnClickReplay += OnClickReplayHandler;
    }


    private async void OnClickNextHandler()
    {
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
    }
}