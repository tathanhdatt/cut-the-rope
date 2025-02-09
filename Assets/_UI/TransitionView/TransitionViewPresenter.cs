using UnityEngine;

public class TransitionViewPresenter : BaseViewPresenter
{
    private TransitionView view;
    public TransitionViewPresenter(GamePresenter gamePresenter, Transform transform) : base(gamePresenter, transform)
    {
    }

    protected override void AddViews()
    {
        this.view = AddView<TransitionView>();
    }
}