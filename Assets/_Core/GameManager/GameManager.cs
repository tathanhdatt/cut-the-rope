﻿using System.Collections.Generic;
using System.Text;
using Core.AudioService;
using Core.Service;
using Cysharp.Threading.Tasks;
using Dt.Attribute;
using Dt.Extension;
using GoogleMobileAds.Api;
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
        public IAdsAdapter AdsAdapter { get; private set; }
        public IAudioService AudioService { get; private set; }

        private void Awake()
        {
            // Initialize();
        }

        private async UniTask Initialize()
        {
            Application.targetFrameRate = 60;
            InitLevelDatabase();
            InitAudioService();
            await InitAdsAdapter();
        }

        private void InitAudioService()
        {
            AudioService = FindAnyObjectByType<NativeAudioService>();
            ServiceLocator.Register(AudioService);
        }

        private void InitLevelDatabase()
        {
            string data = PlayerPrefs.GetString(PlayerPrefsId.LevelDatabase, string.Empty);
            if (data.IsNullOrEmpty())
            {
                string defaultData =
                    "Box_1,0\nLevel_1,\nLevel_2,\nLevel_3,\nLevel_4,\nLevel_5,\nLevel_6,\nLevel_7,\nBox_2,6\nLevel_1,\nLevel_2,\nLevel_3,\nLevel_4,";
                LevelDatabase = new LevelDatabase(defaultData);
            }
            else
            {
                LevelDatabase = JsonUtility.FromJson<LevelDatabase>(data);
            }
        }

        private async UniTask InitAdsAdapter()
        {
            AdsAdapter = new AdmobAdapter();
            await AdsAdapter.Initialize();

            AdsAdapter.SetBannerAdapter(
                new BannerAutoLoadOnFailed(
                    new BannerAdmobAdapter("ca-app-pub-3940256099942544/6300978111",
                        AdPosition.Bottom)));
            AdsAdapter.BannerAdapter.Load();

            AdsAdapter.SetInterstitialAdapter(
                new InterstitialAutoLoadOnFailed(
                    new InterstitialAdmobAdapter("ca-app-pub-3940256099942544/1033173712")
                )
            );
            AdsAdapter.InterstitialAdapter.Load();
        }


        private async void Start()
        {
            this.presenter.Enter(this);
            LoadingViewPresenter loadingViewPresenter = 
                new LoadingViewPresenter(this.presenter, this.presenter.transform);
            this.presenter.AddPresenter(loadingViewPresenter);
            await OnEnter();
        }

        private async UniTask OnEnter()
        {
            await this.presenter.GetViewPresenter<LoadingViewPresenter>().Show();
            await this.presenter.GetViewPresenter<LoadingViewPresenter>()
                .Load(Initialize(), this.presenter.InitialViewPresenters());
            Messenger.AddListener(Message.LevelWin, LevelWinHandler);
            Messenger.AddListener(Message.LevelLose, LevelLoseHandler);
            Messenger.AddListener(Message.CollectStar, CollectStarHandler);
            Messenger.AddListener<int>(Message.PlayLevel, PlayLevel);
            Messenger.AddListener(Message.PlayNextLevel, PlayNextLevel);
            Messenger.AddListener(Message.Replay, ReplayHandler);
            Messenger.AddListener(Message.ClearLevel, ClearLevelHandler);
            Messenger.AddListener<string>(Message.Popup, PopupHandler);
            await this.presenter.GetViewPresenter<HomeViewPresenter>().Show();
            await this.presenter.GetViewPresenter<LoadingViewPresenter>().Hide();
        }

        private void ClearLevelHandler()
        {
            ClearLevel();
        }

        private void ReplayHandler()
        {
            PlayLevel(LevelDatabase.GetCurrentBox().currentLevelId);
        }

        private void CollectStarHandler()
        {
            this.collectedStar++;
            AudioService.PlaySfx($"Star{this.collectedStar}");
            int lastStar = LevelDatabase.GetCurrentBox().GetCurrentLevelStar();
            if (this.collectedStar > lastStar)
            {
                LevelDatabase.GetCurrentBox()
                    .SetCurrentLevelStar(this.collectedStar);
            }
        }

        private async void LevelLoseHandler()
        {
            await this.presenter.GetViewPresenter<TransitionViewPresenter>().Show();
            PlayLevel(LevelDatabase.GetCurrentBox().currentLevelId);
            await this.presenter.GetViewPresenter<TransitionViewPresenter>().Hide();
        }

        private async void PlayLevel(int levelId)
        {
            this.collectedStar = 0;
            if (levelId >= LevelDatabase.GetCurrentBox().stars.Count)
            {
                LevelDatabase.currentBoxId += 1;
                LevelDatabase.currentBoxId %= LevelDatabase.boxes.Count;
                levelId = 0;
            }

            BoxDatabase currentBox = LevelDatabase.GetCurrentBox();
            currentBox.currentLevelId = levelId;
            ClearLevel();

            if (currentBox.currentLevelId > currentBox.levelTopId)
            {
                currentBox.levelTopId = currentBox.currentLevelId;
            }

            string key =
                $"Assets/_Level/Box_{LevelDatabase.currentBoxId + 1}/Level_{levelId + 1:D3}.prefab";
            GameObject prefab = await Addressables.LoadAssetAsync<GameObject>(key);
            this.levelPlayer = Instantiate(prefab).GetComponent<LevelPlayer>();
            await this.levelPlayer.Initialize();
            this.levelPlayer.Play();
        }

        private void ClearLevel()
        {
            if (this.levelPlayer != null)
            {
                Destroy(this.levelPlayer.gameObject);
            }
        }

        private async void LevelWinHandler()
        {
            LevelDatabase.UpdateUnlockedBox();
            await this.presenter.GetViewPresenter<TransitionViewPresenter>().Show();
            await this.presenter.GetViewPresenter<GameplayViewPresenter>().Hide();
            await this.presenter.GetViewPresenter<WinViewPresenter>().Show();
            UpdateLevelTop();
        }

        private void UpdateLevelTop()
        {
            LevelDatabase.GetCurrentBox().UpdateLevelTop();
        }

        private void PlayNextLevel()
        {
            PlayLevel(LevelDatabase.GetCurrentBox().currentLevelId + 1);
        }

        private void PopupHandler(string content)
        {
            if (!this.dialogManager.TryGetDialog(out PopupDialog dialog)) return;
            dialog.SetContent(content);
            dialog.Show();
            dialog.OnHide += OnPopupDialogHideHandler;
        }

        private void OnPopupDialogHideHandler(Dialog dialog)
        {
            dialog.OnHide -= OnPopupDialogHideHandler;
            this.dialogManager.AddDialog(dialog as PopupDialog);
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