namespace ShoppingList4F1.Views;

public partial class ShoppingListPage : ContentPage
{
	public ShoppingListPage()
	{
		InitializeComponent();
		BindingContext = new Models.ShoppingList();
	}

    public ShoppingListPage(Models.ShoppingList shoppingList)
    {
        InitializeComponent();
        BindingContext = shoppingList;
    }

    private void productsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        //if (e.CurrentSelection.Count != 0)
        //{
        //    var product = (Product)e.CurrentSelection[0];
        //    await Navigation.PushAsync(new Product(product));
        //    productsCollection.SelectedItem = null;
        //}
    }

    private async void AddProduct_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((Models.ShoppingList)BindingContext));
    }
}