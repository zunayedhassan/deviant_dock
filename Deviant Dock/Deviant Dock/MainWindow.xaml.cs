
/* ********************************************************************************************************************** *\
 * APPLICATION:     DEVIANT DOCK
 * VERSION:         1.0
 * RELEASE DATE:    24th September, 2011
 * ----------------------------------------------------------------------------------------------------------------------
 * DEVELOPED BY:    Mohammad Zunayed Hassan
 * EMAIL:           zunayed-hassan@live.com
 * 
 * NOTE:
 * 
 * 
\* ********************************************************************************************************************** */


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
using System.Windows.Forms;
using System.Windows.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.IO;
using Button = System.Windows.Controls.Button;
using ListView = System.Windows.Controls.ListView;
using MenuItem = System.Windows.Forms.MenuItem;

namespace Deviant_Dock
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private PrimarySettings currentPrimarySettings;
        private int STANDARD_TASKBAR_HEIGHT = 50;
        private NotifyIcon notifyIcon;
        private System.Windows.Forms.ContextMenu notifyIconContextMenu;
        private string settingsPath = GlobalSettingsPath.path;

        private NotifyIconMenuItem restoreNotifyIconMenuItem,
                                   exitNotifyIconMenuItem,
                                   helpNotifyIconMenuItem,
                                   contentsNotifyIconMenuItem,
                                   aboutNotifyIconMenuItem;

        private CustomImage closeImage,
                            minimizeImage;

        private ListView dockItemListView;

        private DockListViewItem dockyListViewItem,
                                 ringyListViewItem;

        private CustomButton exitButton,
                             helpButton;

        private Docky docky = new Docky();
        private Ringy ringy = new Ringy();

        public MainWindow()
        {
            InitializeComponent();

            // Initializing MainWindow
            this.Title = "Deviant Dock";
            this.Width = 450;
            this.Height = 300;
            this.Left = SystemParameters.PrimaryScreenWidth - (this.Width + 20);
            this.Top = SystemParameters.PrimaryScreenHeight - (this.Height + STANDARD_TASKBAR_HEIGHT);
            this.Topmost = true;
            this.WindowStyle = WindowStyle.None;

            try
            {
                this.Icon = BitmapFrame.Create(new Uri("Contents/Icon/icon.ico", UriKind.RelativeOrAbsolute));
            }
            catch (Exception)
            {
               // Do nothing
            }

            GlassEffect.SetIsEnabled(element: this, value: true);

            // Initialing notifyIconContextMenu
            notifyIconContextMenu = new System.Windows.Forms.ContextMenu();

            // Initialize NotifyIconMenuItems
            restoreNotifyIconMenuItem = new NotifyIconMenuItem(text: "Restore", contextMenu: ref notifyIconContextMenu);
            exitNotifyIconMenuItem = new NotifyIconMenuItem(text: "Exit", contextMenu: ref notifyIconContextMenu);
            notifyIconContextMenu.MenuItems.Add("-");
            helpNotifyIconMenuItem = new NotifyIconMenuItem(text: "Help", contextMenu: ref notifyIconContextMenu);
            contentsNotifyIconMenuItem = new NotifyIconMenuItem(text: "Contents", menuItem: ref helpNotifyIconMenuItem);
            aboutNotifyIconMenuItem = new NotifyIconMenuItem(text: "About", menuItem: ref helpNotifyIconMenuItem);

            // Creating the NotifyIcon
            notifyIcon = new NotifyIcon();
            notifyIcon.Text = "Deviant Dock";                           // The Text property sets the text that will be displayed, in a tooltip, when the mouse hovers over the systray icon.
            notifyIcon.ContextMenu = notifyIconContextMenu;             // The ContextMenu property sets the menu that will appear when the systray icon is right clicked.
            try
            {
                notifyIcon.Icon = new Icon("Contents/Icon/icon.ico");   // The Icon property sets the icon that will appear in the systray for this application.
            }
            catch (Exception)
            {
                // Do nothing
            }

            Canvas deviantDockMainCanvas = new Canvas();

            minimizeImage = new CustomImage(imageName: "Contents/Images/control_button/button_min_regular.png", width: 24, height: 18);
            closeImage = new CustomImage(imageName: "Contents/Images/control_button/button_close_regular.png", width: 43, height: 18);
            minimizeImage.Margin = new Thickness((this.Width - (minimizeImage.Width + closeImage.Width + 15)), 0, 0, 0);
            minimizeImage.ToolTip = "Minimize";
            closeImage.Margin = new Thickness(this.Width - (closeImage.Width + 15), 0, 0, 0);
            closeImage.ToolTip = "Close to system tray";
            
            dockItemListView = new ListView();
            dockItemListView.Margin = new Thickness(10, 40, 0, 0);
            dockItemListView.Width = this.Width - ((2*20) - 5);
            dockItemListView.Height = this.Height - (this.Height/3);
            dockItemListView.Opacity = 0.70;

            dockyListViewItem = new DockListViewItem(title: "Docky", versionNo: "1.0", description: "A simple dock", imageLocation: "Contents/Images/application_image/icon.png");
            ringyListViewItem = new DockListViewItem(title: "Ringy", versionNo: "1.0", description: "Circle shaped\ndock", imageLocation: "Contents/Images/application_image/icon.png");

            exitButton = new CustomButton(buttonContent: "Exit", width: 65, height: 25, thickness: new Thickness(dockItemListView.Width - 55, dockItemListView.Height + 55, 0, 0));
            helpButton = new CustomButton(buttonContent: "Help", width: (int) exitButton.Width, height: (int) exitButton.Height, thickness: new Thickness(dockItemListView.Width - (55 + 5 + exitButton.Width), dockItemListView.Height + 55, 0, 0));

            // Adding Contents
            this.Content = deviantDockMainCanvas;
            deviantDockMainCanvas.Children.Add(new CustomImage(imageName: "Contents/Images/application_image/icon.png", width: 16, height: 16));
            deviantDockMainCanvas.Children.Add(new TextBlock()
                                                   {
                                                       Text = this.Title,
                                                       Margin = new Thickness(16 + 5, 0, 0, 0)
                                                   });
            deviantDockMainCanvas.Children.Add(minimizeImage);
            deviantDockMainCanvas.Children.Add(closeImage);
            deviantDockMainCanvas.Children.Add(dockItemListView);
            dockItemListView.Items.Add(dockyListViewItem);
            dockItemListView.Items.Add(ringyListViewItem);
            deviantDockMainCanvas.Children.Add(exitButton);
            deviantDockMainCanvas.Children.Add(helpButton);

            // Adding Event Handler
            this.MouseLeftButtonDown += new MouseButtonEventHandler(MainWindow_MouseLeftButtonDown);
            restoreNotifyIconMenuItem.Click += new EventHandler(restoreNotifyIconMenuItem_Click);
            exitNotifyIconMenuItem.Click += new EventHandler(exitNotifyIconMenuItem_Click);
            minimizeImage.MouseLeftButtonDown += new MouseButtonEventHandler(minimizeImage_MouseLeftButtonDown);
            closeImage.MouseLeftButtonDown += new MouseButtonEventHandler(closeImage_MouseLeftButtonDown);
            dockyListViewItem.addButton.Click += new RoutedEventHandler(dockyListViewItem_addButton_Click);
            dockyListViewItem.removeButton.Click += new RoutedEventHandler(dockyListViewItem_removeButton_Click);
            ringyListViewItem.addButton.Click += new RoutedEventHandler(ringyListViewItem_addButton_Click);
            ringyListViewItem.removeButton.Click += new RoutedEventHandler(ringyListViewItem_removeButton_Click);
            exitButton.Click += new RoutedEventHandler(exitButton_Click);
            helpButton.Click += new RoutedEventHandler(helpButton_Click);
            contentsNotifyIconMenuItem.Click += new EventHandler(contentsNotifyIconMenuItem_Click);
            aboutNotifyIconMenuItem.Click += new EventHandler(aboutNotifyIconMenuItem_Click);

            // Initializing Application Settings
            closeToSystemTray();

            if (!File.Exists(path: settingsPath + "PrimarySettings.dds"))
                createNewPrimarySettings();

            loadPrimarySettings();
            loadDocky();
            loadRingy();
        }

        private void closeToSystemTray()
        {
            notifyIcon.Visible = true;
            notifyIcon.ShowBalloonTip(timeout: 1, tipTitle: "Devint Dock", tipText: "This application is still running on system tray", tipIcon: ToolTipIcon.Info);
            this.Hide();
        }

        private void restoreMainWindow()
        {
            notifyIcon.Visible = false;
            this.Visibility = Visibility.Visible;
        }

        private void createNewPrimarySettings()
        {
            try
            {
                currentPrimarySettings = new PrimarySettings();

                savePrimarySettings();
            }
            catch (Exception createNewPrimarySettingsException)
            {
                System.Windows.MessageBox.Show(messageBoxText: "Can't create PrimarySettings.dds file in Settings directory. Make sure that the filesystem isn't write protected.", caption: "ERROR", button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }

        private void loadPrimarySettings()
        {
            // Loading settings from PrimarySettings.dds file
            try
            {
                FileStream primarySettingsInput = new FileStream(settingsPath + "PrimarySettings.dds", FileMode.Open, FileAccess.Read);
                BinaryFormatter primarySettingsReader = new BinaryFormatter();
                currentPrimarySettings = new PrimarySettings();

                currentPrimarySettings = (PrimarySettings)primarySettingsReader.Deserialize(primarySettingsInput);

                if (currentPrimarySettings.Docky)
                {
                    dockyListViewItem.addButton.IsEnabled = false;
                    dockyListViewItem.removeButton.IsEnabled = true;
                }
                else
                {
                    dockyListViewItem.addButton.IsEnabled = true;
                    dockyListViewItem.removeButton.IsEnabled = false;
                }

                if (currentPrimarySettings.Ringy)
                {
                    ringyListViewItem.addButton.IsEnabled = false;
                    ringyListViewItem.removeButton.IsEnabled = true;
                }
                else
                {
                    ringyListViewItem.addButton.IsEnabled = true;
                    ringyListViewItem.removeButton.IsEnabled = false;
                }

                primarySettingsInput.Close();
            }
            catch (Exception loadPrimarySettingsException)
            {
                System.Windows.MessageBox.Show(messageBoxText: "Can't open/find PrimarySettings.dds file in Settings directory. PrimarySettings.dds file might be corrupted or deleted.", caption: "ERROR", button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }

        private void savePrimarySettings()
        {
            try
            {
                FileStream primarySettingsFileStream = new FileStream(settingsPath + "PrimarySettings.dds", FileMode.OpenOrCreate, FileAccess.ReadWrite);      // *.dds means 'Deviant Dock Settings'
                BinaryFormatter primarySettingsFormatter = new BinaryFormatter();

                primarySettingsFormatter.Serialize(primarySettingsFileStream, currentPrimarySettings);
                primarySettingsFileStream.Close();
            }
            catch (Exception savePrimarySettingsException)
            {
                System.Windows.MessageBox.Show(messageBoxText: "Can't save settings in PrimarySettings.dds file. Make sure that, PrimarySettings.dds file is realy exists in Settings directory or the system isn't write protected!", caption: "ERROR", button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }

        private void showHelp()
        {
            try
            {
                System.Diagnostics.Process.Start("help.chm");
            }
            catch (Exception)
            {
                System.Windows.MessageBox.Show(messageBoxText: "help.chm file can not be found!", caption: "ERROR", button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }

        private void loadDocky()
        {
            if (currentPrimarySettings.Docky)
                docky.Visibility = Visibility.Visible;
            else
                docky.Visibility = Visibility.Hidden;
        }

        private void loadRingy()
        {
            if (currentPrimarySettings.Ringy)
                ringy.Visibility = Visibility.Visible;
            else
                ringy.Visibility = Visibility.Hidden;
        }

        private void exitAll()
        {
            this.Close();
            docky.Close();
            ringy.Close();
        }

        private void MainWindow_MouseLeftButtonDown(object sender, EventArgs evnt)
        {
            this.DragMove();
        }

        private void restoreNotifyIconMenuItem_Click(object sender, EventArgs evnt)
        {
            restoreMainWindow();
        }

        private void exitNotifyIconMenuItem_Click(object sender, EventArgs evnt)
        {
            exitAll();
        }

        private void contentsNotifyIconMenuItem_Click(object sender, EventArgs eventArgs)
        {
            showHelp();
        }

        private void aboutNotifyIconMenuItem_Click(object sender, EventArgs eventArgs)
        {
            AboutWindow aboutWindow = new AboutWindow();
        }

        private void minimizeImage_MouseLeftButtonDown(object sender, EventArgs evnt)
        {
            this.WindowState = WindowState.Minimized;
        }

        private void closeImage_MouseLeftButtonDown(object senderm, EventArgs evnt)
        {
            closeToSystemTray();
        }

        private void dockyListViewItem_addButton_Click(object sender, EventArgs evnt)
        {
            currentPrimarySettings.Docky = true;
            savePrimarySettings();
            loadDocky();
        }

        private void dockyListViewItem_removeButton_Click(object sender, EventArgs evnt)
        {
            currentPrimarySettings.Docky = false;
            savePrimarySettings();
            loadDocky();
        }

        private void ringyListViewItem_addButton_Click(object sender, EventArgs evnt)
        {
            currentPrimarySettings.Ringy = true;
            savePrimarySettings();
            loadRingy();
        }

        private void ringyListViewItem_removeButton_Click(object sender, EventArgs evnt)
        {
            currentPrimarySettings.Ringy = false;
            savePrimarySettings();
            loadRingy();
        }

        private void exitButton_Click(object sender, EventArgs evnt)
        {
            exitAll();
        }

        private void helpButton_Click(object sender, EventArgs evnt)
        {
            showHelp();
        }
    }
}
