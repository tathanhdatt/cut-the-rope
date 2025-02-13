using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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

    public async UniTask Load(UniTask initManagerTask, UniTask initPresenterTask)
    {
        await this.view.Load(initManagerTask, initPresenterTask);
    }
}