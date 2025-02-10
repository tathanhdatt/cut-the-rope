using System;
using UnityEngine;

public class GameplayViewPresenter : BaseViewPresenter
{
    private GameplayView view;

    public GameplayViewPresenter(GamePresenter gamePresenter, Transform transform) : base(
        gamePresenter, transform)
    {
    }

    protected override void AddViews()
    {
        this.view = AddView<GameplayView>();
    }

    protected override void OnShow()
    {
        base.OnShow();
        this.view.OnClickBack += OnClickBackHandler;
    }

    protected override void OnHide()
    {
        base.OnHide();
        this.view.OnClickBack -= OnClickBackHandler;
    }

    private async void OnClickBackHandler()
    {
        await GamePresenter.GetViewPresenter<TransitionViewPresenter>().Show();
        Messenger.Broadcast(Message.ClearLevel);
        await Hide();
        await GamePresenter.GetViewPresenter<HomeViewPresenter>().Show();
        await GamePresenter.GetViewPresenter<TransitionViewPresenter>().Hide();
    }
}