using Newtonsoft.Json;
using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using Prism_App.Models;
using Prism_App.Services.Field;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Prism_App.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly IPageDialogService _pageDialogService;

        private readonly IFieldsService _fieldsService;
        private INavigationService _navigationService;

        private CancellationTokenSource _createBoundaryCancellationTokenSource;

        private ObservableCollection<Polygon> polygons = new ObservableCollection<Polygon>();

        public DelegateCommand<MapClickedEventArgs> MapClickedCommand { get; private set; }


        public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IFieldsService fieldsService)
            : base(navigationService)
        {
            _navigationService = navigationService;
            _pageDialogService = pageDialogService;
            _fieldsService = fieldsService;
            Title = "";
            IsBusy = false;
            _ismapclickedenabled = true;
            //MapPosition = new Position(51.16338444, 10.44968650);
            //Items = new List<LocationModel>();
            //Items.Add(new LocationModel() { Title = "T1", Description = "D1", Command = MapClickedCommand, Latitude = 51.16338444, Longitude = 10.44968650 });            
            MapClickedCommand = new DelegateCommand<MapClickedEventArgs>(async a => await MapClicked(a)).ObservesCanExecute(() => IsMapClickedEnabled);
        }     

        public override async Task InitializeAsync(INavigationParameters parameters)
        {
            await LoadFields();
        }

        public override async void OnNavigatedTo(INavigationParameters parameters)
        {
            if(parameters.Count != 0)
            {
                int id = parameters.GetValue<int>("FieldId");
                FieldItem item = await App.Database.GetItemAsync(id);
                await AddPolygonWithItsPinAsync(item);
                SetMapPosition();
            }
        }

        private async Task LoadFields()
        {
            //await App.Database.DeleteItemsAsync();
            var Items = await App.Database.GetItemsAsync();
            foreach (FieldItem fieldItem in Items)
            {
                await AddPolygonWithItsPinAsync(fieldItem);
            }
            SetMapPosition();
        }

        private async Task AddPolygonWithItsPinAsync(FieldItem fieldItem)
        {
            string geojson = fieldItem.GeoJson;
            try
            {
                var b = JsonConvert.DeserializeObject<boundary>(geojson);
                double[][] coordinates = b.features[0].geometry.coordinates[0];
                Pin pin = new Pin
                {
                    Label = "",
                    Address = "",
                    Type = PinType.SearchResult,
                    Position = new Position(coordinates[0][1], coordinates[0][0])
                };
                Pins.Add(pin);

                Polygon polygon = new Polygon
                {
                    StrokeWidth = 8,
                    StrokeColor = Color.FromHex("#1BA1E2"),
                    FillColor = Color.FromHex("#881BA1E2"),
                };
                foreach (var p in coordinates)
                {
                    polygon.Geopath.Add(new Position(p[1], p[0]));
                }
                // add the polygon to the map's MapElements collection
                Polygons.Add(polygon);
                polygons.Add(polygon);
            }
            catch(Exception e)
            {
                await _pageDialogService.DisplayAlertAsync("Alert", e.Message + " " + e.InnerException, "Ok");
            }         
        }

        private void SetMapPosition()
        {
            if(polygons.Count>0)
            {
                var minLat = polygons.SelectMany(p => p.Geopath).Min(p => p.Latitude);
                var maxLat = polygons.SelectMany(p => p.Geopath).Max(p => p.Latitude);
                var minLon = polygons.SelectMany(p => p.Geopath).Min(p => p.Longitude);
                var maxLon = polygons.SelectMany(p => p.Geopath).Max(p => p.Longitude);
                var centerLat = (minLat + maxLat) / 2;
                var centerLon = (minLon + maxLon) / 2;

                LatitudeDegree = Math.Abs(maxLat - minLat) * 1.2;
                LongitudeDegree = Math.Abs(maxLon - minLon) * 1.2;
                CenterPosition = new Position(centerLat, centerLon);
            }
            else
            {
                LatitudeDegree = 51;
                LongitudeDegree = 10;
                CenterPosition = new Position(51.133481, 10.018343);
            }                     
        }

        private async Task MapClicked(MapClickedEventArgs args)
        {
            _ismapclickedenabled = false;
            await DetectFieldBoundary(new Position(args.Position.Latitude, args.Position.Longitude));
            _ismapclickedenabled = true;

        }

        private async Task DetectFieldBoundary(Position position)
        {
            if (Connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await _pageDialogService.DisplayAlertAsync("Alert", "Please Check your Internet connection", "Ok");
                return;
            }
            IsBusy = true;
            Pin pin = new Pin
            {
                Label = "",
                Address = "",
                Type = PinType.SearchResult,
                Position = new Position(position.Latitude, position.Longitude)
            };
            Pins.Add(pin);
            try
            {
                _createBoundaryCancellationTokenSource = new CancellationTokenSource(TimeSpan.FromSeconds(30));
                boundary result = await _fieldsService.CreateBoundary(position, cancellationTokenSource: _createBoundaryCancellationTokenSource);
                if (result.fallback_boundary == true)
                {
                    await _pageDialogService.DisplayAlertAsync("Alert", "Detection service isn't available for this region", "Ok");

                }
                else
                {
                    var navParameters = new NavigationParameters();
                    navParameters.Add("Geo", result);
                    navParameters.Add("Position", position);
                    await _navigationService.NavigateAsync("FieldBoundaryViewer", navParameters);
                }
            }
            catch(Exception e)
            {
                await _pageDialogService.DisplayAlertAsync("Alert", e.Message +" "+ e.InnerException , "Ok");
            }
            finally
            {
                IsBusy = false;
                Pins.Remove(pin);
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

        private bool _ismapclickedenabled;

        public bool IsMapClickedEnabled
        {
            get => _ismapclickedenabled;
            set
            {
                SetProperty(ref _ismapclickedenabled, value);
            }
        }

        //private List<FieldItem> _fielditems;

        //public List<FieldItem> FieldItems
        //{
        //    get => _fielditems;
        //    set
        //    {
        //        SetProperty(ref _fielditems, value);
        //    }
        //}

        private ObservableCollection<MapElement> _polygons;

        public ObservableCollection<MapElement> Polygons
        {
            get => _polygons;
            set
            {
                SetProperty(ref _polygons, value);
            }
        }

        private ObservableCollection<Pin> _pins;

        public ObservableCollection<Pin> Pins
        {
            get => _pins;
            set
            {
                SetProperty(ref _pins, value);
            }
        }

        private Position _centerPosition;

        public Position CenterPosition
        {
            get => _centerPosition;
            set
            {
                SetProperty(ref _centerPosition, value);
            }
        }

        private double _latitudeDegree;

        public double LatitudeDegree
        {
            get => _latitudeDegree;
            set
            {
                SetProperty(ref _latitudeDegree, value);
            }
        }

        private double _longitudeDegree;

        public double LongitudeDegree
        {
            get => _longitudeDegree;
            set
            {
                SetProperty(ref _longitudeDegree, value);
            }
        }
    }
    //public class filedPostion
    //{
    //    Position _position;

    //    public Position Position
    //    {
    //        get => _position;
    //        set
    //        {
    //            if (!_position.Equals(value))
    //            {
    //                _position = value;

    //            }
    //        }
    //    }

    //    public filedPostion(Position position)
    //    {
    //        Position = position;
    //    }
    //}
}
