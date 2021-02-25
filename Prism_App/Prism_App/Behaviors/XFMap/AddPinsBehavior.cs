using Prism_App.Behaviors.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Prism_App.Behaviors.XFMap
{
    class AddPinsBehavior : BindableBehavior<Map>
    {
        private static readonly BindablePropertyKey PinsPropertyKey =
            BindableProperty.CreateReadOnly("Pins", typeof(ObservableCollection<Pin>), typeof(AddPinsBehavior), default(ObservableCollection<Pin>));

        public static readonly BindableProperty PinsProperty = PinsPropertyKey.BindableProperty;
        public ObservableCollection<Pin> Pins
        {
            get => (ObservableCollection<Pin>)GetValue(PinsProperty);
            private set => SetValue(PinsPropertyKey, value);
        }

        protected override void OnAttachedTo(Map bindable)
        {
            base.OnAttachedTo(bindable);
            Pins = bindable.Pins as ObservableCollection<Pin>;
        }

        protected override void OnDetachingFrom(Map bindable)
        {
            base.OnDetachingFrom(bindable);
            Pins = null;
        }
    }
}
