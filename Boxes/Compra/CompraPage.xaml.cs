using Microsoft.Maui.Platform;
#if ANDROID
using Android.Graphics.Drawables;
#endif

namespace swissbyte.Boxes.Compra;

public partial class CompraPage : ContentPage
{
    ChecklistViewModel viewModel = new();

    public CompraPage()
	{
		InitializeComponent();
        PageSetup();

        BindingContext = viewModel;
    }

    private void PageSetup()
    {
        // Hide Entry underline
#if ANDROID
        Microsoft.Maui.Handlers.EntryHandler.Mapper
            .AppendToMapping(nameof(Entry),
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
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("CustomCursorColor", (handler, view) =>
        {
            handler.PlatformView.TextCursorDrawable?.SetTint(androidColor);
        });
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("CustomHandles", (handler, view) =>
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
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("CustomHighlight", (handler, view) =>
        {
            handler.PlatformView.SetHighlightColor(androidTextColor);
        });
#endif
    }

    private void OnAddClicked(object sender, EventArgs e)
    {
        viewModel.AddItem();
    }

    private void OnRemoveClicked(object sender, EventArgs e)
    {
        var item = (sender as Button)?.CommandParameter as ChecklistItem;
        if (item != null)
            viewModel.RemoveItem(item);
    }
}