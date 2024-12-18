using System.Collections.ObjectModel;

namespace ShoppingList4F1.Views;

public partial class ProductPage : ContentPage
{
    private Models.ShoppingList ShoppingList { get; set; }
    public ObservableCollection<Models.Category> Categories { get; set; }
    public ObservableCollection<Models.Shop> Shops { get; set; }
    private Models.Shop noShop;

    public ProductPage(Models.ShoppingList shoppingList)
	{
		InitializeComponent();
        ShoppingList = shoppingList;
        BindingContext = new Models.Product();

        Categories = new ObservableCollection<Models.Category>(shoppingList.Categories);
        Categories.Add(new Models.Category("Add new category"));
        CategoryPicker.ItemsSource = Categories;

        Shops = new ObservableCollection<Models.Shop>(Models.AllShoppingLists.Shops);
        noShop = new Models.Shop("") { Name = "-" };
        Shops.Insert(0, noShop);
        Shops.Add(new Models.Shop("Add new shop"));
        ShopPicker.ItemsSource = Shops;
        ShopPicker.SelectedItem = noShop;
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        string name = NameEditor.Text;
        if (string.IsNullOrEmpty(name))
        {
            await DisplayAlert("WARNING!", "Product's name entry is empty! Can't add new product.", "OK");
            return;
        }

        if (!double.TryParse(QuantityEditor.Text, out double quantity))
        {
            await DisplayAlert("WARNING!", "Quantity's entry is empty! Can't add new product.", "OK");
            return;
        }

        string typeOfMeasurement = TypeOfMeasurementEditor.Text;
        if (string.IsNullOrEmpty(typeOfMeasurement))
        {
            await DisplayAlert("WARNING!", "Type of measurement's entry is empty! Can't add new product.", "OK");
            return;
        }

        bool isOptional = IsOptionalCheckBox.IsChecked;

        Models.Category selectedCategory = (Models.Category)CategoryPicker.SelectedItem;
        if (selectedCategory == null || selectedCategory.Name == "Add new category")
        {
            string categoryName = await DisplayPromptAsync("Add new Category", "Enter the name of the new category:");
            if (string.IsNullOrEmpty(categoryName))
            {
                Models.Category category = new Models.Category(categoryName);
                ShoppingList.Categories.Insert(ShoppingList.Categories.Count - 2, category);
                CategoryPicker.SelectedItem = selectedCategory;
            }
            else
            {
                await DisplayAlert("WARNING!", "No category has been selected or created. Can't add new product.", "OK");
                CategoryPicker.SelectedItem = null;
            }
        }

        var newProduct = new Models.Product(name, typeOfMeasurement, isOptional, quantity);

        if (ShopPicker.SelectedItem != null)
        {
            Models.Shop selectedShop = (Models.Shop)ShopPicker.SelectedItem;
            if (selectedShop != null && selectedShop.Name == "-")
            {
                newProduct.ShopId = string.Empty;
            }
            else if (selectedShop != null && selectedShop.Name != "Add new shop")
            {
                newProduct.ShopId = selectedShop.Id;
            }
        }
        
        selectedCategory.Products.Add(newProduct);
        Models.AllShoppingLists.SaveShoppingLists();

        await Shell.Current.GoToAsync("..");
    }

    private async void CategoryPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedCategory = CategoryPicker.SelectedItem as Models.Category;
        if (selectedCategory != null && selectedCategory.Name == "Add new category")
        {
            string categoryName = await DisplayPromptAsync("New category", "Enter the name of the new category:");
            if (!string.IsNullOrEmpty(categoryName))
            {
                Models.Category newCategory = new Models.Category(categoryName);
                ShoppingList.Categories.Add(newCategory);
                Categories.Insert(Categories.Count - 2, newCategory);
                CategoryPicker.SelectedItem = newCategory;
            }
            else
            {
                CategoryPicker.SelectedItem = null;
                await DisplayAlert("WARNING!", "No category name has been entered. Can't create new category.", "OK");
                return;
            }
        }
    }

    private async void ShopPicker_SelectedIndexChanged(object sender, EventArgs e)
    {
        var selectedShop = ShopPicker.SelectedItem as Models.Shop;
        if (selectedShop != null && selectedShop.Name == "Add new shop")
        {
            string shopName = await DisplayPromptAsync("New shop", "Enter the name of the new shop:");

            if (!string.IsNullOrEmpty(shopName))
            {
                Models.Shop newShop = new Models.Shop(shopName);
                Models.AllShoppingLists.Shops.Add(newShop);
                Models.AllShoppingLists.SaveShoppingLists();
                Shops.Insert(Shops.Count - 2, newShop);
                ShopPicker.SelectedItem = newShop;
            }
            else
            {
                ShopPicker.SelectedItem = noShop;
                await DisplayAlert("WARNING!", "No shop name entered. Can't create new shop.", "OK");
                return;
            }
        }
    }
}