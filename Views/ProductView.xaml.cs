using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;

namespace ShoppingList4F1.Views;

public partial class ProductView : ContentView
{
    public static readonly BindableProperty ParentCategoryProperty =
            BindableProperty.Create(
                nameof(ParentCategory),
                typeof(Models.Category),
                typeof(ProductView),
                default(Models.Category));

    public Models.Category ParentCategory
    {
        get => (Models.Category)GetValue(ParentCategoryProperty);
        set => SetValue(ParentCategoryProperty, value);
    }

    public ProductView()
    {
        InitializeComponent();
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (BindingContext is not Models.Product product || ParentCategory == null) return;
        product.IsBought = e.Value;
        ParentCategory.Products.Remove(product);
        if (e.Value)
        {
            ParentCategory.Products.Add(product);
            ((StackLayout)((CheckBox)sender).Parent).BackgroundColor = Colors.Gray;
        }
        else
        {
            ParentCategory.Products.Insert(0, product);
            ((StackLayout)((CheckBox)sender).Parent).BackgroundColor = Colors.Transparent;
        }
        Models.AllShoppingLists.SaveShoppingLists();
    }

    private void IncrementButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Models.Product product)
        {
            product.Quantity++;
            Models.AllShoppingLists.SaveShoppingLists();
        }
    }

    private void DecrementButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Models.Product product && product.Quantity > 0)
        {
            product.Quantity--;
            Models.AllShoppingLists.SaveShoppingLists();
        }
    }

    private void RemoveButton_Clicked(object sender, EventArgs e)
    {
        if (BindingContext is Models.Product product)
        {
            ParentCategory.Products.Remove(product);
            Models.AllShoppingLists.SaveShoppingLists();
        }
    }
}