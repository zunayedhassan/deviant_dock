using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Deviant_Dock
{
    class DockListViewItem : ListViewItem
    {
        public Button addButton,
                      removeButton;

        public DockListViewItem(string title, string versionNo, string description, string imageLocation)
        {
            DockPanel dockPanel = new DockPanel();

            StackPanel leftStackPanel = new StackPanel();
            leftStackPanel.Orientation = Orientation.Horizontal;
            DockPanel.SetDock(leftStackPanel, Dock.Left);

            StackPanel iconStackPanel = new StackPanel();
            StackPanel textStackPanel = new StackPanel();
            textStackPanel.Orientation = Orientation.Vertical;
            
            StackPanel rightStackPanel = new StackPanel();
            rightStackPanel.Orientation = Orientation.Horizontal;
            rightStackPanel.VerticalAlignment = VerticalAlignment.Center;
            DockPanel.SetDock(rightStackPanel, Dock.Right);

            addButton = new Button();
            addButton.Content = "Add";
            addButton.Width = 60;

            removeButton = new Button();
            removeButton.Content = "Remove";
            removeButton.Width = addButton.Width;

            // Adding contents
            this.Content = dockPanel;
            dockPanel.Children.Add(leftStackPanel);
            leftStackPanel.Children.Add(iconStackPanel);
            iconStackPanel.Children.Add(new CustomImage(imageName: imageLocation, width: 48, height: 48));
            leftStackPanel.Children.Add(new TextBlock()
                                            {
                                                Text = "   "
                                            });
            leftStackPanel.Children.Add(textStackPanel);
            textStackPanel.Children.Add(new TextBlock()
                                            {
                                                Text = title,
                                                FontWeight = FontWeights.Bold
                                            });
            textStackPanel.Children.Add(new TextBlock()
                                            {
                                                Text = "Version: " + versionNo,
                                                FontStyle = FontStyles.Italic
                                            });
            textStackPanel.Children.Add(new TextBlock()
            {
                Text = description
            });
            dockPanel.Children.Add(rightStackPanel);
            rightStackPanel.Children.Add(new TextBlock()
                                             {
                                                 Text = "\t"
                                             });
            rightStackPanel.Children.Add(addButton);
            rightStackPanel.Children.Add(new TextBlock()
                                             {
                                                 Text = "   "
                                             });
            rightStackPanel.Children.Add(removeButton);

            // Adding Event Handler
            addButton.Click += new RoutedEventHandler(addButton_Click);
            removeButton.Click += new RoutedEventHandler(removeButton_Click);
        }

        private void addButton_Click(object sender, EventArgs evnt)
        {
            onOrOff(status: true);
        }

        private void removeButton_Click(object sender, EventArgs evnt)
        {
            onOrOff(status: false);
        }

        public void onOrOff(bool status)
        {
            addButton.IsEnabled = !status;
            removeButton.IsEnabled = status;
        }
    }
}
