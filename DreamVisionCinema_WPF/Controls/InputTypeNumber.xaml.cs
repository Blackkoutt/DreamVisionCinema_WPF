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
    public partial class InputTypeNumber : UserControl, INotifyPropertyChanged
    {
        public InputTypeNumber()
        {
            DataContext = this;
            InitializeComponent();
            InputValue = 0;
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

        private int inputValue;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int InputValue
        {
            get { return inputValue; }   
            set 
            {
                inputValue = value;
                OnPropertyChanged();
            }
        }
        private void OnPropertyChanged( [CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void InputTypeNumberTextBox_preview_text_input(object sender, TextCompositionEventArgs e)
        {
            if (isRoomNumber)
            {
                e.Handled = new Regex("[^0-4]+").IsMatch(e.Text);
            }
            else
            {
                e.Handled = new Regex("[^0-9]+").IsMatch(e.Text);
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
                if(InputValue < 4)
                {
                    InputValue += 1;
                }
            }
            else
            {
                InputValue += 1;
            }          
        }
    }
}
