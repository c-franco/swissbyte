using System.Collections.ObjectModel;
using System.ComponentModel;

namespace swissbyte.Boxes.Compra
{
    public class ChecklistViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<ChecklistItem> Items { get; set; } = new();

        public ChecklistViewModel()
        {
        }

        public void AddItem() => Items.Add(new ChecklistItem { Text = "" });
        public void RemoveItem(ChecklistItem item) => Items.Remove(item);

        public event PropertyChangedEventHandler PropertyChanged;
    }

}
