using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Media.Imaging;

using Microsoft.Phone.Controls;


using Coding4Fun.Toolkit.Controls;

namespace pruebarubros
{
    public partial class reportes : PhoneApplicationPage
    {
        public reportes()
        {
            InitializeComponent();
            listPicker1.SetValue(Microsoft.Phone.Controls.ListPicker.ItemCountThresholdProperty, 6);
            listPicker2.SetValue(Microsoft.Phone.Controls.ListPicker.ItemCountThresholdProperty, 7);

            using (datacontextt context = new datacontextt(App.rubrosconnectionString))
            {

                var prueba3 = (from rubross in context.tabla select rubross.nombre).Distinct();
                //comboBox1.ItemsSource = prueba2;            
                listPicker1.ItemsSource = prueba3;

                var cic_existentes = (from cicloss in context.ciclos select cicloss.idciclo).Distinct();
                //comboBox1.ItemsSource = prueba2;            
                listPicker2.ItemsSource = cic_existentes;
            }

        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            using (datacontextt context = new datacontextt(App.rubrosconnectionString))
            {
                   int entradasreales = 0;
                   int salidasreales = 0;
                   int entradasesperadas = 0;
                   int salidasesperadas = 0;
                var TJoin = from rubross in context.tabla
                            join cicloss in context.ciclos on rubross.id equals cicloss.idrubro
                            select new tjoin { id = rubross.id, nombre = rubross.nombre, valorpresupuestado = cicloss.vpresup, valoractual = cicloss.vactual};



                
                    var entradasrealess = (from rubross in context.tabla
                                          join cicloss in context.ciclos on rubross.id equals cicloss.idrubro
                                          where rubross.tipo == "Entrada" && cicloss.idciclo == Convert.ToInt32(listPicker2.SelectedItem)
                                          select cicloss.vactual);

                    if (entradasrealess.Any()) 
                    {
                        entradasreales = Convert.ToInt32(entradasrealess.Sum()); 
                    }

                    /*if (!entradasrealess.Any())
                    {
                        MessageBox.Show("Verifique sus datos");
                    }*/
                
                

                

                var salidassrealess = from rubross in context.tabla
                                      join cicloss in context.ciclos on rubross.id equals cicloss.idrubro
                                      where rubross.tipo == "Salida" && cicloss.idciclo == Convert.ToInt32(listPicker2.SelectedItem)
                                      select cicloss.vactual;

                    if (salidassrealess.Any()) 
                    {
                        salidasreales = Convert.ToInt32(salidassrealess.Sum()); 
                    }

                    /*if (!salidassrealess.Any())
                    {
                        MessageBox.Show("Verifique sus datos");
                    }*/

                var salidassesperadass = from rubross in context.tabla
                                      join cicloss in context.ciclos on rubross.id equals cicloss.idrubro
                                         where rubross.tipo == "Salida" && cicloss.idciclo == Convert.ToInt32(listPicker2.SelectedItem)
                                      select cicloss.vpresup;

                if (salidassesperadass.Any()) 
                    {
                        salidasesperadas = Convert.ToInt32(salidassesperadass.Sum()); 
                    }

                    /*if (!salidassesperadass.Any())
                    {
                        MessageBox.Show("Verifique sus datos");
                    }*/

                    var entradasesperadass = from rubross in context.tabla
                                             join cicloss in context.ciclos on rubross.id equals cicloss.idrubro
                                             where rubross.tipo == "Entrada" && cicloss.idciclo == Convert.ToInt32(listPicker2.SelectedItem)
                                             select cicloss.vpresup;

                    if (entradasesperadass.Any())
                    {
                        entradasesperadas = Convert.ToInt32(entradasesperadass.Sum());
                    }

                    /*if (!entradasesperadass.Any())
                    {
                        MessageBox.Show("Verifique sus datos");
                    }*/


                textBlock1.Text = "\n\nBalance general: " + entradasreales + "-" + salidasreales + " = " + (entradasreales - salidasreales) +"\n"
                                +"Balance salidas: " + salidasesperadas + "-" + salidasreales + " = " + (salidasesperadas - salidasreales)+"\n"
                                +"Balance entradas: " + entradasesperadas + "-" + entradasreales + " = " + (entradasesperadas - entradasreales)+"\n\n";
                
                
                                



                var excede = (from rubross in context.tabla//no alcanzo la meta
                              join cicloss in context.ciclos on rubross.id equals cicloss.idrubro
                              where cicloss.vactual < cicloss.vpresup && cicloss.idciclo == Convert.ToInt32(listPicker2.SelectedItem) && (1 - (cicloss.vactual / cicloss.vpresup)) * 100 > cicloss.toleranciacic
                              select new tjoin { id = rubross.id, nombre = rubross.nombre, valorpresupuestado = cicloss.vpresup, valoractual = cicloss.vactual,ciclo=cicloss.idciclo});

                var excede2 = (from rubross in context.tabla
                              join cicloss in context.ciclos on rubross.id equals cicloss.idrubro
                               where cicloss.vactual > cicloss.vpresup && cicloss.idciclo == Convert.ToInt32(listPicker2.SelectedItem) && ((cicloss.vactual / cicloss.vpresup) * 100) - 100 > cicloss.toleranciacic
                              select new tjoin { id = rubross.id, nombre = rubross.nombre, valorpresupuestado = cicloss.vpresup, valoractual = cicloss.vactual, ciclo = cicloss.idciclo });

                //                    (from cicloss in context.ciclos where cicloss.valoractual < cicloss.valorpresupuestado select rubro);



                foreach (tjoin rub in excede)
                {
                    float porcentaje = (1 - (rub.valoractual / rub.valorpresupuestado)) * 100;
                    textBlock1.Text += rub.nombre + " no alcanzó la meta en "+(rub.valorpresupuestado-rub.valoractual)+" ("+ porcentaje + "%) \n";
                }
                foreach (tjoin rub2 in excede2)
                {
                    float porcentaje = (((rub2.valoractual / rub2.valorpresupuestado)) * 100)-100;
                    textBlock1.Text += rub2.nombre + " se excedió en "+(rub2.valoractual-rub2.valorpresupuestado)+" (" + porcentaje + "%) \n";
                }

                textBlock1.TextAlignment = TextAlignment.Center;



                

/*                var Join = (from rubross in context.tabla
                            join cicloss in context.ciclos on rubross.id equals cicloss.idrubro
                            where cicloss.idciclo == Convert.ToInt32(listPicker2.SelectedItem)
                            select new tjoin { id = cicloss.toleranciacic, nombre = rubross.nombre, valorpresupuestado = cicloss.vpresup, valoractual = cicloss.vactual, ciclo = cicloss.idciclo, fila = cicloss.idfila, tipoo = rubross.tipo }).Distinct();
                listBox1.ItemsSource = Join;*/
                




            }

        }


        


