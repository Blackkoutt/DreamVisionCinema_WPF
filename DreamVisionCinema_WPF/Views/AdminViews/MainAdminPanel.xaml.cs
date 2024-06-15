﻿using DreamVisionCinema_WPF.ViewModels.AdminViewModels;
using System.Windows;

namespace DreamVisionCinema_WPF.Views.AdminViews
{
    /// <summary>
    /// Logika interakcji dla klasy MainAdminPanel.xaml
    /// </summary>
    public partial class MainAdminPanel : Window
    {
        public MainAdminPanel()
        {
            DataContext = MainAdminPanelViewModel.Instance;
            InitializeComponent();
        }

        private void addMovieBtn_Click(object sender, RoutedEventArgs e)
        {
            AddMovie dialog = new AddMovie();
            dialog.Show();
        }
    }
}
