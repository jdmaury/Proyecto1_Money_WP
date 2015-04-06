using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Threading;
using System.IO.IsolatedStorage;

namespace pruebarubros
{
    public partial class MainPage : PhoneApplicationPage
    {
        public IsolatedStorageSettings appSettings;       
        // Constructor
        public MainPage()
        {
            
            DispatcherTimer dt = new DispatcherTimer()
            {
                Interval = TimeSpan.FromMilliseconds(2200)
            };
            dt.Tick += (s, e) =>
                {
                    dt.Stop();
                    NavigationService.Navigate(new Uri("/verRubros.xaml", UriKind.RelativeOrAbsolute));

                };
            dt.Start();

            
            InitializeComponent();


            appSettings = IsolatedStorageSettings.ApplicationSettings;

            
            

            using (datacontextt context = new datacontextt(App.rubrosconnectionString))
            {
                if (!context.DatabaseExists())
                {
                    context.CreateDatabase();
                    //MessageBox.Show("Rubros Database created successfully");
                }

                    // MessageBox.Show("Ya existe la BD");
            }


        }

        

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/verRubros.xaml", UriKind.RelativeOrAbsolute));  
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/reportes.xaml", UriKind.RelativeOrAbsolute));  
        }


        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            

            //try { ApplicationTitle.Text = "USTED SE ENCUENTRA EN EL CICLO " + Convert.ToString(PhoneApplicationService.Current.State["cicloact"]); ApplicationTitle.FontSize = 20; }
//            catch (KeyNotFoundException ex) { ApplicationTitle.Text = "  USTED ESTÁ EN EL CICLO 1"; ApplicationTitle.FontSize = 26; }


            try 
            {
                ApplicationTitle.Text = "USTED SE ENCUENTRA EN EL CICLO "+appSettings["ciclo_act"]+"\nCON UNA TOLERANCIA DE  "+appSettings["tolerancia"]+"%";
                ApplicationTitle.FontSize = 24;
            }
            catch(KeyNotFoundException)
            {
                
                appSettings.Add("ciclo_act", 1);
                appSettings.Add("tolerancia", 0);
                appSettings.Add("tol_change", 0);
                ApplicationTitle.Text = "USTED SE ENCUENTRA EN EL CICLO 1";
                ApplicationTitle.FontSize = 22;
                appSettings.Save();
            }

        }
       
    }
}