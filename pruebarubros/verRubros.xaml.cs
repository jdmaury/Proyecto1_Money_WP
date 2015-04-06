
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Coding4Fun.Toolkit.Controls;
using System.IO.IsolatedStorage;



namespace pruebarubros
{
    public partial class verRubros : PhoneApplicationPage
    {
        public IsolatedStorageSettings appSettings;

        static int cicloact=1;//para que no cambie el valor when refresh->static
        int stack = 0;
        int tolerancia = 0;
        int tol_change;
        
        
    
        public verRubros()
        {

            
            
            InitializeComponent();
            appSettings = IsolatedStorageSettings.ApplicationSettings;
            cicloact = Convert.ToInt32(appSettings["ciclo_act"]);
            //try {tol_change = Convert.ToInt32(PhoneApplicationService.Current.State["tol_change"]); }
            //catch(Exception ex){tol_change=0;};


            try { tol_change = Convert.ToInt32(appSettings["tol_change"]); }
            catch (KeyNotFoundException) { appSettings.Add("tol_change", 0); }
//            MessageBox.Show("tolchange = " + tol_change);

            LoadRubro();

            stackPanel1.Visibility = System.Windows.Visibility.Collapsed;
        }

        private void LoadRubro()
        {
            using (datacontextt context = new datacontextt(App.rubrosconnectionString))
            {
                //MessageBox.Show("Su tolerancia para este ciclo es de " + appSettings["tolerancia"]);
                {
                    try { textBlock1.Text = "RUBROS PRESENTES EN EL CICLO  " + appSettings["ciclo_act"] +
                        "\n            TOLERANCIA DEL " + appSettings["tolerancia"]+"%"; textBlock1.FontSize = 26; }
                    catch (KeyNotFoundException ex) { textBlock1.Text = "RUBROS PRESENTES EN EL CICLO uno"; textBlock1.FontSize = 26; }
                    
                    var Join1 = (from rubross in context.tabla
                               join cicloss in context.ciclos on rubross.id equals cicloss.idrubro where cicloss.idciclo ==cicloact && cicloss.vactual>cicloss.vpresup && rubross.tipo=="Salida"
                               select new tjoin { id = cicloss.idrubro, nombre = rubross.nombre, valorpresupuestado = cicloss.vpresup, valoractual = cicloss.vactual,ciclo=cicloss.idciclo,fila=cicloss.idfila ,tipoo=rubross.tipo,color="Red"}).Distinct();

                    var Join2 = (from rubross in context.tabla
                                 join cicloss in context.ciclos on rubross.id equals cicloss.idrubro
                                 where cicloss.idciclo == cicloact && cicloss.vactual > cicloss.vpresup && rubross.tipo == "Entrada"
                                 select new tjoin { id = cicloss.idrubro, nombre = rubross.nombre, valorpresupuestado = cicloss.vpresup, valoractual = cicloss.vactual, ciclo = cicloss.idciclo, fila = cicloss.idfila, tipoo = rubross.tipo, color = "Green" }).Distinct();

                               var Join3 = (from rubross in context.tabla
                               join cicloss in context.ciclos on rubross.id equals cicloss.idrubro where cicloss.idciclo ==cicloact && cicloss.vactual<=cicloss.vpresup
                                            select new tjoin { id = cicloss.idrubro, nombre = rubross.nombre, valorpresupuestado = cicloss.vpresup, valoractual = cicloss.vactual, ciclo = cicloss.idciclo, fila = cicloss.idfila, tipoo = rubross.tipo, color = "White" }).Distinct();


                               
                               

                               //select new tjoin{ id=cicloss.idrubro,  valorpresupuestado= cicloss.vpresup}).Distinct();
                    //IQueryable<> StudentList = from rubross in context.rubros,cicloss in contextc.ciclos


                        
                        listBox1.ItemsSource =(Join1.Union(Join2)).Union(Join3);
                    


                    

                    

                    
                    



                }


            }
        }



