using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DreamVisionCinema_WPF.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy InputTypeNumber.xaml
    /// </summary>
    public partial class InputTypeNumber : System.Windows.Controls.UserControl, INotifyPropertyChanged
    {
        public InputTypeNumber()
        {
            InitializeComponent();
            DataContext = this;
            InputValue = 0;
            InputTypeNumberTextBox.LostFocus += InputTypeNumberTextBox_LostFocus;
        }

        private bool isRoomNumber;
        public bool IsRoomNumber
        {
            get { return isRoomNumber; }
            set
            {
                isRoomNumber = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public static readonly DependencyProperty InputValueProperty =
            DependencyProperty.Register(nameof(InputValue), typeof(int), typeof(InputTypeNumber),
            new FrameworkPropertyMetadata(0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));

        public int InputValue
        {
            get { return (int)GetValue(InputValueProperty); }
            set 
            {
                SetValue(InputValueProperty, value);
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void InputTypeNumberTextBox_preview_text_input(object sender, TextCompositionEventArgs e)
        {
            if (isRoomNumber)
            {
                e.Handled = !int.TryParse(e.Text, out _) || !Regex.IsMatch(e.Text, "[0-4]");
            }
            else
            {
                e.Handled = !int.TryParse(e.Text, out _);
            }
        }

        private void Button_Down_Click(object sender, RoutedEventArgs e)
        {
            if (InputValue > 0)
            {
                InputValue -= 1;
            }
        }

        private void Button_Up_Click(object sender, RoutedEventArgs e)
        {
            if (IsRoomNumber)
            {
                if (InputValue < 4)
                {
                    InputValue += 1;
                }
            }
            else
            {
                InputValue += 1;
            }
        }
        private void InputTypeNumberTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(InputTypeNumberTextBox.Text, out int value))
            {
                InputValue = value; 
            }
            else
            {
                InputTypeNumberTextBox.Text = InputValue.ToString();
            }
        }
    }
}

