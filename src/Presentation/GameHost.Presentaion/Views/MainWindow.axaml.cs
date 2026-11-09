using Avalonia.Controls;
using Avalonia.Input;

namespace GameHost.Presentaion.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    //

    protected override void OnKeyDown(KeyEventArgs e)
    {
        gameSurface.KeyState.OnKeyDown(e);
        base.OnKeyDown(e);
    }

    protected override void OnKeyUp(KeyEventArgs e)
    {
        gameSurface.KeyState.OnKeyUp(e);
        base.OnKeyUp(e);
    }
}
