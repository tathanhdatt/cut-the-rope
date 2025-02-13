public class InterstitialAutoLoadOnFailed : InterstitialDecorator
{
    public InterstitialAutoLoadOnFailed(IInterstitialAdapter adapter) : base(adapter)
    {
    }

    protected override void OnLoadFailedHandler()
    {
        base.OnLoadFailedHandler();
        Load();
    }
}