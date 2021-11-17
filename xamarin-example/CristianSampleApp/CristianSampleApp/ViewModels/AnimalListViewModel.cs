using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using CristianSampleApp.Models;
using CristianSampleApp.Services;
using Xamarin.Forms;

namespace CristianSampleApp.ViewModels
{
    public class AnimalListViewModel : BaseViewModel
    {
        readonly IAnimalService animalService = DependencyService.Get<IAnimalService>();

        /// <summary>
        /// Whether the list of animals is currently refreshing.
        /// </summary>
        private bool animalListIsRefreshing;
        public bool AnimalListIsRefreshing
        {
            get => animalListIsRefreshing;
            set => SetProperty(ref animalListIsRefreshing, value);
        }

        /// <summary>
        /// Backup of animals we currently have client-side, to make searching faster.
        /// </summary>
        private ObservableCollection<Animal> currentAnimalsBackup = new ObservableCollection<Animal>();
        public ObservableCollection<Animal> CurrentAnimalsBackup
        {
            get => currentAnimalsBackup;
            set => SetProperty(ref currentAnimalsBackup, value);
        }

        /// <summary>
        /// Collection of animals we currently have client-side.
        /// </summary>
        private ObservableCollection<Animal> currentAnimals = new ObservableCollection<Animal>();
        public ObservableCollection<Animal> CurrentAnimals
        {
            get => currentAnimals;
            set => SetProperty(ref currentAnimals, value);
        }

        public AnimalListViewModel()
        {
            Title = "Animal List";
        }

        /// <summary>
        /// Loads an updated copy of the list of animals from the server.
        /// </summary>
        /// <param name="animalFilter">Filter to apply to the request, if any</param>
        public async Task LoadAnimals(AnimalFilter animalFilter)
        {
            AnimalListIsRefreshing = true;
            CurrentAnimals.Clear();

            List<Animal> updatedAnimalsFromServer = await animalService.LoadAnimalsFromBackend(animalFilter);

            if (updatedAnimalsFromServer.Count > 0)
            {
                foreach (Animal animal in updatedAnimalsFromServer)
                    CurrentAnimals.Add(animal);
            }

            CurrentAnimalsBackup = CurrentAnimals;
            AnimalListIsRefreshing = false;
        }

        /// <summary>
        /// Filters the current list of animals without making any external requests or updating the list's "actual" contents.
        /// This routine filters based on the name of the animal.
        /// </summary>
        /// <param name="filter">Search criteria to use for filtering the list</param>
        public void FilterAnimals(string filter)
        {
            if (!string.IsNullOrEmpty(filter))
            {
                ObservableCollection<Animal> filteredAnimals = new ObservableCollection<Animal>();

                foreach (Animal animal in CurrentAnimalsBackup)
                    if (animal.Name.ToLower().Contains(filter.ToLower()))
                        filteredAnimals.Add(animal);

                CurrentAnimals = filteredAnimals;
            }
            else
                CurrentAnimals = CurrentAnimalsBackup;
        }
    }
}
