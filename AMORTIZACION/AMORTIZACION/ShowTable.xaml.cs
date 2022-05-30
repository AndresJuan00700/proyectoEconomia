using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AMORTIZACION.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AMORTIZACION
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ShowTable : ContentPage
    {
        public ShowTable(List<Datos> listDatos)
        {
            InitializeComponent();
            myListView.ItemsSource = listDatos;
        }
    }
}