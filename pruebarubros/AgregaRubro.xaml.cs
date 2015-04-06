using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Windows.Media.Imaging;
using Coding4Fun.Toolkit.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;





namespace pruebarubros
{

    public partial class AgregaRubro : PhoneApplicationPage
    {

        public IsolatedStorageSettings appSettings;
        int ciclo_actual;
        int tolerancia;

        public AgregaRubro()
        {

            appSettings = IsolatedStorageSettings.ApplicationSettings;
            InitializeComponent();
            ApplicationTitle.Visibility = System.Windows.Visibility.Collapsed;
            try { ciclo_actual = Convert.ToInt32(appSettings["ciclo_act"]); }
            catch (KeyNotFoundException ex) { ciclo_actual = 1; }

            //textBlock2.Text = "" + appSettings["tolerancia"];

            ApplicationTitle.Text = "" + ciclo_actual;

        }

        



        private void agregar(object sender, RoutedEventArgs e)
        {
            if((textBox1.Text =="" || textBox2.Text=="") || (radioButton1.IsChecked == false && radioButton2.IsChecked == false))
            {
                MessageBox.Show("Verifique sus datos");
            }
            else
            {
                using (datacontextt context = new datacontextt(App.rubrosconnectionString))
                {


                    var rubroexiste = from rubroexist in context.tabla where rubroexist.nombre == textBox1.Text select rubroexist;
                            if (rubroexiste.Any())
                                MessageBox.Show("El rubro ya existe");
                            else
                            {




                                tablas ru = new tablas()
                                {
                                    //id=1,//dbgenerated
                                    nombre = textBox1.Text,
                                };


                                ;
                                if (radioButton1.IsChecked == true)
                                    ru.tipo = "Entrada";
                                if (radioButton2.IsChecked == true)
                                    ru.tipo = "Salida";

                                context.tabla.InsertOnSubmit(ru);
                                context.SubmitChanges();



                                ciclos ci = new ciclos()
                                {
                                    vactual = 0,
                                    vpresup = Int32.Parse(textBox2.Text),
                                    idrubro = ru.id,
                                    idciclo = ciclo_actual,
                                    toleranciacic = Convert.ToInt32(appSettings["tolerancia"])
                                };


                                context.ciclos.InsertOnSubmit(ci);
                                context.SubmitChanges();





                                ToastPrompt prueba = new ToastPrompt();
                                prueba.Title = "Mensaje";
                                prueba.Message = "Rubro agregado exitosamente";
                                prueba.MillisecondsUntilHidden = 1500;
                                prueba.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                                prueba.TextOrientation = System.Windows.Controls.Orientation.Vertical;
                                prueba.ImageSource = new BitmapImage(new Uri("..\\ApplicationIcon2.jpg", UriKind.RelativeOrAbsolute));
                                prueba.FontSize = 23;
                                prueba.FontWeight = FontWeights.Bold;

                                SolidColorBrush black = new SolidColorBrush(Colors.Black);
                                SolidColorBrush white = new SolidColorBrush(Colors.White);

                                ImageBrush ib = new ImageBrush();
                                ib.ImageSource = new BitmapImage(new Uri("..\\fondo2.png", UriKind.RelativeOrAbsolute));


                                prueba.Background = black;
                                prueba.Foreground = white;
                                prueba.Show();

                                textBox1.Text = "";
                                textBox2.Text = "";

                            }
                         }
                }

       


        
            }

    }
}