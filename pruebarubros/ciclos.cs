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
    [Table(Name = "ciclos")]
    public class ciclos
    {

        [Column(CanBeNull=false,IsPrimaryKey = true, IsDbGenerated = true,AutoSync=AutoSync.OnInsert)]
        public int idfila { get; set; }

        [Column(CanBeNull = false)]
        public int idciclo { get; set; }

        [Column(CanBeNull = false)]
        public int idrubro { get; set; }

        [Column(CanBeNull = false)]
        public float vpresup { get; set; }

        [Column(CanBeNull = false)]
        public float vactual { get; set; }

        [Column(CanBeNull = false)]
        public int toleranciacic { get; set; }

        
        
    }
}
