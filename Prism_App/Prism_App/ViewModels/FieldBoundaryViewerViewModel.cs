using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services.Dialogs;
using Prism_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Maps;

namespace Prism_App.ViewModels
{
    public class FieldBoundaryViewerViewModel : BindableBase, INavigationAware
    {
        private boundary boundary;
        private IDialogService _dialogService { get; }
        private INavigationService _navigationService;
        public FieldBoundaryViewerViewModel(IDialogService dialogService,INavigationService navigationService)
        {
            _dialogService = dialogService;
            _navigationService = navigationService;
        }

        public void OnNavigatedFrom(INavigationParameters parameters)
        {
            //throw new NotImplementedException();
        }

        public void OnNavigatedTo(INavigationParameters parameters)
        {     
            //MapPosition = (Position)parameters["Position"];
            boundary = (boundary)parameters["Geo"];
            Coordinates = boundary.features[0].geometry.coordinates[0];
            var dialogparameters = new DialogParameters();
            dialogparameters.Add("Geo", boundary);
            _dialogService.ShowDialog("DetectedCorrectlyDialog", dialogparameters, callback);
        }
        //
        private void callback(IDialogResult obj)
        {
            if(obj.Parameters.Count!=0)
            {
                var id = obj.Parameters.GetValue<int>("FieldId");
                NavigationParameters parameters = new NavigationParameters();
                parameters.Add("FieldId", id);
                _navigationService.GoBackAsync(parameters);
            }
            else
            {
                _navigationService.GoBackAsync();
            }         
        }

        //private Position _mapposition;

        //public Position MapPosition
        //{
        //    get => _mapposition;
        //    set
        //    {
        //        SetProperty(ref _mapposition, value);
        //    }
        //}

        private double[][] _coordinates;

        public double[][] Coordinates
        {
            get => _coordinates;
            set
            {
                SetProperty(ref _coordinates, value);
            }
        }
    }
}
