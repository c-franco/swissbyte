using swissbyte.Models;
using swissbyte.ViewModels;

namespace swissbyte.Pages.ToWatch;

public partial class ToWatch_Page : ContentPage
{
    private const string StorageKey = "WatchItems";

    ListViewModel viewModel = new(StorageKey);

    public ToWatch_Page()
    {
        InitializeComponent();

        BindingContext = viewModel;
    }

    private void OnAddClicked(object sender, EventArgs e)
    {
        viewModel.AddItem();
    }

    private void OnRemoveClicked(object sender, EventArgs e)
    {
        var item = (sender as Button)?.CommandParameter as ListItemModel;
        if (item != null)
            viewModel.RemoveItem(item);
    }

    private void OnEntryCompleted(object sender, EventArgs e)
    {
        viewModel.AddItem();
    }

    private void Entry_Loaded(object sender, EventArgs e)
    {
        var entry = (Entry)sender;
        if (entry.BindingContext is ListItemModel item && item.IsNew)
        {
            item.IsNew = false;
            Dispatcher.Dispatch(() => entry.Focus());
        }
    }
}