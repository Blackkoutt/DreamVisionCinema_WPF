using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace DreamVisionCinema_WPF.Controls
{
    /// <summary>
    /// Logika interakcji dla klasy CircularProgressBar.xaml
    /// </summary>
    public partial class CircularProgressBar : UserControl, INotifyPropertyChanged
    {
        public CircularProgressBar()
        {
            DataContext = this;
            InitializeComponent();
        }
        private int barValue;

        public int BarValue
        {
            get { return barValue; }
            set 
            {
                barValue = value;
                ConvertValueToAngle();
                OnPropertyChanged();
            }
        }

        private void ConvertValueToAngle()
        {
            EndAngle = ((double)BarValue / 100) * 360;
        }
        private double endAngle;

        public double EndAngle
        {
            get { return endAngle; }
            set 
            {
                endAngle = value;
                OnPropertyChanged();
            }
        }


        private int arcThickness;

        public int ArcThickness
        {
            get { return arcThickness; }
            set 
            {
                arcThickness = value;
                OnPropertyChanged();
            }
        }
        private Brush backgroundFill;

        public Brush BackgroundFill
        {
            get { return backgroundFill; }
            set 
            {
                backgroundFill = value;
                OnPropertyChanged();
            }
        }

        private Brush innerFill;

        public Brush InnerFill
        {
            get { return innerFill; }
            set
            {
                innerFill = value;
                OnPropertyChanged();
            }
        }

        private Brush foregorundFill;

        public Brush ForegorundFill
        {
            get { return foregorundFill; }
            set 
            {
                foregorundFill = value;
                OnPropertyChanged();
            }
        }

        private int height;

        public int BarHeight
        {
            get { return height; }
            set 
            {
                height = value;
                OnPropertyChanged();
            }
        }

        private int width;

        public event PropertyChangedEventHandler? PropertyChanged;

        public int BarWidth
        {
            get { return width; }
            set 
            {
                width = value;
                OnPropertyChanged();
            }
        }
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }

}
