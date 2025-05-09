using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace swissbyte.Pages.FreeGames;

public partial class FreeGamesPage : ContentPage, INotifyPropertyChanged
{
    private static readonly HttpClient httpClient = new HttpClient();
    public ObservableCollection<GameInfo> Games { get; set; } = new();
    public ICommand OpenUrlCommand { get; }
    public bool HasResults => Games?.Any() == true;

    bool _isLoading;
    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            if (_isLoading != value)
            {
                _isLoading = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;
    void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    public FreeGamesPage()
	{
		InitializeComponent();

        BindingContext = this;

        PageSetup();
        LoadFreeGames();


        OpenUrlCommand = new Command<string>(async (url) =>
        {
            if (!string.IsNullOrWhiteSpace(url))
                await Launcher.Default.OpenAsync(url);
        });
    }

    private void PageSetup()
    {
        if (!httpClient.DefaultRequestHeaders.Contains("User-Agent"))
        {
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0");
        }
    }

    private async void LoadFreeGames()
    {
        IsLoading = true;
        OnPropertyChanged(nameof(IsLoading));

        var freeGames = await GetFreeGames();

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

        var html = await httpClient.GetStringAsync(url);

        var doc = new HtmlDocument();
        doc.LoadHtml(html);

        var gameTasks = new List<Task<GameInfo>>();

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
                            gameTasks.Add(ProcessGameBoxAsync(gameBox));
                        }
                    }
                }
            }
        }
        catch (Exception) { }

        var results = await Task.WhenAll(gameTasks);
        games.AddRange(results);

        return games;
    }

    private async Task<GameInfo> ProcessGameBoxAsync(HtmlNode gameBox)
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
        string timeLeft = "";

        if (!string.IsNullOrEmpty(date))
        {
            if (DateTime.TryParse(date, null, DateTimeStyles.RoundtripKind, out DateTime dateTime))
            {
                var culture = new CultureInfo("es-ES");
                timeLeft = dateTime.ToLocalTime().ToString("dddd, dd MMMM yyyy HH:mm", culture);
            }
        }

        return new GameInfo
        {
            Title = title,
            Platform = shopName,
            TimeLeft = timeLeft,
            ImageUrl = imageUrl,
            RedirectUrl = redirectUrl
        };
    }


    private async Task<string> GetGameTimeLeft(string gameUrl)
    {
        try
        {
            var html = await httpClient.GetStringAsync(gameUrl);
            var doc = new HtmlDocument();
            doc.LoadHtml(html);

            var timeNode = doc.DocumentNode.SelectSingleNode("//time[contains(@class, 'timesince') and @datetime]");
            return timeNode?.GetAttributeValue("datetime", "") ?? "";
        }
        catch
        {
            return "";
        }
    }
}