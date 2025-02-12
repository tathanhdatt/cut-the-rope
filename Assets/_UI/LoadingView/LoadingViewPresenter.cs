using UnityEngine;

public class LoadingViewPresenter : BaseViewPresenter
{
    private LoadingView view;

    public LoadingViewPresenter(GamePresenter presenter, Transform transform) : base(
        presenter, transform)
    {
    }

    protected override void AddViews()
    {
        this.view = AddView<LoadingView>();
    }
}