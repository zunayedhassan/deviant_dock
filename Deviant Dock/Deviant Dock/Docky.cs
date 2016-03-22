using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using IWshRuntimeLibrary;
using File = System.IO.File;

namespace Deviant_Dock
{
    class Docky : Window
    {
        private int STANDARD_DOCKY_EDGE_WIDTH = 50,
                    STANDARD_DOCKY_BACKGROUND_WIDTH = 70,
                    STANDARD_DOCKY_BACKGROUND_HEIGHT = 104,
                    STANDARD_DOCKY_SEPARATOR_WIDTH = 5,
                    STANDARD_DOCKY_ICON_DIMENSION = 64;

        private PrimaryDockySettings currentPrimaryDockySettings;
        private List<IconSettings> currentDockyIconSettings;
        private Canvas mainDockyBackgroundCanvas;
        private ContextMenu dockyContextMenu;
        private string settingsPath = GlobalSettingsPath.path;

        private CustomMenuItem addMenuItem,
                               fileAddMenuItem,
                               pathAddMenuItem,
                               separatorAddMenuItem,
                               computerAddMenuItem,
                               networkAddMenuItem,
                               documentsAddMenuItem,
                               picturesAddMenuItem,
                               musicAddMenuItem,
                               gamesAddMenuItem,
                               controlPanelAddMenuItem,
                               recycleBinAddMenuItem,
                               removeMenuItem,
                               iconSettingsMenuItem,
                               screenPositionMenuItem,
                               topScreenPositionMenuItem,
                               bottomScreenPositionMenuItem,
                               leftScreenPositionMenuItem,
                               rightScreenPositionMenuItem,
                               animationEffectMenuItem,
                               hoaverEffectMenuItem,
                               zoomHoaverEffectMenuItem,
                               swingHoaverEffectMenuItem,
                               fadeHoaverEffectMenuItem,
                               rotateHoaverEffectMenuItem,
                               clickEffectMenuItem,
                               zoomClickEffectMenuItem,
                               swingClickEffectMenuItem,
                               fadeClickEffectMenuItem,
                               rotateClickEffectMenuItem,
                               optionsMenuItem,
                               helpMenuItem,
                               contentsMenuItem,
                               aboutMenuItem;

        private AddDockyIconWindow fileAddDockyIconWindow,
                                   pathAddDockyIconWindow;

        private DockyIconSettingsWindow dockyIconSettingsWindow;
        private DockyOptionsWindow dockyOptionsWindow;

        public Docky()
        {
            this.Title = "Docky";
            this.ShowInTaskbar = false;
            this.AllowsTransparency = true;
            this.Background = new SolidColorBrush(Colors.Transparent);
            this.WindowStyle = WindowStyle.None;

            mainDockyBackgroundCanvas = new Canvas();
            mainDockyBackgroundCanvas.AllowDrop = true;
            //mainDockyBackgroundCanvas.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(r: 240, g: 240, b: 240));         // NOTE: For debugging purpose only

            // Adding components
            this.Content = mainDockyBackgroundCanvas;

            // Initializing settings
            if (!File.Exists(path: settingsPath + "PrimaryDockySettings.dds"))
                createNewPrimaryDockySettings();

            if (!File.Exists(path: settingsPath + "DockyIconSettings.dds"))
                createDockyIconSettings();

            refreshDocky();

            // Adding Event Handler
            this.PreviewDragEnter += new DragEventHandler(Docky_PreviewDragEnter);
            this.PreviewDragOver += new DragEventHandler(Docky_PreviewDragOver);
            this.PreviewDrop += new DragEventHandler(Docky_PreviewDrop);
        }

