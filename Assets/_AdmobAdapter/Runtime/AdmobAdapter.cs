using Cysharp.Threading.Tasks;
using GoogleMobileAds.Api;

public class AdmobAdapter : IAdsAdapter
{
    public IBannerAdapter BannerAdapter { get; private set; }
    public IInterstitialAdapter InterstitialAdapter { get; private set; }
    public IMRecAdapter MRecAdapter { get; private set; }
    public IRewardVideoAdapter RewardVideoAdapter { get; private set; }
    public IAppOpenAdapter AppOpenAdapter { get; private set; }
    
    private bool isInitialized;

    public AdmobAdapter()
    {
    }

    public async UniTask Initialize()
    {
        this.isInitialized = false;
        MobileAds.Initialize(InitCompleteAction);
        await UniTask.WaitUntil(() => this.isInitialized);
    }

    public void SetBannerAdapter(IBannerAdapter bannerAdapter)
    {
        BannerAdapter = bannerAdapter;
    }

    public void SetInterstitialAdapter(IInterstitialAdapter interstitialAdapter)
    {
        InterstitialAdapter = interstitialAdapter;
    }

    public void SetRewardVideoAdapter(IRewardVideoAdapter rewardVideoAdapter)
    {
        RewardVideoAdapter = rewardVideoAdapter;
    }

    public void SetAppOpenAdapter(IAppOpenAdapter appOpenAdapter)
    {
        AppOpenAdapter = appOpenAdapter;
    }

    public void SetMRecAdapter(IMRecAdapter mRecAdapter)
    {
        MRecAdapter = mRecAdapter;
    }

    private void InitCompleteAction(InitializationStatus status)
    {
        this.isInitialized = true;
    }
}