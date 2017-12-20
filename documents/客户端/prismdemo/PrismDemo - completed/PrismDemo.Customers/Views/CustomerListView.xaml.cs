using Microsoft.Practices.Prism.Regions;
using PrismDemo.Customers.ViewModels;
using System.Windows.Controls;

namespace PrismDemo.Customers.Views
{
  /// <summary>
  /// Interaction logic for CustomerListView.xaml
  /// </summary>
  [ViewSortHint("02")] //The ViewSortHint helps the RegionManager order the Views within the  Region when using View Discovery.
  public partial class CustomerListView : UserControl
  {
    public CustomerListView(CustomerListViewModel viewModel) // This is depending on the Container to resolve the viewmodel reference using constructor injection.
    {
      InitializeComponent();

      viewModel.Initialize();
      this.DataContext = viewModel;// Setting the DataContext of the view to the ViewModel will enable databinding between them.
    }
  }
}