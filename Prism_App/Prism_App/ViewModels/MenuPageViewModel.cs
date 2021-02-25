using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism_App.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Xamarin.Forms;
using Prism_App.Views;

namespace Prism_App.ViewModels
{
    public class MenuPageViewModel : BindableBase
    {
        private INavigationService _navigationService;

        public ObservableCollection<MenuItemModel> MenuItems { get; set; }

        private MenuItemModel _selectedMenuItem;
        public MenuItemModel SelectedMenuItem
        {
            get => _selectedMenuItem;
            set => SetProperty(ref _selectedMenuItem, value);
        }

        public DelegateCommand NavigateCommand { get; private set; }

        public MenuPageViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;

            MenuItems = new ObservableCollection<MenuItemModel>();

            //nameof(ViewA)
            MenuItems.Add(new MenuItemModel()
            {
                Icon = "map",
                PageName = nameof(MainPage),
                Title = "Main Page"
            });

            MenuItems.Add(new MenuItemModel()
            {
                Icon = "location",
                PageName = nameof(FieldListPage),
                Title = "My Field"
            });

            NavigateCommand = new DelegateCommand(Navigate);
        }

        async void Navigate()
        {
            await _navigationService.NavigateAsync(nameof(NavigationPage) + "/" + SelectedMenuItem.PageName);
        }
    }
}
