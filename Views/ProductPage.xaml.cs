namespace ShoppingList4F1.Views;

public partial class ProductPage : ContentPage
{
    private Models.ShoppingList ShoppingList { get; set; }
    public ProductPage(Models.ShoppingList shoppingList)
	{
		InitializeComponent();
        ShoppingList = shoppingList;
        BindingContext = new Models.Product();
    }

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        string name = NameEditor.Text;
        if (string.IsNullOrEmpty(name))
        {
            name = "Product";
            await DisplayAlert("WARNING", "Product's name entry is empty! It will be replaced with 'Product'", "OK");
        }

        if (!double.TryParse(QuantityEditor.Text, out double quantity))
        {
            quantity = 1;
            await DisplayAlert("WARNING", "Quantity's entry is empty! It will be replaced with '1'", "OK");
        }

        string typeOfMeasurement = TypeOfMeasurementEditor.Text;
        if (string.IsNullOrEmpty(typeOfMeasurement))
        {
            typeOfMeasurement = "Things";
            await DisplayAlert("WARNING", "Type of measurement's entry is empty! It will be replaced with 'Things'", "OK");
        }

        ShoppingList.Products.Add(new Models.Product(name, typeOfMeasurement, quantity));
        Models.AllShoppingLists.SaveShoppingLists();

        await Shell.Current.GoToAsync("..");
    }
}