using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows;
using Wheesper.Chat.Model;

namespace Wheesper.Chat.View
{
    class FriendOrListTemplateSelector : DataTemplateSelector
    {
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            ResourceDictionary directory = new ResourceDictionary();
            directory.Source = new Uri("pack://Application:,,,/GlobeDictionary.xaml", UriKind.RelativeOrAbsolute);
            if (item != null && item is ContactList)
            {
                return directory["ListTemple"] as DataTemplate;
            }
            return directory["FriendTemple"] as DataTemplate;
        }
    }
}
