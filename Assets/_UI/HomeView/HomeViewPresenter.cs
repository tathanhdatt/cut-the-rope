using UnityEngine;

public class HomeViewPresenter : BaseViewPresenter
{
    private HomeView view;
    public HomeViewPresenter(GamePresenter presenter, Transform transform) : base(presenter, transform)
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
        await Hide();
        await Presenter.GetViewPresenter<LevelViewPresenter>().Show();
    }
}