using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services.Dialogs;
using Prism_App.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Prism_App.ViewModels
{
    public class AddFieldDialogViewModel : BindableBase , IDialogAware
    {
        private string _fieldname ;
        private boundary boundary;
        private INavigationService _navigationService;
        public string FieldName
        {
            get => _fieldname;
            set
            {
                SetProperty(ref _fieldname, value);
            }
        }
        public DelegateCommand BackCommand { get; private set; }
        public DelegateCommand NameFieldCommand { get; private set; }
        public AddFieldDialogViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            BackCommand = new DelegateCommand(Back);
            NameFieldCommand = new DelegateCommand(async()=> await NameFieldAndStoreItInDatabaseAsync());
        }

        private async Task NameFieldAndStoreItInDatabaseAsync()
        {
            FieldItem field = new FieldItem();
            field.FieldName = FieldName;
            field.GeoJson = JsonConvert.SerializeObject(boundary);
            int id = await App.Database.SaveItemAsync(field);
            DialogParameters parameters = new DialogParameters();
            parameters.Add("FieldId", id);
            RequestClose(parameters);
        }

        private void Back()
        {
            RequestClose(null);
        }

        public event Action<IDialogParameters> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            boundary = parameters.GetValue<boundary>("Geo");
        }
    }
}
