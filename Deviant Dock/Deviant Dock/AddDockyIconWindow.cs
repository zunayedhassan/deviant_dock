using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using IWshRuntimeLibrary;
using Button = System.Windows.Controls.Button;
using Label = System.Windows.Controls.Label;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using TextBox = System.Windows.Controls.TextBox;

namespace Deviant_Dock
{
    class AddDockyIconWindow : Window
    {
        private int STANDARD_ICON_DIMENSION = 48;
        public string type;
        public Canvas canvas;

        public Button iconButton,
                      browseButton,
                      saveButton,
                      closeButton;

        public TextBox titleTextBox,
                       targetTextBox;

        public CustomImage iconImage;

        public AddDockyIconWindow(string type)
        {
            this.type = type;

            this.Title = "Add " + this.type;
            this.Width = 5 * 64;
            this.Height = 2 * 64;
            this.ShowInTaskbar = false;
            this.Topmost = true;
            this.ResizeMode = ResizeMode.NoResize;
            this.WindowStyle = WindowStyle.ToolWindow;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.Visibility = Visibility.Visible;

            canvas = new Canvas();
            canvas.Background = new SolidColorBrush(Color.FromRgb(r: 235, g: 235, b: 235));

            iconButton = new Button();
            iconButton.Width = this.Width/5;
            iconButton.Height = this.Height/2;
            iconButton.Margin = new Thickness(uniformLength: 10);

            if (this.type == "File")
                setIconForIconButton("Icons/unknown.png");
            else
                setIconForIconButton("Icons/Folder-close.png");

            Label titleLabel = new Label();
            titleLabel.Content = "Title:";
            titleLabel.Margin = new Thickness(left: ((Thickness)iconButton.Margin).Left + iconButton.Width + 10, top: (0 * 25) + 5, right: 0, bottom: 0);

            Label targetLabel = new Label();
            targetLabel.Content = "Target:";
            targetLabel.Margin = new Thickness(left: ((Thickness) titleLabel.Margin).Left,
                                               top: ((Thickness)titleLabel.Margin).Top + (1 * 25) + 5,
                                               right: 0, bottom: 0);

            titleTextBox = new TextBox();
            titleTextBox.Width = this.Width/2;
            titleTextBox.Margin = new Thickness(left: ((Thickness) titleLabel.Margin).Left + (1 * 50) + 10, top: ((Thickness) titleLabel.Margin).Top + 3, right: 0, bottom: 0);

            targetTextBox = new TextBox();
            targetTextBox.Width = titleTextBox.Width - 40;
            targetTextBox.Margin = new Thickness(left: ((Thickness) titleTextBox.Margin).Left, top: ((Thickness) targetLabel.Margin).Top + 3, right: 0, bottom: 0);

            browseButton = new Button();
            browseButton.Content = "...";
            browseButton.Width = titleTextBox.Width - targetTextBox.Width - (titleTextBox.Width - targetTextBox.Width)/2 + 5;
            browseButton.Margin = new Thickness(left: ((Thickness) targetTextBox.Margin).Left + targetTextBox.Width + browseButton.Width/2, top: ((Thickness) targetLabel.Margin).Top + 3, right: 0, bottom: 0);

            saveButton = new Button();
            saveButton.Content = "Save";
            saveButton.Width = this.Width/4 - 5;
            saveButton.Height = this.Height/5;
            saveButton.Margin = new Thickness(left: ((Thickness) targetTextBox.Margin).Left, top: ((Thickness) targetTextBox.Margin).Top + 25 + 5, right: 0, bottom: 0);

            closeButton = new Button();
            closeButton.Content = "Close";
            closeButton.Width = saveButton.Width;
            closeButton.Height = saveButton.Height;
            closeButton.Margin = new Thickness(left: ((Thickness) saveButton.Margin).Left + saveButton.Width + ((this.Width/4) - saveButton.Width), top: ((Thickness) saveButton.Margin).Top, right: 0, bottom: 0);

            // Adding contents
            this.Content = canvas;
            canvas.Children.Add(iconButton);
            iconButton.Content = iconImage;
            canvas.Children.Add(titleLabel);
            canvas.Children.Add(targetLabel);
            canvas.Children.Add(titleTextBox);
            canvas.Children.Add(targetTextBox);
            canvas.Children.Add(browseButton);
            canvas.Children.Add(saveButton);
            canvas.Children.Add(closeButton);

            // Adding Event Handler
            iconButton.Click += new RoutedEventHandler(iconButton_Click);
            browseButton.Click += new RoutedEventHandler(browseButton_Click);
            closeButton.Click += new RoutedEventHandler(closeButton_Click);
        }

        public void setIconForIconButton(string iconLocation)
        {
            iconImage = new CustomImage(imageName: iconLocation, width: STANDARD_ICON_DIMENSION,
                                            height: STANDARD_ICON_DIMENSION);
            iconButton.Content = iconImage;
            iconButton.ToolTip = new FileInfo(iconImage.imageName).Name;
        }

        private void browseButton_Click(object sender, EventArgs eventArgs)
        {
            if (type == "File")
            {
                OpenFileDialog addFileOpenFileDialog = new OpenFileDialog();
                addFileOpenFileDialog.InitialDirectory =
                    Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);

                Nullable<bool> response = addFileOpenFileDialog.ShowDialog();

                if (response == true)
                {
                    targetTextBox.Text = new LauncherInfo(addFileOpenFileDialog.FileName).target;

                    if (titleTextBox.Text.Trim() == string.Empty)
                        titleTextBox.Text = Path.GetFileNameWithoutExtension(addFileOpenFileDialog.FileName);
                }
            }
            else
            {
                FolderBrowserDialog addFolderBrowserDialog = new FolderBrowserDialog();
                addFolderBrowserDialog.Description = "Add location";
                DialogResult response = addFolderBrowserDialog.ShowDialog();

                if (response == System.Windows.Forms.DialogResult.OK)
                {
                    targetTextBox.Text = addFolderBrowserDialog.SelectedPath;

                    if (titleTextBox.Text.Trim() == string.Empty)
                        titleTextBox.Text = Path.GetFileNameWithoutExtension(addFolderBrowserDialog.SelectedPath);
                }
            }
        }

        private void iconButton_Click(object sender, EventArgs eventArgs)
        {
            OpenFileDialog iconImageOpenFileDialog = new OpenFileDialog();
            iconImageOpenFileDialog.InitialDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Icons");
            iconImageOpenFileDialog.Filter = "Portable Network Graphics (.png)|*.png|Windows Icon (.ico)|*.ico|Joint Pictures Expert Group (.jpg)|*.jpg|Windows Bitmap Image (.bmp)|*.bmp|Graphical Interchange format (.gif)|*.gif|All files (*.*)|*.*";
            Nullable<bool> response = iconImageOpenFileDialog.ShowDialog();

            if (response == true)
            {
                setIconForIconButton(iconImageOpenFileDialog.FileName);
            }
        }

        private void closeButton_Click(object sender, EventArgs eventArgs)
        {
            this.Close();
        }
    }
}
