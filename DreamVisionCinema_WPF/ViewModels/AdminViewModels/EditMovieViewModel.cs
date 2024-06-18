﻿using DreamVisionCinema_WPF.DI;
using DreamVisionCinema_WPF.Enums;
using DreamVisionCinema_WPF.Observable;
using DreamVisionCinema_WPF_Logic.Exceptions;
using DreamVisionCinema_WPF_Logic.Interfaces;
using DreamVisionCinema_WPF_Logic.Interfaces.IRepositories;
using DreamVisionCinema_WPF_Logic.Model;
using System.Windows;
using System.Windows.Input;
using Unity;

namespace DreamVisionCinema_WPF.ViewModels.AdminViewModels
{
    public class EditMovieViewModel : BaseViewModel
    {
        private string? title;
        private DateTime date;
        private int price;
        private int roomNumber;
        private int movieId;
        private string? description;
        private string? pathToPoster;
        private int ageCategory;
        private Movie? movie;
        private IMovieRepository movieRepository;
        private IReservationRepository reservationRepository;
        public ICommand DragMoveCommand => base.DragCommand;
        public ICommand EditMovieCommand { get; set; }

        public EditMovieViewModel(IMovieRepository movieRepository, IReservationRepository reservationRepository)
        {
            this.movieRepository = movieRepository;
            this.reservationRepository = reservationRepository; 
            EditMovieCommand = new RelayCommand(EditMovie);
        }
        private void EditMovie(object parameter)
        {
            string id = MovieId.ToString();
            string dateString = Date.ToString("dd/MM/yyyy HH:mm");
            string priceString = Price.ToString();
            string roomNumberString = RoomNumber.ToString();
            string ageCategoryString = AgeCategory.ToString();
            string indirectPosterPath = "pack://application:,,,/Assets/Posters/";
            string fullPosterPath = indirectPosterPath + PathToPoster;

            if (Movie.Price != Price || Movie.AgeCategory != ageCategoryString || Movie.Description != Description || Movie.PathToPoster != fullPosterPath)
            {
                try
                {
                    movieRepository.ModifyMoviePriceDescriptionAgeCategoryPathToPoster(id, priceString, Description, ageCategoryString, PathToPoster);
                }
                catch (CannotConvertException CCE)
                {
                    MakeAlert(CCE.Message, AlertTypeEnum.Error, true);
                    return;
                }
                catch (IncorrectParametrException IPE)
                {
                    MakeAlert(IPE.Message, AlertTypeEnum.Error, true);
                    return;
                }
                catch (NoMovieWithGivenIdException NMWGIE)
                {
                    MakeAlert(NMWGIE.Message, AlertTypeEnum.Error, true);
                    return;
                }
                catch (Exception ex)
                {
                    MakeAlert(ex.Message, AlertTypeEnum.Error, true);
                    return;
                }
            }
            if(Movie.Date.ToString("dd/MM/yyyy HH:mm") != Date.ToString("dd/MM/yyyy HH:mm") || Movie.Room.Number != RoomNumber)
            {
                try
                {
                    reservationRepository.ModifyMovieDateOrRoomWithReservation(id, dateString, roomNumberString);
                }
                catch (CannotConvertException CCE)
                {
                    MakeAlert(CCE.Message, AlertTypeEnum.Error, true);
                    return;
                }
                catch (NoMovieWithGivenIdException NMWGIE)
                {
                    MakeAlert(NMWGIE.Message, AlertTypeEnum.Error, true);
                    return;
                }
                catch (IncorrectParametrException IPE)
                {
                    MakeAlert(IPE.Message, AlertTypeEnum.Error, true);
                    return;
                }
                catch (NoRoomWithGivenNumberException NRWGNE)
                {
                    MakeAlert(NRWGNE.Message, AlertTypeEnum.Error, true);
                    return;
                }
                catch (MoviesCollisionException MCE)
                {
                    MakeAlert(MCE.Message, AlertTypeEnum.Error, true);
                    return;
                }
                catch (CannotDestroyTicketException CDTE)
                {
                    MakeAlert(CDTE.Message, AlertTypeEnum.Error, true);
                    return;
                }
                catch (Exception ex)
                {
                    MakeAlert(ex.Message, AlertTypeEnum.Error, true);
                    return;
                }
            }

            MakeAlert("Poprawnie zmodyfikowano film", AlertTypeEnum.Success, true);
            MoviesListViewModel.Instance.LoadOrRefreshMovieList();

            if (parameter is Window window)
            {
                window.Close();
            }
        }
        public int RoomNumber
        {
            get { return roomNumber; }
            set
            {
                roomNumber = value;
                OnPropertyChanged();
            }
        }
        public int MovieId
        {
            get { return movieId; }
            set { movieId = value; }
        }
        public Movie? Movie
        {
            get { return movie; }
            set { movie = value; }
        }
        public int Price
        {
            get { return price; }
            set
            {
                price = value;
                OnPropertyChanged();
            }
        }


        public DateTime Date
        {
            get { return date; }
            set
            {
                date = value;
                OnPropertyChanged();
            }
        }


        public string? Title
        {
            get { return title; }
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }
        public int AgeCategory
        {
            get { return ageCategory; }
            set
            {
                ageCategory = value;
                OnPropertyChanged();
            }
        }
        public string? Description
        {
            get { return description; }
            set
            {
                description = value;
                OnPropertyChanged();
            }
        }


        public string? PathToPoster
        {
            get { return pathToPoster; }
            set
            {
                pathToPoster = value;
                OnPropertyChanged();
            }
        }
        public static EditMovieViewModel Instance
        {
            get
            {
                return DIContainer.GetContainer().Resolve<EditMovieViewModel>();
            }
        }
    }
}
