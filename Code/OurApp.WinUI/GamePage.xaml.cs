using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using OurApp.Core.Services;
using OurApp.Core.ViewModels;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace OurApp.WinUI
{
    public sealed partial class GamePage : Page
    {
        public GameViewModel ViewModel { get; private set; } = null!;

        public Frame GameFrame => gameFrame;

        public GameService gameService;

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);
            gameService = (GameService)e.Parameter;
            ViewModel = new GameViewModel(gameService, () => Microsoft.UI.Xaml.Application.Current.Exit());
            DataContext = ViewModel;
        }

        public GamePage()
        {
            this.InitializeComponent();
        }
    }
}
