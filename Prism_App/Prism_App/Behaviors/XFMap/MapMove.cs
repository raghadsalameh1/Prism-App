using Prism_App.Behaviors.Base;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Prism_App.Behaviors.XFMap
{
     public class MapMove : BindableBehavior<Map>
     {
        public static readonly BindableProperty MapItemSourceProperty = BindableProperty.Create("MapItemSource", typeof(Position), typeof(MapMove), null, propertyChanged: ItemsSourceChanged);
        public Position MapItemSource
        {
            get { return (Position)GetValue(MapItemSourceProperty); }
            set { SetValue(MapItemSourceProperty, value); }
        }

        private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var map = bindable as MapMove;
            if (map == null) return;
            map.MoveToRegionfun();
        }
        void MoveToRegionfun()
        {
            var map = AssociatedObject;
            map.MoveToRegion(new MapSpan(MapItemSource, 0.01, 0.01));
        }
     }
}
