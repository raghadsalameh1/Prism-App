using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Prism_App.ViewModels
{
    public class FieldListPageViewModel : ViewModelBase
    {
        private INavigationService _navigationService;
        public DelegateCommand<string> TextChangedCommand { get; private set; }
        public DelegateCommand<FieldItem> ItemSelectedCommand { get; private set; }
        public FieldListPageViewModel(INavigationService navigationService) : base(navigationService)
        {
            _navigationService = navigationService;
            TextChangedCommand = new DelegateCommand<string>(async obj =>await FilterFieldNameAsync(obj));
            ItemSelectedCommand = new DelegateCommand<FieldItem>(Navigate);
        }

        public override async Task InitializeAsync(INavigationParameters parameters)
        {
            await LoadFields();
        }

        private void Navigate(FieldItem field)
        {
            //boundary b = Convert
            //_navigationService.NavigateAsync("FieldBoundaryViewer");
        }

        private async Task FilterFieldNameAsync(string NewTextValue)
        {
            FieldItems = await App.Database.GetSearchedFieldsAsync(NewTextValue);
        }

        private async Task LoadFields()
        {
            //await App.Database.DeleteItemsAsync();
            FieldItems = await App.Database.GetItemsAsync();
        }
        private List<FieldItem> _fielditems;

        public List<FieldItem> FieldItems
        {
            get => _fielditems;
            set
            {
                SetProperty(ref _fielditems, value);
            }
        }
    }
}
