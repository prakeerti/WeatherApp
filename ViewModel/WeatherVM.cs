using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WeatherApp.Model;
using WeatherApp.ViewModel.Commands;
using WeatherApp.ViewModel.Helpers;

namespace WeatherApp.ViewModel
{
    public class WeatherVM : INotifyPropertyChanged
    {

        public ObservableCollection<City> Cities { get; set; }

        #region query property 

        private string query;

        public string Query
        {
            get { return query; }
            set 
            { 
                query = value;
                OnPropertyChanged("Query");
            }
        }
        #endregion

        //whenever there is a change in the query property this event willbe fired up  and sen message to the user who has subscribed to this event 
        public event PropertyChangedEventHandler PropertyChanged;

        //the text block with Sanfransisco can subsribe to this event and will show the city name as entered in the text box above. 

        #region current conditions property 

        private CurrentConditions currentConditions;

        //every time this current conditions proprties updated this will be triggered 
        public CurrentConditions CurrentConditions
        {
            get { return currentConditions; }
            set 
            {
                currentConditions = value;
                OnPropertyChanged("CurrentConditions");
            }
        }
        #endregion

        private async void GetCurrentConditions()
        {
            Query= string.Empty;
            Cities.Clear();
            CurrentConditions = await AccuWeatherHelper.GetCurrentCondition(SelectedCity.Key);
        }

        

        #region selected city property 
        private City selectedCity;

        public City SelectedCity
        {
            get { return selectedCity; }
            set 
            { 
                selectedCity = value;
                OnPropertyChanged("SelectedCity");
                GetCurrentConditions();
            }
        }

        #endregion 

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        }

        public async void MakeQuery()
        {
            var citites = await AccuWeatherHelper.GetCities(Query);
            //we have to add cities to this observable collection 

            //we have to make sure that everytime we are assigning a new collction of cities to the collection 
            //the collection should be cleared first from the previous cities. 

            Cities.Clear();
            foreach (var city in citites)
            {
                Cities.Add(city);
                //everytime this method is called it will update the view 
            }
        }

        public SearchCommand SearchCommand { get; set; }
        

        public WeatherVM()
        {
            if (DesignerProperties.GetIsInDesignMode(new System.Windows.DependencyObject()))
            {
                SelectedCity = new City()
                {
                    LocalizedName = "New York"
                };

                CurrentConditions = new CurrentConditions
                {
                    WeatherText = "Partly Cloudy",
                    Temperature = new Temperature
                    {
                        Metric = new Units
                        {
                            Value = "21"
                        }
                    }
                };
            }
            SearchCommand = new SearchCommand(this);
            Cities = new ObservableCollection<City>();
        }

        
    }
}
