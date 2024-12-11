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

    private async void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if(e.Value)
            ((StackLayout)((CheckBox)sender).Parent).BackgroundColor = Color.FromRgb(100, 100, 100);
        else
            ((StackLayout)((CheckBox)sender).Parent).BackgroundColor = Color.FromRgb(40, 40, 40);

        if (sender is CheckBox checkBox && checkBox.BindingContext is Models.Product changedProduct)
        {
            Models.ShoppingList shoppingList = Models.AllShoppingLists.ShoppingLists.FirstOrDefault(sl => sl.Id == ((Models.ShoppingList)BindingContext).Id);
            Models.Product currentProduct = shoppingList.Products.FirstOrDefault(p => p.Id == changedProduct.Id);
            currentProduct.IsBought = e.Value;
            shoppingList.Products = new ObservableCollection<Models.Product>(shoppingList.Products.OrderBy(p => p.IsBought));
            Models.AllShoppingLists.SaveShoppingLists();
        }
    }

    private void IncrementButton_Clicked(object sender, EventArgs e)
    {
        if (sender is ImageButton imageButton && imageButton.BindingContext is Models.Product changedProduct)
        {
            Models.ShoppingList shoppingList = Models.AllShoppingLists.ShoppingLists.FirstOrDefault(sl => sl.Id == ((Models.ShoppingList)BindingContext).Id);
            Models.Product currentProduct = shoppingList.Products.FirstOrDefault(p => p.Id == changedProduct.Id);
            currentProduct.Quantity++;
            Models.AllShoppingLists.SaveShoppingLists();
        }
    }

    private void DecrementButton_Clicked(object sender, EventArgs e)
    {
        if (sender is ImageButton imageButton && imageButton.BindingContext is Models.Product changedProduct)
        {
            Models.ShoppingList shoppingList = Models.AllShoppingLists.ShoppingLists.FirstOrDefault(sl => sl.Id == ((Models.ShoppingList)BindingContext).Id);
            Models.Product currentProduct = shoppingList.Products.FirstOrDefault(p => p.Id == changedProduct.Id);
            currentProduct.Quantity--;
            Models.AllShoppingLists.SaveShoppingLists();
        }
    }

    private void RemoveButton_Clicked(object sender, EventArgs e)
    {
        if (sender is ImageButton imageButton && imageButton.BindingContext is Models.Product productToBeDeleted)
        {
            ((Models.ShoppingList)BindingContext).Products.Remove(productToBeDeleted);
            Models.AllShoppingLists.SaveShoppingLists();
        }
    }
}