using System;
using System.Linq;
using Microsoft.Maui.Controls;

namespace ShoppingList4F1.Views;

public partial class ProductView : ContentView
{
    public Models.ShoppingList CurrentShoppingList { get; set; }

    public ProductView()
    {
        InitializeComponent();
        BindingContext = new Models.Product();
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (sender is CheckBox checkBox && checkBox.BindingContext is Models.Product changedProduct)
        {
            var currentProduct = CurrentShoppingList.Products.FirstOrDefault(p => p.Id == changedProduct.Id);
            if (currentProduct != null)
            {
                currentProduct.IsBought = e.Value;
                Models.AllShoppingLists.SaveShoppingLists();
            }
        }
    }

    private void IncrementButton_Clicked(object sender, EventArgs e)
    {
        if (sender is ImageButton imageButton && imageButton.BindingContext is Models.Product changedProduct)
        {
            var currentProduct = CurrentShoppingList.Products.FirstOrDefault(p => p.Id == changedProduct.Id);
            if (currentProduct != null)
            {
                currentProduct.Quantity++;
                Models.AllShoppingLists.SaveShoppingLists();
            }
        }
    }

    private void DecrementButton_Clicked(object sender, EventArgs e)
    {
        if (sender is ImageButton imageButton && imageButton.BindingContext is Models.Product changedProduct)
        {
            var currentProduct = CurrentShoppingList.Products.FirstOrDefault(p => p.Id == changedProduct.Id);
            if (currentProduct != null)
            {
                currentProduct.Quantity--;
                Models.AllShoppingLists.SaveShoppingLists();
            }
        }
    }

    private void RemoveButton_Clicked(object sender, EventArgs e)
    {
        if (sender is ImageButton imageButton && imageButton.BindingContext is Models.Product productToBeDeleted)
        {
            CurrentShoppingList.Products.Remove(productToBeDeleted);
            Models.AllShoppingLists.SaveShoppingLists();
        }
    }
}