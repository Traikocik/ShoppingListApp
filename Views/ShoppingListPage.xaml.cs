using System.Collections.ObjectModel;
using System.Xml.Linq;
using CommunityToolkit.Maui.Storage;

namespace ShoppingList4F1.Views;

public partial class ShoppingListPage : ContentPage
{
    private bool IsFilteringByNotBought = false;

    public ShoppingListPage()
    {
        InitializeComponent();
        BindingContext = new Models.ShoppingList();

        MessagingCenter.Subscribe<ProductView>(this, "ProductCheckedChanged", (sender) =>
        {
            if (IsFilteringByNotBought)
                FilterByNotBought_Clicked(null, null);
        });
    }

    public ShoppingListPage(Models.ShoppingList shoppingList)
    {
        InitializeComponent();
        BindingContext = shoppingList;

        MessagingCenter.Subscribe<ProductView>(this, "ProductCheckedChanged", (sender) =>
        {
            if (IsFilteringByNotBought)
                FilterByNotBought_Clicked(null, null);
        });
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        SetCollectionToCategoriesWithProducts();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        MessagingCenter.Unsubscribe<ProductView>(this, "ProductCheckedChanged");
    }

    private async void AddProduct_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new ProductPage((Models.ShoppingList)BindingContext));
    }

    private void FilterByNotBought_Clicked(object sender, EventArgs e)
    {
        List<Models.Category> categoriesWithNotBoughtProduct = ((Models.ShoppingList)BindingContext).Categories
            .Select(category => new Models.Category(category.Id, category.Name)
            {
                Products = new ObservableCollection<Models.Product>(category.Products.Where(product => product.IsBought == false))
            })
            .Where(category => category.Products.Any())
            .ToList();

        foreach (Models.Category category in categoriesWithNotBoughtProduct)
        {
            category.IsCategoryNameVisible = false;
            category.IsExpanded = true;
        }
        categoriesCollection.ItemsSource = categoriesWithNotBoughtProduct;
        IsFilteringByNotBought = true;
        resetButton.IsEnabled = true;

    }

    private async void FilterByShop_Clicked(object sender, EventArgs e)
    {
        string[] shopNames = Models.AllShoppingLists.Shops.Select(shop => shop.Name).ToArray();
        string[] newShopNames = new string[shopNames.Length + 1];
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
                IsFilteringByNotBought = false;
                resetButton.IsEnabled = true;
            }
        }
    }

    private void ResetFilterButton_Clicked(object sender, EventArgs e)
    {
        resetButton.IsEnabled = false;
        IsFilteringByNotBought = false;
        SetCollectionToCategoriesWithProducts();
    }

    private async void ExportShoppingList_Clicked(object sender, EventArgs e)
    {
        try
        {
            Models.ShoppingList currentShoppingList = (Models.ShoppingList)BindingContext;

            var rootElement = new XElement("ShoppingListData");

            var usedShopIds = currentShoppingList.Categories
                .SelectMany(category => category.Products)
                .Where(product => !string.IsNullOrEmpty(product.ShopId))
                .Select(product => product.ShopId)
                .Distinct().ToList();

            var shopsElement = new XElement("Shops");
            foreach (var shop in Models.AllShoppingLists.Shops.Where(shop => usedShopIds.Contains(shop.Id)))
            {
                var shopElement = new XElement("Shop",
                    new XAttribute("Id", shop.Id),
                    new XAttribute("Name", shop.Name));
                shopsElement.Add(shopElement);
            }
            rootElement.Add(shopsElement);

            var usedUnitIds = currentShoppingList.Categories
                .SelectMany(category => category.Products)
                .Where(product => !string.IsNullOrEmpty(product.UnitId))
                .Select(product => product.UnitId)
                .Distinct().ToList();

            var unitsElement = new XElement("Units");
            foreach (var unit in Models.AllShoppingLists.Units.Where(unit => usedUnitIds.Contains(unit.Id)))
            {
                var unitElement = new XElement("Unit",
                    new XAttribute("Id", unit.Id),
                    new XAttribute("Name", unit.Name));
                unitsElement.Add(unitElement);
            }
            rootElement.Add(unitsElement);

            var shoppingListElement = new XElement("ShoppingList", 
                new XAttribute("Id", currentShoppingList.Id), 
                new XAttribute("Name", currentShoppingList.Name), 
                currentShoppingList.GetElementFromCategories());
            rootElement.Add(shoppingListElement);

            var doc = new XDocument(rootElement);

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

    private void OnCategoryTapped(object sender, EventArgs e)
    {
        Label label = sender as Label;
        if (label != null)
        {
            Models.Category category = label.BindingContext as Models.Category;
            if (category != null)
            {
                category.IsExpanded = !category.IsExpanded;
            }
        }
    }

    private void SetCollectionToCategoriesWithProducts()
    {
        List<Models.Category> categories = ((Models.ShoppingList)BindingContext).Categories
            .Select(category => new Models.Category(category.Id, category.Name)
            {
                Products = new ObservableCollection<Models.Product>(category.Products.Where(product => product.ShopId == product.ShopId))
            })
            .Where(category => category.Products.Any())
            .ToList();

        categoriesCollection.ItemsSource = categories;
    }
}