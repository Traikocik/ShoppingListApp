namespace ShoppingList4F1.Views;

public partial class CategoryView : ContentView
{
	public CategoryView()
	{
		InitializeComponent();
        BindingContext = new Models.Category();
	}
}