using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;

namespace ShoppingList4F1.Views;

public partial class ProductView : ContentView
{
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

        //if (sender is CheckBox checkBox && checkBox.Parent.Parent.Parent.BindingContext is Models.Category category)
        //{
        //    CurrentCategory = category;
        //    Models.Product currentProduct = CurrentCategory.Products.FirstOrDefault(p => p.Id == ((Models.Product)BindingContext).Id);
        //    if (currentProduct != null)
        //    {
        //        currentProduct.IsBought = e.Value;
        //        CurrentCategory.Products = new ObservableCollection<Models.Product>(CurrentCategory.Products.OrderBy(p => p.IsBought));
        //        Models.AllShoppingLists.SaveShoppingLists();
        //    }
        //}
    }

    private async void IncrementButton_Clicked(object sender, EventArgs e)
    {
        if (sender is ImageButton imageButton && imageButton.Parent.Parent.Parent.BindingContext is Models.Category category)
        {
            CurrentCategory = category;
            var currentProduct = CurrentCategory.Products.FirstOrDefault(p => p.Id == ((Models.Product)BindingContext).Id);
            if (currentProduct != null)
            {
                currentProduct.Quantity++;
                Models.AllShoppingLists.SaveShoppingLists();
                await Shell.Current.GoToAsync("..");
            }
        }
    }

    private async void DecrementButton_Clicked(object sender, EventArgs e)
    {
        if (sender is ImageButton imageButton && imageButton.Parent.Parent.Parent.BindingContext is Models.Category category)
        {
            CurrentCategory = category;
            var currentProduct = CurrentCategory.Products.FirstOrDefault(p => p.Id == ((Models.Product)BindingContext).Id);
            if (currentProduct != null)
            {
                currentProduct.Quantity--;
                Models.AllShoppingLists.SaveShoppingLists();
                await Shell.Current.GoToAsync("..");
            }
        }
    }

    private void RemoveButton_Clicked(object sender, EventArgs e)
    {
        if (sender is ImageButton imageButton && imageButton.Parent.Parent.Parent.BindingContext is Models.Category category)
        {
            CurrentCategory = category;
            CurrentCategory.Products.Remove((Models.Product)BindingContext);
            Models.AllShoppingLists.SaveShoppingLists();
        }
    }
}