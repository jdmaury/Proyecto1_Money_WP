using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Data.Linq;
using Coding4Fun.Toolkit.Controls;
using System.Text.RegularExpressions;


namespace pruebarubros
{
    public class Inputpromptoveride : InputPrompt
    {
        public override void OnCompleted(PopUpEventArgs<string, PopUpResult> result)
        {
            //Validate for empty string, when it fails, bail out.
            int i = Regex.Matches(result.Result.ToString(), "[.]").Count;
            
            if (string.IsNullOrWhiteSpace(result.Result) || i>=1)
            {
                MessageBox.Show("No se admite este valor");
                return;
                
            }

            
            

            //continue if we do not have an empty response
            base.OnCompleted(result);
        }
    }
}
