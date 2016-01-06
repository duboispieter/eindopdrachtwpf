using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Wenskaarten.ViewModel
{
    public class KleurVM : ViewModelBase
    {
        private Model.Kleur Kleur;
        public KleurVM(Model.Kleur kleur)
        {
            Kleur = kleur;
        }

        public SolidColorBrush Borstel
        {
            get
            { return Kleur.Borstel; }

            set
            {
                Kleur.Borstel = value;
                RaisePropertyChanged("Borstel");
            }
        }

        public string Kleurnaam
        {
            get
            { return Kleur.Kleurnaam; }

            set
            {
                Kleur.Kleurnaam = value;
                RaisePropertyChanged("Kleurnaam");
            }
        }
    }
}
