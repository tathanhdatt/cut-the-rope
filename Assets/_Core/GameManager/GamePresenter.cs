using System;
using System.Collections.Generic;
using Core.Game;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class GamePresenter : MonoBehaviour
{
    private readonly Dictionary<Type, BaseViewPresenter> presenters =
        new Dictionary<Type, BaseViewPresenter>();

    private GameManager manager;

    public void Enter(GameManager gameManager)
    {
        this.manager = gameManager;
    }

    private void AddPresenters()
    {
        LoadingViewPresenter loadingViewPresenter = new LoadingViewPresenter(this, transform);
        AddPresenter(loadingViewPresenter);
        HomeViewPresenter homeViewPresenter = new HomeViewPresenter(this, transform);
        AddPresenter(homeViewPresenter);
        TransitionViewPresenter transitionViewPresenter =
            new TransitionViewPresenter(this, transform);
        AddPresenter(transitionViewPresenter);
        WinViewPresenter winViewPresenter = new WinViewPresenter(
            this, transform, this.manager.LevelDatabase);
        AddPresenter(winViewPresenter);
        LevelViewPresenter levelViewPresenter = new LevelViewPresenter(
            this, transform, this.manager.LevelDatabase);
        AddPresenter(levelViewPresenter);
        GameplayViewPresenter gameplayViewPresenter = new GameplayViewPresenter(
            this, transform);
        AddPresenter(gameplayViewPresenter);
    }

    public async UniTask InitialViewPresenters()
    {
        AddPresenters();
        foreach (BaseViewPresenter presenter in this.presenters.Values)
        {
            await presenter.Initialize();
        }
    }

    private void AddPresenter(BaseViewPresenter presenter)
    {
        this.presenters.Add(presenter.GetType(), presenter);
    }

    public T GetViewPresenter<T>() where T : BaseViewPresenter
    {
        Type type = typeof(T);
        return (T)this.presenters[type];
    }

    public async UniTask HideViewPresenters()
    {
        foreach (BaseViewPresenter viewPresenter in this.presenters.Values)
        {
            await viewPresenter.Hide();
        }
    }
}