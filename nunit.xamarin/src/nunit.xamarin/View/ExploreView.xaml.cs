using System;
using System.Collections.Generic;
using NUnit.Runner.ViewModel;
using Xamarin.Forms;

namespace NUnit.Runner.View
{
    public partial class ExploreView : ContentPage
    {
        internal ExploreView(ExploreViewModel model)
        {
            model.Navigation = Navigation;
            BindingContext = model;
            InitializeComponent();
        }

        internal void ViewTest(object sender, SelectedItemChangedEventArgs e)
        {
            var result = e.SelectedItem as TestViewModel;
            if (result != null)
            {
                ((ExploreViewModel)BindingContext).SelectTest(result);
            }

            ((ListView)sender).SelectedItem = null;
        }

    }
}
