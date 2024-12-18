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
        try
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

                if (Models.AllShoppingLists.ShoppingLists.Any(sl => sl.Id == id))
                {
                    await DisplayAlert("ERROR", $"Shopping List with ID: '{id}' already exists.", "OK");
                    return;
                }

                if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(name))
                {
                    await DisplayAlert("ERROR", "Couldn't add new Shopping List, not sufficient data.", "OK");
                    return;
                }

                var shopsElement = doc.Root.Element("Shops");
                if (shopsElement != null)
                {
                    foreach (var shopElement in shopsElement.Elements("Shop"))
                    {
                        string shopName = shopElement.Attribute("Name")?.Value;
                        string shopId = shopElement.Attribute("Id")?.Value;
                        if (!string.IsNullOrEmpty(shopName) && !Models.AllShoppingLists.Shops.Any(s => s.Id == shopId))
                        {
                            Models.AllShoppingLists.Shops.Add(new Models.Shop(shopId, shopName));
                        }
                    }
                }

                var unitsElement = doc.Root.Element("Units");
                if (unitsElement != null)
                {
                    foreach (var unitElement in unitsElement.Elements("Unit"))
                    {
                        string unitName = unitElement.Attribute("Name")?.Value;
                        string unitId = unitElement.Attribute("Id")?.Value;
                        if (!string.IsNullOrEmpty(unitName) && !Models.AllShoppingLists.Units.Any(u => u.Id == unitId))
                        {
                            Models.AllShoppingLists.Units.Add(new Models.Unit(unitId, unitName));
                        }
                    }
                }

                Models.ShoppingList shoppingList = new Models.ShoppingList(id, name);
                shoppingList.SetCategoriesFromElement(doc.Root.Element("Categories"));
                Models.AllShoppingLists.ShoppingLists.Add(shoppingList);
                Models.AllShoppingLists.SaveShoppingLists();

                await DisplayAlert("SUCCESS", "Shopping list imported", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("ERROR", "An error occured while importing from file: " + ex.Message, "OK");
        }
    }
}