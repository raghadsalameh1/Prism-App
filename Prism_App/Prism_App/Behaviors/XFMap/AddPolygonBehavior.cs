using Prism_App.Behaviors.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Prism_App.Behaviors.XFMap
{
    class AddPolygonBehavior : BindableBehavior<Map>
    {
        public static readonly BindableProperty CoordinatesProperty = BindableProperty.Create("CoordinatesProperty", typeof(double[][]), typeof(AddPolygonBehavior), null, propertyChanged: ItemsSourceChanged);

        private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var map = bindable as AddPolygonBehavior;
            if (map == null) return;
            map.AddPolygon();
        }

        public double[][] Coordinates
        {
            get { return (double[][])GetValue(CoordinatesProperty); }
            set { SetValue(CoordinatesProperty, value); }
        }

        void AddPolygon()
        {
            var map = AssociatedObject;
            Polygon polygon = new Polygon
            {
                StrokeWidth = 8,
                StrokeColor = Color.FromHex("#1BA1E2"),
                FillColor = Color.FromHex("#881BA1E2"),
            };
            foreach (var p in Coordinates)
            {
                polygon.Geopath.Add(new Position(p[1], p[0]));
            }
            // add the polygon to the map's MapElements collection
            map.MapElements.Add(polygon);

            List<Polygon> polygons = new List<Polygon> { polygon };
            SetMapPosition(polygons);
        }

        //To edit
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
