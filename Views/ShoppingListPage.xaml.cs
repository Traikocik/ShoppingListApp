using System.Collections.ObjectModel;

namespace ShoppingList4F1.Views;

public partial class ShoppingListPage : ContentPage
{
    public ShoppingListPage()
    {
        InitializeComponent();
        BindingContext = new Models.ShoppingList();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        productsCollection.ItemsSource = ((Models.ShoppingList)BindingContext).Products;

        var shoppingList = (Models.ShoppingList)BindingContext;

        productsCollection.ItemTemplate = new DataTemplate(() =>
        {
            var productView = new ProductView { CurrentShoppingList = shoppingList };
            productView.SetBinding(BindingContextProperty, ".");
            return productView;
        });

        productsCollection.ItemsSource = shoppingList.Products;
    }

    public ShoppingListPage(Models.ShoppingList shoppingList)
    {
        InitializeComponent();
        BindingContext = shoppingList;
    }

    private void productsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {

    }

    private async void AddProduct_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((Models.ShoppingList)BindingContext));
    }
}