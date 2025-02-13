public class BannerAutoLoadOnFailed : BannerDecorator
{
    public BannerAutoLoadOnFailed(IBannerAdapter adapter) : base(adapter)
    {
    }

    protected override void OnLoadFailedHandler()
    {
        base.OnLoadFailedHandler();
        Load();
    }
}