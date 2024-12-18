using System.ComponentModel;

namespace ShoppingList4F1.Views;

public partial class CategoryView : ContentView
{
    //public event EventHandler CategoryChanged;
    public CategoryView()
    {
        InitializeComponent();
        //BindingContextChanged += OnBindingContextChanged;
    }

    //private void OnBindingContextChanged(object sender, EventArgs e)
    //{
    //    if (BindingContext is Models.Category category)
    //    {
    //        foreach (var product in category.Products)
    //        {
    //            product.PropertyChanged -= Product_PropertyChanged;
    //        }
    //        foreach (var product in category.Products)
    //        {
    //            product.PropertyChanged += Product_PropertyChanged;
    //        }
    //    }
    //}

    //private void Product_PropertyChanged(object sender, PropertyChangedEventArgs e)
    //{
    //    if (e.PropertyName == nameof(Models.Product.IsBought))
    //    {
    //        CategoryChanged?.Invoke(this, EventArgs.Empty);
    //    }
    //}

    //protected override void OnParentSet()
    //{
    //    base.OnParentSet();

    //    if (Parent == null && BindingContext is Models.Category category)
    //    {
    //        foreach (var product in category.Products)
    //        {
    //            product.PropertyChanged -= Product_PropertyChanged;
    //        }
    //    }
    //}
}