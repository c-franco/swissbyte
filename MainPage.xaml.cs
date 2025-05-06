#if ANDROID
using Android.Views;
using Microsoft.Maui.Platform;
#endif

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
                //window.DecorView.SystemUiVisibility = (StatusBarVisibility)SystemUiFlags.LightStatusBar;
            }
            #endif
        }

        private async void Box1_Tapped(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new Box1Page());
        }
    }
}
