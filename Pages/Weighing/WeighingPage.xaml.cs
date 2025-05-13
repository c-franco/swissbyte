using System.Text.Json;

namespace swissbyte.Pages.Weighing 
{
    public partial class WeighingPage : ContentPage
    {
        private List<WeightEntry> entries = new();

        public WeighingPage()
        {
            InitializeComponent();
            LoadData();
            //UpdateChart();
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
                //UpdateChart();
                WeightEntry.Text = string.Empty;
            }
        }

        private void RefreshList()
        {
            WeightList.ItemsSource = null;
            WeightList.ItemsSource = entries.OrderByDescending(e => DateTime.Parse(e.Date));
        }

        //private void UpdateChart()
        //{
        //    var chartEntries = entries.Select(e =>
        //        new ChartEntry((float)e.Weight)
        //        {
        //            Label = e.Date,
        //            ValueLabel = e.Weight.ToString(),
        //            Color = SKColor.Parse("#266489")
        //        }).ToList();

        //    WeightChart.Chart = new LineChart { Entries = chartEntries };
        //}

        private void SaveData()
        {
            var json = JsonSerializer.Serialize(entries);
            Preferences.Set("weight_entries", json);
        }

        private void LoadData()
        {
            var json = Preferences.Get("weight_entries", "");
            if (!string.IsNullOrEmpty(json))
            {
                entries = JsonSerializer.Deserialize<List<WeightEntry>>(json);
            }
            RefreshList();
        }
    }
}