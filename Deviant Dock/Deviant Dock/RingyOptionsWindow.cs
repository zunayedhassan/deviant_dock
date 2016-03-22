using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Deviant_Dock
{
    class RingyOptionsWindow : Window
    {
        private int STANDARD_WIDTH = 70,
                    STANDARD_BUTTON_HEIGHT = 25,
                    STANDARD_SEPARATOR_DISTANCE = 5,
                    STANDARD_SMALL_ICON_DIMENSION = 22;

        private CustomGroupBox generalGroupBox,
                               iconSettingsGroupBox,
                               logoImageGroupBox;

        public CustomComboBox themeComboBox,
                              animationEffectComboBox;

        public CheckBox showIconLabelCheckBox;

        private Canvas generalCanvas,
                       logoImageCanvas;

        public Canvas iconSettingsCanvas;

        public CustomImage logoImage;
        public TextBox logoImageLocationTextBox;
        public CustomButton logoImageBrowseButton;

        private CustomButton helpButton,
                             closeButton;

        public CustomButton defaultButton;

        public RingyOptionsWindow(ref List<IconSettings> ringyIconSettings, string logoImageLocation)
        {
            this.Width = 4*64;
            this.Height = 8*64;
            this.ShowInTaskbar = false;
            this.Title = "Ringy Options";
            this.Topmost = true;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStyle = WindowStyle.ToolWindow;
            this.Visibility = Visibility.Visible;
            this.Left = STANDARD_SEPARATOR_DISTANCE * 10;
            this.Top = this.Left;

            GlassEffect.SetIsEnabled(element: this, value: true);

            Canvas baseCanvas = new Canvas();

            Canvas ringyOptionsCanvas = new Canvas();
            ringyOptionsCanvas.Background = new LinearGradientBrush(startColor: Colors.White, endColor: Color.FromRgb(r: 221, g: 231, b: 244), angle: -30);
            ringyOptionsCanvas.Width = this.Width;
            ringyOptionsCanvas.Height = this.Height - ((this.Height)/9);
            ringyOptionsCanvas.Margin = new Thickness(uniformLength: 0);

            generalGroupBox = new CustomGroupBox(title: "General", width: (int) ringyOptionsCanvas.Width - (STANDARD_SEPARATOR_DISTANCE * 3), height: (int) ringyOptionsCanvas.Height/4 - (STANDARD_SEPARATOR_DISTANCE * 0), thickness: new Thickness(uniformLength: STANDARD_SEPARATOR_DISTANCE), baseCanvas: ref ringyOptionsCanvas);
            iconSettingsGroupBox = new CustomGroupBox(title: "Icon Settings", width: (int)generalGroupBox.Width, height: (int)(ringyOptionsCanvas.Height - generalGroupBox.Height - (STANDARD_SEPARATOR_DISTANCE * 14)), thickness: new Thickness(left: ((Thickness)generalGroupBox.Margin).Left, top: ((Thickness)generalGroupBox.Margin).Top + generalGroupBox.Height + (STANDARD_SEPARATOR_DISTANCE * 1), right: 0, bottom: 0), baseCanvas: ref ringyOptionsCanvas);
            logoImageGroupBox = new CustomGroupBox(title: "Logo Image", width: (int) iconSettingsGroupBox.Width, height: (int) (this.Height - generalGroupBox.Height - iconSettingsGroupBox.Height - (STANDARD_SEPARATOR_DISTANCE * 6))/2, thickness: new Thickness(left: ((Thickness) iconSettingsGroupBox.Margin).Left, top: ((Thickness) iconSettingsGroupBox.Margin).Top + iconSettingsGroupBox.Height + (STANDARD_SEPARATOR_DISTANCE * 1), right: 0, bottom: 0), baseCanvas: ref ringyOptionsCanvas);

            generalCanvas = new Canvas();

            CustomLabel themeLabel = new CustomLabel(text: "Theme:", thickness: new Thickness(uniformLength: STANDARD_SEPARATOR_DISTANCE  * 1));
            CustomLabel animationEffectLabel = new CustomLabel(text: "Animation Effect:", thickness: new Thickness(left: ((Thickness)themeLabel.Margin).Left, top: ((Thickness)themeLabel.Margin).Top + (STANDARD_SEPARATOR_DISTANCE * 6), right: 0, bottom: 0));

            themeComboBox = new CustomComboBox(items: getThemeName(themeLocation: Directory.GetFiles(path: "Background")), width: (int) (STANDARD_WIDTH * 1.5), thickness: new Thickness(left: STANDARD_WIDTH * 1.7, top: ((Thickness) themeLabel.Margin).Top, right: 0, bottom: 0));
            animationEffectComboBox = new CustomComboBox(items: new [] { "Zoom", "Fade", "Rotate" }, width: (int) themeComboBox.Width, thickness: new Thickness(left: ((Thickness) themeComboBox.Margin).Left, top: ((Thickness) animationEffectLabel.Margin).Top, right: 0, bottom: 0));

            showIconLabelCheckBox = new CheckBox();
            showIconLabelCheckBox.Content = "Show Icon Label";
            showIconLabelCheckBox.Margin = new Thickness(left: ((Thickness) animationEffectLabel.Margin).Left + STANDARD_SEPARATOR_DISTANCE, top: ((Thickness) animationEffectLabel.Margin).Top + (STANDARD_SEPARATOR_DISTANCE * 7), right: 0, bottom: 0);

            helpButton = new CustomButton(buttonContent: "Help", width: STANDARD_WIDTH, height: STANDARD_BUTTON_HEIGHT, thickness: new Thickness(left: 0, top: ringyOptionsCanvas.Height + (STANDARD_SEPARATOR_DISTANCE * 1), right: 0, bottom: 0));
            closeButton = new CustomButton(buttonContent: "Close", width: (int) helpButton.Width, height: (int) helpButton.Height, thickness: new Thickness(left: this.Width - helpButton.Width - (STANDARD_SEPARATOR_DISTANCE * 1) - 1, top: ((Thickness) helpButton.Margin).Top, right: 0, bottom: 0));
            defaultButton = new CustomButton(buttonContent: "Default", width: (int) closeButton.Width, height: (int) closeButton.Height, thickness: new Thickness(left: ((Thickness) closeButton.Margin).Left - closeButton.Width - STANDARD_SEPARATOR_DISTANCE, top: ((Thickness) closeButton.Margin).Top, right: 0, bottom: 0));

            iconSettingsCanvas = new Canvas();
            logoImageCanvas = new Canvas();

            // Adding Contents
            this.Content = baseCanvas;
            baseCanvas.Children.Add(ringyOptionsCanvas);
            generalGroupBox.Content = generalCanvas;
            generalCanvas.Children.Add(themeLabel);
            generalCanvas.Children.Add(themeComboBox);
            generalCanvas.Children.Add(animationEffectLabel);
            generalCanvas.Children.Add(animationEffectComboBox);
            generalCanvas.Children.Add(showIconLabelCheckBox);
            iconSettingsGroupBox.Content = iconSettingsCanvas;
            logoImageGroupBox.Content = logoImageCanvas;
            baseCanvas.Children.Add(helpButton);
            baseCanvas.Children.Add(closeButton);
            baseCanvas.Children.Add(defaultButton);

            // Addings Event Handler
            helpButton.Click += new RoutedEventHandler(helpButton_Click);
            closeButton.Click += new RoutedEventHandler(closeButton_Click);

            setLogoImageComponents(logoImageLocation);
        }

        private string[] getThemeName(string[] themeLocation)
        {
            string[] themeName = new string[themeLocation.Length];
            int themeNo = 0;

            foreach (var theme in themeLocation)
                themeName[themeNo++] = ((new FileInfo(fileName: theme)).Name).Split('.')[0];        // If file name is "Black.png", then we will get only "Black" as our theme name

            return themeName;
        }

        public void setLogoImageComponents(string logoImageLocation)
        {
            logoImageCanvas.Children.Clear();

            logoImage = new CustomImage(imageName: logoImageLocation, width: STANDARD_SMALL_ICON_DIMENSION, height: STANDARD_SMALL_ICON_DIMENSION);
            logoImage.Margin = new Thickness(uniformLength: STANDARD_SEPARATOR_DISTANCE / 2);

            logoImageLocationTextBox = new TextBox();
            logoImageLocationTextBox.Text = logoImageLocation;
            logoImageLocationTextBox.Width = logoImageGroupBox.Width - (STANDARD_SEPARATOR_DISTANCE * 16);
            logoImageLocationTextBox.Margin = new Thickness(left: ((Thickness)logoImage.Margin).Left + STANDARD_SMALL_ICON_DIMENSION + (STANDARD_SEPARATOR_DISTANCE * 2), top: ((Thickness)logoImage.Margin).Top, right: 0, bottom: 0);

            logoImageBrowseButton = new CustomButton(buttonContent: "...", width: (STANDARD_SEPARATOR_DISTANCE * 4), height: (STANDARD_SEPARATOR_DISTANCE * 4), thickness: new Thickness(left: ((Thickness)logoImageLocationTextBox.Margin).Left + logoImageLocationTextBox.Width + (STANDARD_SEPARATOR_DISTANCE * 2), top: ((Thickness)logoImageLocationTextBox.Margin).Top, right: 0, bottom: 0));

            logoImageCanvas.Children.Add(logoImage);
            logoImageCanvas.Children.Add(logoImageLocationTextBox);
            logoImageCanvas.Children.Add(logoImageBrowseButton);
        }

        private void helpButton_Click(object sender, EventArgs eventArgs)
        {
            try
            {
                System.Diagnostics.Process.Start("help.chm");
            }
            catch (Exception)
            {
                MessageBox.Show(messageBoxText: "help.chm file can not be found!", caption: "ERROR", button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }

        private void closeButton_Click(object sender, EventArgs eventArgs)
        {
            this.Close();
        }
    }
}
