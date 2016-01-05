using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Wenskaarten.ViewModel
{
    public class WenskaartVM: ViewModelBase
    {
        private Model.Wenskaart Wenskaart;
        public WenskaartVM(Model.Wenskaart wenskaart)
        {
            Wenskaart = wenskaart;
            ControlsZichtbaar = false;
            Status = "Nieuw";
        }

        public ImageBrush Achtergrond
        {
            get
            {
                return Wenskaart.Achtergrond;
            }
            set
            {
                Wenskaart.Achtergrond = value;
                RaisePropertyChanged("Achtergrond");
            }
        }

        public ObservableCollection<Model.Bal> Ballen
        {
            get
            { return Wenskaart.BallenLijst; }
            set
            {
                Wenskaart.BallenLijst = value;
                RaisePropertyChanged("Ballen");
            }
        }

        public ObservableCollection<Model.Kleur> Kleurenlijst
        {
            get
            { return Wenskaart.Kleurenlijst; }
            set
            {
                Wenskaart.Kleurenlijst = value;
                RaisePropertyChanged("Kleurenlijst");
            }
        }

        public int Tekstgrootte
        {
            get
            {
                return Wenskaart.Tekstgrootte;
            }

            set
            {
                Wenskaart.Tekstgrootte = value;
                RaisePropertyChanged("Tekstgrootte");
            }
        }

        public FontFamily  Lettertype
        {
            get
            { return Wenskaart.Lettertype; }
            set
            {
                Wenskaart.Lettertype = value;
                RaisePropertyChanged("Lettertype");
            }
        }

        public Boolean ControlsZichtbaar
        {
            get
            { return Wenskaart.ControlsZichtbaar; }
            set
            {
                Wenskaart.ControlsZichtbaar = value;
                RaisePropertyChanged("ControlsZichtbaar");
            }
        }

        public string Status
        {
            get
            { return Wenskaart.Status; }
            set
            {
                Wenskaart.Status = value;
                RaisePropertyChanged("Status");
            }
        }

        public RelayCommand LoadedCommand
        {
            get { return new RelayCommand(Loaded); }
        }

        private void Loaded()
        {
            
            foreach (PropertyInfo info in typeof(Colors).GetProperties())
            {
                BrushConverter bc = new BrushConverter();
                SolidColorBrush deKleur =
                (SolidColorBrush)bc.ConvertFromString(info.Name);
                Model.Kleur kleurke = new Model.Kleur(deKleur, info.Name);

                try
                {
                    MessageBox.Show(kleurke.Kleurnaam);
                }
                catch(Exception ex)
                { MessageBox.Show(ex.Message); }
                
            }
        }
        public RelayCommand AfsluitenCommand
        {
            get { return new RelayCommand(Afsluiten); }
        }

        private void Afsluiten()
        {
            App.Current.MainWindow.Close();
        }

        public RelayCommand<CancelEventArgs> ClosingCommand
        {
            get { return new RelayCommand<CancelEventArgs>(Closing); }
        }

        private void Closing(CancelEventArgs e)
        {
            if (MessageBox.Show("Wilt u het programma afsluiten?", "Afsluiten", MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No) == MessageBoxResult.No)
                e.Cancel = true;
        }

        public RelayCommand NieuwCommand
        {
            get { return new RelayCommand(Nieuw); }
        }

        private void Nieuw()
        {

            ControlsZichtbaar = false;
        }

        public RelayCommand KerstkaartCommand
        {
            get { return new RelayCommand(NieuweKerstkaart); }
        }

        private void NieuweKerstkaart()
        {
            ImageBrush ib = new ImageBrush();
            ib.ImageSource= new BitmapImage(new Uri("pack://application:,,,/Images/kerstkaart.jpg", UriKind.Absolute));
            Achtergrond = ib;

            ControlsZichtbaar = true;
            Tekstgrootte = 30;
            Lettertype = new FontFamily("Arial");
        }

        public RelayCommand GeboortekaartCommand
        {
            get { return new RelayCommand(NieuweGeboortekaart); }
        }

        private void NieuweGeboortekaart()
        {
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/geboortekaart.jpg", UriKind.Absolute));
            Achtergrond = ib;

            ControlsZichtbaar = true;
            Tekstgrootte = 30;
            Lettertype = new FontFamily("Arial");
        }

        public RelayCommand MeerCommand
        {
            get { return new RelayCommand(Meer); }
        }

        private void Meer()
        {
            if(Tekstgrootte<40)
            Tekstgrootte += 1;
        }

        public RelayCommand MinderCommand
        {
            get { return new RelayCommand(Minder); }
        }
        private void Minder()
        {
            if (Tekstgrootte > 10)
                Tekstgrootte -= 1;
        }

        public RelayCommand OpslaanCommand
        {
            get { return new RelayCommand(Opslaan); }
        }

        private void Opslaan()
        {
            Status = "Opslaan";
        }

        public RelayCommand OpenenCommand
        {
            get { return new RelayCommand(Openen); }
        }

        private void Openen()
        {
            Status = "Openen";
        }

        public RelayCommand DragEnterCommand
        {
            get { return new RelayCommand(DragEnter); }
        }
        private void DragEnter()
        {
            MessageBox.Show("Drag gestart");
        }

        public void VoegBalToe()
        {
            MessageBox.Show("Bal toegevoegd");
        }
    }
}
