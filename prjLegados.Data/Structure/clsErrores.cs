using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prjLegados.Data.Structure
{
   public class clsErrores
    {
        public double dblIndice { get; set; }
        public List<string> lstErrores { get; set; }
     
        public clsErrores()
        {
            this.lstErrores = new List<string>();
        }
    }
}
