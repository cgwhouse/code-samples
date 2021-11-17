using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CristianSampleApp.Models;
using Xamarin.Forms;

namespace CristianSampleApp.Services
{
    public interface IAnimalService
    {
        Task<List<Animal>> LoadAnimalsFromBackend(AnimalFilter animalFilter);
    }

    public class AnimalService : IAnimalService
    {
        private int loadCount = 0;

        private string[] animalNames = { "Bear", "Penguin", "Cat", "Goldfish", "Pidgeon" };

        private Dictionary<string, Animal> animalsToSimulate = new Dictionary<string, Animal>
        {
            { "Bear", new Animal("Bear", Color.Brown) { CanFly = false, IsEndangered = true, Cry = "Grunt" } },
            { "Penguin", new Animal("Penguin", Color.Black) { CanFly = true, IsEndangered = true, Cry = "Honk" } },
            { "Cat", new Animal("Cat", Color.Orange) { CanFly = false, IsEndangered = false, Cry = "Meow" } },
            { "Goldfish", new Animal("Goldfish", Color.Gold) { CanFly = false, IsEndangered = false, Cry = "Bloop?" } },
            { "Pidgeon", new Animal("Pidgeon", Color.Gray) { CanFly = true, IsEndangered = false, Cry = "Coo" } }
        };

        private List<Animal> currentSimulatedAnimals = new List<Animal>();

        public AnimalService() { }

        /// <summary>
        /// Simulates the process of making an asynchronous web request to retrieve some info from a backend.
        /// </summary>
        /// <param name="animalFilter">Filter to apply to the backend request</param>
        /// <returns>An updated list of animals from the server</returns>
        /// <exception cref="Exception">Throws if an unexpected filter is passed in</exception>
        public async Task<List<Animal>> LoadAnimalsFromBackend(AnimalFilter animalFilter)
        {
            // We're not actually going to do any web requests, so just simulate a wait
            await Task.Delay(2000);

            // If a filter was applied via the picker, don't simulate grabbing a new animal - just filter current
            if (animalFilter != AnimalFilter.All)
            {
                switch (animalFilter)
                {
                    case AnimalFilter.CanFly:
                        return filterAnimalsByFlight(currentSimulatedAnimals);
                    case AnimalFilter.Endangered:
                        return filterAnimalsByEndangered(currentSimulatedAnimals);
                    default:
                        throw new Exception("Unexpected animal filter!");
                }
            }

            // Also don't load anything new if we've loaded all the animals we can
            if (loadCount == animalNames.Length)
                return currentSimulatedAnimals;

            // Load in the next animal based on loadCount
            currentSimulatedAnimals.Add(animalsToSimulate[animalNames[loadCount]]);
            loadCount++;

            return currentSimulatedAnimals;
        }

        #region Helpers

        private List<Animal> filterAnimalsByFlight(List<Animal> current)
        {
            List<Animal> result = new List<Animal>();

            foreach (Animal animal in current)
                if (animal.CanFly)
                    result.Add(animal);

            return result;
        }

        private List<Animal> filterAnimalsByEndangered(List<Animal> current)
        {
            List<Animal> result = new List<Animal>();

            foreach (Animal animal in current)
                if (animal.IsEndangered)
                    result.Add(animal);

            return result;
        }

        #endregion
    }
}
