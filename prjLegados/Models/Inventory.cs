using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace prjLegados.Models
{
    public class Inventory
    {
        public int idInventario { get; set; }
        public string nombreObjeto { get; set; }
        public double precioUnit { get; set; }
        public int cantidadDisponible { get; set; }
        public DateTime fechaIngreso { get; set; }
    }
}