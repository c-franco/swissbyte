#if ANDROID
using Android.Graphics.Drawables;
using Microsoft.Maui.Handlers;
#endif

using Microsoft.Maui.Controls;
using Microsoft.Maui.Platform;

namespace swissbyte.Helpers
{
    public static class HandlersSetup
    {
        private static bool _registered = false;

        public static void Register()
        {
            if (_registered) return;

            // Hide Entry underline
#if ANDROID
            EntryHandler.Mapper.AppendToMapping(nameof(Entry),
            (handler, view) =>
            {
                handler.PlatformView.BackgroundTintList =
                    Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToPlatform());
            });
#endif

            // Change cursor color
#if ANDROID
            var color = (Color)Application.Current.Resources["GrayText"];
            var androidColor = color.ToPlatform();
            EntryHandler.Mapper.AppendToMapping("CustomCursorColor", (handler, view) =>
            {
                handler.PlatformView.TextCursorDrawable?.SetTint(androidColor);
            });
            EntryHandler.Mapper.AppendToMapping("CustomHandles", (handler, view) =>
            {
                var context = handler.PlatformView.Context;

                var handleColor = Android.Graphics.Color.ParseColor("#E1E1E1");
                var handleDrawable = new ColorDrawable(handleColor);

                handler.PlatformView.TextSelectHandleLeft = handleDrawable;
                handler.PlatformView.TextSelectHandleRight = handleDrawable;
                handler.PlatformView.TextSelectHandle = handleDrawable;
            });
#endif

            // Change selected text color
#if ANDROID
            var colorSelectedText = (Color)Application.Current.Resources["SelectedText"];
            var androidTextColor = colorSelectedText.ToPlatform();
            EntryHandler.Mapper.AppendToMapping("CustomHighlight", (handler, view) =>
            {
                handler.PlatformView.SetHighlightColor(androidTextColor);
            });
#endif

            _registered = true;
        }
    }
}
