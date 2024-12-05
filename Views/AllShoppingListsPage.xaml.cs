namespace ShoppingList4F1.Views;

public partial class AllShoppingListsPage : ContentPage
{
	public AllShoppingListsPage()
	{
		InitializeComponent();
        BindingContext = new Models.AllShoppingLists();
    }

    private void shoppingListsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private async void AddShoppingList_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Models.ShoppingListPage());
    }
}