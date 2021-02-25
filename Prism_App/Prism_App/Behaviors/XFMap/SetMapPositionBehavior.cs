using Prism_App.Behaviors.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Prism_App.Behaviors.XFMap
{
    class SetMapPositionBehavior : BindableBehavior<Map>
    {
        public static readonly BindableProperty CenterPositionProperty = BindableProperty.Create("CenterPosition", typeof(Position), typeof(SetMapPositionBehavior), null, propertyChanged: ItemsSourceChanged);
        public static readonly BindableProperty LatitudeDegreeProperty = BindableProperty.Create("LatitudeDegree", typeof(double), typeof(SetMapPositionBehavior), null);
        public static readonly BindableProperty LongitudeDegreeProperty = BindableProperty.Create("LongitudeDegree", typeof(double), typeof(SetMapPositionBehavior), null);
        public Position CenterPosition
        {
            get { return (Position)GetValue(CenterPositionProperty); }
            set { SetValue(CenterPositionProperty, value); }
        }

        public double LatitudeDegree
        {
            get { return (double)GetValue(LatitudeDegreeProperty); }
            set { SetValue(LatitudeDegreeProperty, value); }
        }

        public double LongitudeDegree
        {
            get { return (double)GetValue(LongitudeDegreeProperty); }
            set { SetValue(LongitudeDegreeProperty, value); }
        }

        private static void ItemsSourceChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var map = bindable as SetMapPositionBehavior;
            if (map == null) return;
            map.MoveToRegionfun();
        }
        void MoveToRegionfun()
        {
            var map = AssociatedObject;
            map.MoveToRegion(
                new MapSpan(
                    CenterPosition,
                    LatitudeDegree,
                    LongitudeDegree));
        }
    }
}
