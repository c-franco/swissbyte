using swissbyte.Models;
using System.Collections.ObjectModel;
using System.Text.Json;

namespace swissbyte.ViewModels
{
    public class ListViewModel
    {
        public ObservableCollection<ListItemModel> Items { get; } = new();

        private const string StorageKey = "ChecklistItems";

        public ListViewModel()
        {
            LoadItems();
        }

        public void AddItem()
        {
            var item = new ListItemModel { Text = "", IsChecked = false, IsNew = true }; 
            item.Changed += SaveItems;
            Items.Add(item);
            SaveItems();
        }

        public void RemoveItem(ListItemModel item)
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
                var items = JsonSerializer.Deserialize<List<ListItemModel>>(json);
                if (items != null)
                {
                    foreach (var raw in items)
                    {
                        var item = new ListItemModel
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