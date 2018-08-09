using prjLegados.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace prjLegados.Controllers
{
    public class LogController : Controller
    {
        // GET: Log
        public ActionResult Log()
        {
            return View();
        }

        public JsonResult ListUser()
        {
            var lstUsuario = new List<Log>();
            SqlCommand sqlComando = null;
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Software"].ConnectionString))
                {
                    sqlComando = new SqlCommand("listarUsuariosLog", sqlConnection);
                    sqlConnection.Open();
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dataReader = sqlComando.ExecuteReader();
                    while (dataReader.Read())
                    {

                        var log = new Log();
                        log.nombreUsuario2 = dataReader["nombreUsuario"].ToString();
                        log.fechaLog = dataReader["fechaLog"].ToString();
                        log.horaLog = dataReader["horaLog"].ToString();
                        lstUsuario.Add(log);

                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return Json(lstUsuario, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult ingresarLog(Log usuario)
        {
            SqlCommand sqlComando = null;
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Software"].ConnectionString))
                {
                    sqlConnection.Open();
                    sqlComando = new SqlCommand("ingresarUsuariosLog", sqlConnection);
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@nombreUsuario", usuario.nombreUsuario2);
                    sqlComando.Parameters.AddWithValue("@fechaLog", usuario.fechaLog);
                    sqlComando.Parameters.AddWithValue("@horaLog", usuario.horaLog);
                    
                    //SqlDataReader dataReader = sqlComando.ExecuteReader();
                    sqlComando.ExecuteNonQuery();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return Json("Log ingresado con éxito", JsonRequestBehavior.AllowGet);

        }
    }
}