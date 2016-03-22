using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Deviant_Dock
{
    class DockyIconSettingsListViewItem : ListViewItem
    {
        private int STANARD_ICON_DIMENSION = 64;
        public CustomImage iconImage;

        public TextBlock titleTextBlock,
                         targetTextBlock;

        public DockyIconSettingsListViewItem(string imageLocation, string iconTitle, string target)
        {
            StackPanel baseStackPanel = new StackPanel();
            baseStackPanel.Orientation = Orientation.Horizontal;

            iconImage = new CustomImage(imageName: imageLocation, width: STANARD_ICON_DIMENSION/2, height: STANARD_ICON_DIMENSION/2);

            StackPanel descriptionStackPanel = new StackPanel();
            descriptionStackPanel.Orientation = Orientation.Vertical;

            titleTextBlock = new TextBlock()
                                 {
                                     Text = iconTitle,
                                     FontWeight = FontWeights.Bold
                                 };

            targetTextBlock = new TextBlock()
                                  {
                                      Text = target,
                                  };

            this.Content = baseStackPanel;
            baseStackPanel.Children.Add(iconImage);
            baseStackPanel.Children.Add(new TextBlock()
                                            {
                                                Text = "  "
                                            });
            baseStackPanel.Children.Add(descriptionStackPanel);
            descriptionStackPanel.Children.Add(titleTextBlock);
            descriptionStackPanel.Children.Add(targetTextBlock);
        }

        public DockyIconSettingsListViewItem()
        {
            StackPanel baseStackPanel = new StackPanel();
            baseStackPanel.Orientation = Orientation.Horizontal;

            StackPanel titleStackPanel = new StackPanel();
            titleStackPanel.Orientation = Orientation.Vertical;

            titleTextBlock = new TextBlock();
            titleTextBlock.Text = "<-- Separator -->";

            this.Content = baseStackPanel;
            baseStackPanel.Children.Add(new TextBlock()
                                            {
                                                Text = "         "
                                            });
            baseStackPanel.Children.Add(titleStackPanel);
            titleStackPanel.Children.Add(new TextBlock()
                                            {
                                                Text = string.Empty
                                            });
            titleStackPanel.Children.Add(titleTextBlock);
            titleStackPanel.Children.Add(new TextBlock()
                                            {
                                                Text = string.Empty
                                            });
        }
    }
}
