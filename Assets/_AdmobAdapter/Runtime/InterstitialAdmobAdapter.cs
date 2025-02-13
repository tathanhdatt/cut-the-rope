using System;
using GoogleMobileAds.Api;

public class InterstitialAdmobAdapter : IInterstitialAdapter
{
    private readonly string adUnitId;
    private InterstitialAd ad;

    public event Action OnLoadSucceeded;
    public event Action OnLoadFailed;
    public event Action OnClosed;
    public event Action OnOpened;
    public event Action OnClicked;

    public InterstitialAdmobAdapter(string adUnitId)
    {
        this.adUnitId = adUnitId;
    }

    public void Load()
    {
        InterstitialAd.Load(this.adUnitId, new AdRequest(), OnLoadFinishedHandler);
    }

    private void OnLoadFinishedHandler(InterstitialAd ad, LoadAdError error)
    {
        if (error != null || ad == null)
        {
            OnLoadFailed?.Invoke();
        }
        else
        {
            this.ad = ad;
            this.ad.OnAdFullScreenContentClosed += OnClosedAdsHandler;
            this.ad.OnAdClicked += OnAdClickedHandler;
            OnLoadSucceeded?.Invoke();
        }
    }

    private void OnAdClickedHandler()
    {
        OnClicked?.Invoke();
    }

    private void OnClosedAdsHandler()
    {
        OnClosed?.Invoke();
        Load();
    }

    public void Show()
    {
        if (this.ad != null && this.ad.CanShowAd())
        {
            this.ad.Show();
            OnOpened?.Invoke();
        }
    }

    public void Hide()
    {
    }
}