using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wheesper.Login.events;

namespace Wheesper.Chat.View
{
    /// <summary>
    /// Interaction logic for WheesperView.xaml
    /// </summary>
    public partial class WheesperView : UserControl
    {
        public WheesperView(IUnityContainer container)
        {
            InitializeComponent();
        }
    }
}