        private void InputPrompt(object sender, System.Windows.Input.GestureEventArgs e)
        {
            definirtolerancia();
        }

                
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            LoadRubro();
            PhoneApplicationService.Current.State["cicloact"] = cicloact;
            appSettings["ciclo_act"] = cicloact;
            appSettings.Save();
            
            
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {

            cicloact = Convert.ToInt32(appSettings["ciclo_act"]);
        }
        

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            /*cicloact = cicloact + 1;
            tol_change = 0;//para una nueva tolerancia en otro ciclo

            

            PhoneApplicationService.Current.State["cicloact"] = cicloact;
            PhoneApplicationService.Current.State["tol_change"] = tol_change;



            using (datacontextt context = new datacontextt(App.rubrosconnectionString))
            {

                if (cicloact > 1)
                {
                    
                    var existentess = (from cicloss in context.ciclos
                                       select new existentes { rubroidd = cicloss.idrubro, valorpresup = cicloss.vpresup }).Distinct();
                    foreach (existentes exist in existentess)
                    {
                        ciclos existcic = new ciclos()
                        {
                            vactual = 0,
                            vpresup = exist.valorpresup,
                            idciclo = cicloact,
                            idrubro = exist.rubroidd

                        };
                        context.ciclos.InsertOnSubmit(existcic);
                        context.SubmitChanges();
                    }
                }

            }
            NavigationService.Navigate(new Uri("/MainPage.xaml?id="+cicloact, UriKind.Relative));*/
            
            toleranciamayoralciclo1();
        }



        void verificaresperado(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            if (e.PopUpResult == PopUpResult.Ok)
            {
                //MessageBox.Show("Ud presiono ACEPTAR");
                button4.IsEnabled = true;
                
            }
            else
            {
                //MessageBox.Show("Ud presiono CANCELAR");                
                button4.IsEnabled = false;
                
            }
        }




        void toleranciamayoralciclo1()
        {
            cicloact = cicloact + 1;
            appSettings["ciclo_act"] = cicloact;
            


            MessageBox.Show("Este es su ciclo número :" + appSettings["ciclo_act"]);
            tol_change = 0;//para una nueva tolerancia en otro ciclo

            appSettings["tol_change"] = tol_change;
            appSettings.Save();



            PhoneApplicationService.Current.State["cicloact"] = cicloact;
            PhoneApplicationService.Current.State["tol_change"] = tol_change;
            InputPrompt tolerancia = new Inputpromptoveride();
            tolerancia.Title = "Tolerancia";
            tolerancia.InputScope = new InputScope { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };//para solo numeros                
            tolerancia.Message = "Digite el nivel de tolerancia para este ciclo";
            tolerancia.Show();
            tolerancia.Completed += input_completed2;



            //PARA PREGUNTAR SI DESEA MODIFICAR EL VALOR ESPERADO

            
            
            
        }




        void input_completed(object sender, PopUpEventArgs<string, PopUpResult> e)//PARA EDITAR VALOR ACTUAL
        {

            tjoin tj = (tjoin)listBox1.SelectedItem;


                //if (cicloact == tj.ciclo)
                  //  NavigationService.Navigate(new Uri("/modificarRubros.xaml?id=" + tj.id + "&ciclo=" + tj.ciclo, UriKind.RelativeOrAbsolute));
                //else
                  //  MessageBox.Show("eror");
            
            using (datacontextt context = new datacontextt(App.rubrosconnectionString))
            {
                var rubroLoaded = (from ciclo in context.ciclos
                                   where ciclo.idrubro == tj.id && ciclo.idciclo == tj.ciclo
                                   select ciclo).FirstOrDefault();
               // MessageBox.Show("El id del rubro es  " + rubroLoaded.idrubro + "y estamos en el " + rubroLoaded.idciclo + " ciclo");
                if (rubroLoaded != null)
                {
                    rubroLoaded.vactual += Int32.Parse(e.Result);
                    context.SubmitChanges();
                }
            }

            LoadRubro();



        }


        void input_completed2(object sender, PopUpEventArgs<string, PopUpResult> e)//PARA VERIFICAR TOLERANCIA
        {


          

            
            
                tolerancia = Int32.Parse(e.Result);



                appSettings["tolerancia"] = tolerancia;
                //appSettings.Add("tolerancia", tolerancia);

                //textBlock1.Text = "" + tolerancia;
                PhoneApplicationService.Current.State["tolerancia"] = tolerancia;
                tol_change = 1;
                PhoneApplicationService.Current.State["tol_change"] = tol_change;
                appSettings["tol_change"] = tol_change;
                appSettings.Save();

                using (datacontextt context = new datacontextt(App.rubrosconnectionString))
                {

                    if (cicloact > 1)
                    {

                        var existentess = (from cicloss in context.ciclos
                                           select new existentes { rubroidd = cicloss.idrubro, valorpresup = cicloss.vpresup }).Distinct();
                        foreach (existentes exist in existentess)
                        {
                            ciclos existcic = new ciclos()
                            {
                                vactual = 0,
                                vpresup = exist.valorpresup,
                                idciclo = cicloact,
                                idrubro = exist.rubroidd,
                                toleranciacic = tolerancia

                            };
                            context.ciclos.InsertOnSubmit(existcic);
                            context.SubmitChanges();
                        }
                        NavigationService.Navigate(new Uri("/verRubros.xaml", UriKind.RelativeOrAbsolute));
                        LoadRubro();
                        MessagePrompt mp = new MessagePrompt();

                        mp.Title = "Información";
                        mp.Body = "¿Desea modificar algun valor esperado?";
                        mp.Show();
                        mp.IsAppBarVisible = true;
                        mp.IsCancelVisible = true;
                        mp.Completed += verificaresperado;
                    }
                    else
                        NavigationService.Navigate(new Uri("/AgregaRubro.xaml", UriKind.RelativeOrAbsolute));
                }




                


        }