        void input_completed(object sender, PopUpEventArgs<string, PopUpResult> e)
        {
            textBlock1.Text =e.Result;
            
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            
            
            ToastPrompt prueba = new ToastPrompt();
            prueba.Title = "Mensaje";
            prueba.Message = "Rubro agregado exitosamente";
            prueba.MillisecondsUntilHidden = 1500;
            prueba.VerticalAlignment = System.Windows.VerticalAlignment.Center;            
            prueba.TextOrientation = System.Windows.Controls.Orientation.Vertical;
            prueba.ImageSource = new BitmapImage(new Uri("..\\ApplicationIcon.png", UriKind.RelativeOrAbsolute));
            prueba.FontSize = 20;
            
            SolidColorBrush gray = new SolidColorBrush(Colors.LightGray);
            SolidColorBrush white = new SolidColorBrush(Colors.White);

            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri("..\\fondo2.png", UriKind.RelativeOrAbsolute));
            
            
            prueba.Background = ib;
            prueba.Foreground = white;
            prueba.Show();

            
            using (datacontextt context = new datacontextt(App.rubrosconnectionString))
            {

                var prueba2 = (from cicloss in context.ciclos select cicloss.vpresup).Distinct();
                //comboBox1.ItemsSource = prueba2;            
                listPicker1.ItemsSource = prueba2;
            }
            
            
            
            
            
            
        }

        private void button2_Click_1(object sender, RoutedEventArgs e)
        {
            String mensajerubro = "";
            using (datacontextt context = new datacontextt(App.rubrosconnectionString))
            {
                if (listPicker1.SelectedIndex!=-1)
                {
                    var mensaje = (from rubross in context.tabla
                                   join cicloss in context.ciclos on rubross.id equals cicloss.idrubro
                                   where rubross.nombre == listPicker1.SelectedItem.ToString()
                                   select new tjoin { id = cicloss.idciclo, nombre = rubross.nombre, valorpresupuestado = cicloss.vpresup, valoractual = cicloss.vactual, ciclo = cicloss.idciclo, fila = cicloss.idfila, tipoo = rubross.tipo }).Distinct();



                    foreach (tjoin mensajes in mensaje)
                    {
                        mensajerubro += "Valor presupuestado en el ciclo " + mensajes.ciclo + " : " + mensajes.valorpresupuestado
                            + "\nValor real en el ciclo " + mensajes.ciclo + "                  :  " + mensajes.valoractual + "\n\n";

                    }
                    MessageBox.Show(mensajerubro);

                }
                else
                    MessageBox.Show("Aún no hay rubros creados.");

            }
            
            
        }
        
        
        








        
    }
    
}