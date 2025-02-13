using Cysharp.Threading.Tasks;

public interface IAdsAdapter
{
    IBannerAdapter BannerAdapter { get; }
    IInterstitialAdapter InterstitialAdapter { get; }
    IMRecAdapter MRecAdapter { get; }
    IRewardVideoAdapter RewardVideoAdapter { get; }
    IAppOpenAdapter AppOpenAdapter { get; }
    
    UniTask Initialize();
    void SetBannerAdapter(IBannerAdapter bannerAdapter);
    void SetInterstitialAdapter(IInterstitialAdapter interstitialAdapter);
    void SetRewardVideoAdapter(IRewardVideoAdapter rewardVideoAdapter);
    void SetAppOpenAdapter(IAppOpenAdapter appOpenAdapter);
    void SetMRecAdapter(IMRecAdapter mRecAdapter);
}