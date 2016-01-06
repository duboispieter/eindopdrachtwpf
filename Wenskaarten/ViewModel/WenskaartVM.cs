using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Wenskaarten.ViewModel
{
    public class WenskaartVM : ViewModelBase
    {
        #region Properties
        private Model.Wenskaart Wenskaart;
        public WenskaartVM(Model.Wenskaart wenskaart)
        {
            Wenskaart = wenskaart;
        }

        private ObservableCollection<BalVM> ballen = new ObservableCollection<BalVM>();

        public ObservableCollection<BalVM> Ballen
        {
            get
            { return ballen; }
            set
            {
                ballen = value;
                RaisePropertyChanged("Ballen");
            }
        }

        private ObservableCollection<KleurVM> kleurlijst = new ObservableCollection<KleurVM>();

        public ObservableCollection<KleurVM> Kleurlijst
        {
            get
            { return kleurlijst; }
            set
            {
                kleurlijst = value;
                RaisePropertyChanged("Kleurlijst");
            }
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

        public string Wens
        {
            get
            { return Wenskaart.Wens; }
            set
            {
                Wenskaart.Wens = value;
                RaisePropertyChanged("Wens");
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

        public FontFamily Lettertype
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

        public Boolean MenuEnabled
        {
            get
            { return Wenskaart.MenuEnabled; }

            set
            {
                Wenskaart.MenuEnabled = value;
                RaisePropertyChanged("MenuEnabled");
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
        #endregion

        #region Commands
        #region Window_Commands
        public RelayCommand LoadedCommand
        {
            get { return new RelayCommand(Loaded); }
        }

        private void Loaded()
        {
            LaadKleuren();
            MenuEnabled = false;
            ControlsZichtbaar = false;
            Status = "Nieuw";

        }

        private void LaadKleuren()
        {
            foreach (PropertyInfo info in typeof(Colors).GetProperties())
            {
                BrushConverter bc = new BrushConverter();
                SolidColorBrush deKleur =
                (SolidColorBrush)bc.ConvertFromString(info.Name);
                Model.Kleur kleurM = new Model.Kleur(deKleur, info.Name);
                KleurVM kleurVM = new KleurVM(kleurM);
                Kleurlijst.Add(kleurVM);
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
        #endregion

        #region Menu_Bestand
        public RelayCommand NieuwCommand
        {
            get { return new RelayCommand(Nieuw); }
        }

        private void Nieuw()
        {
            MenuEnabled = false;
            ControlsZichtbaar = false;
            Status = "Nieuw";
        }

        public RelayCommand OpslaanCommand
        {
            get { return new RelayCommand(Opslaan); }
        }

        private void Opslaan()
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.FileName = "wenskaart";
                sfd.DefaultExt = ".krt";
                sfd.Filter = "wenskaarten | *.krt";

                if (sfd.ShowDialog() == true)
                {
                    using (StreamWriter bestand = new StreamWriter(sfd.FileName))
                    {
                        bestand.WriteLine(Achtergrond.ImageSource.ToString());
                        bestand.WriteLine(ballen.Count());

                        foreach (BalVM b in ballen)
                        {
                            bestand.WriteLine(b.BalKleur.ToString());
                            bestand.WriteLine(b.xPos.ToString());
                            bestand.WriteLine(b.yPos.ToString());
                        }
                        bestand.WriteLine(Wens);
                        bestand.WriteLine(Lettertype);
                        bestand.WriteLine(Tekstgrootte);
                    }
                }
                MessageBox.Show("Uw kaart werd opgeslagen!", "Geslaagd", MessageBoxButton.OK, MessageBoxImage.Information);
                Status = sfd.FileName;
            }

            catch (Exception ex)
            {
                MessageBox.Show("Opslaan mislukt" + ex.Message);
            }

        }

        public RelayCommand OpenenCommand
        {
            get { return new RelayCommand(Openen); }
        }

        private void Openen()
        {
            try
            {

                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "wenskaarten | *.krt";
                ofd.DefaultExt = ".krt";

                if (ofd.ShowDialog() == true)
                {
                    Reset();
                    using (StreamReader bestand = new StreamReader(ofd.FileName))
                    {
                        ImageBrush ib = new ImageBrush();
                        ib.ImageSource = new BitmapImage(new Uri(bestand.ReadLine(), UriKind.Absolute));
                        Achtergrond = ib;

                        int aantalBallen = int.Parse(bestand.ReadLine());

                        if (aantalBallen != 0)
                        {
                            for (int i = 0; i < aantalBallen; i++)
                            {
                                BrushConverter bc = new BrushConverter();
                                Brush kleur = (Brush)bc.ConvertFromString(bestand.ReadLine());
                                Model.Bal balM = new Model.Bal();
                                balM.BalKleur = kleur;
                                balM.xPos = double.Parse(bestand.ReadLine());
                                balM.yPos = double.Parse(bestand.ReadLine());

                                ballen.Add(new BalVM(balM));
                            }
                        }

                        Wens = bestand.ReadLine();
                        Lettertype = new FontFamily(bestand.ReadLine());
                        Tekstgrootte = int.Parse(bestand.ReadLine());
                    }
                }


                Status = ofd.FileName;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        public RelayCommand AfdrukvoorbeeldCommand
        {
            get { return new RelayCommand(Afdrukvoorbeeld); }
        }

        private void Afdrukvoorbeeld()
        {
            MessageBox.Show("Afdrukvoorbeeld", "Afdrukvoorbeeld", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        #endregion

        #region Menu_Kaarten
        public RelayCommand KerstkaartCommand
        {
            get { return new RelayCommand(NieuweKerstkaart); }
        }

        private void NieuweKerstkaart()
        {
            ImageBrush ib = new ImageBrush();
            ib.ImageSource = new BitmapImage(new Uri("pack://application:,,,/Images/kerstkaart.jpg", UriKind.Absolute));
            Achtergrond = ib;

            Reset();
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
            Reset();

        }
        #endregion

        #region Functionaliteit

        public RelayCommand MeerCommand
        {
            get { return new RelayCommand(Meer); }
        }

        private void Meer()
        {
            if (Tekstgrootte < 40)
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

        public RelayCommand<MouseEventArgs> MouseMoveCommand
        {
            get { return new RelayCommand<MouseEventArgs>(MouseMove); }
        }


        private void MouseMove(MouseEventArgs e)
        {
            Ellipse bal = new Ellipse();
            bal = (Ellipse)e.Source;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DataObject sleepBal = new DataObject("deBal", bal.Fill);

                try
                {
                    DragDrop.DoDragDrop(bal, sleepBal, DragDropEffects.Move);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, ex.GetType().ToString());
                }


            }
        }

        public RelayCommand<DragEventArgs> DropCommand
        {
            get { return new RelayCommand<DragEventArgs>(Drop); }
        }

        private void Drop(DragEventArgs e)
        {
            Canvas canvas = new Canvas();
            canvas = (Canvas)e.Source;
            Model.Bal balM = new Model.Bal();
            balM.BalKleur = (Brush)e.Data.GetData("deBal");

            balM.xPos = e.GetPosition(canvas).X - 20;
            balM.yPos = e.GetPosition(canvas).Y - 20;

            BalVM balVM = new BalVM(balM);

            ballen.Add(balVM);
        }

        #endregion

        private void Reset()
        {
            ControlsZichtbaar = true;
            Tekstgrootte = 30;
            Lettertype = new FontFamily("Arial");
            ballen.Clear();
            Wens = "";
            MenuEnabled = true;
            Status = "Nieuw";
        }
        
        #endregion

    }
}
