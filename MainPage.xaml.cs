#if ANDROID
using Microsoft.Maui.Platform;
#endif
using swissbyte.Pages.Compra;
using swissbyte.Pages.FreeGames;
using swissbyte.Pages.Plans;
using swissbyte.Pages.SystemInfo;
using swissbyte.Pages.ToDo;
using swissbyte.Pages.ToWatch;
using swissbyte.Pages.Weighing;

namespace swissbyte
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            #if ANDROID
            var window = Platform.CurrentActivity?.Window;
            if (window != null)
            {
                var color = (Color)Application.Current.Resources["MainGray"];
                var androidColor = color.ToPlatform();
                window.SetStatusBarColor(androidColor);    
            }
            #endif
        }

        private async void Compra_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new CompraPage());
        }

        private async void FreeGames_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new FreeGamesPage());
        }

        private async void SystemInfo_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new SystemInfoPage());
        }
        
        private async void Weighing_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new WeighingPage());
        }

        private async void ToDo_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ToDoPage());
        }

        private async void ToWatch_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new ToWatch_Page());
        }

        private async void Plans_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new Plans_Page());
        }
    }
}
