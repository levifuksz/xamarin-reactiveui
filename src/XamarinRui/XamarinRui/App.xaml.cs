using System;
using Splat;
using ReactiveUI;
using ReactiveUI.XamForms;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]

namespace XamarinRui
{
    public partial class App : Application, IScreen
    {
        public RoutingState Router { get; set; }

        public App()
        {
            InitializeComponent();

            this.Router = new RoutingState();

            // Register ourselves as the Screen instance
            Locator.CurrentMutable.RegisterConstant(this, typeof(IScreen));

            // Register the views for the view models
            Locator.CurrentMutable.Register(() => new MainPage(), typeof(IViewFor<MainPageViewModel>));
            Locator.CurrentMutable.Register(() => new DetailsPage(), typeof(IViewFor<DetailsPageViewModel>));

            // Navigate
            this.Router.Navigate.Execute(new MainPageViewModel());

            MainPage = new RoutedViewHost();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
