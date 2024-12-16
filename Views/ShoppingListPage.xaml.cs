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

        var shoppingList = (Models.ShoppingList)BindingContext;

        categoriesCollection.ItemTemplate = new DataTemplate(() =>
        {
            var categoryView = new CategoryView();
            categoryView.SetBinding(BindingContextProperty, ".");
            return categoryView;
        });

        categoriesCollection.ItemsSource = shoppingList.Categories;
    }

    //protected override void OnAppearing()
    //{
    //    base.OnAppearing();
    //    categoriesCollection.ItemsSource = ((Models.ShoppingList)BindingContext).Categories;

    //    var shoppingList = (Models.ShoppingList)BindingContext;

    //    categoriesCollection.ItemTemplate = new DataTemplate(() =>
    //    {
    //        var categoryView = new CategoryView { CurrentShoppingList = shoppingList };
    //        categoryView.SetBinding(BindingContextProperty, ".");
    //        return categoryView;
    //    });

    //    categoriesCollection.ItemsSource = shoppingList.Categories;
    //}

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