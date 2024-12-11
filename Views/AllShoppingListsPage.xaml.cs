using System.Xml.Linq;

namespace ShoppingList4F1.Views;

public partial class AllShoppingListsPage : ContentPage
{
	public AllShoppingListsPage()
	{
		InitializeComponent();
        BindingContext = new Models.AllShoppingLists();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Models.AllShoppingLists.LoadShoppingLists();
        shoppingListsCollection.ItemsSource = Models.AllShoppingLists.ShoppingLists;
    }

    private async void shoppingListsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            var shoppingList = (Models.ShoppingList)e.CurrentSelection[0];
            await Navigation.PushAsync(new ShoppingListPage(shoppingList));
            shoppingListsCollection.SelectedItem = null;
        }
    }

    private async void AddShoppingList_Clicked(object sender, EventArgs e)
    {
        //await Shell.Current.GoToAsync(nameof(CreateShoppingListPage));
        await Navigation.PushAsync(new CreateShoppingListPage());
    }

    private void RemoveButton_Clicked(object sender, EventArgs e)
    {
        if (sender is ImageButton imageButton && imageButton.BindingContext is Models.ShoppingList shoppingListToBeDeleted)
        {
            Models.AllShoppingLists.ShoppingLists.Remove(shoppingListToBeDeleted);
            Models.AllShoppingLists.SaveShoppingLists();
        }
    }
}