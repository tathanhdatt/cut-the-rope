using Core.AudioService;
using Core.Service;
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
        ServiceLocator.GetService<IAudioService>().PlayMusic(AudioName.Menu);
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
        ServiceLocator.GetService<IAudioService>().PlaySfx(AudioName.Tap);
        await Hide();
        await Presenter.GetViewPresenter<LevelViewPresenter>().Show();
    }
}