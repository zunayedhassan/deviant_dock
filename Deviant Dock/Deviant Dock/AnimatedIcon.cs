using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Deviant_Dock
{
    class AnimatedIcon : CustomImage
    {
        private static int STANDARD_ICON_DIMENSION = 64;

        private string screenPosition,
                       hoaverEffect,
                       clickEffect,
                       iconTitle,
                       target;

        private bool showIconLabel;

        public AnimatedIcon(string imageLocation, string iconTitle, string target, PrimaryDockySettings primaryDockySettings)
            : base(imageLocation, STANDARD_ICON_DIMENSION, STANDARD_ICON_DIMENSION)
        {
            this.iconTitle = iconTitle;
            this.target = target;
            this.screenPosition = primaryDockySettings.screenPosition;
            this.hoaverEffect = primaryDockySettings.hoaverEffect;
            this.clickEffect = primaryDockySettings.clickEffect;
            this.showIconLabel = primaryDockySettings.showIconLabel;

            setToolTip();
            setEventHandler();
        }

        public AnimatedIcon(string imageLocation)
            : base(imageLocation, 1, 54)
        {
            // Do nothing
        }

        public AnimatedIcon(string imageLocation, string iconTitle, string target, int iconNo, PrimaryRingySettings primaryRingySettings) : base(imageLocation, (int) Math.Sqrt(STANDARD_ICON_DIMENSION)*12, (int) Math.Sqrt(STANDARD_ICON_DIMENSION)*12)
        {
            this.iconTitle = iconTitle;
            this.target = target;
            this.hoaverEffect = primaryRingySettings.hoaverEffect;
            this.clickEffect = "None";
            this.showIconLabel = primaryRingySettings.showIconLabel;

            int row = 0,
                column = 0;

            switch (iconNo)
            {
                case 1:
                    row = 0;
                    column = 1;
                    break;

                case 2:
                    row = 0;
                    column = 2;
                    break;

                case 3:
                    row = 1;
                    column = 2;
                    break;

                case 4:
                    row = 2;
                    column = 2;
                    break;

                case 5:
                    row = 2;
                    column = 1;
                    break;

                case 6:
                    row = 2;
                    column = 0;
                    break;

                case 7:
                    row = 1;
                    column = 0;
                    break;

                case 8:
                    row = 0;
                    column = 0;
                    break;
            }

            Grid.SetColumn(element: this, value: column);
            Grid.SetRow(element: this, value: row);

            setToolTip();
            setEventHandler();
        }

        private void setToolTip()
        {
            if (this.showIconLabel)
            {
                this.ToolTip = new ToolTip()
                {
                    Content = new TextBlock()
                    {
                        Text = this.iconTitle
                    },

                    Background = new SolidColorBrush(Colors.Transparent),
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 18
                };
            }
        }

        private void setEventHandler()
        {
            // Adding Event Handler
            this.MouseEnter += new System.Windows.Input.MouseEventHandler(AnimatedIcon_MouseEnter);
            this.MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(AnimatedIcon_MouseLeftButtonDown);
        }

        private void zoomAnimation()
        {
            TransformGroup zoomAnimationTransformGroup = new TransformGroup();
            ScaleTransform zoomAnimationScaleTransform = new ScaleTransform();
            zoomAnimationTransformGroup.Children.Add(zoomAnimationScaleTransform);

            zoomAnimationScaleTransform.CenterX = this.Width / 2;
            zoomAnimationScaleTransform.CenterY = this.Height / 2;

            this.RenderTransform = zoomAnimationTransformGroup;

            DoubleAnimation zoomDoubleAnimation = new DoubleAnimation();
            zoomDoubleAnimation.From = 1;
            zoomDoubleAnimation.To = 1.50;
            zoomDoubleAnimation.AutoReverse = true;
            zoomAnimationScaleTransform.BeginAnimation(ScaleTransform.ScaleXProperty, zoomDoubleAnimation);
            zoomAnimationScaleTransform.BeginAnimation(ScaleTransform.ScaleYProperty, zoomDoubleAnimation);
        }

        private void swingAnimation(string screenPosition)
        {
            TransformGroup springAnimationTransformGroup = new TransformGroup();
            ScaleTransform springAnimationScaleTransformGroup = new ScaleTransform();
            springAnimationScaleTransformGroup.CenterX = this.Width / 2;
            springAnimationScaleTransformGroup.CenterY = this.Height / 2;

            springAnimationTransformGroup.Children.Add(springAnimationScaleTransformGroup);

            this.RenderTransform = springAnimationScaleTransformGroup;

            DoubleAnimation springDoubleAnimation = new DoubleAnimation();
            springDoubleAnimation.From = 1;
            springDoubleAnimation.To = 1.5;
            springDoubleAnimation.AutoReverse = true;
            springDoubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(500));

            DoubleAnimation jumpDoubleAnimation = new DoubleAnimation();
            jumpDoubleAnimation.From = STANDARD_ICON_DIMENSION;
            jumpDoubleAnimation.To = STANDARD_ICON_DIMENSION * 2;
            jumpDoubleAnimation.AutoReverse = true;
            jumpDoubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(500));

            if ((screenPosition == "Top") | (screenPosition == "Bottom"))
            {
                springAnimationScaleTransformGroup.BeginAnimation(ScaleTransform.ScaleXProperty, springDoubleAnimation);
                this.BeginAnimation(AnimatedIcon.HeightProperty, jumpDoubleAnimation);
            }
            else
            {
                springAnimationScaleTransformGroup.BeginAnimation(ScaleTransform.ScaleYProperty, springDoubleAnimation);
                this.BeginAnimation(AnimatedIcon.WidthProperty, jumpDoubleAnimation);
            }
        }

        private void fadeAnimation()
        {
            DoubleAnimation fadeDoubleAnimation = new DoubleAnimation();
            fadeDoubleAnimation.From = 1;
            fadeDoubleAnimation.To = 0.20;
            fadeDoubleAnimation.AutoReverse = true;
            fadeDoubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(1000));
            this.BeginAnimation(AnimatedIcon.OpacityProperty, fadeDoubleAnimation);
        }

        private void rotateAnimation()
        {
            TransformGroup rotateAnimationTransformGroup = new TransformGroup();
            RotateTransform rotateAnimationRotateTransform = new RotateTransform(angle: 0, centerX: this.Width / 2, centerY: this.Height / 2);
            rotateAnimationTransformGroup.Children.Add(rotateAnimationRotateTransform);

            this.RenderTransform = rotateAnimationTransformGroup;

            DoubleAnimation rotateDoubleAnimation = new DoubleAnimation();
            rotateDoubleAnimation.From = 0;
            rotateDoubleAnimation.To = -30;
            rotateDoubleAnimation.AutoReverse = true;
            rotateDoubleAnimation.Duration = new Duration(TimeSpan.FromMilliseconds(500));
            rotateAnimationRotateTransform.BeginAnimation(RotateTransform.AngleProperty, rotateDoubleAnimation);
        }

        private void startAnimation(string effect)
        {
            switch (effect)
            {
                case "Zoom":
                    zoomAnimation();
                    break;

                case "Swing":
                    swingAnimation(screenPosition: this.screenPosition);
                    break;

                case "Fade":
                    fadeAnimation();
                    break;

                case "Rotate":
                    rotateAnimation();
                    break;
            }
        }

        private void AnimatedIcon_MouseEnter(object sender, EventArgs eventArgs)
        {
            startAnimation(effect: this.hoaverEffect);
        }

        private void AnimatedIcon_MouseLeftButtonDown(object sender, EventArgs eventArgs)
        {
            startAnimation(effect: this.clickEffect);

            try
            {
                System.Diagnostics.Process.Start(this.target);
            }
            catch (Exception targetLaunchException)
            {
                MessageBox.Show(messageBoxText: "Can't open " + iconTitle + " file. Target path might be invalid or file might be missing or corrupted.", caption: "ERROR", button: MessageBoxButton.OK, icon: MessageBoxImage.Error);
            }
        }
    }
}
