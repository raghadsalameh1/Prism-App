using Newtonsoft.Json;
using Prism.Logging;
using Prism_App.Behaviors.Base;
using Prism_App.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Prism_App.Behaviors.XFMap
{
    class InitializeMapBehavior : BindableBehavior<Map>
    {
        public static readonly BindableProperty ItemsSourceProperty = BindableProperty.Create("ItemsSource", typeof(List<FieldItem>), typeof(InitializeMapBehavior), null, propertyChanged: ItemsSourceChanged);

        public List<FieldItem> ItemsSource
        {
            get { return (List<FieldItem>)GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var behavior = bindable as InitializeMapBehavior;
            if (behavior == null) return;
            behavior.LoadData();
        }

        private void LoadData()
        {
            var map = AssociatedObject;
            map.MapElements.Clear();
            map.Pins.Clear();
            if(ItemsSource.Count()>0)
            {
                try
                {
                    List<Polygon> polygons = new List<Polygon>();
                    foreach (FieldItem fieldItem in ItemsSource)
                    {
                        string geojson = fieldItem.GeoJson;
                        //boundary b = JsonConvert.DeserializeObject<boundary>(geojson);
                        var b = JsonConvert.DeserializeObject<boundary>(geojson);
                        double[][] coordinates = b.features[0].geometry.coordinates[0];
                        Pin pin = new Pin
                        {
                            Label = "",
                            Address = "",
                            Type = PinType.SearchResult,
                            Position = new Position(coordinates[0][1], coordinates[0][0])
                        };
                        map.Pins.Add(pin);                      
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
                        map.MapElements.Add(polygon);
                        polygons.Add(polygon);                      
                    }
                    SetMapPosition(polygons);
                }
                catch (Exception e)
                {

                }              
            }
            else
            {
                Position position = new Position(51.133481, 10.018343);
                MapSpan mapSpan = new MapSpan(position, 51, 10);
                map.MoveToRegion(mapSpan);
            }          
        }

        private void SetMapPosition(List<Polygon> polygons)
        {
            var map = AssociatedObject;
            var minLat = polygons.SelectMany(p => p.Geopath).Min(p => p.Latitude);
            var maxLat = polygons.SelectMany(p => p.Geopath).Max(p => p.Latitude);
            var minLon = polygons.SelectMany(p => p.Geopath).Min(p => p.Longitude);
            var maxLon = polygons.SelectMany(p => p.Geopath).Max(p => p.Longitude);
            var centerLat = (minLat + maxLat) / 2;
            var centerLon = (minLon + maxLon) / 2;

            map.MoveToRegion(
                new MapSpan(
                    new Position(centerLat, centerLon),
                    Math.Abs(maxLat - minLat) * 1.2,
                    Math.Abs(maxLon - minLon) * 1.2));
        }
    }
}
