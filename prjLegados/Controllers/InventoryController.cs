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
    public class InventoryController : Controller
    {
        // GET: Inventory
        public ActionResult Inventory()
        {
            return View();
        }
        public JsonResult ListInventory()

        {
            var lstInventario = new List<Inventory>();
            SqlCommand sqlComando = null;
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Software"].ConnectionString))
                {
                    sqlComando = new SqlCommand("consultarInventario", sqlConnection);
                    sqlConnection.Open();
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dataReader = sqlComando.ExecuteReader();
                    while (dataReader.Read())
                    {
                        var inventario = new Inventory();
                        inventario.idInventario = (int)dataReader["idInventario"];
                        inventario.nombreObjeto = dataReader["nombreObjeto"].ToString();
                        inventario.precioUnit = (double)dataReader["precioUnit"];
                        inventario.cantidadDisponible = (int)dataReader["cantidadDisponible"];
                        inventario.fechaIngreso = (DateTime)dataReader["fechaIngreso"];
                        lstInventario.Add(inventario);

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

            return Json(lstInventario, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Add(Inventory inventario)
        {
            SqlCommand sqlComando = null;
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Software"].ConnectionString))
                {
                    sqlComando = new SqlCommand("insertarInventario", sqlConnection);
                    sqlComando.Parameters.AddWithValue("@nombre", inventario.nombreObjeto);
                    sqlComando.Parameters.AddWithValue("@precio", inventario.precioUnit);
                    sqlComando.Parameters.AddWithValue("@cantidad", inventario.cantidadDisponible);
                    sqlComando.Parameters.AddWithValue("@fecha", inventario.fechaIngreso);
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

            return Json("Inventario ingresado con éxito", JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetbyID(int ID)
        {
            var inventario = new Inventory();
            SqlCommand sqlComando = null;
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Software"].ConnectionString))
                {
                    sqlComando = new SqlCommand("encontrarInventario", sqlConnection);
                    sqlComando.Parameters.AddWithValue("@id", ID);
                    sqlConnection.Open();
                    sqlComando.CommandType = CommandType.StoredProcedure;
                    SqlDataReader dataReader = sqlComando.ExecuteReader();
                    while (dataReader.Read())
                    {

                        inventario.idInventario = (int)dataReader["idInventario"];
                        inventario.nombreObjeto = dataReader["nombreObjeto"].ToString();
                        inventario.precioUnit = (double)dataReader["precioUnit"];
                        inventario.cantidadDisponible = (int)dataReader["cantidadDisponible"];
                        inventario.fechaIngreso = (DateTime)dataReader["fechaIngreso"];

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

            return Json(inventario, JsonRequestBehavior.AllowGet);

        }

        public JsonResult Update(Inventory inventario)
        {
            SqlCommand sqlComando = null;
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Software"].ConnectionString))
                {
                    sqlComando = new SqlCommand("actualizarInventario", sqlConnection);
                    sqlComando.Parameters.AddWithValue("@id", inventario.idInventario);
                    sqlComando.Parameters.AddWithValue("@nombre", inventario.nombreObjeto);
                    sqlComando.Parameters.AddWithValue("@precio", inventario.precioUnit);
                    sqlComando.Parameters.AddWithValue("@cantidad", inventario.cantidadDisponible);
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

            return Json("Inventario actualizado con éxito", JsonRequestBehavior.AllowGet);

        }
        public JsonResult Delete(int ID)
        {
            SqlCommand sqlComando = null;
            SqlConnection sqlConnection = null;
            try
            {
                using (sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["Software"].ConnectionString))
                {
                    sqlComando = new SqlCommand("eliminarInventario", sqlConnection);
                    sqlComando.Parameters.AddWithValue("@id", ID);
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

            return Json("Se ha eliminado con éxito el objeto del inventario", JsonRequestBehavior.AllowGet);

        }
    }
}