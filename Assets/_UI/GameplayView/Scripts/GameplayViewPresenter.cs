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
        this.collectedStar = 0;
        base.OnShow();
        this.view.OnClickBack += OnClickBackHandler;
        this.view.OnClickReplay += OnClickReplayHandler;
    }


    protected override void OnHide()
    {
        base.OnHide();
        this.view.OnClickBack -= OnClickBackHandler;
        this.view.OnClickReplay -= OnClickReplayHandler;
    }

    private async void OnClickBackHandler()
    {
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
        Time.timeScale = 0;
        this.collectedStar = 0;
        this.view.HideAllCollectedStar();
        await Presenter.GetViewPresenter<TransitionViewPresenter>().Show();
        Messenger.Broadcast(Message.Replay);
        Time.timeScale = 1;
        await Presenter.GetViewPresenter<TransitionViewPresenter>().Hide();
    }
}