namespace ShoppingList4F1.Views;

public partial class CategoryView : ContentView
{
    public Models.Category CurrentCategory
    {
        get => BindingContext as Models.Category;
    }

    public CategoryView()
	{
		InitializeComponent();
        //BindingContext = new Models.Category();
	}

    protected override void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        foreach (var product in CurrentCategory?.Products ?? Enumerable.Empty<Models.Product>())
        {
            var productView = new ProductView();
            productView.CurrentCategory = CurrentCategory;
            productView.BindingContext = product;
        }
    }
}