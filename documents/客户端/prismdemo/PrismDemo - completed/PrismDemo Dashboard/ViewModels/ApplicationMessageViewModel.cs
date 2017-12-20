using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using Microsoft.Practices.Unity;
using PrismDemo.Infrastructure.Events;
using System.Windows.Input;

namespace PrismDemo.Dashboard.ViewModels
{
  public class ApplicationMessageViewModel : NotificationObject
  {
    private IUnityContainer container = null;
    private IEventAggregator eventAggregator = null;

    #region Message

    private string message = string.Empty;

    public string Message
    {
      get
      {
        return message;
      }

      set
      {
        message = value;
        RaisePropertyChanged(() => this.Message);
      }
    }

    #endregion Message

    #region Close Command

    private ICommand closeCommand;

    public ICommand CloseCommand
    {
      get
      {
        if (this.closeCommand == null)
        {
          this.closeCommand = new DelegateCommand(this.Close);
        }
        return this.closeCommand;
      }

      set
      {
        this.closeCommand = value;
      }
    }

    public void Close()
    {
      this.eventAggregator.GetEvent<HideApplicationMessageEvent>().Publish(null);
    }

    #endregion Close Command

    public ApplicationMessageViewModel(IUnityContainer container)
    {
      this.container = container;
      this.eventAggregator = this.container.Resolve<IEventAggregator>();
    }

    public void Initialize(string message)
    {
      this.Message = message;
    }
  }
}