using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Deviant_Dock
{
    class RingyIconConfigurationCheckBox : CheckBox
    {
        private int STANDARD_SEPARATOR_DISTANCE = 5,
                    STANDARD_SMALL_ICON_DIMENSION = 22,
                    STANDARD_MAX_SHORT_TARGET_LENGTH = 20;

        public string iconLocation,
                      iconTitle,
                      target;

        private string shortTarget;

        public int iconNo;

        private StackPanel mainStackPanel,
                           iconDescriptionStackPanel;

        private static int top = 5;
        public CustomButton configureButton;
        public AddRingyIconWindow addRingyIconWindow;

        public RingyIconConfigurationCheckBox(string iconLocation, string iconTitle, string target, int iconNo, ref Canvas baseCanvas, bool status)
        {
            this.Width = 5 * 64;
            this.Height = ((STANDARD_SEPARATOR_DISTANCE * STANDARD_SEPARATOR_DISTANCE) * 2) + STANDARD_SEPARATOR_DISTANCE;
            this.Margin = new Thickness(left: STANDARD_SEPARATOR_DISTANCE, top: top, right: 0, bottom: 0);
            this.IsChecked = status;

            if (top == 5 + (30 * 7))
                top = 5;
            else
                top += 30;

            this.iconLocation = iconLocation;
            this.iconTitle = iconTitle;
            this.target = target;
            this.iconNo = iconNo;

            mainStackPanel = new StackPanel();
            mainStackPanel.Orientation = Orientation.Horizontal;

            configureButton = new CustomButton(buttonContent: new CustomImage("Icons/Toolbar Icon/configure.png", 16, 16), width: (STANDARD_SEPARATOR_DISTANCE * 4), height: (STANDARD_SEPARATOR_DISTANCE * 4), thickness: new Thickness(uniformLength: 0));

            setContents(this.iconLocation, this.iconTitle, this.target);

            baseCanvas.Children.Add(this);

            this.Click += new RoutedEventHandler(RingyIconConfigurationCheckBox_Click);
        }

        public void setContents(string iconLocation, string iconTitle, string target)
        {
            mainStackPanel.Children.Clear();

            iconDescriptionStackPanel = new StackPanel();
            iconDescriptionStackPanel.Orientation = Orientation.Vertical;

            if (target.Length <= STANDARD_MAX_SHORT_TARGET_LENGTH)
                this.shortTarget = target;
            else
                this.shortTarget = target.Substring(startIndex: 0, length: STANDARD_MAX_SHORT_TARGET_LENGTH) +
                                   "...";

            this.Content = mainStackPanel;
            mainStackPanel.Children.Add(new CustomImage(imageName: iconLocation, width: STANDARD_SMALL_ICON_DIMENSION,
                                                        height: STANDARD_SMALL_ICON_DIMENSION));
            mainStackPanel.Children.Add(new TextBlock()
                                            {
                                                Text = " "
                                            });
            mainStackPanel.Children.Add(iconDescriptionStackPanel);
            iconDescriptionStackPanel.Children.Add(new TextBlock()
            {
                Text = this.iconNo + ". " + iconTitle,
                FontWeight = FontWeights.Bold
            });
            iconDescriptionStackPanel.Children.Add(new TextBlock()
            {
                Text = this.shortTarget,
                FontFamily = new FontFamily(familyName: "Consolas")
            });
            mainStackPanel.Children.Add(new TextBlock()
                                            {
                                                Text = "  "
                                            });
            mainStackPanel.Children.Add(configureButton);
        }

        private void RingyIconConfigurationCheckBox_Click(object sender, EventArgs eventArgs)
        {
            if (this.IsChecked == true)
                mainStackPanel.IsEnabled = true;
            else
                mainStackPanel.IsEnabled = false;
        }
    }
}
