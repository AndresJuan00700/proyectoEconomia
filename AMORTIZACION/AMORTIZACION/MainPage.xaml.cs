using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AMORTIZACION
{
    public partial class MainPage : FlyoutPage
    {
        public MainPage()
        {
            InitializeComponent();
            this.Flyout = new MenuInfo();
            this.Detail = new NavigationPage(new AddInfo())
            {
                
                 BarBackgroundColor  =  Color . FromHex ( "#EF5350"),
                BarTextColor = Color.DarkRed
            };

            App.FpPage = this;

        }
    }
}
