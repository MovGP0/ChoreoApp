namespace ChoreoApp;

public partial class MainPage
{
	public MainPage()
	{
		InitializeComponent();
        ViewModel = new MainViewModel();
        this.WhenActivated(d =>
        {
            // Bindings and other activation logic can go here
        });
	}
}
