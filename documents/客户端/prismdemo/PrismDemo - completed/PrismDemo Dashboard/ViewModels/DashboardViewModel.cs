using Microsoft.Practices.Prism.ViewModel;

namespace PrismDemo.Dashboard.ViewModels
{
  public class DashboardViewModel : NotificationObject
  {
    #region Title

    private string title = "Dashboard";

    public string Title
    {
      get { return title; }
      set { title = value; }
    }

    #endregion Title

    public DashboardViewModel()
    {
    }
  }
}