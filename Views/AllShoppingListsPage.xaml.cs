using System.Xml.Linq;

namespace ShoppingList4F1.Views;

public partial class AllShoppingListsPage : ContentPage
{
    public AllShoppingListsPage()
    {
        InitializeComponent();
        BindingContext = new Models.AllShoppingLists();
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        Models.AllShoppingLists.LoadShoppingLists();
        shoppingListsCollection.ItemsSource = Models.AllShoppingLists.ShoppingLists;
    }

    private async void shoppingListsCollection_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.Count != 0)
        {
            var shoppingList = (Models.ShoppingList)e.CurrentSelection[0];
            await Navigation.PushAsync(new ShoppingListPage(shoppingList));
            shoppingListsCollection.SelectedItem = null;
        }
    }

    private async void AddShoppingList_Clicked(object sender, EventArgs e)
    {
        await Navigation.PushAsync(new CreateShoppingListPage());
    }

    private void RemoveButton_Clicked(object sender, EventArgs e)
    {
        if (sender is ImageButton imageButton && imageButton.BindingContext is Models.ShoppingList shoppingListToBeDeleted)
        {
            Models.AllShoppingLists.ShoppingLists.Remove(shoppingListToBeDeleted);
            Models.AllShoppingLists.SaveShoppingLists();
        }
    }

    private async void ImportShoppingList_Clicked(object sender, EventArgs e)
    {

        var XMLFileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
        {
            { DevicePlatform.Android, new[] { "application/xml", "text/xml" } },
            { DevicePlatform.iOS, new[] { "public.xml" } },
            { DevicePlatform.MacCatalyst, new[] { "public.xml" } },
            { DevicePlatform.WinUI, new[] { ".xml" } }
        });

        PickOptions pickOptions = new PickOptions();
        pickOptions.FileTypes = XMLFileTypes;
        pickOptions.PickerTitle = "Choose a XML type file";

        FileResult fileResult = await FilePicker.PickAsync(pickOptions);

        if (fileResult != null)
        {
            XDocument doc = XDocument.Load(fileResult.FullPath);

            string id = doc.Root.Attribute("Id").Value;
            string name = doc.Root.Attribute("Name").Value;

            if (id != null && name != null)
            {
                Models.ShoppingList shoppingList = new Models.ShoppingList(id, name);
                shoppingList.SetCategoriesFromElement(doc.Root.Element("Categories"));
                Models.AllShoppingLists.ShoppingLists.Add(shoppingList);
                Models.AllShoppingLists.SaveShoppingLists();

                await DisplayAlert("SUCCESS", "Shopping list imported", "OK");
            }
            else
            {
                await DisplayAlert("ERROR", "Couldn't add new Shopping List, not sufficient data.", "OK");
            }
        }
    }
}