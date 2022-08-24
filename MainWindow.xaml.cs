using System;
using System.ComponentModel;
using System.Windows;
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

        private Command<WindowSettingsDTO> storeLastPositionAndPoint;

        public MainWindow()
        {
            InitializeComponent();
            DataContextChanged += OnDataContextChanged;
            lastSize = new Size(0, 0);
            lastPoint = new Point(0, 0);
        }

        public void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (DataContext is MainViewModel mainViewModel)
            {
                RestoreWindowPosition(mainViewModel.GetLastPositionAndPoint());
                storeLastPositionAndPoint = mainViewModel.SaveLastPositionAndSize;
                mainViewModel.PropertyChanged += OnCollapsedChanged;
            }
        }

        private void RestoreWindowPosition(WindowSettingsDTO windowSettings)
        {
            lastSize = new Size(width: windowSettings.LastSize.Width, height: windowSettings.LastSize.Height);
            lastPoint = new Point(windowSettings.LastPosition.X, windowSettings.LastPosition.Y);

            if (windowSettings.LastWasCollapsed)
            {
                CollapseView();
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
            }

            this.storeLastPositionAndPoint?.Execute(new WindowSettingsDTO()
            {
                LastPosition = lastPoint,
                LastSize = lastSize
            });
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

                CollapseView();
            }
            else
            {
                RestoreView(lastSize, lastPoint);
            }
        }

        private void CollapseView()
        {
            WindowStyle = WindowStyle.None;
            ResizeMode = ResizeMode.NoResize;

            Width = CollapsedWidth;
            Height = CollapsedHeight;

            var workingArea = SystemParameters.WorkArea;
            Left = workingArea.Right - Width;
            Top = workingArea.Bottom - Height;
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

        private void OnLogTextUpdate(object _, EventArgs __)
        {
            LogScrollViewer.ScrollToEnd();
        }
    }
}
