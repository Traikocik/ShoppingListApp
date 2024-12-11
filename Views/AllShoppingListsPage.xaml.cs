using ShoppingList4F1.Models;

namespace ShoppingList4F1.Views;

public partial class AllShoppingListsPage : ContentPage
{
	public AllShoppingListsPage()
	{
		InitializeComponent();
        BindingContext = new AllShoppingLists();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        AllShoppingLists.LoadShoppingLists();
        shoppingListsCollection.ItemsSource = AllShoppingLists.ShoppingLists;
    }

    private async void shoppingListsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            var shoppingList = (ShoppingList)e.CurrentSelection[0];
            await Navigation.PushAsync(new ShoppingListPage(shoppingList));
            shoppingListsCollection.SelectedItem = null;
        }
    }

    private async void AddShoppingList_Clicked(object sender, EventArgs e)
    {
        //await Shell.Current.GoToAsync(nameof(CreateShoppingListPage));
        await Navigation.PushAsync(new CreateShoppingListPage());
    }
}