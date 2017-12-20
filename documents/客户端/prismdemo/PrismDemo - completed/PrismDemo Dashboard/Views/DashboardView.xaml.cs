using Microsoft.Practices.Prism.Regions;
using PrismDemo.Dashboard.ViewModels;
using System.Windows.Controls;

namespace PrismDemo.Dashboard.Views
{
  /// <summary>
  /// Interaction logic for DashboadView.xaml
  /// </summary>
  [ViewSortHint("01")] //The ViewSortHint helps the RegionManager order the Views within the  Region when using View Discovery.
  public partial class DashboardView : UserControl
  {
    public DashboardView(DashboardViewModel viewModel) // This is depending on the Container to resolve the viewmodel reference using constructor injection.
    {
      InitializeComponent();

      this.DataContext = viewModel; // Setting the DataContext of the view to the ViewModel will enable databinding between them.
    }
  }
}