using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using WorkTime.Properties;
using WorkTime.ViewModels;

namespace WorkTime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static double CollapsedWidth => 120;
        public static double CollapsedHeight => 90;

        private Size lastSize;

        private Point lastPoint;

        private Point lastCollapsedPoint;

        private readonly Action<Point, Point, Size> storeLastPositionAndPoint;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(MainViewModel mainViewModel)  : this()
        {
            this.DataContext = mainViewModel;
            
            RestoreWindowPosition(mainViewModel.GetLastPositionAndPoint());
            storeLastPositionAndPoint = mainViewModel.StoreLastPositionAndPoint;
            mainViewModel.PropertyChanged += OnCollapsedChanged;
        }

        private void RestoreWindowPosition(WindowSettingsDTO windowSettings)
        {
            lastSize = new Size(width: windowSettings.LastSize.Width, height: windowSettings.LastSize.Height);
            lastPoint = new Point(windowSettings.LastPosition.X, windowSettings.LastPosition.Y);

            lastCollapsedPoint = new Point(windowSettings.LastCollapsedPosition.X, windowSettings.LastCollapsedPosition.Y);

            if (windowSettings.LastWasCollapsed)
            {
                CollapseView(lastCollapsedPoint);
            }
            else
            {
                RestoreView(lastSize, lastPoint);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (DataContext is MainViewModel mainViewModel)
            {
                mainViewModel.PropertyChanged -= OnCollapsedChanged;

                if (mainViewModel.IsCollapsed)
                {
                    this.storeLastPositionAndPoint?.Invoke(lastPoint, new Point(Left, Top), lastSize);
                }
                else
                {
                    this.storeLastPositionAndPoint?.Invoke(new Point(Left, Top), lastCollapsedPoint, new Size(Width, Height));
                }
            }

            base.OnClosing(e);
        }

        private void OnCollapsedChanged(object _, PropertyChangedEventArgs eventArgs)
        {
            if(eventArgs.PropertyName != nameof(MainViewModel.IsCollapsed) || DataContext is not MainViewModel viewModel)
            {
                return;
            }

            if (viewModel.IsCollapsed)
            {
                lastSize = new Size(width: Width, height: Height);
                lastPoint = new Point(x: Left, y: Top);

                CollapseView(lastCollapsedPoint);
            }
            else
            {
                lastCollapsedPoint = new Point(x: Left, y: Top);
                RestoreView(lastSize, lastPoint);
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e)
        {
            base.OnMouseLeftButtonDown(e);

            this.DragMove();
        }

        private void CollapseView(Point lastCollapsedPoint)
        {
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;

            Width = CollapsedWidth;
            Height = CollapsedHeight;

            Left = lastCollapsedPoint.X;
            Top = lastCollapsedPoint.Y;
        }

        private void RestoreView(Size lastSize, Point lastPosition)
        {
            WindowStyle = WindowStyle.ThreeDBorderWindow;
            ResizeMode = ResizeMode.CanResizeWithGrip;

            Height = lastSize.Height;
            Width = lastSize.Width;

            Left = lastPosition.X;
            Top = lastPosition.Y;
        }

        private void OnCloseWindowClick(object _, EventArgs __)
        {
            Close();
        }

        private void OnLogTextUpdate(object _, EventArgs __)
        {
            LogScrollViewer.ScrollToEnd();
        }
    }
}
