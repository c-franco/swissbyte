using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace swissbyte.Pages.FreeGames;

public partial class FreeGamesPage : ContentPage, INotifyPropertyChanged
{
    public ObservableCollection<GameInfo> Games { get; set; } = new();
    public ICommand OpenUrlCommand { get; }

    bool isLoading;
    public bool IsLoading
    {
        get => isLoading;
        set
        {
            if (isLoading != value)
            {
                isLoading = value;
                OnPropertyChanged();
            }
        }
    }

    public bool HasResults => Games?.Any() == true;

    public FreeGamesPage()
	{
		InitializeComponent();

        BindingContext = this;

        LoadFreeGames();

        OpenUrlCommand = new Command<string>(async (url) =>
        {
            if (!string.IsNullOrWhiteSpace(url))
                await Launcher.Default.OpenAsync(url);
        });

    }

    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private async void LoadFreeGames()
    {
        IsLoading = true;
        OnPropertyChanged(nameof(IsLoading));

        Stopwatch sw = Stopwatch.StartNew();

        var freeGames = await GetFreeGames();

        sw.Stop();
        var test = sw.ElapsedMilliseconds;

        Games.Clear();

        foreach (var game in freeGames)
            Games.Add(game);

        OnPropertyChanged(nameof(HasResults));
        IsLoading = false;
        OnPropertyChanged(nameof(IsLoading));
    }

    public async Task<List<GameInfo>> GetFreeGames()
    {
        var games = new List<GameInfo>();
        var url = "https://gg.deals/deals/pc/?maxPrice=0&minDiscount=100&minRating=0";

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");

        var html = await httpClient.GetStringAsync(url);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        try
        {
            var listItems = doc.DocumentNode.SelectNodes("//div[contains(@class, 'list-items')]");
            if (listItems != null)
            {
                foreach (var listItem in listItems)
                {
                    var gameBoxes = listItem.SelectNodes(".//div[contains(@class, 'game-box-options')]");
                    if (gameBoxes != null)
                    {
                        foreach (var gameBox in gameBoxes)
                        {
                            var titleNode = gameBox?.SelectSingleNode(".//div[contains(@class,'game-info-title')]/a");
                            var title = titleNode?.InnerText.Trim() ?? "";

                            var shopName = gameBox?.GetAttributeValue("data-shop-name", "");

                            var imageNode = gameBox?.SelectSingleNode(".//a[contains(@class, 'main-image')]/picture/img");
                            var imageUrl = imageNode?.GetAttributeValue("src", "") ?? "";

                            var redirectNode = gameBox?.SelectSingleNode(".//div[contains(@class, 'game-cta')]/a[contains(@class, 'shop-link')]");
                            var redirectHref = redirectNode?.GetAttributeValue("href", "") ?? "";
                            var redirectUrl = string.IsNullOrEmpty(redirectHref) ? "" : $"https://gg.deals{redirectHref}";

                            var gameHref = titleNode?.GetAttributeValue("href", "");
                            var gameUrl = string.IsNullOrEmpty(gameHref) ? "" : $"https://gg.deals{gameHref}";

                            string date = await GetGameTimeLeft(gameUrl);
                            string timeLeft = string.Empty;
                            if (date != null && date != string.Empty)
                            {
                                DateTime dateTime = DateTime.Parse(date, null, System.Globalization.DateTimeStyles.RoundtripKind);
                                var culture = new System.Globalization.CultureInfo("es-ES");
                                timeLeft = dateTime.ToLocalTime().ToString("dddd, dd MMMM yyyy HH:mm", culture);
                            }

                            games.Add(new GameInfo
                            {
                                Title = title,
                                Platform = shopName,
                                TimeLeft = timeLeft,
                                ImageUrl = imageUrl,
                                RedirectUrl = redirectUrl
                            });
                        }
                    }
                }
            }
        }
        catch { }

        return games;
    }

    public async Task<string> GetGameTimeLeft(string gameUrl)
    {
        var timeLeft = string.Empty;

        var httpClient = new HttpClient();
        httpClient.DefaultRequestHeaders.UserAgent.ParseAdd("Mozilla/5.0");

        var html = await httpClient.GetStringAsync(gameUrl);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        try
        {
            var timeNode = doc.DocumentNode.SelectSingleNode("//time[contains(@class, 'timesince') and @datetime]");
            if (timeNode != null)
            {
                timeLeft = timeNode.GetAttributeValue("datetime", "");
            }
        }
        catch { }

        return timeLeft;
    }
}