using Prism_App.Behaviors.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Prism_App.Behaviors.XFMap
{
    class AddPolygonsBehavior : BindableBehavior<Map>
    {
        private static readonly BindablePropertyKey PolygonsPropertyKey =
            BindableProperty.CreateReadOnly("Polygons", typeof(ObservableCollection<MapElement>), typeof(AddPolygonsBehavior), default(ObservableCollection<MapElement>));

        public static readonly BindableProperty PolygonsProperty = PolygonsPropertyKey.BindableProperty;
        public ObservableCollection<MapElement> Polygons
        {
            get => (ObservableCollection<MapElement>)GetValue(PolygonsProperty);
            private set => SetValue(PolygonsPropertyKey, value);
        }

        protected override void OnAttachedTo(Map bindable)
        {
            base.OnAttachedTo(bindable);         
            Polygons = bindable.MapElements as ObservableCollection<MapElement>;                     
        }

        protected override void OnDetachingFrom(Map bindable)
        {
            base.OnDetachingFrom(bindable);
            Polygons = null;
        }
    }
}
