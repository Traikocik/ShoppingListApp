using System;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;

namespace ShoppingList4F1.Views;

public partial class ProductView : ContentView
{
    //public event EventHandler<bool> ProductCheckedChanged;

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

        BindingContextChanged += OnBindingContextChanged;
    }

    private void OnBindingContextChanged(object sender, EventArgs e)
    {
        if (BindingContext is Models.Product product)
        {
            StackLayout stackLayout = (StackLayout)Children[0];
            stackLayout.BackgroundColor = Colors.Transparent;

            if (product.IsOptional)
                stackLayout.BackgroundColor = Color.FromArgb("#FF705000");

            if (product.IsBought)
                stackLayout.BackgroundColor = Color.FromArgb("#FF333333");

            Models.AllShoppingLists.SaveShoppingLists();
        }
    }

    private void CheckBox_CheckedChanged(object sender, CheckedChangedEventArgs e)
    {
        if (BindingContext is not Models.Product product || ParentCategory == null) return;

        product.IsBought = e.Value;

        //ProductCheckedChanged?.Invoke(this, product.IsBought);

        StackLayout stackLayout = (StackLayout)Children[0];
        stackLayout.BackgroundColor = Colors.Transparent;

        if (product.IsOptional)
            stackLayout.BackgroundColor = Color.FromArgb("#FF705000");

        if (product.IsBought)
            stackLayout.BackgroundColor = Color.FromArgb("#FF333333");


        var sortedProducts = ParentCategory.Products
            .OrderBy(p => p.IsBought)
            .ThenBy(p => p.Id)
            .ToList();

        ParentCategory.Products.Clear();
        foreach (var sortedProduct in sortedProducts)
        {
            ParentCategory.Products.Add(sortedProduct);
        }

        Models.AllShoppingLists.SaveShoppingLists();
        MessagingCenter.Send(this, "ProductCheckedChanged");
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
        if (BindingContext is Models.Product product && product.Quantity > 1)
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