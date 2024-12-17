using System.Collections.ObjectModel;
using System.Xml.Linq;
using CommunityToolkit.Maui.Storage;

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

    private async void FilterByShop_Clicked(object sender, EventArgs e)
    {
        string[] shopNames = Models.AllShoppingLists.Shops.Select(shop => shop.Name).ToArray();
        string[] newShopNames = new string[shopNames.Length+1];
        newShopNames[0] = "-";
        Array.Copy(shopNames, 0, newShopNames, 1, shopNames.Length);

        string selectedShopName = await DisplayActionSheet("Select Shop", "Cancel", null, newShopNames);

        if (!string.IsNullOrEmpty(selectedShopName) && selectedShopName != "Cancel")
        {
            Models.Shop selectedShop = selectedShopName != "-" ? 
                Models.AllShoppingLists.Shops.FirstOrDefault(shop => shop.Name == selectedShopName) :
                new Models.Shop("", "");
            if (selectedShop != null)
            {
                List<Models.Category> filteredCategories = ((Models.ShoppingList)BindingContext).Categories
                    .Select(category => new Models.Category(category.Id, category.Name)
                    {
                        Products = new ObservableCollection<Models.Product>(category.Products.Where(product => product.ShopId == selectedShop.Id))
                    })
                    .Where(category => category.Products.Any())
                    .ToList();

                categoriesCollection.ItemsSource = filteredCategories;
                resetButton.IsEnabled = true;
            }
        }
    }

    private void ResetFilterButton_Clicked(object sender, EventArgs e)
    {
        categoriesCollection.ItemsSource = ((Models.ShoppingList)BindingContext).Categories;
        resetButton.IsEnabled = false;
    }

    private async void ExportShoppingList_Clicked(object sender, EventArgs e)
    {
        try
        {
            Models.ShoppingList currentShoppingList = (Models.ShoppingList)BindingContext;
            var doc = new XDocument(
                new XElement("ShoppingList",
                    new XAttribute("Id", currentShoppingList.Id),
                    new XAttribute("Name", currentShoppingList.Name),
                    currentShoppingList.GetElementFromCategories()
                )
            );

            var folderResult = await FolderPicker.Default.PickAsync();

            if (folderResult != null && folderResult.Exception == null)
            {
                var filePath = Path.Combine(folderResult.Folder.Path, $"{currentShoppingList.Name}.xml");
                doc.Save(filePath);
                await DisplayAlert("SUCCESS", $"Shopping list exported to: {filePath}", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("ERROR", "An error occured while exporting from file: " + ex.Message, "OK");
        }
    }
}