namespace ShoppingList4F1.Views;

public partial class CreateShoppingListPage : ContentPage
{
    private Models.AllShoppingLists AllShoppingLists { get; set; }
    public CreateShoppingListPage(Models.AllShoppingLists allShoppingLists)
	{
        InitializeComponent();
        AllShoppingLists = allShoppingLists;
	}

    private async void SaveButton_Clicked(object sender, EventArgs e)
    {
        string name = NameEditor.Text;
        if (string.IsNullOrEmpty(name))
        {
            name = "Shopping List";
            await DisplayAlert("WARNING", "Shopping list's name entry is empty! It will be replaced with 'Shopping List'", "OK");
        }

        AllShoppingLists.ShoppingLists.Add(new Models.ShoppingList(name));
        AllShoppingLists.SaveShoppingLists();

        await Shell.Current.GoToAsync("..");
    }
}