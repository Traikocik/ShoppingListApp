using System.Collections.ObjectModel;

namespace ShoppingList4F1.Views;

public partial class ProductPage : ContentPage
{
    private Models.ShoppingList ShoppingList { get; set; }
    public ObservableCollection<Models.Category> Categories { get; set; }

    public ProductPage(Models.ShoppingList shoppingList)
	{
		InitializeComponent();
        ShoppingList = shoppingList;
        BindingContext = new Models.Product();
        Categories = new ObservableCollection<Models.Category>(shoppingList.Categories);
        Categories.Add(new Models.Category("Add new category"));
        CategoryPicker.ItemsSource = Categories;
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        string name = NameEditor.Text;
        if (string.IsNullOrEmpty(name))
        {
            await DisplayAlert("WARNIN!G", "Product's name entry is empty! Can't add new product.", "OK");
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

        Models.Category selectedCategory = (Models.Category)CategoryPicker.SelectedItem;
        if (selectedCategory == null || selectedCategory.Name == "Add New Category")
        {
            string categoryName = await DisplayPromptAsync("Add new category", "Enter the name of the new category:");
            if (string.IsNullOrEmpty(categoryName))
            {
                await DisplayAlert("WARNING!", "No category has been selected or created. Can't add new product.", "OK");
                return;
            }
            else
                ShoppingList.Categories.Add(new Models.Category(categoryName));
        }

        var newProduct = new Models.Product(name, typeOfMeasurement, quantity);
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
                Categories.Add(newCategory);
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
}