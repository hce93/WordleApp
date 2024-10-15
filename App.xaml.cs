namespace WordleApp;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();
        
        // intialize main page straight away as not using a shell structure
		MainPage = new MainPage();
	}
	
	protected override Window CreateWindow(IActivationState? activationState)
    {
        var window = base.CreateWindow(activationState);

        const int minWidth = 850;
        const int minHeight = 850;
        
        window.MinimumWidth = minWidth;
        window.MinimumHeight = minHeight;

        return window;
    }
}
