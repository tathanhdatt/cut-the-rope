﻿using UnityEngine;

public class WinViewPresenter : BaseViewPresenter
{
    private WinView view;
    private LevelDatabase levelDatabase;

    public WinViewPresenter(GamePresenter gamePresenter,
        Transform transform, LevelDatabase levelDatabase) : base(gamePresenter, transform)
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
            await GamePresenter.GetViewPresenter<LevelViewPresenter>().Show();
            await GamePresenter.GetViewPresenter<TransitionViewPresenter>().Hide();
            await GamePresenter.GetViewPresenter<LevelViewPresenter>()
                .ScrollToBox(this.levelDatabase.currentBoxId);
        }
        else
        {
            Messenger.Broadcast(Message.PlayNextLevel);
            await Hide();
            await GamePresenter.GetViewPresenter<TransitionViewPresenter>().Hide();
        }
    }

    private void OnClickReplayHandler()
    {
        Messenger.Broadcast(Message.Replay);
    }

    protected override void OnHide()
    {
        base.OnHide();
        this.view.OnClickNext -= OnClickNextHandler;
        this.view.OnClickReplay -= OnClickReplayHandler;
    }
}