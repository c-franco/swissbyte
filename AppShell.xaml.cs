using swissbyte.Boxes.Compra;

namespace swissbyte
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(CompraPage), typeof(CompraPage));
        }
    }
}