        private void button2_Click(object sender, RoutedEventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {

                
                
                InputPrompt input = new Inputpromptoveride();
                input.Title = "Editar rubro:";
                input.Completed += input_completed;
                input.InputScope= new InputScope{Names={new InputScopeName(){NameValue=InputScopeNameValue.CurrencyAmount}}};//para solo numeros                
                input.Message = "Digite el valor a sumar al valor actual";
                input.Show();
                
                

            }
            
            
            
        }

        private void button5_Click(object sender, RoutedEventArgs e)
        {
            if (stack == 0)
            {
                stackPanel1.Visibility = System.Windows.Visibility.Visible;
                stack = 1;
            }
            else
            {
                stackPanel1.Visibility = System.Windows.Visibility.Collapsed;
                stack = 0;
            }
            
        }

        

        private void button65_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/reportes.xaml", UriKind.RelativeOrAbsolute));  
            
        }

        



        void definirtolerancia()
        {
            tol_change = Convert.ToInt32(appSettings["tol_change"]);
            if (tol_change == 0)
            {


                InputPrompt tolerancia = new Inputpromptoveride();                
                tolerancia.Title = "Tolerancia";

                tolerancia.InputScope = new InputScope { Names = { new InputScopeName() { NameValue = InputScopeNameValue.Number } } };//para solo numeros                
                tolerancia.Message = "Digite el nivel de tolerancia para este ciclo";
                tolerancia.Completed += input_completed2;
                tolerancia.Show();
                
                 
                
                
                
                //tolerancia.Completed += input_completed2;
            }
            if (tol_change == 1)
                NavigationService.Navigate(new Uri("/AgregaRubro.xaml", UriKind.RelativeOrAbsolute));


        }

        private void button18_Click(object sender, RoutedEventArgs e)
        {
            definirtolerancia();
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.RelativeOrAbsolute));
            
        }

        void editexpect(object sender, PopUpEventArgs<string, PopUpResult> e)//PARA EDITAR VALOR ACTUAL
        {

            tjoin tj = (tjoin)listBox1.SelectedItem;


            //if (cicloact == tj.ciclo)
            //  NavigationService.Navigate(new Uri("/modificarRubros.xaml?id=" + tj.id + "&ciclo=" + tj.ciclo, UriKind.RelativeOrAbsolute));
            //else
            //  MessageBox.Show("eror");

            using (datacontextt context = new datacontextt(App.rubrosconnectionString))
            {
                var rubroLoaded = (from ciclo in context.ciclos
                                   where ciclo.idrubro == tj.id && ciclo.idciclo == tj.ciclo
                                   select ciclo).FirstOrDefault();
                // MessageBox.Show("El id del rubro es  " + rubroLoaded.idrubro + "y estamos en el " + rubroLoaded.idciclo + " ciclo");
                if (rubroLoaded != null)
                {
                    rubroLoaded.vpresup = Int32.Parse(e.Result);
                    context.SubmitChanges();
                }
            }

            LoadRubro();



        }



        private void editaresperado(object sender, RoutedEventArgs e)
        {
            
            if (listBox1.SelectedIndex != -1)
            {



                InputPrompt input = new Inputpromptoveride();
                input.Title = "Editar rubro:";
                input.Completed += editexpect;
                input.InputScope = new InputScope { Names = { new InputScopeName() { NameValue = InputScopeNameValue.CurrencyAmount } } };//para solo numeros                
                input.Message = "Digite el nuevo valor esperado";
                input.Show();



            }

        }

        


        
    }
}