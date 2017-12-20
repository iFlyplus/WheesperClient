using System.Windows;

namespace PrismDemo.UI
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    protected override void OnStartup(StartupEventArgs e)
    {
      base.OnStartup(e);

      /// Unity Bootstrapper - replaces Window.Show with a new approach.
      var bootstrapper = new Bootstrapper();
      bootstrapper.Run();
    }
  }
}