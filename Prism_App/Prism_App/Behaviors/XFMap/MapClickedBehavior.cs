using Prism.Commands;
using Prism_App.Behaviors.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Maps;

namespace Prism_App.Behaviors.XFMap
{
    public class MapClickedBehavior : MapEventToCommandBase<MapClickedBehavior>
    {
        protected override void OnAttachedTo(Map bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.MapClicked += OnMapClicked;
        }

        protected override void OnDetachingFrom(Map bindable)
        {
            base.OnDetachingFrom(bindable);
            bindable.MapClicked -= OnMapClicked;
        }

        private void OnMapClicked(object sender, MapClickedEventArgs e)
        {
            if (Command != null && Command.CanExecute(null))
                Command?.Execute(e);
        }
    }
}
