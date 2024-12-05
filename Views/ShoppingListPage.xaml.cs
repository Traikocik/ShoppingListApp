namespace ShoppingList4F1.Models;

public partial class ShoppingListPage : ContentPage
{
	public ShoppingListPage()
	{
		InitializeComponent();
		BindingContext = new ShoppingList();
	}

    private void productsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private async void AddProduct_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new Views.ProductPage());
    }
}