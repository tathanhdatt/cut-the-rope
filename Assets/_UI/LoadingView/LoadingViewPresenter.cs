using UnityEngine;

public class LoadingViewPresenter : BaseViewPresenter
{
    private LoadingView view;

    public LoadingViewPresenter(GamePresenter gamePresenter, Transform transform) : base(
        gamePresenter, transform)
    {
    }

    protected override void AddViews()
    {
        this.view = AddView<LoadingView>();
    }
}