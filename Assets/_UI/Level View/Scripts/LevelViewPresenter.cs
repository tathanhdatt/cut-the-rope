using Cysharp.Threading.Tasks;
using UnityEngine;

public class LevelViewPresenter : BaseViewPresenter
{
    private LevelView view;
    private readonly LevelDatabase levelDatabase;

    private bool isOpenedLevel = false;

    public LevelViewPresenter(GamePresenter gamePresenter,
        Transform transform, LevelDatabase levelDatabase) : base(gamePresenter, transform)
    {
        this.levelDatabase = levelDatabase;
    }

    protected override void AddViews()
    {
        this.view = AddView<LevelView>();
        LoadBoxes();
    }

    private void LoadBoxes()
    {
        for (int i = 0; i < this.levelDatabase.boxes.Count; i++)
        {
            Sprite sprite = Resources.Load<Sprite>($"BoxCovers/{i + 1}");
            this.view.AddBox(i, sprite);
        }
    }

    protected override void OnShow()
    {
        base.OnShow();
        this.view.OnSelectedBox += OnSelectedBoxHandler;
        this.view.OnClickBack += OnClickBackHandler;
        this.isOpenedLevel = false;
    }

    protected override void OnHide()
    {
        base.OnHide();
        this.view.OnSelectedBox -= OnSelectedBoxHandler;
        this.view.OnClickBack -= OnClickBackHandler;
    }


    private async void OnSelectedBoxHandler(int boxId)
    {
        this.isOpenedLevel = true;
        await GamePresenter.GetViewPresenter<TransitionViewPresenter>().Show();
        this.levelDatabase.currentBoxId = boxId;
        this.view.SetActionBoxScrollRect(false);
        BoxDatabase boxDatabase = this.levelDatabase.boxes[boxId];

        for (int i = 0; i < boxDatabase.stars.Count; i++)
        {
            bool isLocked = i > boxDatabase.levelTopId;
            LevelUI level = this.view.ShowLevel(i, isLocked, boxDatabase.stars[i]);
            level.OnClickPlay -= OnClickPlayHandler;
            level.OnClickPlay += OnClickPlayHandler;
        }

        this.view.SetActiveLevelContent(true);
        await GamePresenter.GetViewPresenter<TransitionViewPresenter>().Hide();
    }

    private async void OnClickPlayHandler(int levelId)
    {
        await GamePresenter.GetViewPresenter<TransitionViewPresenter>().Show();
        await Hide();
        Messenger.Broadcast(Message.PlayLevel, levelId);
        await UniTask.WaitForSeconds(0.2f);
        await GamePresenter.GetViewPresenter<GameplayViewPresenter>().Show();
        await GamePresenter.GetViewPresenter<TransitionViewPresenter>().Hide();
    }

    private async void OnClickBackHandler()
    {
        if (this.isOpenedLevel)
        {
            await GamePresenter.GetViewPresenter<TransitionViewPresenter>().Show();
            this.view.HideAllLevels();
            this.view.SetActionBoxScrollRect(true);
            this.isOpenedLevel = false;
            await GamePresenter.GetViewPresenter<TransitionViewPresenter>().Hide();
        }
        else
        {
            await Hide();
            await GamePresenter.GetViewPresenter<HomeViewPresenter>().Show();
        }
    }

    public async UniTask ScrollToBox(int boxId)
    {
        await this.view.ScrollToBox(boxId);
    }
}