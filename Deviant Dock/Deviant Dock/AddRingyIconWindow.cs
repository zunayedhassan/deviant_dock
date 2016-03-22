using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Deviant_Dock
{
    class AddRingyIconWindow : AddDockyIconWindow
    {
        private CustomComboBox launcherTypeComboBox;

        public AddRingyIconWindow() : base("File")
        {
            double initialWidth = this.Width;

            this.Width = 6*64;
            this.Title = "Configure Launcher";
            this.WindowStartupLocation = WindowStartupLocation.CenterOwner;

            launcherTypeComboBox = new CustomComboBox(items: new[] { "File", "Path" }, width: (int)(this.Width - initialWidth), thickness: new Thickness(left: ((Thickness)targetTextBox.Margin).Left, top: ((Thickness)targetTextBox.Margin).Top, right: 0, bottom: 0));
            launcherTypeComboBox.SelectedIndex = 0;

            targetTextBox.Width -= 5;

            titleTextBox.Width += this.Width - initialWidth;
            targetTextBox.Margin = new Thickness(left: ((Thickness)targetTextBox.Margin).Left + (this.Width - initialWidth) + 10, top: ((Thickness)targetTextBox.Margin).Top, right: 0, bottom: 0);
            browseButton.Margin = new Thickness(left: ((Thickness)browseButton.Margin).Left + this.Width - initialWidth, top: ((Thickness)browseButton.Margin).Top, right: 0, bottom: 0);
            saveButton.Margin = new Thickness(left: ((Thickness)saveButton.Margin).Left + this.Width - initialWidth, top: ((Thickness)saveButton.Margin).Top, right: 0, bottom: 0);
            closeButton.Margin = new Thickness(left: ((Thickness)closeButton.Margin).Left + this.Width - initialWidth, top: ((Thickness)closeButton.Margin).Top, right: 0, bottom: 0);

            canvas.Children.Add(launcherTypeComboBox);

            launcherTypeComboBox.SelectionChanged += new SelectionChangedEventHandler(launcherTypeComboBox_SelectionChanged);
        }

        private void launcherTypeComboBox_SelectionChanged(object sender, EventArgs eventArgs)
        {
            this.type = launcherTypeComboBox.SelectedValue.ToString();
        }
    }
}
