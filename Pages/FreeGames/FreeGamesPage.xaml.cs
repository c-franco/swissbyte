using HtmlAgilityPack;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace swissbyte.Pages.FreeGames;

public partial class FreeGamesPage : ContentPage
{
    public ObservableCollection<GameInfo> Games { get; set; } = new();
    public ICommand OpenUrlCommand { get; }

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

    private async void LoadFreeGames()
    {
        var freeGames = await GetFreeGames();

        Games.Clear();

        foreach (var game in freeGames)
        {
            Games.Add(game);
        }
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

                            var timeLeftNode = gameBox?.SelectSingleNode(".//div[contains(@class, 'time-tag')]/span[@class='title']//time");
                            var timeLeft = timeLeftNode?.InnerText.Trim() ?? "";

                            var redirectNode = gameBox?.SelectSingleNode(".//div[contains(@class, 'game-cta')]/a[contains(@class, 'shop-link')]");
                            var redirectHref = redirectNode?.GetAttributeValue("href", "") ?? "";
                            var redirectUrl = string.IsNullOrEmpty(redirectHref) ? "" : $"https://gg.deals{redirectHref}";

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
}