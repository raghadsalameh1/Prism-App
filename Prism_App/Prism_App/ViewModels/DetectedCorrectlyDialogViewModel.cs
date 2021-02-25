using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using Prism.Services.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Prism_App.ViewModels
{
    public class DetectedCorrectlyDialogViewModel : BindableBase , IDialogAware
    {
        private IDialogService _dialogService;
        private object boundary;
        public DelegateCommand NameFieldCommand { get; private set; }
        public DelegateCommand BackCommand { get; private set; }

        
        public DetectedCorrectlyDialogViewModel(IDialogService dialogService)
        {
            _dialogService = dialogService;         
            NameFieldCommand = new DelegateCommand(NavigateToAddFieldDialog);
            BackCommand = new DelegateCommand(Back);
        }

        private void Back()
        {
            RequestClose(null);
        }

        private void NavigateToAddFieldDialog()
        {
            var dialogparameters = new DialogParameters();
            dialogparameters.Add("Geo", boundary);
            _dialogService.ShowDialog("AddFieldDialog",dialogparameters, callback);
        }

        private void callback(IDialogResult obj)
        {
            if(obj.Parameters.Count!=0)
            {
                var id = obj.Parameters.GetValue<int>("FieldId");
                DialogParameters parameters = new DialogParameters();
                parameters.Add("FieldId", id);
                RequestClose(parameters);
            }
            else
            {
                RequestClose(null);
            }         
        }

        public event Action<IDialogParameters> RequestClose;

        public bool CanCloseDialog()
        {
            return true;
        }

        public void OnDialogClosed()
        {
            //_navigationService.GoBackToRootAsync();
        }

        public void OnDialogOpened(IDialogParameters parameters)
        {
            boundary = parameters.GetValue<object>("Geo");
        }
    }
}
