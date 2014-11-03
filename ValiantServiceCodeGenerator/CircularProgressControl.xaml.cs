using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace ValiantServiceCodeGenerator {
    /// <summary>
    /// Interaction logic for CircularProgressControl.xaml
    /// </summary>
    public partial class CircularProgressControl : UserControl {

        private DoubleAnimation canvasAnimation;
        private Storyboard spinStoryboard;

        public string WaitMessage {
            get { return waitMessageTextBlock.Text; }
            set { waitMessageTextBlock.Text = value; }
        }

        public double BoxWidth {
            get { return RootLayout.Width; }
            set { RootLayout.Width = value; }
        }

        public double BoxHeight {
            get { return RootLayout.Height; }
            set { RootLayout.Height = value; }
        }

        public CircularProgressControl() {
            InitializeComponent();
            spinStoryboard = new Storyboard();

            canvasAnimation = new DoubleAnimation();
            canvasAnimation.From = 0.0;
            canvasAnimation.To = 360.0;
            canvasAnimation.SpeedRatio = 0.8;
            canvasAnimation.RepeatBehavior = RepeatBehavior.Forever;

            Storyboard.SetTarget(canvasAnimation, ProgressCanvas);
            Storyboard.SetTargetProperty(canvasAnimation, new PropertyPath("(Canvas.RenderTransform).(RotateTransform.Angle)"));
            spinStoryboard.Duration = canvasAnimation.Duration;
            spinStoryboard.Children.Add(canvasAnimation);
        }      


        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e) {
            if (System.ComponentModel.DesignerProperties.GetIsInDesignMode(this)) {
                return;
            }
            bool isVisible = (bool)e.NewValue;
            if (isVisible) {
                spinStoryboard.Begin();
            } else {
                spinStoryboard.Stop();
            }
        }
    }
}
