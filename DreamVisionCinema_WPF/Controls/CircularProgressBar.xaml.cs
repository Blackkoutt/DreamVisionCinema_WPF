using OxyPlot.Series;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace DreamVisionCinema_WPF.Controls
{
    public partial class CircularProgressBar : UserControl, INotifyPropertyChanged
    {
        private double barValue;
        public CircularProgressBar()
        {
            InitializeComponent();
            DataContext = this;
        }

        public double BarValue
        {
            get { return barValue; }
            set 
            {
                barValue = value;
                OnPropertyChanged();
                ConvertValueToAngle();
            }
        }

        private void ConvertValueToAngle()
        {
            var angle = (BarValue / 100) * 360;
            EndAngle = angle > 360 ? 360 : angle; 
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
        public int BarWidth
        {
            get { return width; }
            set
            {
                width = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
