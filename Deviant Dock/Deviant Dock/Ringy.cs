using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Microsoft.Win32;

namespace Deviant_Dock
{
    internal class Ringy : Window
    {
        private int STANDARD_ICON_DIMENSION = 8*12,
                    STANDARD_BACKGROUND_IMAGE_DIMENSION = 354,
                    TOTAL_NO_OF_GRID_ROW_COLUMN = 3;

        private string settingsPath = GlobalSettingsPath.path;
        private PrimaryRingySettings currentPrimaryRingySettings;
        private Canvas mainRingyCanvas;
        private StackPanel backgroundStackPanel;
        private Grid iconGrid;
        private List<IconSettings> currentRingyIconSettings;
        private CustomImage logoImage;

        private CustomMenuItem optionsMenuItem,
                               animationEffectMenuItem,
                               zoomAnimationEffectMenuItem,
                               fadeAnimationEffectMenuItem,
                               rotateAnimationEffectMenuItem,
                               showIconLabelMenuItem,
                               helpMenuItem,
                               contentsMenuItem,
                               aboutMenuItem;

        private RingyOptionsWindow ringyOptionsWindow;
        private RingyIconConfigurationCheckBox[] ringyIconConfigurationCheckBox = new RingyIconConfigurationCheckBox[8];

        public Ringy()
        {
            this.Width = STANDARD_ICON_DIMENSION*4.5;
            this.Height = this.Width;
            this.ShowInTaskbar = false;
            this.WindowStyle = WindowStyle.None;
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.AllowsTransparency = true;
            this.Background = new SolidColorBrush(Colors.Transparent);

            mainRingyCanvas = new Canvas();

            backgroundStackPanel = new StackPanel();
            backgroundStackPanel.Margin =
                new Thickness(uniformLength: (this.Width - STANDARD_BACKGROUND_IMAGE_DIMENSION)/2);

            iconGrid = new Grid();
            //iconGrid.ShowGridLines = true;                              // NOTE: For debugging purpose only
            iconGrid.Width = this.Width;
            iconGrid.Height = this.Height;

            ColumnDefinition[] iconGridColumnDefinition = new ColumnDefinition[TOTAL_NO_OF_GRID_ROW_COLUMN];
            RowDefinition[] iconGridRowDefinition = new RowDefinition[TOTAL_NO_OF_GRID_ROW_COLUMN];

            for (int i = 0; i < TOTAL_NO_OF_GRID_ROW_COLUMN; i++)
            {
                iconGridColumnDefinition[i] = new ColumnDefinition();
                iconGridRowDefinition[i] = new RowDefinition();

                iconGrid.ColumnDefinitions.Add(iconGridColumnDefinition[i]);
                iconGrid.RowDefinitions.Add(iconGridRowDefinition[i]);
            }

            ContextMenu ringyContextMenu = new ContextMenu();

            optionsMenuItem = new CustomMenuItem(text: "Options", contextMenu: ref ringyContextMenu);
            ringyContextMenu.Items.Add(new System.Windows.Controls.Separator());
            animationEffectMenuItem = new CustomMenuItem(text: "Animation Effect", contextMenu: ref ringyContextMenu);
            zoomAnimationEffectMenuItem = new CustomMenuItem(text: "Zoom", menuItem: ref animationEffectMenuItem);
            fadeAnimationEffectMenuItem = new CustomMenuItem(text: "Fade", menuItem: ref animationEffectMenuItem);
            rotateAnimationEffectMenuItem = new CustomMenuItem(text: "Rotate", menuItem: ref animationEffectMenuItem);
            showIconLabelMenuItem = new CustomMenuItem(text: "Show Icon Label", contextMenu: ref ringyContextMenu);
            ringyContextMenu.Items.Add(new System.Windows.Controls.Separator());
            helpMenuItem = new CustomMenuItem(text: "Help", contextMenu: ref ringyContextMenu);
            contentsMenuItem = new CustomMenuItem(text: "Contents", menuItem: ref helpMenuItem);
            aboutMenuItem = new CustomMenuItem(text: "About", menuItem: ref helpMenuItem);

            // Adding Contents
            this.Content = mainRingyCanvas;
            mainRingyCanvas.Children.Add(backgroundStackPanel);
            mainRingyCanvas.Children.Add(iconGrid);
            this.ContextMenu = ringyContextMenu;

            // Initializing Settings
            refreshRingy();

            // Adding Event Handler
            optionsMenuItem.Click += new RoutedEventHandler(optionsMenuItem_Click);
            zoomAnimationEffectMenuItem.Click += new RoutedEventHandler(AnimationEffectMenuItem_Click);
            fadeAnimationEffectMenuItem.Click += new RoutedEventHandler(AnimationEffectMenuItem_Click);
            rotateAnimationEffectMenuItem.Click += new RoutedEventHandler(AnimationEffectMenuItem_Click);
            showIconLabelMenuItem.Click += new RoutedEventHandler(showIconLabelMenuItem_Click);
            contentsMenuItem.Click += new RoutedEventHandler(contentsMenuItem_Click);
            aboutMenuItem.Click += new RoutedEventHandler(aboutMenuItem_Click);
        }

