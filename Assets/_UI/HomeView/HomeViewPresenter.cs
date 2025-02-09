using UnityEngine;

public class HomeViewPresenter : BaseViewPresenter
{
    private HomeView view;
    public HomeViewPresenter(GamePresenter gamePresenter, Transform transform) : base(gamePresenter, transform)
    {
    }

    protected override void AddViews()
    {
        this.view = AddView<HomeView>();
    }

    protected override void OnShow()
    {
        base.OnShow();
        this.view.OnClickPlay += OnClickPlayHandler;
    }

    protected override void OnHide()
    {
        base.OnHide();
        this.view.OnClickPlay -= OnClickPlayHandler;
    }

    private async void OnClickPlayHandler()
    {
        Messenger.Broadcast(Message.PlayLevel, 1);
        await GamePresenter.GetViewPresenter<TransitionViewPresenter>().Show();
        await Hide();
        await GamePresenter.GetViewPresenter<TransitionViewPresenter>().Hide();
    }
}