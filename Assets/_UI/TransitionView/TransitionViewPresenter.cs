using UnityEngine;

public class TransitionViewPresenter : BaseViewPresenter
{
    private TransitionView view;
    public TransitionViewPresenter(GamePresenter presenter, Transform transform) : base(presenter, transform)
    {
    }

    protected override void AddViews()
    {
        this.view = AddView<TransitionView>();
    }
}