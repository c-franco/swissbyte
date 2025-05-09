#if ANDROID
using Android.Views;
using Microsoft.Maui.Platform;
#endif

using swissbyte.Pages.Compra;
using swissbyte.Pages.FreeGames;

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
    }
}
