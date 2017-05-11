using System;

using Splat;
using ReactiveUI;

namespace XamarinRui
{
    public class DetailsPageViewModel : ReactiveObject, IRoutableViewModel
    {
        public Person CurrentPerson { get; private set; }

        public string UrlPathSegment => $"{CurrentPerson.Name} - Details";
        public IScreen HostScreen { get; set; }

        public DetailsPageViewModel(Person currentPerson)
        {
            this.CurrentPerson = currentPerson;
            this.HostScreen = Locator.Current.GetService<IScreen>();
        }
    }
}
