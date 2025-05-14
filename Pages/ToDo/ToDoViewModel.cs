using System.Collections.ObjectModel;
using System.Text.Json;

namespace swissbyte.Pages.ToDo
{
    public class ToDoViewModel
    {
        public ObservableCollection<ToDoItem> Items { get; } = new();

        private const string StorageKey = "ToDoItems";

        public ToDoViewModel()
        {
            LoadItems();
        }

        public void AddItem()
        {
            var item = new ToDoItem { Text = "", IsChecked = false, IsNew = true };
            item.Changed += SaveItems;
            Items.Add(item);
            SaveItems();
        }

        public void RemoveItem(ToDoItem item)
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
                var items = JsonSerializer.Deserialize<List<ToDoItem>>(json);
                if (items != null)
                {
                    foreach (var raw in items)
                    {
                        var item = new ToDoItem
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
