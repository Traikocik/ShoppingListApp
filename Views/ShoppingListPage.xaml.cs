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

    private async void ExportShoppingList_Clicked(object sender, EventArgs e)
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
}