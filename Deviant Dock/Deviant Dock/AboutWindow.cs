using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Deviant_Dock
{
    class AboutWindow : Window
    {
        public AboutWindow()
        {
            this.Title = "About: Deviant Dock";
            this.Width = 378;
            this.Height = 256;
            this.ResizeMode = ResizeMode.NoResize;
            this.ShowInTaskbar = false;
            this.WindowStyle = WindowStyle.ToolWindow;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Visibility = Visibility.Visible;

            AboutStackPanel aboutStackPanel = new AboutStackPanel();

            CustomButton thanksButton = new CustomButton(buttonContent: "Thanks", width: 70, height: 30, thickness: new Thickness(uniformLength: 0));
            thanksButton.Click += new RoutedEventHandler(delegate (object sender, RoutedEventArgs routedEventArgs)
                                                             {
                                                                 this.Close();
                                                             });

            this.Content = aboutStackPanel;
            aboutStackPanel.Children.Add(new TextBlock());
            aboutStackPanel.Children.Add(thanksButton);
        }
    }
}
