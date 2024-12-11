namespace ShoppingList4F1
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(Views.CreateShoppingListPage), typeof(Views.CreateShoppingListPage));
            Routing.RegisterRoute(nameof(Views.ShoppingListPage), typeof(Views.ShoppingListPage));
            //Routing.RegisterRoute(nameof(Views.ProductPage), typeof(Views.ProductPage));
        }
    }
}
