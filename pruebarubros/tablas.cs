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
using System.Data.Linq.Mapping;

namespace pruebarubros
{
    [Table(Name = "rubros")]
    public class tablas
    {   
            [Column(IsPrimaryKey = true, IsDbGenerated = true,AutoSync=AutoSync.OnInsert)]
            public int id { get; set; }

            [Column(CanBeNull = false)]
            public String nombre { get; set; }

            [Column(CanBeNull = true)]
            public String tipo { get; set; }
        
    }
}
