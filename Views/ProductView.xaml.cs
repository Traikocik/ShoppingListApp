using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;

namespace ShoppingList4F1.Views;

public partial class ProductView : ContentView
{
    //public Models.ShoppingList CurrentShoppingList { get; set; }
    public Models.Category CurrentCategory { get; set; }

    public ProductView()
    {
        InitializeComponent();
        BindingContext = new Models.Product();
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (e.Value)
            ((StackLayout)((CheckBox)sender).Parent).BackgroundColor = Color.FromRgb(100, 100, 100);
        else
            ((StackLayout)((CheckBox)sender).Parent).BackgroundColor = Color.FromRgb(40, 40, 40);

        if (sender is CheckBox checkBox && checkBox.BindingContext is Models.Product changedProduct)
        {
            var currentProduct = CurrentCategory.Products.FirstOrDefault(p => p.Id == changedProduct.Id);
            if (currentProduct != null)
            {
                currentProduct.IsBought = e.Value;
                CurrentCategory.Products = new ObservableCollection<Models.Product>(CurrentCategory.Products.OrderBy(p => p.IsBought));
                Models.AllShoppingLists.SaveShoppingLists();
            }
        }
    }

    private void IncrementButton_Clicked(object sender, EventArgs e)
    {
        if (sender is ImageButton imageButton && imageButton.BindingContext is Models.Product changedProduct)
        {
            var currentProduct = CurrentCategory.Products.FirstOrDefault(p => p.Id == changedProduct.Id);
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
            var currentProduct = CurrentCategory.Products.FirstOrDefault(p => p.Id == changedProduct.Id);
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
            CurrentCategory.Products.Remove(productToBeDeleted);
            Models.AllShoppingLists.SaveShoppingLists();
        }
    }
}