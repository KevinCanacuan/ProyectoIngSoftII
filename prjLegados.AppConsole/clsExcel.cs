using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using prjLegados.Data.Structure;
using Excel = Microsoft.Office.Interop.Excel;
using prjLegados.AzureStorage.Blobs;
using System.Reflection;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using prjLegados.AzureStorage.Search;

namespace prjLegados.AppConsole
{
    public class clsExcel
    {
        
        public void fntValidarExcel()
        {
           
                string strvalue = ConfigurationManager.AppSettings["Direccion"];
                string strconnectionString = "Provider=Microsoft.ACE.OLEDB.12.0; Data Source=" + strvalue + ";Extended Properties='Excel 12.0 Macro;HDR=YES'";

                OleDbConnection OleDbconn = new OleDbConnection(strconnectionString);
                OleDbCommand OleDbcmd = new OleDbCommand();
                OleDbDataAdapter dataAdapter = new OleDbDataAdapter();

                System.Data.DataTable dttDataTable = new System.Data.DataTable();
                OleDbcmd.Connection = OleDbconn;
            try
            {
                OleDbconn.Open();
                OleDbcmd.CommandText = "SELECT * From [Hoja1$]";
                dataAdapter.SelectCommand = OleDbcmd;
                dataAdapter.Fill(dttDataTable);
                OleDbconn.Close();
            }
            catch (Exception ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Para poder abrir el archivo excel se deben instalar los drivers necesarios");
            }

                var excel = dttDataTable;

                Application myExcelApp = new Application();

                Workbook myExcelWorkbook;
                Worksheet myExcelSheet;

                myExcelWorkbook = myExcelApp.Workbooks.Open(strvalue, 0, false, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", true, false, 0, true, 1, 0);

                myExcelSheet = (Worksheet)myExcelWorkbook.Sheets.get_Item(1);

                myExcelWorkbook.Close();
                 Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[{0}] Procesando...",DateTime.Now);
            
            foreach (DataRow row in excel.Rows)
                {
                    var index = new IndexFiles();
                    var error = new clsErrores();

                    if (row.Field<object>(0) is null)
                        continue;

                    //asignacion de celdas a variables
                    double dblIndice = row.Field<double>(0);
                    string strrutaArchivo = row.Field<string>(1);
                    string strnombreArchivo = row.Field<string>(2);
                    string strnombreAlternoArchivo = row.Field<string>(3);
                    string strExtension = Path.GetExtension(strrutaArchivo);
                    string strextensionArchivo = "." + row.Field<string>(4);
                    string strDescripcionArchivo = row.Field<string>(5);
                    string strCompaniaArchivo = row.Field<string>(6);
                    string strAplicativoArchivo = row.Field<string>(7);
                    string strModuloArchivo = row.Field<string>(8);
                    string strAnioReferencialArchivo = row.Field<string>(9);
                    string strMesReferencialArchivo = row.Field<string>(10);
                    string strNombreContainerArchivo = row.Field<string>(11);
                    string strCarga = row.Field<string>(12);
                    string strIndexo = row.Field<string>(13);
                    string strMensajeError = row.Field<string>(14);

                    strnombreArchivo = strNomralizarString(strnombreArchivo);
                    strnombreAlternoArchivo = strNomralizarString(strnombreAlternoArchivo);
                    strDescripcionArchivo = strNomralizarString(strDescripcionArchivo);
                    strCompaniaArchivo = strNomralizarString(strCompaniaArchivo);
                    strAplicativoArchivo = strNomralizarString(strAplicativoArchivo);
                    strNombreContainerArchivo = strNomralizarString(strNombreContainerArchivo);
                    strModuloArchivo = strNomralizarString(strModuloArchivo);
                
                    if (strCarga == null)
                    {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine("[{0}] Procesando Documento " + dblIndice, DateTime.Now);
                    error.dblIndice = dblIndice;
                        User usr = new User();
                        BlobStorage blb = new BlobStorage();

                        //validacion de campos de excel

                        //path
                        if (!File.Exists(strrutaArchivo))
                        {
                            error.lstErrores.Add("Error: No existe la ruta para el archivo");
                        }

                        //nombre archivo
                        if (String.IsNullOrEmpty(strnombreArchivo))
                        {
                            error.lstErrores.Add("Error: No se ha ingrsado un nombre el archivo");
                        }
                        else
                        {
                            if (strnombreArchivo != Path.GetFileNameWithoutExtension(strrutaArchivo))
                                error.lstErrores.Add("Error: No se ha ingresado correctamente el nombre del archivo");
                        }

                        //nombre alterno archivo

                        if (String.IsNullOrEmpty(strnombreAlternoArchivo))
                        {
                            error.lstErrores.Add("Error: nombre alterno del  archivo se encuentra vacio ");
                        }

                        //extension
                        if (strExtension != strextensionArchivo)
                        {
                            error.lstErrores.Add("Extensiones no coinciden");
                        }

                        //Modulo
                        if (String.IsNullOrEmpty(strModuloArchivo))
                        {
                            error.lstErrores.Add("Error: No se ha ingresado modulo");
                        }

                        //Año referncial

                        if (String.IsNullOrEmpty(strAnioReferencialArchivo))
                        {
                            error.lstErrores.Add("Error: No se ha ingresado año referencial");
                        }
                        else
                        {

                            if (strAnioReferencialArchivo.Length > 4 || strAnioReferencialArchivo.Length < 4)
                            {
                                error.lstErrores.Add("Error: Se debe ingresar el año en formato aaaa");
                            }
                        }

                        //Mes referencial
                        if (String.IsNullOrEmpty(strMesReferencialArchivo))
                        {
                            error.lstErrores.Add("Error: No se ha ingresado mes referencial");
                        }
                        else
                        {
                            if (strMesReferencialArchivo.Length > 2)
                            {
                                error.lstErrores.Add("Error: Se debe ingresar el mes en formato mm ");

                            }
                        }

                        //Empresa
                        if (String.IsNullOrEmpty(strCompaniaArchivo))
                        {
                            error.lstErrores.Add("Error: El nombre de la empresa se encuentra vacío");
                        }
                        else
                        {
                            if (usr.fntCompanyStr != strCompaniaArchivo)
                            {
                                error.lstErrores.Add("Error: El nombre de la empresa mencionanda no existe");
                            }
                        }

                        //aplicativo

                        if (String.IsNullOrEmpty(strAplicativoArchivo))
                        {
                            error.lstErrores.Add("Error:Se debe ingresar un  nombre para el aplicativo");

                        }
                        else

                        {
                            int valor = usr.fntAplicationsLst.Count;

                            for (int i = 0; i < usr.fntAplicationsLst.Count; i++)
                            {
                                if (usr.fntAplicationsLst[i] != strAplicativoArchivo)
                                {
                                    valor = valor - 1;
                                }

                            }

                            if (valor == 0)
                            {
                                error.lstErrores.Add("Error: El nombre ingresado para el aplicativo no existe");

                            }
                        }

                        //container

                        var lstContainer = blb.fntListBlobContainerLst(usr);
                        Container containerRow = null;

                        if (String.IsNullOrEmpty(strNombreContainerArchivo))
                        {
                            error.lstErrores.Add("Error:Se debe ingresar un  nombre para el container");
                        }
                        else
                        {
                            try { containerRow = lstContainer.First(x => x.fntFullNameStr == strNombreContainerArchivo); }
                            catch (Exception ex) { error.lstErrores.Add("Error: El nombre ingresado para el container no existe"); }

                        }

                        List<clsErrores> lstListaErroresExcel = new List<clsErrores>();
                    lstListaErroresExcel.Add(error);
                    try
                    {
                        if (error.lstErrores.Count > 0)
                        {

                            //insercion de mensajes de error

                            using (var cn = new OleDbConnection(strconnectionString))
                            {
                                cn.Open();
                                OleDbCommand cmd1 = new OleDbCommand();
                                cmd1.Connection = cn;
                                var str = new StringBuilder();
                                foreach (var item in error.lstErrores)
                                {
                                    str.Append("- " + item + "\n");
                                }
                                string sql = "Update [Hoja1$] set Validaciones='" + str + "\n" + "' where ORD=" + error.dblIndice;
                                cmd1.CommandText = sql;
                                cmd1.ExecuteNonQuery();
                                cn.Close();
                            }

                            using (var cn = new OleDbConnection(strconnectionString))
                            {
                                cn.Open();
                                OleDbCommand cmd1 = new OleDbCommand();
                                cmd1.Connection = cn;
                                string sql = "Update [Hoja1$] set cargo='No' where ORD=" + error.dblIndice;
                                cmd1.CommandText = sql;
                                cmd1.ExecuteNonQuery();
                                cn.Close();
                            }

                            using (var cn = new OleDbConnection(strconnectionString))
                            {
                                cn.Open();
                                OleDbCommand cmd1 = new OleDbCommand();
                                cmd1.Connection = cn;
                                string sql = "Update [Hoja1$] set indexo='No' where ORD=" + error.dblIndice;
                                cmd1.CommandText = sql;
                                cmd1.ExecuteNonQuery();
                                cn.Close();
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error al momento de escribir en el documento excel");
                    }

                    try
                    {
                        if (error.lstErrores.Count == 0)
                        {

                            FileUpload flSubirArchivos = new FileUpload()
                            {
                                fntNameFileStr = strnombreAlternoArchivo,
                                fntLocalPathStr = strrutaArchivo,
                                fntCompanyStr = strCompaniaArchivo,
                                fntApplicationStr = strAplicativoArchivo,
                            };

                            Dictionary<string, object> diccionario = new Dictionary<string, object>();
                            strextensionArchivo = strextensionArchivo.Replace(".", "");
                            int sstrAnioReferencialArchivo = int.Parse(strAnioReferencialArchivo);
                            int sstrMesReferencialArchivo = int.Parse(strMesReferencialArchivo);

                            diccionario.Add("Id", dblIndice.ToString());
                            diccionario.Add("FileName", strnombreArchivo);
                            diccionario.Add("AlterName", strnombreAlternoArchivo);
                            diccionario.Add("Extension", strextensionArchivo);
                            diccionario.Add("Company", strCompaniaArchivo);
                            diccionario.Add("Container", strNombreContainerArchivo);
                            diccionario.Add("Application", strAplicativoArchivo);
                            diccionario.Add("Module", strModuloArchivo);
                            diccionario.Add("DocumentUploadAt", DateTimeOffset.Now);
                            diccionario.Add("ReferencialYear", sstrAnioReferencialArchivo);
                            diccionario.Add("ReferencialMonth", sstrMesReferencialArchivo);
                            diccionario.Add("Path", strrutaArchivo);
                            diccionario.Add("Description", strDescripcionArchivo);
                            diccionario.Add("Tags", null);

                            flSubirArchivos.fntMetadataDct = diccionario;

                            IEnumerable<FileUpload> lstArchivos = new List<FileUpload>() { flSubirArchivos };

                            var documentosCargar = blb.fntUploadBlobs(containerRow, lstArchivos, usr);

                            var documentosIndexar = index.AddDocumentsToIndex(documentosCargar, usr);

                            if (documentosCargar.First().IsUploaded)
                            {

                                using (var cn = new OleDbConnection(strconnectionString))
                                {
                                    cn.Open();
                                    OleDbCommand cmd1 = new OleDbCommand();
                                    cmd1.Connection = cn;
                                    string sql = "Update [Hoja1$] set cargo='Si' where ORD=" + error.dblIndice;
                                    cmd1.CommandText = sql;
                                    cmd1.ExecuteNonQuery();
                                    cn.Close();
                                }

                            }
                            else
                            {

                                using (var cn = new OleDbConnection(strconnectionString))
                                {

                                    cn.Open();
                                    OleDbCommand cmd1 = new OleDbCommand();
                                    cmd1.Connection = cn;
                                    string sql = "Update [Hoja1$] set cargo='No' where ORD=" + error.dblIndice;
                                    cmd1.CommandText = sql;
                                    cmd1.ExecuteNonQuery();
                                    cn.Close();

                                }

                            }

                            if (documentosIndexar.First().IsIndexed)
                            {

                                using (var cn = new OleDbConnection(strconnectionString))
                                {
                                    cn.Open();
                                    OleDbCommand cmd1 = new OleDbCommand();
                                    cmd1.Connection = cn;
                                    string sql = "Update [Hoja1$] set indexo='Si' where ORD=" + error.dblIndice;
                                    cmd1.CommandText = sql;
                                    cmd1.ExecuteNonQuery();
                                    cn.Close();
                                }

                            }
                            else
                            {

                                using (var cn = new OleDbConnection(strconnectionString))
                                {

                                    cn.Open();
                                    OleDbCommand cmd1 = new OleDbCommand();
                                    cmd1.Connection = cn;
                                    string sql = "Update [Hoja1$] set indexo='No' where ORD=" + error.dblIndice;
                                    cmd1.CommandText = sql;
                                    cmd1.ExecuteNonQuery();
                                    cn.Close();

                                }

                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Error al momento de escribir en el documento excel");
                    }
                }
            }

                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("[{0}] Proceso Terminado ", DateTime.Now);
        }
        public string strNomralizarString(string strinput)
        {
            
            var normalizedString = strinput.Normalize(NormalizationForm.FormD);
            var sb = new StringBuilder();
            for (int i = 0; i < normalizedString.Length; i++)
            {

                var uc = System.Globalization.CharUnicodeInfo.GetUnicodeCategory(normalizedString[i]);
                if (uc != System.Globalization.UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(normalizedString[i]);
                }
            }
            return ((sb.ToString().Normalize(NormalizationForm.FormC)));
        }

    }
}
   

