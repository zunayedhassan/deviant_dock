using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Deviant_Dock
{
    class DockyIconSettingsWindow : Window
    {
        private int STANDARD_BUTTON_WIDTH = 70,
                    STANDARD_BUTTON_HEIGHT = 25,
                    STANDARD_SEPARATOR_DISTANCE = 5,
                    STANDARD_ICON_DIMENSION = 64,
                    STANDARD_SMALL_ICON_DIMENSION = 22;

        public ListView iconsListView;

        public CustomButton addFileButton,
                            addPathButton,
                            addSeparatorButton,
                            editButton,
                            removeButton,
                            moveUpButton,
                            moveDownButton;

        public DockyIconSettingsWindow()
        {
            this.Width = 5*64;
            this.Height = 6*64;
            this.Title = "Icon Settings";
            this.ResizeMode = ResizeMode.NoResize;
            this.ShowInTaskbar = false;
            this.WindowStyle = WindowStyle.ToolWindow;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Visibility = Visibility.Visible;

            Canvas canvas = new Canvas();
            canvas.Background = new LinearGradientBrush(startColor: Colors.White, endColor: Color.FromRgb(r: 221, g: 231, b: 244), angle: -45);

            iconsListView = new ListView();
            iconsListView.Width = this.Width - (8 * (2 * STANDARD_SEPARATOR_DISTANCE)) - STANDARD_BUTTON_WIDTH;
            iconsListView.Height = this.Height - (5 * (2 * STANDARD_SEPARATOR_DISTANCE) - STANDARD_SEPARATOR_DISTANCE);
            iconsListView.SelectionMode = SelectionMode.Single;
            iconsListView.Margin = new Thickness(uniformLength: 2 * STANDARD_SEPARATOR_DISTANCE);

            CustomGroupBox addIconGroupBox = new CustomGroupBox(title: "Add Icon", width: (int)(this.Width - iconsListView.Width - 4 * (2 * STANDARD_SEPARATOR_DISTANCE)), height: (int)(this.Height / 3 + (2 * STANDARD_SEPARATOR_DISTANCE)), thickness: new Thickness(left: ((Thickness)iconsListView.Margin).Left + iconsListView.Width + (2.5 * STANDARD_SEPARATOR_DISTANCE), top: ((Thickness)iconsListView.Margin).Top, right: 0, bottom: 0), baseCanvas: ref canvas);

            addFileButton = new CustomButton(buttonContent: "Add File", width: STANDARD_BUTTON_WIDTH, height: STANDARD_BUTTON_HEIGHT, thickness: new Thickness(left: (3 * STANDARD_SEPARATOR_DISTANCE), top: (2 * STANDARD_SEPARATOR_DISTANCE), right: 0, bottom: 0));
            addPathButton = new CustomButton(buttonContent: "Add Path", width: STANDARD_BUTTON_WIDTH, height: STANDARD_BUTTON_HEIGHT, thickness: new Thickness(left: ((Thickness)addFileButton.Margin).Left, top: ((Thickness)addFileButton.Margin).Top + addFileButton.Height + (2 * STANDARD_SEPARATOR_DISTANCE), right: 0, bottom: 0));
            addSeparatorButton = new CustomButton(buttonContent: "Separator", width: STANDARD_BUTTON_WIDTH, height: STANDARD_BUTTON_HEIGHT, thickness: new Thickness(left: ((Thickness)addPathButton.Margin).Left, top: ((Thickness)addPathButton.Margin).Top + addPathButton.Height + (2 * STANDARD_SEPARATOR_DISTANCE), right: 0, bottom: 0));

            CustomGroupBox organizeIconGroupBox = new CustomGroupBox(title: "Organize", width: (int) addIconGroupBox.Width, height: (int) (this.Height - this.Height/3 - (this.Height/3 - addIconGroupBox.Height) - (16 * STANDARD_SEPARATOR_DISTANCE)), thickness: new Thickness(left: ((Thickness) addIconGroupBox.Margin).Left, top: ((Thickness) addIconGroupBox.Margin).Top + addIconGroupBox.Height + (3 * STANDARD_SEPARATOR_DISTANCE), right: 0, bottom: 0), baseCanvas: ref canvas);

            editButton = new CustomButton(buttonContent: "Edit", width: STANDARD_BUTTON_WIDTH, height: STANDARD_BUTTON_HEIGHT, thickness: new Thickness(left: ((Thickness) addFileButton.Margin).Left, top: ((Thickness) addFileButton.Margin).Top, right: 0, bottom: 0));
            removeButton = new CustomButton(buttonContent: "Remove", width: STANDARD_BUTTON_WIDTH, height: STANDARD_BUTTON_HEIGHT, thickness: new Thickness(left: ((Thickness) editButton.Margin).Left, top: ((Thickness) editButton.Margin).Top + editButton.Height + (2 * STANDARD_SEPARATOR_DISTANCE), right: 0, bottom: 0));
            moveUpButton = new CustomButton(imageIcon: new CustomImage(imageName: "Icons/Toolbar Icon/go-up.png", width: STANDARD_SMALL_ICON_DIMENSION, height: STANDARD_SMALL_ICON_DIMENSION), width: STANDARD_ICON_DIMENSION / 2, height: STANDARD_ICON_DIMENSION/2, thickness: new Thickness(left: (((Thickness)removeButton.Margin).Left * 2.7) - STANDARD_SEPARATOR_DISTANCE, top: ((Thickness)removeButton.Margin).Top + removeButton.Height + (3 * STANDARD_SEPARATOR_DISTANCE), right: 0, bottom: 0));
            moveDownButton = new CustomButton(imageIcon: new CustomImage(imageName: "Icons/Toolbar Icon/go-down.png", width: STANDARD_SMALL_ICON_DIMENSION, height: STANDARD_SMALL_ICON_DIMENSION), width: (int) moveUpButton.Width, height: (int) moveUpButton.Height, thickness: new Thickness(left: ((Thickness) moveUpButton.Margin).Left, top: ((Thickness) moveUpButton.Margin).Top + moveUpButton.Height + STANDARD_SEPARATOR_DISTANCE, right: 0, bottom: 0));

            // Adding Components
            this.Content = canvas;
            canvas.Children.Add(iconsListView);
            addIconGroupBox.groupBoxCanvas.Children.Add(addFileButton);
            addIconGroupBox.groupBoxCanvas.Children.Add(addPathButton);
            addIconGroupBox.groupBoxCanvas.Children.Add(addSeparatorButton);
            organizeIconGroupBox.groupBoxCanvas.Children.Add(editButton);
            organizeIconGroupBox.groupBoxCanvas.Children.Add(removeButton);
            organizeIconGroupBox.groupBoxCanvas.Children.Add(moveUpButton);
            organizeIconGroupBox.groupBoxCanvas.Children.Add(moveDownButton);

            // Initializing
            setEnableOrDisableAllOrganizeIconButtons(status: false);
        }

        public void setEnableOrDisableAllOrganizeIconButtons(bool status)
        {
            editButton.IsEnabled = status;
            removeButton.IsEnabled = status;
            moveUpButton.IsEnabled = status;
            moveDownButton.IsEnabled = status;
        }
    }
}
