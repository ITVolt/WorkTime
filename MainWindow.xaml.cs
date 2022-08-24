using System;
using System.ComponentModel;
using System.Windows;
using WorkTime.ViewModels;

namespace WorkTime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static double CollapsedWidth => 140;
        public static double CollapsedHeight => 110;

        private Action<Point, Size> storeLastPositionAndPoint;

        public MainWindow()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
        }

        public void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {            
            if(DataContext is MainViewModel mainViewModel)
            {
                RestoreWindowPosition(mainViewModel.GetLastPositionAndPoint());
                storeLastPositionAndPoint = mainViewModel.StoreLastPositionAndPoint;
            }
        }

        private void RestoreWindowPosition((Point lastPoint, Size lastSize) value)
        {
            Width = value.lastSize.Width;
            Height = value.lastSize.Height;
            Left = value.lastPoint.X;
            Top = value.lastPoint.Y;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.storeLastPositionAndPoint?.Invoke(new Point(x: Left, y: Top), new Size(width: Width, height: Height));
            base.OnClosing(e);
        }

        private void CollapseButton_OnClick(object sender, RoutedEventArgs e)
        {
            Width = CollapsedWidth;
            Height = CollapsedHeight;

            var workingArea = SystemParameters.WorkArea;
            Left = workingArea.Right - Width;
            Top = workingArea.Bottom - Height;
        }

        private void OnLogTextUpdate(object _, EventArgs __)
        {
            LogScrollViewer.ScrollToEnd();
        }
    }
}
