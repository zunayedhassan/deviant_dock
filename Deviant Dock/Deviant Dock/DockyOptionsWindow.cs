using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.IO;

namespace Deviant_Dock
{
    class DockyOptionsWindow : Window
    {
        private int STANDARD_WIDTH = 70,
                    STANDARD_BUTTON_HEIGHT = 25,
                    STANDARD_BUTTON_SEPARATOR = 6;

        public DockyOptionsTabItem positionDockyOptionsTabItem,
                                   styleDockyOptionsTabItem;

        private DockyOptionsTabItem aboutDockyOptionsTabItem;

        private CustomButton helpButton,
                             closeButton;

        public CustomButton defaultButton;

        public CustomComboBox screenPositionComboBox,
                              layeringComboBox,
                              themeComboBox,
                              hoaverEffectComboBox,
                              clickEffectComboBox;

        public CustomSlider centeringSlider,
                            edgeOffsetSlider;

        public CheckBox showIconLabelCheckBox;

        private string[] screenPositions = { "Top",
                                             "Bottom",
                                             "Left",
                                             "Right" };

        private string[] layering = { "Normal",
                                      "Topmost" };

        private string[] animationEffects = { "Zoom",
                                              "Swing",
                                              "Fade",
                                              "Rotate" };

        public DockyOptionsWindow()
        {
            this.Width = 6*64;
            this.Height = 5*64;
            this.ResizeMode = ResizeMode.NoResize;
            this.ShowInTaskbar = false;
            this.Title = "Docky Options";
            this.Topmost = true;
            this.WindowStyle = WindowStyle.ToolWindow;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Visibility = Visibility.Visible;
            GlassEffect.SetIsEnabled(element: this, value: true);

            Canvas mainCanvas = new Canvas();

            TabControl categoryTabControl = new TabControl();
            categoryTabControl.Width = this.Width - (this.Width/(this.Width/6));
            categoryTabControl.Height = this.Height - (this.Height/5);

            positionDockyOptionsTabItem = new DockyOptionsTabItem(title: "Position", iconLocation: "Icons/Toolbar Icon/step.png", tabControl: ref categoryTabControl);
            styleDockyOptionsTabItem = new DockyOptionsTabItem(title: "Style", iconLocation: "Icons/Toolbar Icon/style.png", tabControl: ref categoryTabControl);
            aboutDockyOptionsTabItem = new DockyOptionsTabItem(title: "About", iconLocation: "Icons/Toolbar Icon/info.png", tabControl: ref categoryTabControl);

            Canvas positionDockyOptionsCanvas = new Canvas();
            positionDockyOptionsCanvas.Background = new LinearGradientBrush(startColor: Colors.White, endColor: Color.FromRgb(r: 235, g: 235, b: 235), angle: -45);

            CustomLabel screenPositionLabel = new CustomLabel(text: "Screen Positions:", thickness: new Thickness(uniformLength: 10));
            CustomLabel layeringLabel = new CustomLabel(text: "Layering:", thickness: new Thickness(left: ((Thickness)screenPositionLabel.Margin).Left, top: ((Thickness)screenPositionLabel.Margin).Top + (1 * 30), right: 0, bottom: 0));
            CustomLabel centeringLabel = new CustomLabel(text: "Centering:", thickness: new Thickness(left: ((Thickness)layeringLabel.Margin).Left, top: ((Thickness)layeringLabel.Margin).Top + (2 * 30), right: 0, bottom: 0));
            CustomLabel edgeOffsetLabel = new CustomLabel(text: "Edge Offset:", thickness: new Thickness(left: ((Thickness)centeringLabel.Margin).Left, top: ((Thickness)centeringLabel.Margin).Top + (1 * 30), right: 0, bottom: 0));

            screenPositionComboBox = new CustomComboBox(items: screenPositions, width: STANDARD_WIDTH * 2, thickness: new Thickness(left: ((Thickness)screenPositionLabel.Margin).Left + 120, top: ((Thickness)screenPositionLabel.Margin).Top + 3, right: 0, bottom: 0));
            layeringComboBox = new CustomComboBox(items: layering, width: (int) screenPositionComboBox.Width, thickness: new Thickness(left: ((Thickness) screenPositionComboBox.Margin).Left, top: ((Thickness) layeringLabel.Margin).Top + 3, right: 0, bottom: 0));

            centeringSlider = new CustomSlider(width: STANDARD_WIDTH * 3, startValue: -100, endValue: 100, thickness: new Thickness(left: ((Thickness) layeringComboBox.Margin).Left - 5, top: ((Thickness) centeringLabel.Margin).Top, right: 0, bottom: 0));
            edgeOffsetSlider = new CustomSlider(width: (int)centeringSlider.Width, startValue: -15, endValue: 128, thickness: new Thickness(left: ((Thickness)layeringComboBox.Margin).Left - 5, top: ((Thickness) edgeOffsetLabel.Margin).Top, right: 0, bottom: 0));

            Canvas styleDockyOptionsCanvas = new Canvas();
            styleDockyOptionsCanvas.Background = new LinearGradientBrush(startColor: Colors.White, endColor: Color.FromRgb(r: 235, g: 235, b: 235), angle: -45);

            CustomLabel themeLabel = new CustomLabel(text: "Theme:", thickness: new Thickness(uniformLength: 10));
            CustomLabel hoaverEffectLabel = new CustomLabel(text: "Hoaver Effect:", thickness: new Thickness(left: ((Thickness)themeLabel.Margin).Left, top: ((Thickness)themeLabel.Margin).Top + (1 * 30), right: 0, bottom: 0));
            CustomLabel clickEffectLabel = new CustomLabel(text: "Click Effect:", thickness: new Thickness(left: ((Thickness)hoaverEffectLabel.Margin).Left, top: ((Thickness)hoaverEffectLabel.Margin).Top + 30, right: 0, bottom: 0));


            themeComboBox = new CustomComboBox(items: getThemeName(themeDirectories: Directory.GetDirectories(Directory.GetCurrentDirectory() + @"\Skins")), width: STANDARD_WIDTH * 2, thickness: new Thickness(left: ((Thickness)themeLabel.Margin).Left + 120, top: ((Thickness)themeLabel.Margin).Top + 3, right: 0, bottom: 0));
            hoaverEffectComboBox = new CustomComboBox(items: animationEffects, width: STANDARD_WIDTH * 2, thickness: new Thickness(left: ((Thickness) themeComboBox.Margin).Left, top: ((Thickness) hoaverEffectLabel.Margin).Top + 3, right: 0, bottom: 0));
            clickEffectComboBox = new CustomComboBox(items: animationEffects, width: STANDARD_WIDTH * 2, thickness: new Thickness(left: ((Thickness) hoaverEffectComboBox.Margin).Left, top: ((Thickness) clickEffectLabel.Margin).Top + 3, right: 0, bottom: 0));

            showIconLabelCheckBox = new CheckBox();
            showIconLabelCheckBox.Content = "Show Icon Label";
            showIconLabelCheckBox.Margin = new Thickness(left: ((Thickness) clickEffectLabel.Margin).Left, top: ((Thickness) clickEffectLabel.Margin).Top + (3 * 30), right: 0, bottom: 0);

            helpButton = new CustomButton(buttonContent: "Help", width: STANDARD_WIDTH, height: STANDARD_BUTTON_HEIGHT, thickness: new Thickness(left: 0, top: this.Height - (2 * STANDARD_BUTTON_HEIGHT), right: 0, bottom: 0));
            defaultButton = new CustomButton(buttonContent: "Default", width: STANDARD_WIDTH, height: STANDARD_BUTTON_HEIGHT, thickness: new Thickness(left: this.Width - (2 * STANDARD_WIDTH) - (2 * STANDARD_BUTTON_SEPARATOR), top: ((Thickness)helpButton.Margin).Top, right: 0, bottom: 0));
            closeButton = new CustomButton(buttonContent: "Close", width: STANDARD_WIDTH, height: STANDARD_BUTTON_HEIGHT, thickness: new Thickness(left: this.Width - (1 * STANDARD_WIDTH) - (1 * STANDARD_BUTTON_SEPARATOR), top: ((Thickness)defaultButton.Margin).Top, right: 0, bottom: 0));

            // Adding contents
            this.Content = mainCanvas;
            mainCanvas.Children.Add(categoryTabControl);
            mainCanvas.Children.Add(helpButton);
            mainCanvas.Children.Add(defaultButton);
            mainCanvas.Children.Add(closeButton);
            positionDockyOptionsTabItem.Content = positionDockyOptionsCanvas;
            positionDockyOptionsCanvas.Children.Add(screenPositionLabel);
            positionDockyOptionsCanvas.Children.Add(screenPositionComboBox);
            positionDockyOptionsCanvas.Children.Add(layeringLabel);
            positionDockyOptionsCanvas.Children.Add(layeringComboBox);
            positionDockyOptionsCanvas.Children.Add(centeringLabel);
            positionDockyOptionsCanvas.Children.Add(centeringSlider);
            positionDockyOptionsCanvas.Children.Add(edgeOffsetLabel);
            positionDockyOptionsCanvas.Children.Add(edgeOffsetSlider);
            styleDockyOptionsTabItem.Content = styleDockyOptionsCanvas;
            styleDockyOptionsCanvas.Children.Add(themeLabel);
            styleDockyOptionsCanvas.Children.Add(themeComboBox);
            styleDockyOptionsCanvas.Children.Add(hoaverEffectLabel);
            styleDockyOptionsCanvas.Children.Add(hoaverEffectComboBox);
            styleDockyOptionsCanvas.Children.Add(clickEffectLabel);
            styleDockyOptionsCanvas.Children.Add(clickEffectComboBox);
            styleDockyOptionsCanvas.Children.Add(showIconLabelCheckBox);
            aboutDockyOptionsTabItem.Content = new AboutStackPanel();

            // Adding Event Handler
            this.MouseLeftButtonDown += new MouseButtonEventHandler(DockyOptionsWindow_MouseLeftButtonDown);
            helpButton.Click += new RoutedEventHandler(helpButton_Click);
            closeButton.Click += new RoutedEventHandler(closeButton_Click);
        }

        private string[] getThemeName(string[] themeDirectories)
        {
            string[] themeName = new string[themeDirectories.Length];
            int themeNo = 0;

            foreach (var themeDirectory in themeDirectories)
            {
                string[] themeDirectoryNameWithFullPath = themeDirectory.Split('\\');
                themeName[themeNo++] = themeDirectoryNameWithFullPath[themeDirectoryNameWithFullPath.Length - 1];
            }

            return themeName;
        }

        private void DockyOptionsWindow_MouseLeftButtonDown(object sender, EventArgs eventArgs)
        {
            this.DragMove();
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
