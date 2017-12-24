﻿using Microsoft.Practices.Prism.Events;
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
    /// Interaction logic for SystemMessageView.xaml
    /// </summary>
    public partial class SystemMessageView : UserControl
    {
        #region private menber
        private IUnityContainer container = null;
        private IEventAggregator eventAggregator = null;
        #endregion private menber

        public SystemMessageView(IUnityContainer container)
        {
            InitializeComponent();
            this.container = container;
            eventAggregator = container.Resolve<IEventAggregator>();
        }

        private void MouseDown_SystemMessage(object sender, MouseButtonEventArgs e)
        {
            eventAggregator.GetEvent<MouseKeyDownASystemMessageEvent>().Publish(Convert.ToInt32(((System.Windows.Controls.TextBlock)e.Source).Text));
        }
    }
}
