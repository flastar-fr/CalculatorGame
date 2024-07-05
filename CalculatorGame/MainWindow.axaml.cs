using Avalonia.Controls;

namespace CalculatorGame;

public partial class MainWindow : Window
{
    private MenuWindow? _menuWindow;
    
    public MainWindow()
    {
        InitializeComponent();
        ShowMenu();
    }

    public void ShowGame(int amount,
        (bool, string, string) add,
        (bool, string, string) sub,
        (bool, string, string) mul,
        (bool, string, string) sqr)
    {
        _menuWindow = null;
        MainContent.Content = new GameWindow(this, amount, add, sub, mul, sqr);
    }

    public void ShowMenu()
    {
        _menuWindow = new MenuWindow(this);
        MainContent.Content = _menuWindow;
    }
    
    public void OnExit(object sender, WindowClosingEventArgs e)
    {
        _menuWindow?.SaveBoxDatas();
    }
}