namespace swissbyte.Boxes.Compra;

public partial class CompraPage : ContentPage
{
    ChecklistViewModel viewModel = new();

    public CompraPage()
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
        var item = (sender as Button)?.CommandParameter as ChecklistItem;
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
        if (entry.BindingContext is ChecklistItem item && item.IsNew)
        {
            item.IsNew = false;
            Dispatcher.Dispatch(() => entry.Focus());
        }
    }
}