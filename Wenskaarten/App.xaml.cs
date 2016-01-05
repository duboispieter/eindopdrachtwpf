using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Wenskaarten
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            Model.Wenskaart wenskaart = new Model.Wenskaart();
            ViewModel.WenskaartVM wenskaartVM = new ViewModel.WenskaartVM(wenskaart);
            View.WenskaartView wenskaartView = new View.WenskaartView();
            wenskaartView.DataContext = wenskaartVM;
            wenskaartView.Show();

            
        }
    }
}