        private void savePrimaryDockySettings()
        {
            try
            {
                FileStream primaryDockySettingsFileStream = new FileStream(settingsPath + "PrimaryDockySettings.dds", FileMode.OpenOrCreate, FileAccess.ReadWrite);      // *.dds means 'Deviant Dock Settings'
                BinaryFormatter primaryDockySettingsFormatter = new BinaryFormatter();

                primaryDockySettingsFormatter.Serialize(primaryDockySettingsFileStream, currentPrimaryDockySettings);
                primaryDockySettingsFileStream.Close();
            }
            catch (Exception createNewPrimaryDockySettingsException)
            {
                MessageBox.Show(messageBoxText: Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/Deviant Dock/Can't create PrimaryDockySettings.dds file in Settings directory. Make sure that the filesystem isn't write protected.", caption: "ERROR", button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }

        private void createNewPrimaryDockySettings()
        {
            currentPrimaryDockySettings = new PrimaryDockySettings();
            savePrimaryDockySettings();
        }

        private void loadPrimaryDockySettings()
        {
            // Loading settings from PrimaryDockySettings.dds file
            try
            {
                FileStream primaryDockySettingsInput = new FileStream(settingsPath + "PrimaryDockySettings.dds", FileMode.Open, FileAccess.Read);
                BinaryFormatter primaryDockySettingsReader = new BinaryFormatter();
                currentPrimaryDockySettings = new PrimaryDockySettings();

                currentPrimaryDockySettings = (PrimaryDockySettings)primaryDockySettingsReader.Deserialize(primaryDockySettingsInput);

                primaryDockySettingsInput.Close();
            }
            catch (Exception loadPrimaryDockySettingsException)
            {
                MessageBox.Show(messageBoxText: "Can't open/find PrimaryDockySettings.dds file in Settings directory. PrimaryDockySettings.dds file might be corrupted or deleted.", caption: "ERROR", button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }

        private void refreshDocky()
        {
            loadPrimaryDockySettings();

            int dockyWindowDimensionA = (STANDARD_DOCKY_EDGE_WIDTH * 2) +
                                        (STANDARD_DOCKY_BACKGROUND_WIDTH * currentPrimaryDockySettings.totalIcon) +
                                        (STANDARD_DOCKY_SEPARATOR_WIDTH * currentPrimaryDockySettings.totalSeparator),
                dockyWindowDimensionB = STANDARD_DOCKY_BACKGROUND_HEIGHT + (STANDARD_DOCKY_ICON_DIMENSION - ((STANDARD_DOCKY_BACKGROUND_HEIGHT - STANDARD_DOCKY_ICON_DIMENSION) / 2));

            if ((currentPrimaryDockySettings.screenPosition == "Top") | (currentPrimaryDockySettings.screenPosition == "Bottom"))
            {
                this.Width = dockyWindowDimensionA;
                this.Height = dockyWindowDimensionB;

                if (currentPrimaryDockySettings.screenPosition == "Top")
                    this.Top = currentPrimaryDockySettings.edgeOffset;
                else
                    this.Top = SystemParameters.PrimaryScreenHeight -
                               (this.Height + currentPrimaryDockySettings.edgeOffset);

                this.Left = ((SystemParameters.PrimaryScreenWidth - this.Width) / 2) + (int)((SystemParameters.PrimaryScreenWidth / 100) * currentPrimaryDockySettings.centering) / 2;

                resetBackground(rotateAngle: 0);
            }
            else
            {
                this.Width = dockyWindowDimensionB;
                this.Height = dockyWindowDimensionA;

                if (currentPrimaryDockySettings.screenPosition == "Left")
                    this.Left = currentPrimaryDockySettings.edgeOffset;
                else
                    this.Left = SystemParameters.PrimaryScreenWidth -
                                (this.Width + currentPrimaryDockySettings.edgeOffset);

                this.Top = ((SystemParameters.PrimaryScreenHeight - this.Height) / 2) + (int)((SystemParameters.PrimaryScreenHeight / 100) * currentPrimaryDockySettings.centering) / 2;

                resetBackground(rotateAngle: 90);
            }

            if (currentPrimaryDockySettings.layering == "Topmost")
                this.Topmost = true;
            else
                this.Topmost = false;

            loadDockyIcons();
            loadContextMenu();
        }

        private void resetBackground(int rotateAngle)
        {
            mainDockyBackgroundCanvas.Children.Clear();

            int LEFT = 0,
                TOP = 0;

            if (!Directory.Exists(path: "Skins/" + currentPrimaryDockySettings.theme))
                currentPrimaryDockySettings.theme = "VistaBlack";

            CustomImage leftBackgroundImage = new CustomImage(imageName: "Skins/" + currentPrimaryDockySettings.theme + "/bg_left.png", width: STANDARD_DOCKY_EDGE_WIDTH, height: STANDARD_DOCKY_BACKGROUND_HEIGHT);
            leftBackgroundImage.LayoutTransform = new RotateTransform(angle: rotateAngle);
            mainDockyBackgroundCanvas.Children.Add(leftBackgroundImage);

            switch (currentPrimaryDockySettings.screenPosition)
            {
                case "Top":
                    leftBackgroundImage.Margin = new Thickness(left: LEFT, top: TOP, right: 0, bottom: 0);
                    LEFT += STANDARD_DOCKY_EDGE_WIDTH;
                    break;

                case "Bottom":
                    TOP = (int)(this.Height - STANDARD_DOCKY_BACKGROUND_HEIGHT);
                    leftBackgroundImage.Margin = new Thickness(left: LEFT, top: TOP, right: 0, bottom: 0);
                    LEFT += STANDARD_DOCKY_EDGE_WIDTH;
                    break;

                case "Left":
                    leftBackgroundImage.Margin = new Thickness(left: LEFT, top: TOP, right: 0, bottom: 0);
                    TOP += STANDARD_DOCKY_EDGE_WIDTH;
                    break;

                case "Right":
                    LEFT = (int)(this.Width - STANDARD_DOCKY_BACKGROUND_HEIGHT);
                    leftBackgroundImage.Margin = new Thickness(left: LEFT, top: TOP, right: 0, bottom: 0);
                    TOP += STANDARD_DOCKY_EDGE_WIDTH;
                    break;
            }

            CustomImage[] midBackgroundImage = new CustomImage[currentPrimaryDockySettings.totalIcon];
            for (int i = 0; i < midBackgroundImage.Length; i++)
            {
                midBackgroundImage[i] = new CustomImage(imageName: "Skins/" + currentPrimaryDockySettings.theme + "/bg_mid.png", width: STANDARD_DOCKY_BACKGROUND_WIDTH, height: STANDARD_DOCKY_BACKGROUND_HEIGHT);
                midBackgroundImage[i].LayoutTransform = new RotateTransform(angle: rotateAngle);
                midBackgroundImage[i].Margin = new Thickness(left: LEFT, top: TOP, right: 0, bottom: 0);
                mainDockyBackgroundCanvas.Children.Add(midBackgroundImage[i]);

                if ((currentPrimaryDockySettings.screenPosition == "Top") | (currentPrimaryDockySettings.screenPosition == "Bottom"))
                    LEFT += STANDARD_DOCKY_BACKGROUND_WIDTH;
                else
                    TOP += STANDARD_DOCKY_BACKGROUND_WIDTH;
            }

            CustomImage[] separatorBackgroundImage = new CustomImage[currentPrimaryDockySettings.totalSeparator];
            for (int i = 0; i < separatorBackgroundImage.Length; i++)
            {
                separatorBackgroundImage[i] = new CustomImage(imageName: "Skins/" + currentPrimaryDockySettings.theme + "/bg_sep.png", width: STANDARD_DOCKY_SEPARATOR_WIDTH, height: STANDARD_DOCKY_BACKGROUND_HEIGHT);
                separatorBackgroundImage[i].LayoutTransform = new RotateTransform(angle: rotateAngle);
                separatorBackgroundImage[i].Margin = new Thickness(left: LEFT, top: TOP, right: 0, bottom: 0);
                mainDockyBackgroundCanvas.Children.Add(separatorBackgroundImage[i]);

                if ((currentPrimaryDockySettings.screenPosition == "Top") | (currentPrimaryDockySettings.screenPosition == "Bottom"))
                    LEFT += STANDARD_DOCKY_SEPARATOR_WIDTH;
                else
                    TOP += STANDARD_DOCKY_SEPARATOR_WIDTH;
            }

            CustomImage rightBackgroundImage = new CustomImage(imageName: "Skins/" + currentPrimaryDockySettings.theme + "/bg_right.png", width: STANDARD_DOCKY_EDGE_WIDTH, height: STANDARD_DOCKY_BACKGROUND_HEIGHT);
            rightBackgroundImage.LayoutTransform = new RotateTransform(angle: rotateAngle);
            mainDockyBackgroundCanvas.Children.Add(rightBackgroundImage);
            rightBackgroundImage.Margin = new Thickness(left: LEFT, top: TOP, right: 0, bottom: 0);
        }

        private void saveDockyIconSettings()
        {
            try
            {
                FileStream dockyIconSettingsFileStream = new FileStream(settingsPath + "DockyIconSettings.dds", FileMode.OpenOrCreate, FileAccess.ReadWrite);      // *.dds means 'Deviant Dock Settings'
                BinaryFormatter dockyIconSettingsFormatter = new BinaryFormatter();

                dockyIconSettingsFormatter.Serialize(dockyIconSettingsFileStream, currentDockyIconSettings);
                dockyIconSettingsFileStream.Close();
            }
            catch (Exception saveDockyIconSettingsException)
            {
                MessageBox.Show(messageBoxText: "Can't create DockyIconSettings.dds file in Settings directory. Make sure that the filesystem isn't write protected.", caption: "ERROR", button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }

        private void createDockyIconSettings()
        {
            currentDockyIconSettings = new List<IconSettings>();

            currentDockyIconSettings.Add(new IconSettings(imageLocation: "Icons/My-Computer.png", iconTitle: "My Computer", target: @"::{20D04FE0-3AEA-1069-A2D8-08002B30309D}"));
            currentDockyIconSettings.Add(new IconSettings(imageLocation: "Icons/My-Network.png", iconTitle: "My Network", target: @"::{F02C1A0D-BE21-4350-88B0-7367FC96EF3C}"));
            currentDockyIconSettings.Add(new IconSettings(imageLocation: "Icons/My-Documents.png", iconTitle: "My Documents", target: @"::{031E4825-7B94-4DC3-B131-E946B44C8DD5}\Documents.library-ms"));
            currentDockyIconSettings.Add(new Separator().getSeparator());
            currentDockyIconSettings.Add(new IconSettings(imageLocation: "Icons/ie.png", iconTitle: "Internet Explorer", target: @"C:\Program Files\Internet Explorer\iexplore.exe"));
            currentDockyIconSettings.Add(new IconSettings(imageLocation: "Icons/win_media_player.png", iconTitle: "Windows Media Player", target: @"C:\Program Files\Windows Media Player\wmplayer.exe"));
            currentDockyIconSettings.Add(new IconSettings(imageLocation: "Icons/win_logo_green.png", iconTitle: "Windows Media Center", target: @"C:\Windows\ehome\ehshell.exe"));
            currentDockyIconSettings.Add(new Separator().getSeparator());
            currentDockyIconSettings.Add(new IconSettings(imageLocation: "Icons/Control-Panel.png", iconTitle: "Control Panel", target: @"::{26EE0668-A00A-44D7-9371-BEB064C98683}\0"));
            currentDockyIconSettings.Add(new IconSettings(imageLocation: "Icons/Trashcan_full.png", iconTitle: "Recycle Bin", target: @"::{645FF040-5081-101B-9F08-00AA002F954E}"));

            saveDockyIconSettings();
        }

        private void loadDockyIcons()
        {
            AnimatedIcon[] animatedIcon = new AnimatedIcon[currentPrimaryDockySettings.totalIcon + currentPrimaryDockySettings.totalSeparator];
            int iconNo = 0;
            int currentLeft = STANDARD_DOCKY_EDGE_WIDTH +
                              (STANDARD_DOCKY_BACKGROUND_WIDTH - STANDARD_DOCKY_ICON_DIMENSION) / 2,
                currentTop = (STANDARD_DOCKY_BACKGROUND_HEIGHT - STANDARD_DOCKY_ICON_DIMENSION) / 2;

            // Loading settings from DockyIconSettings.dds file
            try
            {
                using (Stream dockyIconSettingsStream = File.Open(path: settingsPath + "DockyIconSettings.dds", mode: FileMode.Open))
                {
                    BinaryFormatter dockyIconSettingsFormatter = new BinaryFormatter();
                    currentDockyIconSettings = (List<IconSettings>)dockyIconSettingsFormatter.Deserialize(dockyIconSettingsStream);

                    foreach (var dockyIconSettings in currentDockyIconSettings)
                    {
                        if (!(dockyIconSettings.imageLocation == "Skins/sep.png"))
                        {
                            animatedIcon[iconNo] = new AnimatedIcon(imageLocation: dockyIconSettings.imageLocation,
                                                                    iconTitle: dockyIconSettings.iconTitle,
                                                                    target: dockyIconSettings.target,
                                                                    primaryDockySettings: currentPrimaryDockySettings);
                        }
                        else
                            animatedIcon[iconNo] = new AnimatedIcon(imageLocation: dockyIconSettings.imageLocation);

                        switch (currentPrimaryDockySettings.screenPosition)
                        {
                            case "Top":
                                animatedIcon[iconNo].Margin = new Thickness(left: currentLeft, top: currentTop, right: 0, bottom: 0);
                                currentLeft += STANDARD_DOCKY_ICON_DIMENSION + (STANDARD_DOCKY_BACKGROUND_WIDTH - STANDARD_DOCKY_ICON_DIMENSION);
                                break;

                            case "Bottom":
                                animatedIcon[iconNo].Margin = new Thickness(left: currentLeft, top: currentTop + (this.Height - STANDARD_DOCKY_BACKGROUND_HEIGHT), right: 0, bottom: 0);
                                currentLeft += STANDARD_DOCKY_ICON_DIMENSION + (STANDARD_DOCKY_BACKGROUND_WIDTH - STANDARD_DOCKY_ICON_DIMENSION);
                                break;

                            case "Left":
                                animatedIcon[iconNo].Margin = new Thickness(left: currentTop, top: currentLeft, right: 0, bottom: 0);
                                currentLeft += STANDARD_DOCKY_ICON_DIMENSION + (STANDARD_DOCKY_BACKGROUND_WIDTH - STANDARD_DOCKY_ICON_DIMENSION);
                                break;

                            case "Right":
                                animatedIcon[iconNo].Margin = new Thickness(left: currentTop + (this.Width - STANDARD_DOCKY_BACKGROUND_HEIGHT), top: currentLeft, right: 0, bottom: 0);
                                currentLeft += STANDARD_DOCKY_ICON_DIMENSION + (STANDARD_DOCKY_BACKGROUND_WIDTH - STANDARD_DOCKY_ICON_DIMENSION);
                                break;
                        }

                        if (dockyIconSettings.imageLocation == "Skins/sep.png")
                        {
                            currentLeft -= STANDARD_DOCKY_ICON_DIMENSION - STANDARD_DOCKY_SEPARATOR_WIDTH;

                            if (currentPrimaryDockySettings.screenPosition == "Left")
                                animatedIcon[iconNo].LayoutTransform = new RotateTransform(270);
                            else if (currentPrimaryDockySettings.screenPosition == "Right")
                                animatedIcon[iconNo].LayoutTransform = new RotateTransform(90);
                        }

                        mainDockyBackgroundCanvas.Children.Add(animatedIcon[iconNo]);

                        iconNo++;
                    }
                }
            }
            catch (Exception loadPrimaryDockySettingsException)
            {
                MessageBox.Show(messageBoxText: "Can't open/find DockyIconSettings.dds file in Settings directory. DockyIconSettings.dds file might be corrupted or deleted.", caption: "ERROR", button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }

        private MenuItem getRemoveMenuItem(IconSettings iconSettings)
        {
            MenuItem removeMenuItem = new MenuItem();

            if (iconSettings.iconTitle == string.Empty)
                removeMenuItem.Header = "<-- Separator -->";
            else
            {
                removeMenuItem.Header = iconSettings.iconTitle;
                removeMenuItem.Icon = new CustomImage(iconSettings.imageLocation, STANDARD_DOCKY_ICON_DIMENSION / 4, STANDARD_DOCKY_ICON_DIMENSION / 4);
            }

            removeMenuItem.Click += new RoutedEventHandler(delegate(object sender, RoutedEventArgs routedEventArgs)
            {
                currentDockyIconSettings.Remove(iconSettings);
                saveDockyIconSettings();

                if (iconSettings.iconTitle == string.Empty)
                    currentPrimaryDockySettings.totalSeparator -= 1;
                else
                    currentPrimaryDockySettings.totalIcon -= 1;

                savePrimaryDockySettings();
                refreshDocky();
                refreshRemoveMenuItem();
            });

            return removeMenuItem;
        }

        private void refreshRemoveMenuItem()
        {
            removeMenuItem.Items.Clear();

            foreach (var iconSettings in currentDockyIconSettings)
            {
                removeMenuItem.Items.Add(getRemoveMenuItem(iconSettings));
            }

            if (removeMenuItem.Items.IsEmpty)
                removeMenuItem.IsEnabled = false;
            else
                removeMenuItem.IsEnabled = true;
        }

        private void loadContextMenu()
        {
            dockyContextMenu = new ContextMenu();

            addMenuItem = new CustomMenuItem(text: "Add", contextMenu: ref dockyContextMenu);
            fileAddMenuItem = new CustomMenuItem(text: "File", menuItem: ref addMenuItem);
            pathAddMenuItem = new CustomMenuItem(text: "Path", menuItem: ref addMenuItem);
            separatorAddMenuItem = new CustomMenuItem(text: "Separator", menuItem: ref addMenuItem);
            addMenuItem.Items.Add(new System.Windows.Controls.Separator());
            computerAddMenuItem = new CustomMenuItem(text: "Computer", menuItem: ref addMenuItem);
            networkAddMenuItem = new CustomMenuItem(text: "Network", menuItem: ref addMenuItem);
            documentsAddMenuItem = new CustomMenuItem(text: "Documents", menuItem: ref addMenuItem);
            picturesAddMenuItem = new CustomMenuItem(text: "Pictures", menuItem: ref addMenuItem);
            musicAddMenuItem = new CustomMenuItem(text: "Music", menuItem: ref addMenuItem);
            gamesAddMenuItem = new CustomMenuItem(text: "Games", menuItem: ref addMenuItem);
            controlPanelAddMenuItem = new CustomMenuItem(text: "Control Panel", menuItem: ref addMenuItem);
            recycleBinAddMenuItem = new CustomMenuItem(text: "Recycle Bin", menuItem: ref addMenuItem);
            removeMenuItem = new CustomMenuItem(text: "Remove", contextMenu: ref dockyContextMenu);
            iconSettingsMenuItem = new CustomMenuItem(text: "Icon Settings", contextMenu: ref dockyContextMenu);
            dockyContextMenu.Items.Add(new System.Windows.Controls.Separator());
            screenPositionMenuItem = new CustomMenuItem(text: "Screen Position", contextMenu: ref dockyContextMenu);
            topScreenPositionMenuItem = new CustomMenuItem(text: "Top", menuItem: ref screenPositionMenuItem);
            bottomScreenPositionMenuItem = new CustomMenuItem(text: "Bottom", menuItem: ref screenPositionMenuItem);
            screenPositionMenuItem.Items.Add(new System.Windows.Controls.Separator());
            leftScreenPositionMenuItem = new CustomMenuItem(text: "Left", menuItem: ref screenPositionMenuItem);
            rightScreenPositionMenuItem = new CustomMenuItem(text: "Right", menuItem: ref screenPositionMenuItem);
            animationEffectMenuItem = new CustomMenuItem(text: "Animation Effect", contextMenu: ref dockyContextMenu);
            hoaverEffectMenuItem = new CustomMenuItem(text: "Hoaver Effect", menuItem: ref animationEffectMenuItem);
            zoomHoaverEffectMenuItem = new CustomMenuItem(text: "Zoom", menuItem: ref hoaverEffectMenuItem);
            swingHoaverEffectMenuItem = new CustomMenuItem(text: "Swing", menuItem: ref hoaverEffectMenuItem);
            fadeHoaverEffectMenuItem = new CustomMenuItem(text: "Fade", menuItem: ref hoaverEffectMenuItem);
            rotateHoaverEffectMenuItem = new CustomMenuItem(text: "Rotate", menuItem: ref hoaverEffectMenuItem);
            clickEffectMenuItem = new CustomMenuItem(text: "Click Effect", menuItem: ref animationEffectMenuItem);
            zoomClickEffectMenuItem = new CustomMenuItem(text: "Zoom", menuItem: ref clickEffectMenuItem);
            swingClickEffectMenuItem = new CustomMenuItem(text: "Swing", menuItem: ref clickEffectMenuItem);
            fadeClickEffectMenuItem = new CustomMenuItem(text: "Fade", menuItem: ref clickEffectMenuItem);
            rotateClickEffectMenuItem = new CustomMenuItem(text: "Rotate", menuItem: ref clickEffectMenuItem);
            optionsMenuItem = new CustomMenuItem(text: "Options", contextMenu: ref dockyContextMenu);
            dockyContextMenu.Items.Add(new System.Windows.Controls.Separator());
            helpMenuItem = new CustomMenuItem(text: "Help", contextMenu: ref dockyContextMenu);
            contentsMenuItem = new CustomMenuItem(text: "Contents", menuItem: ref helpMenuItem);
            aboutMenuItem = new CustomMenuItem(text: "About", menuItem: ref helpMenuItem);

            refreshRemoveMenuItem();

            this.ContextMenu = dockyContextMenu;

            switch (currentPrimaryDockySettings.screenPosition)
            {
                case "Top":
                    topScreenPositionMenuItem.IsChecked = true;
                    break;

                case "Bottom":
                    bottomScreenPositionMenuItem.IsChecked = true;
                    break;

                case "Left":
                    leftScreenPositionMenuItem.IsChecked = true;
                    break;

                case "Right":
                    rightScreenPositionMenuItem.IsChecked = true;
                    break;
            }

            switch (currentPrimaryDockySettings.hoaverEffect)
            {
                case "Zoom":
                    zoomHoaverEffectMenuItem.IsChecked = true;
                    break;

                case "Swing":
                    swingHoaverEffectMenuItem.IsChecked = true;
                    break;

                case "Fade":
                    fadeHoaverEffectMenuItem.IsChecked = true;
                    break;

                case "Rotate":
                    rotateHoaverEffectMenuItem.IsChecked = true;
                    break;
            }

            switch (currentPrimaryDockySettings.clickEffect)
            {
                case "Zoom":
                    zoomClickEffectMenuItem.IsChecked = true;
                    break;

                case "Swing":
                    swingClickEffectMenuItem.IsChecked = true;
                    break;

                case "Fade":
                    fadeClickEffectMenuItem.IsChecked = true;
                    break;

                case "Rotate":
                    rotateClickEffectMenuItem.IsChecked = true;
                    break;
            }

            // Adding Event Handler
            fileAddMenuItem.Click += new RoutedEventHandler(fileAddMenuItem_Click);
            pathAddMenuItem.Click += new RoutedEventHandler(pathAddMenuItem_Click);
            separatorAddMenuItem.Click += new RoutedEventHandler(separatorAddMenuItem_Click);
            computerAddMenuItem.Click += new RoutedEventHandler(addSpecialIcon_AddMenuItem_Click);
            networkAddMenuItem.Click += new RoutedEventHandler(addSpecialIcon_AddMenuItem_Click);
            documentsAddMenuItem.Click += new RoutedEventHandler(addSpecialIcon_AddMenuItem_Click);
            picturesAddMenuItem.Click += new RoutedEventHandler(addSpecialIcon_AddMenuItem_Click);
            musicAddMenuItem.Click += new RoutedEventHandler(addSpecialIcon_AddMenuItem_Click);
            gamesAddMenuItem.Click += new RoutedEventHandler(addSpecialIcon_AddMenuItem_Click);
            controlPanelAddMenuItem.Click += new RoutedEventHandler(addSpecialIcon_AddMenuItem_Click);
            recycleBinAddMenuItem.Click += new RoutedEventHandler(addSpecialIcon_AddMenuItem_Click);
            iconSettingsMenuItem.Click += new RoutedEventHandler(iconSettingsMenuItem_Click);
            topScreenPositionMenuItem.Click += new RoutedEventHandler(screenPositionMenuItems_Click);
            bottomScreenPositionMenuItem.Click += new RoutedEventHandler(screenPositionMenuItems_Click);
            leftScreenPositionMenuItem.Click += new RoutedEventHandler(screenPositionMenuItems_Click);
            rightScreenPositionMenuItem.Click += new RoutedEventHandler(screenPositionMenuItems_Click);
            zoomHoaverEffectMenuItem.Click += new RoutedEventHandler(animationEffectMenuItems_Click);
            swingHoaverEffectMenuItem.Click += new RoutedEventHandler(animationEffectMenuItems_Click);
            fadeHoaverEffectMenuItem.Click += new RoutedEventHandler(animationEffectMenuItems_Click);
            rotateHoaverEffectMenuItem.Click += new RoutedEventHandler(animationEffectMenuItems_Click);
            zoomClickEffectMenuItem.Click += new RoutedEventHandler(animationEffectMenuItems_Click);
            swingClickEffectMenuItem.Click += new RoutedEventHandler(animationEffectMenuItems_Click);
            fadeClickEffectMenuItem.Click += new RoutedEventHandler(animationEffectMenuItems_Click);
            rotateClickEffectMenuItem.Click += new RoutedEventHandler(animationEffectMenuItems_Click);
            optionsMenuItem.Click += new RoutedEventHandler(optionsMenuItem_Click);
            contentsMenuItem.Click += new RoutedEventHandler(contentsMenuItem_Click);
            aboutMenuItem.Click += new RoutedEventHandler(aboutMenuItem_Click);
        }

        private void setDockyScreenPosition(string screenPosition)
        {
            currentPrimaryDockySettings.screenPosition = screenPosition;
            savePrimaryDockySettings();
            refreshDocky();
        }

        private void performAddIconThroughAddDockyIconWindow(ref AddDockyIconWindow addDockyIconWindow)
        {
            currentDockyIconSettings.Add(new IconSettings(imageLocation: addDockyIconWindow.iconImage.imageName, iconTitle: addDockyIconWindow.titleTextBox.Text.Trim(), target: addDockyIconWindow.targetTextBox.Text.Trim()));
            saveDockyIconSettings();

            currentPrimaryDockySettings.totalIcon += 1;
            savePrimaryDockySettings();

            addDockyIconWindow.Close();
        }

        private void addIconThroughAddDockyIconWindow(ref AddDockyIconWindow addDockyIconWindow)
        {
            if ((addDockyIconWindow.titleTextBox.Text.Trim() != string.Empty) & (addDockyIconWindow.targetTextBox.Text.Trim() != string.Empty))
                performAddIconThroughAddDockyIconWindow(addDockyIconWindow: ref addDockyIconWindow);
            else
                MessageBox.Show(messageBoxText: "Please, enter tilte and target path in textboxes.", caption: "WARNING", button: MessageBoxButton.OK, icon: MessageBoxImage.Warning);

            refreshDocky();
        }

        private bool addIconThroughAddDockyIconWindowWithConfirmation(ref AddDockyIconWindow addDockyIconWindow)
        {
            bool success = false;

            if ((addDockyIconWindow.titleTextBox.Text.Trim() != string.Empty) & (addDockyIconWindow.targetTextBox.Text.Trim() != string.Empty))
            {
                performAddIconThroughAddDockyIconWindow(addDockyIconWindow: ref addDockyIconWindow);
                success = true;
            }
            else
            {
                MessageBox.Show(messageBoxText: "Please, enter tilte and target path in textboxes.", caption: "WARNING", button: MessageBoxButton.OK, icon: MessageBoxImage.Warning);
            }

            return success;
        }

        private void loadDockyIconsAsListViewItems()
        {
            foreach (var iconSettings in currentDockyIconSettings)
            {
                if (iconSettings.imageLocation != "Skins/sep.png")
                    dockyIconSettingsWindow.iconsListView.Items.Add(new DockyIconSettingsListViewItem(imageLocation: iconSettings.imageLocation, iconTitle: iconSettings.iconTitle, target: iconSettings.target));
                else
                    dockyIconSettingsWindow.iconsListView.Items.Add(new DockyIconSettingsListViewItem());
            }
        }

        private void addSpecialIcon(string iconLocation, string iconTitle, string launchTarget)
        {
            currentDockyIconSettings.Add(new IconSettings(imageLocation: iconLocation, iconTitle: iconTitle, target: launchTarget));
            saveDockyIconSettings();

            currentPrimaryDockySettings.totalIcon += 1;
            savePrimaryDockySettings();

            refreshDocky();
        }

        private void getIconSettingsFromListViewItems()
        {
            currentDockyIconSettings.Clear();

            for (int i = 0; i < dockyIconSettingsWindow.iconsListView.Items.Count; i++)
            {
                DockyIconSettingsListViewItem dockyIconSettingsListViewItem = (DockyIconSettingsListViewItem)dockyIconSettingsWindow.iconsListView.Items[i];

                if (dockyIconSettingsListViewItem.titleTextBlock.Text == "<-- Separator -->")
                    currentDockyIconSettings.Add(new Separator().getSeparator());
                else
                    currentDockyIconSettings.Add(new IconSettings(imageLocation: dockyIconSettingsListViewItem.iconImage.imageName, iconTitle: dockyIconSettingsListViewItem.titleTextBlock.Text, target: dockyIconSettingsListViewItem.targetTextBlock.Text));
            }
        }

        private void setAnimationEffect(string type, string effect)
        {
            if (type == "Hoaver")
                currentPrimaryDockySettings.hoaverEffect = effect;
            else
                currentPrimaryDockySettings.clickEffect = effect;

            savePrimaryDockySettings();
            refreshDocky();
        }

        private void setValueForDockyOptionsWindow()
        {
            dockyOptionsWindow.screenPositionComboBox.SelectedValue = currentPrimaryDockySettings.screenPosition;
            dockyOptionsWindow.layeringComboBox.SelectedValue = currentPrimaryDockySettings.layering;
            dockyOptionsWindow.centeringSlider.Value = currentPrimaryDockySettings.centering;
            dockyOptionsWindow.edgeOffsetSlider.Value = currentPrimaryDockySettings.edgeOffset;
            dockyOptionsWindow.themeComboBox.SelectedValue = currentPrimaryDockySettings.theme;
            dockyOptionsWindow.hoaverEffectComboBox.SelectedValue = currentPrimaryDockySettings.hoaverEffect;
            dockyOptionsWindow.clickEffectComboBox.SelectedValue = currentPrimaryDockySettings.clickEffect;
            dockyOptionsWindow.showIconLabelCheckBox.IsChecked = currentPrimaryDockySettings.showIconLabel;
        }

        private void setEventHandlerForDockyOptionsWindow()
        {
            dockyOptionsWindow.screenPositionComboBox.SelectionChanged += new SelectionChangedEventHandler(dockyOptionsWindow_screenPositionComboBox_SelectionChanged);
            dockyOptionsWindow.layeringComboBox.SelectionChanged += new SelectionChangedEventHandler(dockyOptionsWindow_layeringComboBox_SelectionChanged);
            dockyOptionsWindow.centeringSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(dockyOptionsWindow_centeringSlider_ValueChanged);
            dockyOptionsWindow.edgeOffsetSlider.ValueChanged += new RoutedPropertyChangedEventHandler<double>(dockyOptionsWindow_edgeOffsetSlider_ValueChanged);
            dockyOptionsWindow.themeComboBox.SelectionChanged += new SelectionChangedEventHandler(dockyOptionsWindow_themeComboBox_SelectionChanged);
            dockyOptionsWindow.hoaverEffectComboBox.SelectionChanged += new SelectionChangedEventHandler(dockyOptionsWindow_hoaverEffectComboBox_SelectionChanged);
            dockyOptionsWindow.clickEffectComboBox.SelectionChanged += new SelectionChangedEventHandler(dockyOptionsWindow_clickEffectComboBox_SelectionChanged);
            dockyOptionsWindow.showIconLabelCheckBox.Click += new RoutedEventHandler(dockyOptionsWindow_showIconLabelCheckBox_Click);
            dockyOptionsWindow.defaultButton.Click += new RoutedEventHandler(dockyOptionsWindow_defaultButton_Click);
        }

        private void Docky_PreviewDragEnter(object sender, DragEventArgs dragEventArgs)
        {
            bool isCorrect = true;

            if (dragEventArgs.Data.GetDataPresent(DataFormats.FileDrop, true))
            {
                string[] filenames = (string[])dragEventArgs.Data.GetData(DataFormats.FileDrop, true);
                foreach (string filename in filenames)
                {
                    if (!File.Exists(filename))
                    {
                        isCorrect = false;
                        break;
                    }
                }
            }

            if (isCorrect)
                dragEventArgs.Effects = DragDropEffects.All;
            else
                dragEventArgs.Effects = DragDropEffects.None;

            dragEventArgs.Handled = true;
        }

        private void Docky_PreviewDragOver(object sender, DragEventArgs dragEventArgs)
        {
            dragEventArgs.Handled = true;
        }

        private void Docky_PreviewDrop(object sender, DragEventArgs dragEventArgs)
        {
            string[] filenames = (string[])dragEventArgs.Data.GetData(DataFormats.FileDrop, true);

            foreach (var filename in filenames)
            {
                string target = new LauncherInfo(filename).target;

                if (target == null)
                    target = filename;

                if (Directory.Exists(filename))
                    currentDockyIconSettings.Add(new IconSettings(imageLocation: "Icons/Folder-close.png", iconTitle: System.IO.Path.GetFileNameWithoutExtension(filename), target: target));
                else
                    currentDockyIconSettings.Add(new IconSettings(imageLocation: "Icons/unknown.png", iconTitle: System.IO.Path.GetFileNameWithoutExtension(filename), target: target));

                saveDockyIconSettings();

                currentPrimaryDockySettings.totalIcon += 1;
                savePrimaryDockySettings();
            }

            refreshDocky();
            refreshRemoveMenuItem();

            dragEventArgs.Handled = true;
        }

        private void fileAddMenuItem_Click(object sender, EventArgs evnt)
        {
            fileAddDockyIconWindow = new AddDockyIconWindow(type: "File");
            fileAddDockyIconWindow.saveButton.Click += new RoutedEventHandler(delegate(object sndr, RoutedEventArgs routedEventArgs)
            {
                addIconThroughAddDockyIconWindow(ref fileAddDockyIconWindow);
            });
        }

        private void pathAddMenuItem_Click(object sender, EventArgs eventArgs)
        {
            pathAddDockyIconWindow = new AddDockyIconWindow(type: "Path");
            pathAddDockyIconWindow.saveButton.Click += new RoutedEventHandler(delegate(object sndr, RoutedEventArgs routedEventArgs)
            {
                addIconThroughAddDockyIconWindow(ref pathAddDockyIconWindow);
            });
        }

        private void separatorAddMenuItem_Click(object sender, EventArgs eventArgs)
        {
            currentDockyIconSettings.Add((new Separator()).getSeparator());
            saveDockyIconSettings();
            currentPrimaryDockySettings.totalSeparator += 1;
            savePrimaryDockySettings();
            refreshDocky();
        }

        private void addSpecialIcon_AddMenuItem_Click(object sender, EventArgs eventArgs)
        {
            if (sender == computerAddMenuItem)
                addSpecialIcon(iconLocation: "Icons/My-Computer.png", iconTitle: "Computer", launchTarget: @"::{20D04FE0-3AEA-1069-A2D8-08002B30309D}");
            else if (sender == networkAddMenuItem)
                addSpecialIcon(iconLocation: "Icons/My-Network.png", iconTitle: "Network", launchTarget: @"::{F02C1A0D-BE21-4350-88B0-7367FC96EF3C}");
            else if (sender == documentsAddMenuItem)
                addSpecialIcon(iconLocation: "Icons/My-Documents.png", iconTitle: "Documents", launchTarget: @"::{031E4825-7B94-4DC3-B131-E946B44C8DD5}\Documents.library-ms");
            else if (sender == picturesAddMenuItem)
                addSpecialIcon(iconLocation: "Icons/My-Pictures.png", iconTitle: "Pictures", launchTarget: @"::{031E4825-7B94-4DC3-B131-E946B44C8DD5}\Pictures.library-ms");
            else if (sender == musicAddMenuItem)
                addSpecialIcon(iconLocation: "Icons/My-Music.png", iconTitle: "Music", launchTarget: @"::{031E4825-7B94-4DC3-B131-E946B44C8DD5}\Music.library-ms");
            else if (sender == gamesAddMenuItem)
                addSpecialIcon(iconLocation: "Icons/gamepad.png", iconTitle: "Games", launchTarget: @"::{ED228FDF-9EA8-4870-83B1-96B02CFE0D52}");
            else if (sender == controlPanelAddMenuItem)
                addSpecialIcon(iconLocation: "Icons/Control-Panel.png", iconTitle: "Control Panel", launchTarget: @"::{26EE0668-A00A-44D7-9371-BEB064C98683}\0");
            else if (sender == recycleBinAddMenuItem)
                addSpecialIcon(iconLocation: "Icons/Trashcan_full.png", iconTitle: "Recycle Bin", launchTarget: @"::{645FF040-5081-101B-9F08-00AA002F954E}");
        }

        private void iconSettingsMenuItem_Click(object sender, EventArgs eventArgs)
        {
            dockyIconSettingsWindow = new DockyIconSettingsWindow();

            loadDockyIconsAsListViewItems();

            // Adding Event Handler
            dockyIconSettingsWindow.iconsListView.SelectionChanged += new SelectionChangedEventHandler(dockyIconSettingsWindow_iconsListView_SelectionChanged);
            dockyIconSettingsWindow.addFileButton.Click += new RoutedEventHandler(dockyIconSettingsWindow_addFileButton_Click);
            dockyIconSettingsWindow.addPathButton.Click += new RoutedEventHandler(dockyIconSettingsWindow_addPathButton_Click);
            dockyIconSettingsWindow.addSeparatorButton.Click += new RoutedEventHandler(dockyIconSettingsWindow_addSeparatorButton_Click);
            dockyIconSettingsWindow.editButton.Click += new RoutedEventHandler(dockyIconSettingsWindow_editButton_Click);
            dockyIconSettingsWindow.removeButton.Click += new RoutedEventHandler(dockyIconSettingsWindow_removeButton_Click);
            dockyIconSettingsWindow.moveUpButton.Click += new RoutedEventHandler(dockyIconSettingsWindow_moveUpButton_Click);
            dockyIconSettingsWindow.moveDownButton.Click += new RoutedEventHandler(dockyIconSettingsWindow_moveDownButton_Click);
        }

        private void dockyIconSettingsWindow_iconsListView_SelectionChanged(object sender, EventArgs eventArgs)
        {
            try
            {
                if (currentDockyIconSettings[dockyIconSettingsWindow.iconsListView.SelectedIndex].iconTitle != string.Empty)
                    dockyIconSettingsWindow.setEnableOrDisableAllOrganizeIconButtons(status: true);
                else
                {
                    dockyIconSettingsWindow.setEnableOrDisableAllOrganizeIconButtons(status: true);
                    dockyIconSettingsWindow.editButton.IsEnabled = false;
                }

                if ((dockyIconSettingsWindow.iconsListView.SelectedIndex - 1) < 0)
                    dockyIconSettingsWindow.moveUpButton.IsEnabled = false;
                else if ((dockyIconSettingsWindow.iconsListView.SelectedIndex + 1) > (dockyIconSettingsWindow.iconsListView.Items.Count - 1))
                    dockyIconSettingsWindow.moveDownButton.IsEnabled = false;

            }
            catch (Exception)
            {
                // Do nothing
            }
        }

        private void dockyIconSettingsWindow_addFileButton_Click(object sender, EventArgs eventArgs)
        {
            AddDockyIconWindow addFileDockyIconWindowForDockyIconsSettings = new AddDockyIconWindow(type: "File");
            addFileDockyIconWindowForDockyIconsSettings.saveButton.Click += new RoutedEventHandler(delegate(object sndr, RoutedEventArgs routedEventArgs)
            {
                bool response = addIconThroughAddDockyIconWindowWithConfirmation(addDockyIconWindow: ref addFileDockyIconWindowForDockyIconsSettings);

                if (response)
                {
                    dockyIconSettingsWindow.iconsListView.Items.Add(new DockyIconSettingsListViewItem(imageLocation: addFileDockyIconWindowForDockyIconsSettings.iconImage.imageName, iconTitle: addFileDockyIconWindowForDockyIconsSettings.titleTextBox.Text, target: addFileDockyIconWindowForDockyIconsSettings.targetTextBox.Text));
                    refreshDocky();
                }
            });
        }

        private void dockyIconSettingsWindow_addPathButton_Click(object sender, EventArgs eventArgs)
        {
            AddDockyIconWindow addPathDockyIconWindowForDockyIconsSettings = new AddDockyIconWindow(type: "Path");
            addPathDockyIconWindowForDockyIconsSettings.saveButton.Click +=
                new RoutedEventHandler(delegate(object sndr, RoutedEventArgs routedEventArgs)
                {
                    bool response = addIconThroughAddDockyIconWindowWithConfirmation(addDockyIconWindow: ref addPathDockyIconWindowForDockyIconsSettings);

                    if (response)
                    {
                        dockyIconSettingsWindow.iconsListView.Items.Add(new DockyIconSettingsListViewItem(imageLocation: addPathDockyIconWindowForDockyIconsSettings.iconImage.imageName, iconTitle: addPathDockyIconWindowForDockyIconsSettings.titleTextBox.Text, target: addPathDockyIconWindowForDockyIconsSettings.targetTextBox.Text));
                        refreshDocky();
                    }
                });
        }

        private void dockyIconSettingsWindow_addSeparatorButton_Click(object sender, EventArgs eventArgs)
        {
            currentDockyIconSettings.Add((new Separator()).getSeparator());
            saveDockyIconSettings();
            currentPrimaryDockySettings.totalSeparator += 1;
            savePrimaryDockySettings();

            dockyIconSettingsWindow.iconsListView.Items.Add(new DockyIconSettingsListViewItem());

            refreshDocky();
        }

        private void dockyIconSettingsWindow_editButton_Click(object sender, EventArgs eventArgs)
        {
            AddDockyIconWindow editAddDockyIconWindow;

            if (File.Exists(path: currentDockyIconSettings[dockyIconSettingsWindow.iconsListView.SelectedIndex].target))
                editAddDockyIconWindow = new AddDockyIconWindow(type: "File");
            else
                editAddDockyIconWindow = new AddDockyIconWindow(type: "Path");

            editAddDockyIconWindow.iconImage = new CustomImage(imageName: currentDockyIconSettings[dockyIconSettingsWindow.iconsListView.SelectedIndex].imageLocation, width: 48, height: 48);
            editAddDockyIconWindow.iconButton.Content = editAddDockyIconWindow.iconImage;

            editAddDockyIconWindow.titleTextBox.Text =
                currentDockyIconSettings[dockyIconSettingsWindow.iconsListView.SelectedIndex].iconTitle;
            editAddDockyIconWindow.targetTextBox.Text =
                currentDockyIconSettings[dockyIconSettingsWindow.iconsListView.SelectedIndex].target;

            int currentIndex = dockyIconSettingsWindow.iconsListView.SelectedIndex;

            editAddDockyIconWindow.saveButton.Click += new RoutedEventHandler(delegate(object sndr, RoutedEventArgs routedEventArgs)
            {
                if ((editAddDockyIconWindow.titleTextBox.Text.Trim() != string.Empty) & (editAddDockyIconWindow.targetTextBox.Text.Trim() != string.Empty))
                {
                    currentDockyIconSettings[currentIndex].imageLocation = editAddDockyIconWindow.iconImage.imageName;
                    currentDockyIconSettings[currentIndex].iconTitle = editAddDockyIconWindow.titleTextBox.Text;
                    currentDockyIconSettings[currentIndex].target = editAddDockyIconWindow.targetTextBox.Text;
                    saveDockyIconSettings();

                    editAddDockyIconWindow.Close();

                    dockyIconSettingsWindow.iconsListView.Items.Clear();
                    loadDockyIconsAsListViewItems();
                    dockyIconSettingsWindow.setEnableOrDisableAllOrganizeIconButtons(status: false);

                    refreshDocky();
                }
                else
                {
                    MessageBox.Show(messageBoxText: "Please, enter tilte and target path in textboxes.", caption: "WARNING", button: MessageBoxButton.OK, icon: MessageBoxImage.Warning);
                }
            });
        }

        private void dockyIconSettingsWindow_removeButton_Click(object sender, EventArgs eventArgs)
        {
            int index = dockyIconSettingsWindow.iconsListView.SelectedIndex;

            if (index != -1)
            {
                DockyIconSettingsListViewItem dockyIconSettingsListViewItem = (DockyIconSettingsListViewItem)dockyIconSettingsWindow.iconsListView.Items[index];

                if (currentDockyIconSettings[index].imageLocation == "Skins/sep.png")
                    currentPrimaryDockySettings.totalSeparator -= 1;
                else
                    currentPrimaryDockySettings.totalIcon -= 1;

                savePrimaryDockySettings();

                currentDockyIconSettings.RemoveAt(index);
                saveDockyIconSettings();

                dockyIconSettingsWindow.iconsListView.Items.Remove(dockyIconSettingsListViewItem);
                dockyIconSettingsWindow.setEnableOrDisableAllOrganizeIconButtons(status: false);
            }

            refreshDocky();
        }

        private void dockyIconSettingsWindow_moveUpButton_Click(object sender, EventArgs eventArgs)
        {
            int index = dockyIconSettingsWindow.iconsListView.SelectedIndex;

            if ((index - 1) != -1)
            {
                DockyIconSettingsListViewItem dockyIconSettingsListViewItem = (DockyIconSettingsListViewItem)dockyIconSettingsWindow.iconsListView.Items[index];

                dockyIconSettingsWindow.iconsListView.Items.Remove(removeItem: dockyIconSettingsListViewItem);
                dockyIconSettingsWindow.iconsListView.Items.Insert(insertIndex: index - 1, insertItem: dockyIconSettingsListViewItem);

                dockyIconSettingsWindow.iconsListView.SelectedIndex = index - 1;

                getIconSettingsFromListViewItems();
                saveDockyIconSettings();
            }

            refreshDocky();
        }

        private void dockyIconSettingsWindow_moveDownButton_Click(object sender, EventArgs eventArgs)
        {
            int index = dockyIconSettingsWindow.iconsListView.SelectedIndex;

            if ((index + 1) < dockyIconSettingsWindow.iconsListView.Items.Count)
            {
                DockyIconSettingsListViewItem dockyIconSettingsListViewItem = (DockyIconSettingsListViewItem)dockyIconSettingsWindow.iconsListView.Items[index];

                dockyIconSettingsWindow.iconsListView.Items.Remove(removeItem: dockyIconSettingsListViewItem);
                dockyIconSettingsWindow.iconsListView.Items.Insert(insertIndex: index + 1, insertItem: dockyIconSettingsListViewItem);

                dockyIconSettingsWindow.iconsListView.SelectedIndex = index + 1;

                getIconSettingsFromListViewItems();
                saveDockyIconSettings();
            }

            refreshDocky();
        }

        private void screenPositionMenuItems_Click(object sender, EventArgs eventArgs)
        {
            if (sender == topScreenPositionMenuItem)
                setDockyScreenPosition(screenPosition: "Top");
            else if (sender == bottomScreenPositionMenuItem)
                setDockyScreenPosition(screenPosition: "Bottom");
            else if (sender == leftScreenPositionMenuItem)
                setDockyScreenPosition(screenPosition: "Left");
            else if (sender == rightScreenPositionMenuItem)
                setDockyScreenPosition(screenPosition: "Right");
        }

        private void animationEffectMenuItems_Click(object sender, EventArgs eventArgs)
        {
            if (sender == zoomHoaverEffectMenuItem)
                setAnimationEffect(type: "Hoaver", effect: "Zoom");
            else if (sender == swingHoaverEffectMenuItem)
                setAnimationEffect(type: "Hoaver", effect: "Swing");
            else if (sender == fadeHoaverEffectMenuItem)
                setAnimationEffect(type: "Hoaver", effect: "Fade");
            else if (sender == rotateHoaverEffectMenuItem)
                setAnimationEffect(type: "Hoaver", effect: "Rotate");
            else if (sender == zoomClickEffectMenuItem)
                setAnimationEffect(type: "Click", effect: "Zoom");
            else if (sender == swingClickEffectMenuItem)
                setAnimationEffect(type: "Click", effect: "Swing");
            else if (sender == fadeClickEffectMenuItem)
                setAnimationEffect(type: "Click", effect: "Fade");
            else if (sender == rotateClickEffectMenuItem)
                setAnimationEffect(type: "Click", effect: "Rotate");
        }

        private void optionsMenuItem_Click(object sender, EventArgs eventArgs)
        {
            dockyOptionsWindow = new DockyOptionsWindow();
            setValueForDockyOptionsWindow();

            // Adding Event Handler
            setEventHandlerForDockyOptionsWindow();
        }

        private void dockyOptionsWindow_screenPositionComboBox_SelectionChanged(object sender, EventArgs eventArgs)
        {
            currentPrimaryDockySettings.screenPosition = dockyOptionsWindow.screenPositionComboBox.SelectedValue.ToString();
            savePrimaryDockySettings();
            refreshDocky();
        }

        private void dockyOptionsWindow_layeringComboBox_SelectionChanged(object sender, EventArgs eventArgs)
        {
            currentPrimaryDockySettings.layering = dockyOptionsWindow.layeringComboBox.SelectedValue.ToString();
            savePrimaryDockySettings();
            refreshDocky();
        }

        private void dockyOptionsWindow_centeringSlider_ValueChanged(object sender, EventArgs eventArgs)
        {
            currentPrimaryDockySettings.centering = (int)dockyOptionsWindow.centeringSlider.Value;
            savePrimaryDockySettings();
            refreshDocky();
        }

        private void dockyOptionsWindow_edgeOffsetSlider_ValueChanged(object sender, EventArgs eventArgs)
        {
            currentPrimaryDockySettings.edgeOffset = (int)dockyOptionsWindow.edgeOffsetSlider.Value;
            savePrimaryDockySettings();
            refreshDocky();
        }

        private void dockyOptionsWindow_themeComboBox_SelectionChanged(object sender, EventArgs eventArgs)
        {
            currentPrimaryDockySettings.theme = dockyOptionsWindow.themeComboBox.SelectedValue.ToString();
            savePrimaryDockySettings();
            refreshDocky();
        }

        private void dockyOptionsWindow_hoaverEffectComboBox_SelectionChanged(object sender, EventArgs eventArgs)
        {
            currentPrimaryDockySettings.hoaverEffect = dockyOptionsWindow.hoaverEffectComboBox.SelectedValue.ToString();
            savePrimaryDockySettings();
            refreshDocky();
        }

        private void dockyOptionsWindow_clickEffectComboBox_SelectionChanged(object sender, EventArgs eventArgs)
        {
            currentPrimaryDockySettings.clickEffect = dockyOptionsWindow.clickEffectComboBox.SelectedValue.ToString();
            savePrimaryDockySettings();
            refreshDocky();
        }

        private void dockyOptionsWindow_showIconLabelCheckBox_Click(object sender, EventArgs eventArgs)
        {
            if (dockyOptionsWindow.showIconLabelCheckBox.IsChecked == true)
                currentPrimaryDockySettings.showIconLabel = true;
            else
                currentPrimaryDockySettings.showIconLabel = false;

            savePrimaryDockySettings();
            refreshDocky();
        }

        private void dockyOptionsWindow_defaultButton_Click(object sender, EventArgs eventArgs)
        {
            int totalIcon = currentPrimaryDockySettings.totalIcon,
                totalSeparator = currentPrimaryDockySettings.totalSeparator;

            currentPrimaryDockySettings = new PrimaryDockySettings();
            currentPrimaryDockySettings.totalIcon = totalIcon;
            currentPrimaryDockySettings.totalSeparator = totalSeparator;

            savePrimaryDockySettings();
            refreshDocky();

            dockyOptionsWindow.Close();
            dockyOptionsWindow = new DockyOptionsWindow();

            setValueForDockyOptionsWindow();
            setEventHandlerForDockyOptionsWindow();
        }

        private void contentsMenuItem_Click(object sender, EventArgs eventArgs)
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

        private void aboutMenuItem_Click(object sender, EventArgs eventArgs)
        {
            AboutWindow aboutWindow = new AboutWindow();
        }
    }
}
