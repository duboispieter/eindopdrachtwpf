using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Wenskaarten.Model
{
    public class Wenskaart
    {
        public ImageBrush Achtergrond { get; set; }
        public int Tekstgrootte { get; set; }
        public FontFamily Lettertype { get; set; }
        public Boolean ControlsZichtbaar { get; set; }
        public string Status { get; set; }
        public ObservableCollection<Bal> BallenLijst { get; set; }
        public ObservableCollection<Kleur> Kleurenlijst { get; set; }
        
    }
}
