﻿using DreamVisionCinema_WPF.ViewModels.AdminViewModels;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace DreamVisionCinema_WPF.Views.AdminViews
{
    /// <summary>
    /// Logika interakcji dla klasy StatisticsPanel.xaml
    /// </summary>
    public partial class StatisticsPanel : UserControl
    {
        public StatisticsPanel()
        {
            var viewModel = StatisticsPanelViewModel.Instance;
            DataContext = viewModel;    
            InitializeComponent();
            progressBar.BarValue = viewModel.IncomePercent;
        }
    }
}
