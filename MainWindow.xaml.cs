using System;
using System.Windows;

namespace WorkTime
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public static double CollapsedWidth => 140;
        public static double CollapsedHeight => 110;

        public MainWindow()
        {
            InitializeComponent();
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
