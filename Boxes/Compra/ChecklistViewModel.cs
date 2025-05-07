using System.Collections.ObjectModel;
using System.Text.Json;

namespace swissbyte.Boxes.Compra
{
    public class ChecklistViewModel
    {
        public ObservableCollection<ChecklistItem> Items { get; } = new();

        private const string StorageKey = "ChecklistItems";

        public ChecklistViewModel()
        {
            LoadItems();
        }

        public void AddItem()
        {
            var item = new ChecklistItem { Text = "", IsChecked = false, IsNew = true }; 
            item.Changed += SaveItems;
            Items.Add(item);
            SaveItems();
        }

        public void RemoveItem(ChecklistItem item)
        {
            if (Items.Contains(item))
            {
                item.Changed -= SaveItems;
                Items.Remove(item);
                SaveItems();
            }
        }

        private void SaveItems()
        {
            var list = Items.ToList();
            var json = JsonSerializer.Serialize(list);
            Preferences.Set(StorageKey, json);
        }

        private void LoadItems()
        {
            var json = Preferences.Get(StorageKey, string.Empty);
            if (!string.IsNullOrWhiteSpace(json))
            {
                var items = JsonSerializer.Deserialize<List<ChecklistItem>>(json);
                if (items != null)
                {
                    foreach (var raw in items)
                    {
                        var item = new ChecklistItem
                        {
                            Text = raw.Text,
                            IsChecked = raw.IsChecked
                        };
                        item.Changed += SaveItems;
                        Items.Add(item);
                    }
                }
            }
        }
    }
}
