using Cysharp.Threading.Tasks;
using UnityEngine;

public class LevelViewPresenter : BaseViewPresenter
{
    private LevelView view;
    private readonly LevelDatabase levelDatabase;

    private bool isOpenedLevel;

    public LevelViewPresenter(GamePresenter presenter, Transform transform,
        LevelDatabase levelDatabase) : base(presenter, transform)
    {
        this.levelDatabase = levelDatabase;
        this.levelDatabase.OnUpdateBox += OnUpdateBoxHandler;
    }

    protected override void AddViews()
    {
        this.view = AddView<LevelView>();
        LoadBoxes();
        this.levelDatabase.UpdateUnlockedBox();
        this.view.SetTotalCollectedStar(this.levelDatabase.GetTotalStars());
    }

    private void LoadBoxes()
    {
        for (int i = 0; i < this.levelDatabase.boxes.Count; i++)
        {
            Sprite sprite = Resources.Load<Sprite>($"BoxCovers/{i + 1}");
            int starToUnlock = this.levelDatabase.boxes[i].starToUnlocked;
            this.view.AddBox(i, sprite, starToUnlock);
            this.view.AddFooterDot();
        }
    }

    protected override void OnShow()
    {
        base.OnShow();
        this.view.OnSelectedBox += OnSelectedBoxHandler;
        this.view.OnClickBack += OnClickBackHandler;
        this.isOpenedLevel = false;
    }

    private void OnUpdateBoxHandler()
    {
        for (int i = 0; i < this.levelDatabase.boxes.Count; i++)
        {
            this.view.UpdateBox(i, this.levelDatabase.IsBoxUnlocked(i));
        }

        this.view.SetTotalCollectedStar(this.levelDatabase.GetTotalStars());
    }

    protected override void OnHide()
    {
        base.OnHide();
        this.view.OnSelectedBox -= OnSelectedBoxHandler;
        this.view.OnClickBack -= OnClickBackHandler;
    }


    private async void OnSelectedBoxHandler(int boxId)
    {
        if (this.levelDatabase.IsBoxUnlocked(boxId))
        {
            this.isOpenedLevel = true;
            await Presenter.GetViewPresenter<TransitionViewPresenter>().Show();
            ShowLevelInBox(boxId);
            this.view.SetActionBoxScrollRect(false);
            this.view.SetActiveFooter(false);
            this.view.SetActiveLevelContent(true);
            await Presenter.GetViewPresenter<TransitionViewPresenter>().Hide();
        }
        else
        {
            int requiredStar = this.levelDatabase.boxes[boxId].starToUnlocked
                               - this.levelDatabase.GetTotalStars();
            string content = $"You need more {requiredStar} <sprite=\"star\" index=1>";
            Messenger.Broadcast(Message.Popup, content);
        }
    }

    private void ShowLevelInBox(int boxId)
    {
        this.levelDatabase.currentBoxId = boxId;
        BoxDatabase boxDatabase = this.levelDatabase.boxes[boxId];
        for (int i = 0; i < boxDatabase.stars.Count; i++)
        {
            bool isLocked = i > boxDatabase.levelTopId;
            LevelUI level = this.view.ShowLevel(i, isLocked, boxDatabase.stars[i]);
            level.OnClickPlay -= OnClickPlayHandler;
            level.OnClickPlay += OnClickPlayHandler;
        }
    }

    private async void OnClickPlayHandler(int levelId)
    {
        await Presenter.GetViewPresenter<TransitionViewPresenter>().Show();
        await Hide();
        Messenger.Broadcast(Message.PlayLevel, levelId);
        await UniTask.WaitForSeconds(0.2f);
        await Presenter.GetViewPresenter<GameplayViewPresenter>().Show();
        await Presenter.GetViewPresenter<TransitionViewPresenter>().Hide();
    }

    private async void OnClickBackHandler()
    {
        if (this.isOpenedLevel)
        {
            await Presenter.GetViewPresenter<TransitionViewPresenter>().Show();
            this.view.HideAllLevels();
            this.view.SetActionBoxScrollRect(true);
            this.view.SetActiveFooter(true);
            this.isOpenedLevel = false;
            await Presenter.GetViewPresenter<TransitionViewPresenter>().Hide();
        }
        else
        {
            await Hide();
            await Presenter.GetViewPresenter<HomeViewPresenter>().Show();
        }
    }

    public async UniTask ScrollToBox(int boxId)
    {
        await this.view.ScrollToBox(boxId);
    }
}