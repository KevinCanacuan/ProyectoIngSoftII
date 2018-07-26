using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjLegados.Controllers
{
    public class UploadController : Controller
    {
        // GET: Upload
        public ActionResult Upload()
        {
            return View();
        }

        public JsonResult cargarCsv(string nombre) {
            List<string> lstLinea = new List<string>();
            List<String> lstFinal = new List<String>();
            string line;
            string ruta = @"E:/Documents/Visual Studio/";
            ruta +=nombre;

            // Read the file and display it line by line.  
            System.IO.StreamReader file =
                new System.IO.StreamReader(ruta);
            while ((line = file.ReadLine()) != null)
            {
                lstLinea.Add(line);

            }

            file.Close();
            lstLinea.RemoveAt(0);
            foreach (string item in lstLinea) {
                Char delimiter = ',';
                String[] substrings = item.Split(delimiter);
                foreach (string a in substrings) {
                    lstFinal.Add(a.ToString());
                }
            }

            return Json(lstFinal, JsonRequestBehavior.AllowGet);
        }
    }
}