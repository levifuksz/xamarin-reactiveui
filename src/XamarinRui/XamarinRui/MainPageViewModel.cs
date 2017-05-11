using System;
using System.Linq;
using System.Reactive.Linq;
using System.Collections.Generic;

using Splat;
using ReactiveUI;

namespace XamarinRui
{
    public class MainPageViewModel : ReactiveObject, IRoutableViewModel
    {
        /// <summary>
        /// This will be displayed as page title
        /// </summary>
        public string UrlPathSegment => "Persons";

        /// <summary>
        /// Reference to the IScreen instance. Will be used for navigation
        /// </summary>
        public IScreen HostScreen { get; set; }

        /// <summary>
        /// This is the backing field for the SearchTerm property
        /// </summary>
        private string _searchTerm;

        /// <summary>
        /// It will be bound to an input used to search persons
        /// Does a RaisePropertyChanged to update the input when set to a new value
        /// </summary>
        public string SearchTerm
        {
            get { return _searchTerm; }
            set { this.RaiseAndSetIfChanged(ref _searchTerm, value); }
        }

        /// <summary>
        /// This command will populate the list of persons
        /// It takes a string as a parameter and it returns a list of Persons with names containing that parameter
        /// </summary>
        public ReactiveCommand<string, List<Person>> Search { get; set; }

        /// <summary>
        /// ObservableAsPropertyHelper subcribes to an IObservable and stores the copy of the latest value
        /// in that IObservable. Whenever that current value changes it also calls RaisePropertyChanged.
        /// This will take in the result returned after executing the Refresh command
        /// </summary>
        private ObservableAsPropertyHelper<List<Person>> _searchResults;

        /// <summary>
        /// We pipe the ObservableAsPropertyHelper's value from above into a property storing the persons
        /// returned by the Search command
        /// </summary>
        public List<Person> Persons => _searchResults.Value;

        /// <summary>
        /// Backing field for the SelectedPerson property
        /// </summary>
        private Person _selectedPerson;

        /// <summary>
        /// The currently selected person
        /// </summary>
        public Person SelectedPerson
        {
            get { return _selectedPerson; }
            set { this.RaiseAndSetIfChanged(ref _selectedPerson, value); }
        }

        /// <summary>
        /// ObservableAsPropertyHelper that will subscribe to the execution of the Search command
        /// </summary>
        private ObservableAsPropertyHelper<bool> _isSearching;

        /// <summary>
        /// This will be used to display a loading indicator
        /// </summary>
        public bool IsSearching => _isSearching.Value;

        /// <summary>
        /// Shows the details for a person
        /// </summary>
        public ReactiveCommand ShowDetails { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>        
        public MainPageViewModel()
        {
            this.HostScreen = Locator.Current.GetService<IScreen>();

            // Define what the search command does
            this.Search = ReactiveCommand.CreateFromTask<string, List<Person>>(async (search) =>
            {
                var personsProvider = new PersonsProvider();
                return await personsProvider.GetPersons(search);
            });

            // We can turn a property into an observable that will push a value whenever
            // "SearchTerm" value changes and will also throtle this change
            // When the change occurs we will invoke the Search command
            this.WhenAnyValue(vm => vm.SearchTerm)
                .Throttle(TimeSpan.FromMilliseconds(500), RxApp.MainThreadScheduler)
                .Select(s => s?.Trim())
                .DistinctUntilChanged()                
                .InvokeCommand(this.Search);

            // We want to react to the execution of the command so we can
            // display a loading indicator
            this._isSearching = this.Search.IsExecuting
                .ToProperty(this, vm => vm.IsSearching, false);

            // The result of the Search command put in the search results
            _searchResults = this.Search.ToProperty(this, vm => vm.Persons, new List<Person>());

            // We do an initial load of the persons
            // Please note that the Subscribe is required because normally we would have to use await 
            this.Search.Execute(string.Empty).Subscribe();

            // Define what the show details command does
            this.ShowDetails = ReactiveCommand.CreateFromTask<Person>(async (person) =>
            {
                await this.HostScreen.Router.Navigate.Execute(new DetailsPageViewModel(person));
                this.SelectedPerson = null;
            });

            // Invoke the ShowDetails command each time the SelectedPerson changes
            this.WhenAnyValue(vm => vm.SelectedPerson)
                .Where(p => p != null)
                .InvokeCommand(this.ShowDetails);
        }
    }
}
