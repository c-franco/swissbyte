using System.Text.Json;

namespace swissbyte.Pages.Weighing 
{
    public partial class WeighingPage : ContentPage
    {
        private List<WeightEntry> entries = new();

        private const string StorageKey = "WeightEntries";

        public WeighingPage()
        {
            InitializeComponent();

            LoadData();
        }

        private void OnSaveClicked(object sender, EventArgs e)
        {
            if (double.TryParse(WeightEntry.Text, out double weight))
            {
                var newEntry = new WeightEntry
                {
                    Date = DateTime.Now.ToString("dd/MM/yyyy"),
                    Weight = weight
                };

                entries.Add(newEntry);

                SaveData();
                RefreshList();
                UpdateResumen();

                WeightEntry.Text = string.Empty;
            }
        }

        private void OnWeightEntryTextChanged(object sender, TextChangedEventArgs e)
        {
            var entry = (Entry)sender;
            string text = entry.Text;

            if (string.IsNullOrWhiteSpace(text)) return;

            if (double.TryParse(text, out double value))
            {
                int index = text.IndexOf(',');
                if (index >= 0 && text.Length - index - 1 > 2)
                {
                    entry.Text = value.ToString("F2");
                }
            }
            else
            {
                entry.Text = e.OldTextValue;
            }
        }

        private void LoadData()
        {
            var json = Preferences.Get(StorageKey, string.Empty);

            if (!string.IsNullOrEmpty(json))
            {
                entries = JsonSerializer.Deserialize<List<WeightEntry>>(json);
            }

            RefreshList();
            UpdateResumen();
        }

        private void SaveData()
        {
            var json = JsonSerializer.Serialize(entries);
            Preferences.Set(StorageKey, json);
        }

        private void RefreshList()
        {
            WeightList.ItemsSource = null;
            WeightList.ItemsSource = entries.AsEnumerable().Reverse().ToList();
        }

        private void UpdateResumen()
        {
            if (entries.Count < 1)
            {
                InitialWeightLabel.Text = "N/D";
                ActualWeightLabel.Text = "N/D";
                LowestWeightLabel.Text = "N/D";
                HighestWeightLabel.Text = "N/D";
                WeeklyTrendLabel.Text = "N/D";
                MonthlyTrendLabel.Text = "N/D";

                return;
            }

            var ordered = entries.AsEnumerable().Reverse().ToList();

            InitialWeightLabel.Text = $"{ordered.First().Weight} kg";
            ActualWeightLabel.Text = $"{ordered.Last().Weight} kg";
            LowestWeightLabel.Text = $"{ordered.Min(e => e.Weight)} kg";
            HighestWeightLabel.Text = $"{ordered.Max(e => e.Weight)} kg";

            DateTime past7 = DateTime.Now.AddDays(-7);
            var week = entries.Where(e => DateTime.Parse(e.Date) >= past7).ToList();

            if (week.Count >= 2)
            {
                double delta = week.Last().Weight - week.First().Weight;
                WeeklyTrendLabel.Text = $"{delta:+0.0;-0.0;0.0} kg {(delta > 0 ? " 📈" : delta < 0 ? " 📉" : " ➖")}";
                WeeklyTrendLabel.TextColor = delta switch
                {
                    > 0 => Colors.Red,
                    < 0 => Colors.Green,
                    _ => Colors.Gray
                };
            }

            var past30 = DateTime.Now.AddDays(-30);
            var month = entries.Where(e => DateTime.Parse(e.Date) >= past30).ToList();

            if (month.Count >= 2)
            {
                double delta = month.Last().Weight - month.First().Weight;
                MonthlyTrendLabel.Text = $"{delta:+0.0;-0.0;0.0} kg {(delta > 0 ? " 📈" : delta < 0 ? " 📉" : " ➖")}";
                MonthlyTrendLabel.TextColor = delta switch
                {
                    > 0 => Colors.Red,
                    < 0 => Colors.Green,
                    _ => Colors.Gray
                };
            }
        }
    }
}