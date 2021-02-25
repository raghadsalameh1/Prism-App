using Prism;
using Prism.Ioc;
using Prism_App.ViewModels;
using Prism_App.Views;
using Xamarin.Essentials.Interfaces;
using Xamarin.Essentials.Implementation;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism_App.Data;
using Prism_App.Services.Request;
using Prism_App.Services.Field;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Prism_App
{
    public partial class App
    {
        /* 
         * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
         * This imposes a limitation in which the App class must have a default constructor. 
         * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
         */
        public App() : this(null) { }

        public App(IPlatformInitializer initializer) : base(initializer) { }

        static FieldDatabaseController database;
        public static FieldDatabaseController Database
        {
            get
            {
                if (database == null)
                {
                    database = new FieldDatabaseController();
                }
                return database;
            }
        }

        protected override async void OnInitialized()
        {
            InitializeComponent();

            await NavigationService.NavigateAsync("MenuPage/NavigationPage/MainPage");
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();

            // Register services
            containerRegistry.Register<IRequestService, RequestService>();
            containerRegistry.Register<IFieldsService, FieldsService>();
            containerRegistry.RegisterForNavigation<MenuPage, MenuPageViewModel>();
            containerRegistry.RegisterForNavigation<FieldBoundaryViewer, FieldBoundaryViewerViewModel>();
            containerRegistry.RegisterDialog<DetectedCorrectlyDialog, DetectedCorrectlyDialogViewModel>();
            containerRegistry.RegisterDialog<AddFieldDialog, AddFieldDialogViewModel>();
            containerRegistry.RegisterForNavigation<FieldListPage, FieldListPageViewModel>();
        }
    }
}
