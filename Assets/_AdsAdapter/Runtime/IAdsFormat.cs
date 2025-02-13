using System;

public interface IAdsFormat
{
    event Action OnLoadSucceeded;
    event Action OnLoadFailed;
    event Action OnClosed;
    event Action OnOpened;
    event Action OnClicked;
    void Load();
    void Show();
    void Hide();
}