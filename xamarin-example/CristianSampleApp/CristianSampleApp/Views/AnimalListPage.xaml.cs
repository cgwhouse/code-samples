using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CristianSampleApp.Models;
using CristianSampleApp.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CristianSampleApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AnimalListPage : ContentPage
    {
        private readonly AnimalListViewModel viewModel;
        
        private bool isFirstAppearance = true;

        private readonly Dictionary<string, AnimalFilter> animalFilterFromStringDict = new Dictionary<string, AnimalFilter>
        {
            { "All", AnimalFilter.All },
            { "Can fly", AnimalFilter.CanFly },
            { "Endangered", AnimalFilter.Endangered }
        };

        public AnimalListPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new AnimalListViewModel();

            // Populate status picker and default selection to "All"
            AnimalStatusPicker.ItemsSource = new List<string>(animalFilterFromStringDict.Keys);
            AnimalStatusPicker.SelectedItem = "All";

            // On tablets, we assume we have enough screen real estate no matter the orientation, so lock this portion of the page into a horizontal display (which is the more aggressive)
            if (DeviceInfo.Idiom == DeviceIdiom.Tablet)
                OrientSearchAndPicker(vertical: false);
            else
                // Otherwise, we want to handle dynanmically based on device orientation. So add event handler for that
                // TODO there is an open issue with this event firing incorrectly, but specifically on Android emulators. Planned to be fixed in next milestone release: https://github.com/xamarin/Essentials/issues/1355
                DeviceDisplay.MainDisplayInfoChanged += DeviceDisplay_MainDisplayInfoChanged;
        }

        private void OrientSearchAndPicker(bool vertical = true)
        {
            if (vertical)
            {
                GridVertical.IsVisible = true;
                GridHorizontal.IsVisible = false;

                GridVertical.Children.Add(AnimalSearchBar, 0, 0);
                GridVertical.Children.Add(AnimalStatusPicker, 0, 1);
            }
            else
            {
                GridVertical.IsVisible = false;
                GridHorizontal.IsVisible = true;

                GridHorizontal.Children.Add(AnimalSearchBar, 0, 0);
                GridHorizontal.Children.Add(AnimalStatusPicker, 1, 0);
            }
        }

        private void DeviceDisplay_MainDisplayInfoChanged(object sender, DisplayInfoChangedEventArgs e)
        {
            OrientSearchAndPicker(e.DisplayInfo.Orientation == DisplayOrientation.Portrait);
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            // Clear search criteria, if any
            if (!string.IsNullOrEmpty(AnimalSearchBar.Text))
                AnimalSearchBar.Text = string.Empty;

            // If we're on a phone, call the helper and make sure we haven't changed our screen orientation since this page was last displayed. May need to update
            if (DeviceInfo.Idiom == DeviceIdiom.Phone)
                OrientSearchAndPicker(DeviceDisplay.MainDisplayInfo.Orientation == DisplayOrientation.Portrait);

            // Retrieve fresh copy of animals from server
            await LoadAnimalList(AnimalFilter.All);
        }

        private async Task LoadAnimalList(AnimalFilter animalFilter)
        {
            // Use viewModel to request the AnimalService
            await viewModel.LoadAnimals(animalFilter);

            // Re-apply search bar filter after reloading list contents, if applicable
            if (!string.IsNullOrEmpty(AnimalSearchBar.Text))
                viewModel.FilterAnimals(AnimalSearchBar.Text);
        }

        private async void AnimalStatusPicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            // This event fires right after we set SelectedItem in the constructor, this is a workaround to prevent firing that first time
            if (isFirstAppearance)
            {
                isFirstAppearance = false;
                return;
            }

            // Extract the decision from user via the helper dict
            AnimalFilter animalFilter = animalFilterFromStringDict[(string)AnimalStatusPicker.SelectedItem];

            // Refresh list contents (this is a server-side refresh)
            await LoadAnimalList(animalFilter);
        }

        private void AnimalSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Filter text on the fly as user changes contents of search box
            viewModel.FilterAnimals(e.NewTextValue);
        }

        private async void AnimalList_Refreshing(object sender, EventArgs e)
        {
            await LoadAnimalList(animalFilterFromStringDict[(string)AnimalStatusPicker.SelectedItem]);
        }

        private async void AnimalList_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem != null)
            {
                Animal selectedAnimal = (Animal)e.SelectedItem;

                await DisplayAlert("Animal Cry Test", $"The {selectedAnimal.Name} has the following cry: {selectedAnimal.Cry}", "Dismiss");

                AnimalList.SelectedItem = null;
            }
        }
    }
}
