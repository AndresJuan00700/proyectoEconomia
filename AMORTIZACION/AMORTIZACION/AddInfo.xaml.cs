using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.Design;
using System.Data;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using AMORTIZACION.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace AMORTIZACION
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddInfo : ContentPage
    {
        public ObservableCollection<Datos> ListDatos { get; set; }

        #region Generar Tabla

        private void BtnGenerar_OnClicked(object sender, EventArgs e)
        {

            if (pk_tipo.SelectedIndex==0)
            {
                ListDatos.Clear();
                Picker.SelectedIndex = -1;
                pk_fijo.SelectedIndex = -1;
                pk_tipo.SelectedIndex = -1;
                pk_variable.SelectedIndex = -1;
                calculatefijo();
                txtMonto.Text = "";
                txtCuotas.Text = "";
                txtTasa.Text = "";
                txt_anticipada.Text = "";
                txt_anticipada.IsVisible = false;
            }else if (pk_tipo.SelectedIndex==1)
            {
                ListDatos.Clear();
                Picker.SelectedIndex = -1;
                pk_fijo.SelectedIndex = -1;
                pk_tipo.SelectedIndex = -1;
                pk_variable.SelectedIndex = -1;
                calculatevariable();
                txtMonto.Text = "";
                txtCuotas.Text = "";
                txtTasa.Text = "";
                txt_anticipada.Text = "";
                txt_anticipada.IsVisible = false;
                txt_creciente.Text = "";
                txt_decreciente.Text = "";
                txt_creciente.IsVisible = false;
                txt_decreciente.IsVisible= false;
            }

        }

        #endregion

        #region Calculo de Amortizacion Fija

        private async void calculatefijo()
        {
            if (ValidateInformation())
            {
                double Monto = Convert.ToDouble(txtMonto.Text);
                int Cuotas = Convert.ToInt32(txtCuotas.Text);
                double Interes = Convert.ToDouble(txtTasa.Text);
                double cuotainicial= Convert.ToDouble(txt_anticipada.Text);

                if (cuotainicial  != 0)
                {
                    if (Interes > 19.71)
                    {
                        await DisplayAlert("UPS", "LA TASA NO VALIDA", "ACEPTAR");

                    }
                    else
                    {


                        double answer = (1 - Math.Pow(1 + Interes, Cuotas * -1)) / Interes;

                        double cuotanswer = Math.Round(Monto / answer, 2);

                        this.txtCuotas.Text = cuotanswer.ToString();

                        double Intereses = 0;
                        double TotalIntereses = 0;
                        double capital = 0;
                        double TotalAmortizado = 0;
                        double TotalCuotas = 0;
                        double SaldoIni = 0;
                        double acumulado = 0;
                        double saldofin = Monto;

                        for (int i = 0; i <= Cuotas; i++)
                        {
                            if (i == 0)
                            {
                                SaldoIni = saldofin;
                                saldofin = Math.Round(SaldoIni - cuotainicial, 2);
                                ListDatos.Add(new Datos()
                                {
                                    Id = i,
                                    SaldoIni = SaldoIni,
                                    Cuota = cuotainicial,
                                    Intereses = 0,
                                    saldofin = saldofin,
                                    acumulado = acumulado
                                });
                            }
                            else
                            {
                                Intereses = Math.Round(saldofin * Interes, 2);
                                TotalIntereses += Intereses;
                                acumulado += Intereses;
                                SaldoIni = saldofin;
                                capital = Math.Round(cuotanswer - Interes, 2);
                                TotalAmortizado += capital;
                                saldofin -= capital;
                                TotalCuotas += cuotanswer;
                                ListDatos.Add(new Datos()
                                {
                                    Id = i,
                                    SaldoIni = SaldoIni,
                                    Cuota = cuotanswer,
                                    Intereses = Intereses,
                                    saldofin = saldofin,
                                    acumulado = acumulado
                                });
                            }
                            

                        }

                        App.FpPage.IsPresented = false;
                        await App.FpPage.Detail.Navigation.PushAsync(new ShowTable(new List<Datos>(ListDatos)));
                    }
                }
                else
                {
                    if (Interes > 19.71)
                    {
                        await DisplayAlert("UPS", "LA TASA NO VALIDA", "ACEPTAR");

                    }
                    else
                    {


                        double answer = (1 - Math.Pow(1 + Interes, Cuotas * -1)) / Interes;

                        double cuotanswer = Math.Round(Monto / answer, 2);

                        this.txtCuotas.Text = cuotanswer.ToString();

                        double Intereses = 0;
                        double TotalIntereses = 0;
                        double capital = 0;
                        double TotalAmortizado = 0;
                        double TotalCuotas = 0;
                        double SaldoIni = 0;
                        double acumulado = 0;
                        double saldofin = Monto;

                        for (int i = 1; i <= Cuotas; i++)
                        {
                            Intereses = Math.Round(saldofin * Interes, 2);
                            TotalIntereses += Intereses;
                            acumulado += Intereses;
                            SaldoIni = saldofin;
                            capital = Math.Round(cuotanswer - Interes, 2);
                            TotalAmortizado += capital;
                            saldofin -= capital;
                            TotalCuotas += cuotanswer;
                            ListDatos.Add(new Datos()
                            {
                                Id = i,
                                SaldoIni = SaldoIni,
                                Cuota = cuotanswer,
                                Intereses = Intereses,
                                saldofin = saldofin,
                                acumulado = acumulado
                            });

                        }

                        App.FpPage.IsPresented = false;
                        await App.FpPage.Detail.Navigation.PushAsync(new ShowTable(new List<Datos>(ListDatos)));
                    }
                }
               
            }
            else
            {
                await DisplayAlert("ADVERTENCIA", "DEBE LLENAR TODOS LOS CAMPOS", "ACEPTAR");
            }
            
           txtTasa.IsReadOnly=false;
           txtCuotas.IsReadOnly=false;
           
        }
        #endregion

        #region Calculo de Amortizacion Variable

        private async void calculatevariable()
        {
            if (ValidateInformation())
            {
                double Monto = Convert.ToDouble(txtMonto.Text);
                int Cuotas = Convert.ToInt32(txtCuotas.Text);
                double Interes = Convert.ToDouble(txtTasa.Text);
                double valorcreci = Convert.ToDouble(txt_creciente.Text);
                double valordecreci = Convert.ToDouble(txt_decreciente.Text);

                if (valorcreci != 0 && valorcreci>0)
                {
                    if (Interes > 19.71)
                    {
                        await DisplayAlert("UPS", "LA TASA NO VALIDA", "ACEPTAR");

                    }
                    else
                    {


                        double answer = (1 - Math.Pow(1 + Interes, Cuotas * -1)) / Interes;

                        double cuotanswer = Math.Round(Monto / answer, 2);

                        this.txtCuotas.Text = cuotanswer.ToString();

                        double Intereses = 0;
                        double TotalIntereses = 0;
                        double capital = 0;
                        double TotalAmortizado = 0;
                        double TotalCuotas = 0;
                        double SaldoIni = 0;
                        double acumulado = 0;
                        double saldofin = Monto;

                        for (int i = 1; i <= Cuotas; i++)
                        {
                            cuotanswer += valorcreci;
                            Intereses = Math.Round(saldofin * Interes, 2);
                            TotalIntereses += Intereses;
                            acumulado += Intereses;
                            SaldoIni = saldofin;
                            capital = Math.Round(cuotanswer - Interes, 2);
                            TotalAmortizado += capital;
                            saldofin -= capital;
                            TotalCuotas += cuotanswer;
                            ListDatos.Add(new Datos()
                            {
                                Id = i,
                                SaldoIni = SaldoIni,
                                Cuota = cuotanswer,
                                Intereses = Intereses,
                                saldofin = saldofin,
                                acumulado = acumulado
                            });

                        }

                        App.FpPage.IsPresented = false;
                        await App.FpPage.Detail.Navigation.PushAsync(new ShowTable(new List<Datos>(ListDatos)));
                    }
                }
                else if (valordecreci !=0 && valordecreci >0)
                {
                    if (Interes > 19.71)
                    {
                        await DisplayAlert("UPS", "LA TASA NO VALIDA", "ACEPTAR");

                    }
                    else
                    {


                        double answer = (1 - Math.Pow(1 + Interes, Cuotas * -1)) / Interes;

                        double cuotanswer = Math.Round(Monto / answer, 2);

                        this.txtCuotas.Text = cuotanswer.ToString();

                        double Intereses = 0;
                        double TotalIntereses = 0;
                        double capital = 0;
                        double TotalAmortizado = 0;
                        double TotalCuotas = 0;
                        double SaldoIni = 0;
                        double acumulado = 0;
                        double saldofin = Monto;

                        for (int i = 1; i <= Cuotas; i++)
                        {
                            cuotanswer -= valordecreci;
                            Intereses = Math.Round(saldofin * Interes, 2);
                            TotalIntereses += Intereses;
                            acumulado += Intereses;
                            SaldoIni = saldofin;
                            capital = Math.Round(cuotanswer - Interes, 2);
                            TotalAmortizado += capital;
                            saldofin -= capital;
                            TotalCuotas += cuotanswer;
                            ListDatos.Add(new Datos()
                            {
                                Id = i,
                                SaldoIni = SaldoIni,
                                Cuota = cuotanswer,
                                Intereses = Intereses,
                                saldofin = saldofin,
                                acumulado = acumulado
                            });

                        }

                        App.FpPage.IsPresented = false;
                        await App.FpPage.Detail.Navigation.PushAsync(new ShowTable(new List<Datos>(ListDatos)));
                    }
                }

            }
            else
            {
               await DisplayAlert("ADVERTENCIA", "DEBE LLENAR TODOS LOS CAMPOS", "ACEPTAR");
            }

            txtTasa.IsReadOnly = false;
            txtCuotas.IsReadOnly = false;

        }

        #endregion

        #region Validacion de Informacion




        public bool ValidateInformation()
        {
            bool answer;
            if (string.IsNullOrEmpty(txtMonto.Text))
            {
                answer=false;
            }else if (string.IsNullOrEmpty(txtCuotas.Text))
            {
                answer = false;
            }else if (string.IsNullOrEmpty(txtTasa.Text))
            {
                answer = false;
            }
            else
            {
                answer = true;
            }

            return answer;
        }

        #endregion

        #region Pickers

        private void Picker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (Picker.SelectedIndex!=-1)
            {
                String op = Picker.SelectedItem.ToString();
                int resul_cuota = 0;
                double resul_interes = 0;

                if (ValidateInformation())
                {
                    int Cuotas = Int32.Parse(txtCuotas.Text);
                    double interes = double.Parse(txtTasa.Text);
                    if (interes > 19.71)
                    {
                        DisplayAlert("UPS", "LA TASA DE INTERES NO ES VALIDA", "ACEPTAR");
                        Picker.SelectedIndex = -1;
                    }
                    else
                    {

                        if (op == "Mensual")
                        {
                            resul_cuota = Cuotas;
                            resul_interes = (interes / 100);

                        }
                        else if (op == "Anual")
                        {
                            resul_cuota = Cuotas * 12;
                            resul_interes = (interes / 100)/12;

                        }
                        else if (op == "Trimestral")
                        {
                            resul_cuota = Cuotas * 3;
                            resul_interes = (interes / 100) / 12;

                        }
                        else if (op == "Bimestral")
                        {
                            resul_cuota = Cuotas * 2;
                            resul_interes = (interes / 100) / 12;

                        }
                        else if (op == "Semestral")
                        {
                            resul_cuota = Cuotas * 6;
                            resul_interes = (interes / 100) / 12;
                        }
                        txtCuotas.Text = resul_cuota.ToString();
                        txtTasa.Text = resul_interes.ToString();
                        txtTasa.IsReadOnly = true;
                        txtCuotas.IsReadOnly = true;
                    }
                }
                else
                {
                    Picker.SelectedIndex = -1;

                }
                
            }

        }

        private void Pk_fijo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (pk_fijo.SelectedIndex != -1)
            {
                String option = pk_fijo.SelectedItem.ToString();
                if (option == "Anticipado")
                {
                    txt_anticipada.IsVisible = true;
                }else if (option == "Vencido") 
                {
                    txt_anticipada.IsVisible = false;
                }
            }
        }

        private void Pk_tipo_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (pk_tipo.SelectedIndex !=-1)
            {
                String op = pk_tipo.SelectedItem.ToString();
                if (op == "Fijo")
                {
                    txt_venoanti.IsVisible=true;
                    pk_fijo.IsVisible=true;
                    txt_creciente.IsVisible = false;
                    pk_variable.IsVisible = false;
                    txt_decreciente.IsVisible = false;
                    txt_variable.IsVisible=false;
                }else if (op== "Variable")
                {
                    txt_venoanti.IsVisible = false;
                    pk_fijo.IsVisible = false;
                    txt_creciente.IsVisible=false;
                    pk_variable.IsVisible=true;
                    txt_variable.IsVisible=true;
                    txt_decreciente.IsVisible=false;
                }
                
            }
            else
            {
                pk_variable.IsVisible = false;
                txt_variable.IsVisible = false;
                txt_venoanti.IsVisible = false;
                pk_fijo.IsVisible = false;
                txt_creciente.IsVisible= false;
                txt_decreciente.IsVisible=false;
            }
        }
        private void Pk_variable_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (pk_variable.SelectedIndex !=-1)

            {
                String op = pk_variable.SelectedItem.ToString();
                if (op=="Creciente")
                {
                    txt_creciente.IsVisible=true;
                    txt_decreciente.IsVisible = false;
                }else if (op=="Decreciente")
                {
                    txt_decreciente.IsVisible = true;
                    txt_creciente.IsVisible = false;
                }
            }
        }

        #endregion

        public AddInfo()
        {
            InitializeComponent();
            ListDatos = new ObservableCollection<Datos>();
            var interesList = new List<string>();

            interesList.Add("Mensual");
            interesList.Add("Anual");
            interesList.Add("Trimestral");
            interesList.Add("Bimestral");
            interesList.Add("Semestral");

            Picker.ItemsSource = interesList;
            var fijolist = new List<string>();
            fijolist.Add("Vencido");
            fijolist.Add("Anticipado");
            pk_fijo.ItemsSource = fijolist;

            var tipolist = new List<string>();
            tipolist.Add("Fijo");
            tipolist.Add("Variable");
            pk_tipo.ItemsSource= tipolist;

            var variablelist=new List<string>();
            variablelist.Add("Creciente");
            variablelist.Add("Decreciente");
            pk_variable.ItemsSource= variablelist;
        }

    }
}