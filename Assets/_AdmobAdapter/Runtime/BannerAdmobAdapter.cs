using System;
using GoogleMobileAds.Api;

public class BannerAdmobAdapter : IBannerAdapter
{
    private readonly BannerView bannerView;
    public event Action OnLoadSucceeded;
    public event Action OnLoadFailed;
    public event Action OnClosed;
    public event Action OnOpened;
    public event Action OnClicked;

    public BannerAdmobAdapter(string adUnitId, AdPosition position)
    {
        this.bannerView = new BannerView(adUnitId, AdSize.Banner, position);
        this.bannerView.OnBannerAdLoaded += OnLoadedHandler;
        this.bannerView.OnBannerAdLoadFailed += OnLoadFailedHandler;
        this.bannerView.OnAdClicked += OnClickedHandler;
    }

    private void OnLoadedHandler()
    {
        OnLoadSucceeded?.Invoke();
        this.bannerView.Hide();
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
        this.bannerView.LoadAd(new AdRequest());
    }

    public void Show()
    {
        this.bannerView.Show();
        OnOpened?.Invoke();
    }

    public void Hide()
    {
        this.bannerView.Hide();
        OnClosed?.Invoke();
    }
}