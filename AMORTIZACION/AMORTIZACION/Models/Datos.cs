using System;
using System.Collections.Generic;
using System.Text;

namespace AMORTIZACION.Models
{
    public class Datos
    {
        public int Id { get; set; }
        public double Intereses { get; set; }
        public double Cuota { get; set; }
        public double SaldoIni { get; set; }
        public double acumulado { get; set; }
        public double saldofin { get; set; }
        public string Fecha { get; set; }
    }
}
