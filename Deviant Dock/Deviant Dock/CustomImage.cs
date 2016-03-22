using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Deviant_Dock
{
    class CustomImage : Image
    {
        public string imageName;

        public CustomImage(string imageName, int width, int height)
        {
            this.imageName = imageName;

            // Create Image Element
            this.Width = width;
            this.Height = height;

            // Create source
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.BeginInit();
            bitmapImage.UriSource = new Uri(imageName, UriKind.RelativeOrAbsolute);
            bitmapImage.DecodePixelWidth = width;
            bitmapImage.DecodePixelHeight = height;
            bitmapImage.EndInit();

            //set image source
            this.Source = bitmapImage;
        }
    }
}
