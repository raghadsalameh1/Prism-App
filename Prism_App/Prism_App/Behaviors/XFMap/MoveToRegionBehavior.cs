using Prism_App.Behaviors.Base;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Prism_App.Behaviors.XFMap
{
    public class MoveToRegionBehavior: BindableBehavior<Map>
    {
        private static readonly BindableProperty SpecificPositionProperty = BindableProperty.Create("SpecificPosition",typeof(Position), typeof(MoveToRegionBehavior), null, propertyChanged: OnPositionChanged);

        public Position SpecificPosition
        {
            get { return (Position)GetValue(SpecificPositionProperty); }
            set { SetValue(SpecificPositionProperty, value); }
        }

        private static void OnPositionChanged(BindableObject bindable, object oldValue, object newValue)
        {
            //var map = bindable as MoveToRegionBehavior;
            //if (map == null) return;
            //map.MoveToRegionfun();
            
        }
        void MoveToRegionfun()
        {
            var map = AssociatedObject;
            map.MoveToRegion(new MapSpan(SpecificPosition, 0.01, 0.01));
        }
    }
}
