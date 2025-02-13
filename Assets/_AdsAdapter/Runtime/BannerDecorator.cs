using System;

public abstract class BannerDecorator : IBannerAdapter
{
    private readonly IBannerAdapter adapter;
    public event Action OnLoadSucceeded;
    public event Action OnLoadFailed;
    public event Action OnClosed;
    public event Action OnOpened;
    public event Action OnClicked;

    protected BannerDecorator(IBannerAdapter adapter)
    {
        this.adapter = adapter;
        this.adapter.OnLoadSucceeded += OnLoadSucceededHandler;
        this.adapter.OnLoadFailed += OnLoadFailedHandler;
        this.adapter.OnClosed += OnClosedHandler;
        this.adapter.OnOpened += OnOpenedHandler;
        this.adapter.OnClicked += OnClickedHandler;
    }


    protected virtual void OnLoadSucceededHandler()
    {
        OnLoadSucceeded?.Invoke();
    }

    protected virtual void OnLoadFailedHandler()
    {
        OnLoadFailed?.Invoke();
    }

    protected virtual void OnClosedHandler()
    {
        OnClosed?.Invoke();
    }

    protected virtual void OnOpenedHandler()
    {
        OnOpened?.Invoke();
    }

    protected virtual void OnClickedHandler()
    {
        OnClicked?.Invoke();
    }

    public void Load()
    {
        this.adapter.Load();
    }

    public void Show()
    {
        this.adapter.Show();
    }

    public void Hide()
    {
        this.adapter.Hide();
    }
}