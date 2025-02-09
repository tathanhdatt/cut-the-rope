using System.Text;
using Cysharp.Threading.Tasks;
using Dt.Attribute;
using Dt.Extension;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.Game
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField, Required]
        private GamePresenter presenter;

        [Line]
        [SerializeField, Required]
        private DialogManager dialogManager;

        [SerializeField, ReadOnly]
        private LevelPlayer levelPlayer;


        [SerializeField, ReadOnly]
        private int collectedStar;

        public LevelDatabase LevelDatabase { get; private set; }

        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            Application.targetFrameRate = 60;
            InitLevelDatabase();
        }

        private void InitLevelDatabase()
        {
            string data = PlayerPrefs.GetString(PlayerPrefsId.LevelDatabase, string.Empty);
            if (data.IsNullOrEmpty())
            {
                LevelDatabase = new LevelDatabase();
                LevelDatabase.boxes.Add(new BoxDatabase());
                LevelDatabase.AddNewLevels();
            }
            else
            {
                LevelDatabase = JsonUtility.FromJson<LevelDatabase>(data);
            }
        }

        private async void Start()
        {
            this.presenter.Enter(this);
            await this.presenter.InitialViewPresenters();
            await OnEnter();
        }

        private async UniTask OnEnter()
        {
            Messenger.AddListener(Message.LevelWin, LevelWinHandler);
            Messenger.AddListener(Message.LevelLose, LevelLoseHandler);
            Messenger.AddListener(Message.CollectStar, CollectStarHandler);
            Messenger.AddListener<int>(Message.PlayLevel, PlayLevel);
            Messenger.AddListener(Message.PlayNextLevel, PlayNextLevel);
            Messenger.AddListener(Message.Replay, ReplayHandler);
            await this.presenter.GetViewPresenter<LoadingViewPresenter>().Show();
            await this.presenter.GetViewPresenter<HomeViewPresenter>().Show();
            await this.presenter.GetViewPresenter<LoadingViewPresenter>().Hide();
        }

        private async void ReplayHandler()
        {
            await this.presenter.GetViewPresenter<WinViewPresenter>().Hide();
            PlayLevel(LevelDatabase.currentLevel);
            await this.presenter.GetViewPresenter<TransitionViewPresenter>().Hide();
        }

        private void CollectStarHandler()
        {
            this.collectedStar++;
            int lastStar = LevelDatabase.GetCurrentLevelStar();
            if (this.collectedStar > lastStar)
            {
                LevelDatabase.SetCurrentLevelStar(this.collectedStar);
            }
        }

        private async void LevelLoseHandler()
        {
            await this.presenter.GetViewPresenter<TransitionViewPresenter>().Show();
            PlayLevel(LevelDatabase.currentLevel);
            await this.presenter.GetViewPresenter<TransitionViewPresenter>().Hide();
        }

        private async void PlayLevel(int level)
        {
            this.collectedStar = 0;
            LevelDatabase.currentLevel = level;
            if (this.levelPlayer != null)
            {
                Destroy(this.levelPlayer.gameObject);
            }

            if (LevelDatabase.currentLevel > LevelDatabase.levelTop)
            {
                LevelDatabase.levelTop = LevelDatabase.currentLevel;
                LevelDatabase.AddNewLevels();
            }

            GameObject prefab = await Addressables.LoadAssetAsync<GameObject>($"Level_{level:D3}");
            this.levelPlayer = Instantiate(prefab).GetComponent<LevelPlayer>();
            await this.levelPlayer.Initialize();
            this.levelPlayer.Play();
        }

        private async void LevelWinHandler()
        {
            await this.presenter.GetViewPresenter<TransitionViewPresenter>().Show();
            await this.presenter.GetViewPresenter<WinViewPresenter>().Show();
            UpdateLevelTop();
        }

        private void UpdateLevelTop()
        {
            LevelDatabase.levelTop += 1;
            LevelDatabase.AddNewLevels();
        }

        private void PlayNextLevel()
        {
            PlayLevel(LevelDatabase.currentLevel + 1);
        }

#if UNITY_EDITOR
        private void OnApplicationQuit()
        {
            SaveData();
        }
#else
        private void OnApplicationPause(bool pauseStatus)
        {
            SaveData();
        }
#endif
        private void SaveData()
        {
            SaveLevels();
            PlayerPrefs.Save();
        }

        private void SaveLevels()
        {
            string data = JsonUtility.ToJson(LevelDatabase);
            PlayerPrefs.SetString(PlayerPrefsId.LevelDatabase, data);

#if UNITY_EDITOR
            System.IO.File.WriteAllBytes(
                Application.persistentDataPath + "/" + "a.json",
                Encoding.UTF8.GetBytes(data)
            );
#endif
        }
    }
}