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
using System.Data.Linq.Mapping;

namespace pruebarubros
{
    public class datacontextt :DataContext
    {
        public datacontextt(String connectionString)
            : base(connectionString)
        {

        }

        public Table<tablas> tabla
        {
            get
            {
                return this.GetTable<tablas>();
            }
        }

        public Table<ciclos> ciclos
        {
            get 
            {
                return this.GetTable<ciclos>();
            }


        }
    }
}
