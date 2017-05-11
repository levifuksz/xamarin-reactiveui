using System;
using ReactiveUI;
using Xamarin.Forms;

namespace XamarinRui
{
    public partial class MainPage : ContentPage, IViewFor<MainPageViewModel>
    {
        public MainPage()
        {
            InitializeComponent();

            // Xamarin uses the BindingContext to bind data to the UI
            // This is how we forward the Reactive ViewModel to it
            this.WhenAnyValue(v => v.ViewModel).BindTo(this, v => v.BindingContext);
        }

        // We declare a bindable property for the ViewModel
        BindableProperty ViewModelPoperty = BindableProperty.Create(nameof(ViewModel), typeof(MainPageViewModel), typeof(MainPage));

        public MainPageViewModel ViewModel
        {
            get { return (MainPageViewModel)GetValue(ViewModelPoperty); }
            set { SetValue(ViewModelPoperty, value); }
        }

        object IViewFor.ViewModel
        {
            get { return ViewModel; }
            set { ViewModel = (MainPageViewModel)value; }
        }
    }
}
