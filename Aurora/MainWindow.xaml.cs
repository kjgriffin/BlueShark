using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Midnight.Build;
using Midnight.Compiling;
using Midnight.Parsers;
using Xenon.Helpers;

namespace Aurora
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        IntellisenseParser IntellisenseParser = new IntellisenseParser();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void tbSourceChanged(object sender, TextChangedEventArgs e)
        {
            ShowIntellisense();
        }



        private void ShowIntellisense()
        {
            int caretPos = tbSource.SelectionStart;
            string uptocarret = tbSource.Text.Substring(0, caretPos);
            var suggestions = IntellisenseParser.GetSuggestions(uptocarret);
            if (suggestions.success)
            {
                StringBuilder sb = new StringBuilder();
                foreach (var suggestion in suggestions.suggestions)
                {
                    sb.AppendLine(suggestion);
                }
                tbhelp.Text = sb.ToString();
            }
            else
            {
                tbhelp.Text = "No help available. Failed to get suggestions";
            }
        }


        private void tbSourceChangedPreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                if (Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    ShowIntellisense();
                }
            }
        }

        private void tbSourceSelectionChanged(object sender, RoutedEventArgs e)
        {
            ShowIntellisense();
        }

        private void btnBuildClick(object sender, RoutedEventArgs e)
        {
            MidnightBuildService buildService = new MidnightBuildService();
            buildService.BuildProject(tbSource.Text);
            testslide.Source = ((Bitmap)buildService.renderedSlides[0].SlideContent).ConvertToBitmapImage().UriSource;
        }
    }
}
