using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace WPFWeather.UserControls
{
    /// <summary>
    /// Interaction logic for ErrorDisplay.xaml
    /// </summary>
    public partial class ErrorDisplay : UserControl
    {
        public ErrorDisplay()
        {
            InitializeComponent();
        }

        public string ButtonText
        {
            get => (string)GetValue(ButtonTextProperty);
            set => SetValue(ButtonTextProperty, value);
        }

        public string ErrorText
        {
            get => (string)GetValue(ErrorTextProperty);
            set => SetValue(ErrorTextProperty, value);
        }

        public ICommand ButtonCommand
        {
            get => (ICommand)GetValue(ButtonCommandProperty);
            set => SetValue(ButtonCommandProperty, value);
        }

        public static readonly DependencyProperty ButtonTextProperty =
            DependencyProperty.Register(nameof(ButtonText), typeof(string), typeof(ErrorDisplay), new PropertyMetadata(default(string)));



        public static readonly DependencyProperty ErrorTextProperty =
            DependencyProperty.Register(nameof(ErrorText), typeof(string), typeof(ErrorDisplay), new PropertyMetadata(default(string)));



        public static readonly DependencyProperty ButtonCommandProperty =
            DependencyProperty.Register(nameof(ButtonCommand), typeof(ICommand), typeof(ErrorDisplay), new PropertyMetadata(default(ICommand)));
    }
}