        private void loadPrimaryRingySettings()
        {
            if (!File.Exists(path: settingsPath + "PrimaryRingySettings.dds"))
                createNewPrimaryRingySettings();

            // Loading settings from PrimaryRingySettings.dds file
            try
            {
                FileStream primaryRingySettingsInput = new FileStream(settingsPath + "PrimaryRingySettings.dds",
                                                                      FileMode.Open, FileAccess.Read);
                BinaryFormatter primaryRingySettingsReader = new BinaryFormatter();
                currentPrimaryRingySettings = new PrimaryRingySettings();

                currentPrimaryRingySettings =
                    (PrimaryRingySettings) primaryRingySettingsReader.Deserialize(primaryRingySettingsInput);
                primaryRingySettingsInput.Close();
            }
            catch (Exception loadPrimaryRingySettingsException)
            {
                MessageBox.Show(
                    messageBoxText:
                        "Can't open/find PrimaryRingySettings.dds file in Settings directory. PrimaryRingySettings.dds file might be corrupted or deleted.",
                    caption: "ERROR", button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }

        private void createNewPrimaryRingySettings()
        {
            try
            {
                currentPrimaryRingySettings = new PrimaryRingySettings();
                savePrimaryRingySettings();
            }
            catch (Exception createNewPrimaryRingySettingsException)
            {
                MessageBox.Show(
                    messageBoxText:
                        "Can't create PrimaryRingySettings.dds file in Settings directory. Make sure that the filesystem isn't write protected.",
                    caption: "ERROR", button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }

        private void savePrimaryRingySettings()
        {
            try
            {
                FileStream primaryRingySettingsFileStream = new FileStream(settingsPath + "PrimaryRingySettings.dds",
                                                                           FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    // *.dds means 'Deviant Dock Settings'
                BinaryFormatter primaryRingySettingsFormatter = new BinaryFormatter();

                primaryRingySettingsFormatter.Serialize(primaryRingySettingsFileStream, currentPrimaryRingySettings);
                primaryRingySettingsFileStream.Close();
            }
            catch (Exception savePrimaryRingySettingsException)
            {
                MessageBox.Show(
                    messageBoxText:
                        "Can't save settings in PrimaryRingySettings.dds file. Make sure that, PrimaryRingySettings.dds file is realy exists in Settings directory or the system isn't write protected!",
                    caption: "ERROR", button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }

        private void createNewRingyIconSettings()
        {
            currentRingyIconSettings = new List<IconSettings>();

            try
            {
                currentRingyIconSettings.Add(new IconSettings(imageLocation: "Icons/My-Computer.png",
                                                              iconTitle: "My Computer",
                                                              target: @"::{20D04FE0-3AEA-1069-A2D8-08002B30309D}",
                                                              iconNo: 1));
                currentRingyIconSettings.Add(new IconSettings(imageLocation: "Icons/My-Documents.png",
                                                              iconTitle: "My Documenyts",
                                                              target:
                                                                  @"::{031E4825-7B94-4DC3-B131-E946B44C8DD5}\Documents.library-ms",
                                                              iconNo: 2));
                currentRingyIconSettings.Add(new IconSettings(imageLocation: "Icons/ie.png",
                                                              iconTitle: "Internet Explorer",
                                                              target: @"C:\Program Files\Internet Explorer\iexplore.exe",
                                                              iconNo: 3));
                currentRingyIconSettings.Add(new IconSettings(imageLocation: "Icons/win_media_player.png",
                                                              iconTitle: "Windows Media Player",
                                                              target:
                                                                  @"C:\Program Files\Windows Media Player\wmplayer.exe",
                                                              iconNo: 4));
                currentRingyIconSettings.Add(new IconSettings(imageLocation: "Icons/notepad.png", iconTitle: "Notepad",
                                                              target: @"C:\Windows\system32\notepad.exe", iconNo: 5));
                currentRingyIconSettings.Add(new IconSettings(imageLocation: "Icons/Trashcan_full.png",
                                                              iconTitle: "Recycle Bin",
                                                              target: @"::{645FF040-5081-101B-9F08-00AA002F954E}",
                                                              iconNo: 6));
                currentRingyIconSettings.Add(new IconSettings(imageLocation: "Icons/calculator.png",
                                                              iconTitle: "Calculator",
                                                              target: @"C:\Windows\system32\calc.exe", iconNo: 7));
                currentRingyIconSettings.Add(new IconSettings(imageLocation: "Icons/paint.png", iconTitle: "Paint",
                                                              target: @"C:\Windows\system32\mspaint.exe", iconNo: 8));

                saveRingyIconSettings();
            }
            catch (Exception ringyIconSettingsException)
            {
                MessageBox.Show(
                    messageBoxText:
                        "Can't create RingyIconSettings.dds file in Settings directory. Make sure that the filesystem isn't write protected.",
                    caption: "ERROR", button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }

        private void loadRingyIcons()
        {
            if (!File.Exists(path: settingsPath + "RingyIconSettings.dds"))
                createNewRingyIconSettings();

            // Loading settings from RingyIconSettings.dds file
            try
            {
                using (
                    Stream ringyIconSettingsStream = File.Open(path: settingsPath + "RingyIconSettings.dds",
                                                               mode: FileMode.Open))
                {
                    BinaryFormatter ringyIconSettingsFormatter = new BinaryFormatter();
                    currentRingyIconSettings =
                        (List<IconSettings>) ringyIconSettingsFormatter.Deserialize(ringyIconSettingsStream);

                    iconGrid.Children.Clear();

                    foreach (var iconSettings in currentRingyIconSettings)
                    {
                        if (iconSettings.iconTitle != string.Empty)
                        {
                            iconGrid.Children.Add(element:
                                                  new AnimatedIcon(imageLocation: iconSettings.imageLocation,
                                                                   iconTitle: iconSettings.iconTitle,
                                                                   target: iconSettings.target,
                                                                   iconNo: iconSettings.iconNo,
                                                                   primaryRingySettings: currentPrimaryRingySettings));
                        }
                    }
                }

                logoImage = new CustomImage(imageName: currentPrimaryRingySettings.logoImageLocation,
                                            width: (STANDARD_ICON_DIMENSION/12)*8,
                                            height: (STANDARD_ICON_DIMENSION/12)*8);
                Grid.SetColumn(element: logoImage, value: 1);
                Grid.SetRow(element: logoImage, value: 1);

                iconGrid.Children.Add(logoImage);
            }
            catch (Exception loadRingyIconsException)
            {
                MessageBox.Show(
                    messageBoxText:
                        "Can't open/find RingyIconSettings.dds file in Settings directory. PrimaryRingySettings.dds file might be corrupted or deleted.",
                    caption: "ERROR", button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }

        private void saveRingyIconSettings()
        {
            try
            {
                FileStream ringyIconSettingsFileStream = new FileStream(settingsPath + "RingyIconSettings.dds",
                                                                        FileMode.OpenOrCreate, FileAccess.ReadWrite);
                    // *.dds means 'Deviant Dock Settings'
                BinaryFormatter ringyIconSettingsFormatter = new BinaryFormatter();

                ringyIconSettingsFormatter.Serialize(ringyIconSettingsFileStream, currentRingyIconSettings);
                ringyIconSettingsFileStream.Close();
            }
            catch (Exception saveRingyIconSettingsException)
            {
                MessageBox.Show(
                    messageBoxText:
                        "Can't save settings in RingyIconSettings.dds file. Make sure that, PrimaryRingySettings.dds file is realy exists in Settings directory or the system isn't write protected!",
                    caption: "ERROR", button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }

        private void refreshRingy()
        {
            loadPrimaryRingySettings();

            zoomAnimationEffectMenuItem.IsChecked = false;
            fadeAnimationEffectMenuItem.IsChecked = false;
            rotateAnimationEffectMenuItem.IsChecked = false;

            switch (currentPrimaryRingySettings.hoaverEffect)
            {
                case "Zoom":
                    zoomAnimationEffectMenuItem.IsChecked = true;
                    break;

                case "Fade":
                    fadeAnimationEffectMenuItem.IsChecked = true;
                    break;

                case "Rotate":
                    rotateAnimationEffectMenuItem.IsChecked = true;
                    break;
            }

            if (!currentPrimaryRingySettings.showIconLabel)
                showIconLabelMenuItem.IsChecked = false;
            else
                showIconLabelMenuItem.IsChecked = true;

            backgroundStackPanel.Children.Clear();

            backgroundStackPanel.Children.Add(
                new CustomImage(imageName: "Background/" + currentPrimaryRingySettings.theme + ".png",
                                width: STANDARD_BACKGROUND_IMAGE_DIMENSION, height: STANDARD_BACKGROUND_IMAGE_DIMENSION));

            loadRingyIcons();
        }

        private void optionsMenuItem_Click(object sender, EventArgs eventArgs)
        {
            ringyOptionsWindow = new RingyOptionsWindow(ringyIconSettings: ref currentRingyIconSettings,
                                                        logoImageLocation: currentPrimaryRingySettings.logoImageLocation);

            setValueForRingyOptionsWindow();
            setEventHandlerForRingyOptionsWindow();
        }

        private void setValueForRingyOptionsWindow()
        {
            // Initializing RingyOptionsWindow for first run
            ringyOptionsWindow.themeComboBox.SelectedValue = currentPrimaryRingySettings.theme;
            ringyOptionsWindow.animationEffectComboBox.SelectedValue = currentPrimaryRingySettings.hoaverEffect;
            ringyOptionsWindow.showIconLabelCheckBox.IsChecked = currentPrimaryRingySettings.showIconLabel;

            int index = 0;

            foreach (var iconSettings in currentRingyIconSettings)
            {
                if (iconSettings.iconTitle != string.Empty)
                    ringyIconConfigurationCheckBox[index] = new RingyIconConfigurationCheckBox(iconLocation: iconSettings.imageLocation, iconTitle: iconSettings.iconTitle, target: iconSettings.target, iconNo: index + 1, baseCanvas: ref ringyOptionsWindow.iconSettingsCanvas, status: true);
                else
                    ringyIconConfigurationCheckBox[index] = new RingyIconConfigurationCheckBox(iconLocation: "Icons/unknown.png", iconTitle: "<invalid>", target: string.Empty, iconNo: index + 1, baseCanvas: ref ringyOptionsWindow.iconSettingsCanvas, status: false);

                index++;
            }
        }

        private void setEventHandlerForRingyOptionsWindow()
        {
            // Adding Event Handler
            ringyOptionsWindow.themeComboBox.SelectionChanged +=
                new SelectionChangedEventHandler(ringyOptionsWindow_themeComboBox_SelectionChanged);
            ringyOptionsWindow.animationEffectComboBox.SelectionChanged +=
                new SelectionChangedEventHandler(ringyOptionsWindow_animationEffectComboBox_SelectionChanged);
            ringyOptionsWindow.showIconLabelCheckBox.Click += new RoutedEventHandler(ringyOptionsWindow_showIconLabelCheckBox_Click);
            ringyOptionsWindow.logoImageBrowseButton.Click += new RoutedEventHandler(ringyOptionsWindow_logoImageBrowseButton_Click);
            ringyOptionsWindow.defaultButton.Click += new RoutedEventHandler(ringyOptionsWindow_defaultButton_Click);

            for (int i = 0; i < currentRingyIconSettings.Count; i++)
            {
                ringyIconConfigurationCheckBox[i].Click += new RoutedEventHandler(ringyIconConfigurationCheckBox_Ringy_Click);

                ringyIconConfigurationCheckBox[i].configureButton.Click +=
                    new RoutedEventHandler(ringyIconConfigurationCheckBox_configureButton_Click);
            }
        }

        private void ringyOptionsWindow_themeComboBox_SelectionChanged(object sender, EventArgs eventArgs)
        {
            currentPrimaryRingySettings.theme = ringyOptionsWindow.themeComboBox.SelectedValue.ToString();

            savePrimaryRingySettings();
            refreshRingy();
        }

        private void ringyOptionsWindow_animationEffectComboBox_SelectionChanged(object sender, EventArgs eventArgs)
        {
            currentPrimaryRingySettings.hoaverEffect =
                ringyOptionsWindow.animationEffectComboBox.SelectedValue.ToString();

            savePrimaryRingySettings();
            refreshRingy();
        }

        private void ringyOptionsWindow_showIconLabelCheckBox_Click(object sender, EventArgs eventArgs)
        {
            currentPrimaryRingySettings.showIconLabel = (bool) ringyOptionsWindow.showIconLabelCheckBox.IsChecked;

            savePrimaryRingySettings();
            refreshRingy();
        }

        private void ringyIconConfigurationCheckBox_configureButton_Click(object sender, EventArgs eventArgs)
        {
            int index;

            for (index = 0; index < currentRingyIconSettings.Count; index++)
                if (sender == ringyIconConfigurationCheckBox[index].configureButton)
                    break;

            ringyIconConfigurationCheckBox[index].addRingyIconWindow = new AddRingyIconWindow();
            ringyIconConfigurationCheckBox[index].addRingyIconWindow.setIconForIconButton(iconLocation: currentRingyIconSettings[index].imageLocation);
            ringyIconConfigurationCheckBox[index].addRingyIconWindow.titleTextBox.Text = currentRingyIconSettings[index].iconTitle;
            ringyIconConfigurationCheckBox[index].addRingyIconWindow.targetTextBox.Text = currentRingyIconSettings[index].target;

            ringyIconConfigurationCheckBox[index].addRingyIconWindow.saveButton.Click +=
                    new RoutedEventHandler(delegate (object sndr, RoutedEventArgs routedEventArgs)
                                               {
                                                   if ((ringyIconConfigurationCheckBox[index].addRingyIconWindow.titleTextBox.Text != string.Empty) | (ringyIconConfigurationCheckBox[index].addRingyIconWindow.targetTextBox.Text != string.Empty))
                                                   {
                                                       currentRingyIconSettings.RemoveAt(index);
                                                       currentRingyIconSettings.Insert(index, new IconSettings(imageLocation: ringyIconConfigurationCheckBox[index].addRingyIconWindow.iconImage.imageName, iconTitle: ringyIconConfigurationCheckBox[index].addRingyIconWindow.titleTextBox.Text, target: ringyIconConfigurationCheckBox[index].addRingyIconWindow.targetTextBox.Text, iconNo: index + 1));

                                                       saveRingyIconSettings();
                                                       refreshRingy();

                                                       ringyIconConfigurationCheckBox[index].setContents(iconLocation: ringyIconConfigurationCheckBox[index].addRingyIconWindow.iconImage.imageName, iconTitle: ringyIconConfigurationCheckBox[index].addRingyIconWindow.titleTextBox.Text, target: ringyIconConfigurationCheckBox[index].addRingyIconWindow.targetTextBox.Text);
                                                       ringyIconConfigurationCheckBox[index].addRingyIconWindow.Close();
                                                       ringyIconConfigurationCheckBox[index].IsChecked = true;
                                                   }
                                                   else
                                                   {
                                                       MessageBox.Show(messageBoxText: "Please, enter tilte and target path in textboxes.", caption: "WARNING", button: MessageBoxButton.OK, icon: MessageBoxImage.Warning);
                                                   }
                                               });
        }

        private void ringyIconConfigurationCheckBox_Ringy_Click(object sender, EventArgs eventArgs)
        {
            int index;

            for (index = 0; index < currentRingyIconSettings.Count; index++)
                if (sender == ringyIconConfigurationCheckBox[index])
                    break;

            currentRingyIconSettings.RemoveAt(index);

            if (ringyIconConfigurationCheckBox[index].IsChecked == false)
            {
                currentRingyIconSettings.Insert(index, new IconSettings(imageLocation: "<none>", iconTitle: string.Empty, target: string.Empty));

                saveRingyIconSettings();
                refreshRingy();
            }
            else
            {
                if (ringyIconConfigurationCheckBox[index].iconTitle != "<invalid>")
                {
                    currentRingyIconSettings.Insert(index,
                                                    new IconSettings(
                                                        imageLocation: ringyIconConfigurationCheckBox[index].iconLocation,
                                                        iconTitle: ringyIconConfigurationCheckBox[index].iconTitle,
                                                        target: ringyIconConfigurationCheckBox[index].target,
                                                        iconNo: index + 1));
                }
                else
                {
                    ringyIconConfigurationCheckBox[index].IsChecked = false;

                    currentRingyIconSettings.Insert(index,
                                                    new IconSettings(imageLocation: "<none>", iconTitle: string.Empty,
                                                                     target: string.Empty, iconNo: index + 1));

                    MessageBox.Show(messageBoxText: "Please, configure the icon first.", caption: "WARNING",
                                    button: MessageBoxButton.OK, icon: MessageBoxImage.Warning);
                }

                saveRingyIconSettings();
                refreshRingy();
            }
        }

        private void ringyOptionsWindow_logoImageBrowseButton_Click(object sender, EventArgs eventArgs)
        {
            OpenFileDialog logoImageOpenFileDialog = new OpenFileDialog();
            logoImageOpenFileDialog.InitialDirectory = "Icons";
            logoImageOpenFileDialog.Filter = "Portable Network Graphics (.png)|*.png|Windows Icon (.ico)|*.ico|Joint Pictures Expert Group (.jpg)|*.jpg|Windows Bitmap Image (.bmp)|*.bmp|Graphical Interchange format (.gif)|*.gif|All files (*.*)|*.*";

            Nullable<bool> response = logoImageOpenFileDialog.ShowDialog();

            if (response == true)
            {
                currentPrimaryRingySettings.logoImageLocation = logoImageOpenFileDialog.FileName;

                savePrimaryRingySettings();
                refreshRingy();

                ringyOptionsWindow.setLogoImageComponents(logoImageLocation: logoImageOpenFileDialog.FileName);
                ringyOptionsWindow.logoImageBrowseButton.Click += new RoutedEventHandler(ringyOptionsWindow_logoImageBrowseButton_Click);
            }
        }

        private void ringyOptionsWindow_defaultButton_Click(object sender, EventArgs eventArgs)
        {
            currentPrimaryRingySettings = new PrimaryRingySettings();

            savePrimaryRingySettings();
            refreshRingy();

            ringyOptionsWindow.Close();

            ringyOptionsWindow = new RingyOptionsWindow(ref currentRingyIconSettings, currentPrimaryRingySettings.logoImageLocation);
            setValueForRingyOptionsWindow();
            setEventHandlerForRingyOptionsWindow();
        }

        private void AnimationEffectMenuItem_Click(object sender, EventArgs eventArgs)
        {
            if (sender == zoomAnimationEffectMenuItem)
                currentPrimaryRingySettings.hoaverEffect = "Zoom";
            else if (sender == fadeAnimationEffectMenuItem)
                currentPrimaryRingySettings.hoaverEffect = "Fade";
            else if (sender == rotateAnimationEffectMenuItem)
                currentPrimaryRingySettings.hoaverEffect = "Rotate";

            savePrimaryRingySettings();
            refreshRingy();
        }

        private void showIconLabelMenuItem_Click(object sender, EventArgs eventArgs)
        {
            if (currentPrimaryRingySettings.showIconLabel)
                currentPrimaryRingySettings.showIconLabel = false;
            else
                currentPrimaryRingySettings.showIconLabel = true;

            savePrimaryRingySettings();

            refreshRingy();
        }

        private void contentsMenuItem_Click(object sender, EventArgs eventArgs)
        {
            try
            {
                System.Diagnostics.Process.Start("help.chm");
            }
            catch (Exception)
            {
                MessageBox.Show(messageBoxText: "help.chm file can not be found!", caption: "ERROR",
                                button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }

        private void aboutMenuItem_Click(object sender, EventArgs eventArgs)
        {
            AboutWindow aboutWindow = new AboutWindow();
        }
    }
}
