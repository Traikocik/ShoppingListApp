using System.Collections.ObjectModel;

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

    private async void AddProduct_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((Models.ShoppingList)BindingContext));
    }
}