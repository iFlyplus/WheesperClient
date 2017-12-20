using Microsoft.Practices.Unity;
using Microsoft.Practices.Prism.UnityExtensions;
using System.Windows;
using System;
using System.Diagnostics;

using Wheesper.Infrastructure.services;
using Wheesper.Messaging;
using Wheesper.Test;
using Microsoft.Practices.Prism.Modularity;
using Wheesper.Login;

namespace Wheesper.Desktop
{
    class Bootstrapper : UnityBootstrapper
    {


        #region override method
        protected override DependencyObject CreateShell()
        {
            return new Shell();
        }
        protected override void InitializeShell()
        {
            base.InitializeShell();
            App.Current.MainWindow = (Window)this.Shell;
            App.Current.MainWindow.Show();
            Console.WriteLine("InitializeShell");
            IMessagingService i = Container.Resolve<IMessagingService>();
        }

        protected override void ConfigureContainer()
        {
            base.ConfigureContainer();

            RegisterTypeIfMissing(typeof(IMessagingService), typeof(Messaging.Messaging), true);
        }

        protected override IModuleCatalog CreateModuleCatalog()
        {
            var moduleCatalog = new ModuleCatalog();

            //Type testModule = typeof(MessagingTest);
            //moduleCatalog.AddModule(new ModuleInfo() { ModuleName = testModule.Name, ModuleType = testModule.AssemblyQualifiedName });

            Type loginModule = typeof(LoginModule);
            moduleCatalog.AddModule(new ModuleInfo() { ModuleName = loginModule.Name, ModuleType = loginModule.AssemblyQualifiedName });

            return moduleCatalog;
        }
        #endregion overrire method
    }
}
