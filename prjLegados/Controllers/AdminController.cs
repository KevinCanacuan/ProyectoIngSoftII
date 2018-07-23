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
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Admin()
        {
            return View();
        }

        public JsonResult ListAdmin(){
            var lstUsuario = new List<Administrador>();
            SqlCommand sqlComando = null;
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Software"].ConnectionString))
                {
                    sqlComando = new SqlCommand("listarUsuarios", sqlConnection);
                    sqlConnection.Open();
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dataReader = sqlComando.ExecuteReader();
                    while (dataReader.Read())
                    {
                        
                        var usuario = new Administrador();
                        usuario.idUsuario = (int)dataReader["idUsuario"];
                        usuario.nombreUsuario = dataReader["nombreUsuario"].ToString();
                        lstUsuario.Add(usuario);

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

        public JsonResult Add(Administrador usuario)
        {
            SqlCommand sqlComando = null;
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Software"].ConnectionString))
                {
                    sqlComando = new SqlCommand("insertarUsuarios", sqlConnection);
                    sqlComando.Parameters.AddWithValue("@nombre", usuario.nombreUsuario);
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

            return Json("Usuario ingresado con éxito", JsonRequestBehavior.AllowGet);

        }

        public JsonResult Delete(int ID)
        {
            SqlCommand sqlComando = null;
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Software"].ConnectionString))
                {
                    sqlComando = new SqlCommand("eliminarUsuario", sqlConnection);
                    sqlComando.Parameters.AddWithValue("@idUser", ID);
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

            return Json("Se ha eliminado con éxito", JsonRequestBehavior.AllowGet);

        }

        public JsonResult Update(Administrador usuario)
        {
            SqlCommand sqlComando = null;
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Software"].ConnectionString))
                {
                    sqlComando = new SqlCommand("actualizarUsuario", sqlConnection);
                    sqlComando.Parameters.AddWithValue("@id", usuario.idUsuario);
                    sqlComando.Parameters.AddWithValue("@nombre", usuario.nombreUsuario);
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

            return Json("Usuario actualizado con éxito", JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetbyID(int ID)
        {
            var usuario = new Administrador();
            SqlCommand sqlComando = null;
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Software"].ConnectionString))
                {
                    sqlComando = new SqlCommand("encontrarUsuario", sqlConnection);
                    sqlComando.Parameters.AddWithValue("@id", ID);
                    sqlConnection.Open();
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dataReader = sqlComando.ExecuteReader();
                    while (dataReader.Read())
                    {

                        usuario.idUsuario = (int)dataReader["idUsuario"];
                        usuario.nombreUsuario = dataReader["nombreUsuario"].ToString();
                        
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

            return Json(usuario, JsonRequestBehavior.AllowGet);

        }
    }
}