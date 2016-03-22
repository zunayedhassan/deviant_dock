using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Deviant_Dock
{
    class AboutStackPanel : StackPanel
    {
        public AboutStackPanel()
        {
            this.Background = new LinearGradientBrush(startColor: Colors.White, endColor: Color.FromRgb(r: 235, g: 235, b: 235), angle: -45);
            this.Orientation = Orientation.Vertical;

            StackPanel headerStackPanel = new StackPanel();
            headerStackPanel.Orientation = Orientation.Horizontal;

            this.Children.Add(new TextBlock());
            this.Children.Add(headerStackPanel);
            headerStackPanel.Children.Add(new TextBlock()
                                  {
                                      Text = "\t"
                                  });
            headerStackPanel.Children.Add(new CustomImage(imageName: "Contents/Icon/icon.png", width: 48, height: 48));
            headerStackPanel.Children.Add(new TextBlock()
                                              {
                                                  Text = "  Deviant Dock 1.0",
                                                  FontSize = 24,
                                                  FontWeight = FontWeights.Bold
                                              });
            this.Children.Add(new CustomTextBlock(text: "Developed by:\n" +
                                                        "Mohammad Zunayed Hassan\n" +
                                                        "Email: zunayed-hassan@live.com\n\n" + 
                                                        "Artwork by:\n" +
                                                        "David Vignoni, Vincent Garnier, Gabriel (z08-Styles)\n" +
                                                        "Punklab, VeryIcon.com and VistaIco.com"));
        }
    }
}
