using System;
using Cysharp.Threading.Tasks;
using GoogleMobileAds.Api;
using UnityEngine;

public class BannerAdmobAdapter : IBannerAdapter
{
    private BannerView bannerView;
    private readonly string adUnitId;
    private readonly AdPosition adPosition;
    public event Action OnLoadSucceeded;
    public event Action OnLoadFailed;
    public event Action OnClosed;
    public event Action OnOpened;
    public event Action OnClicked;

    public BannerAdmobAdapter(string adUnitId, AdPosition position)
    {
        this.adUnitId = adUnitId;
        this.adPosition = position;
    }

    private void CreateBannerView()
    {
        if (this.bannerView != null)
        {
            this.bannerView.OnBannerAdLoaded -= OnLoadedHandler;
            this.bannerView.OnBannerAdLoadFailed -= OnLoadFailedHandler;
            this.bannerView.OnAdClicked -= OnClickedHandler;
            this.bannerView.Destroy();
        }

        this.bannerView = new BannerView(this.adUnitId, AdSize.Banner, this.adPosition);
        this.bannerView.OnBannerAdLoaded += OnLoadedHandler;
        this.bannerView.OnBannerAdLoadFailed += OnLoadFailedHandler;
        this.bannerView.OnAdClicked += OnClickedHandler;
    }

    private void OnLoadedHandler()
    {
        OnLoadSucceeded?.Invoke();
        this.bannerView?.Hide();
    }

    private void OnLoadFailedHandler(LoadAdError error)
    {
        OnLoadFailed?.Invoke();
    }

    private void OnClickedHandler()
    {
        OnClicked?.Invoke();
    }

    public void Load()
    {
        CreateBannerView();
        this.bannerView.LoadAd(new AdRequest());
    }

    public void Show()
    {
        OnOpened?.Invoke();
        this.bannerView?.Show();
    }

    public void Hide()
    {
        this.bannerView?.Hide();
        OnClosed?.Invoke();
    }
}