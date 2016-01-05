using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Wenskaarten.Model
{
    public class Kleur
    {
        public SolidColorBrush Borstel { get; set; }
        public string Kleurnaam{ get; set; }

        public Kleur(SolidColorBrush borstel, string kleurnaam)
        {
            Borstel = borstel;
            Kleurnaam = kleurnaam;
        }
    }
}
