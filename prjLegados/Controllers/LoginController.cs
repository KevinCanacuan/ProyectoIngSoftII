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
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public JsonResult comprobarLogin(string strUsername, string strPassword) {
          
            string strMensaje = "Ingreso con éxito";
            SqlCommand sqlComando = null;
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Software"].ConnectionString))
                {
                    sqlComando = new SqlCommand("insertarUsuarios", sqlConnection);
                    sqlComando.Parameters.AddWithValue("@nombre", strUsername);
                    sqlComando.Parameters.AddWithValue("@clave", strPassword);
                    sqlConnection.Open();
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dataReader = sqlComando.ExecuteReader();
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

            return Json(strMensaje);
        }

        [HttpPost]
        public JsonResult ingresarUsuario(string strUsername, string strPassword)
        {

            string strMensaje = "Registro con éxito";
            SqlCommand sqlComando = null;
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Software"].ConnectionString))
                {
                    sqlComando = new SqlCommand("insertarUsuarios", sqlConnection);
                    
                    sqlConnection.Open();
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@username", strUsername);
                    sqlComando.Parameters.AddWithValue("@password", strPassword);
                    sqlComando.ExecuteNonQuery();
    
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Mensaje de error");
                Console.WriteLine(e.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return Json(strMensaje);
        }

        [HttpPost]
        public JsonResult consultarUsuario(string strUsername, string strPassword)
        {

            string strMensaje = "Log in exitoso";
            SqlCommand sqlComando = null;
            SqlConnection sqlConnection = null;
            try
            {
                bool status = false;
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Software"].ConnectionString))
                {
                    sqlComando = new SqlCommand("consultarUsuarios", sqlConnection);

                    sqlConnection.Open();
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    sqlComando.Parameters.AddWithValue("@username", strUsername);
                    sqlComando.Parameters.AddWithValue("@password", strPassword);
                    sqlComando.ExecuteNonQuery();
                    status = Convert.ToBoolean(sqlComando.ExecuteScalar());
                    strMensaje = status.ToString();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Mensaje de error");
                Console.WriteLine(e.Message);
            }
            finally
            {
                sqlConnection.Close();
            }

            return Json(strMensaje);
        }

       

    }
}