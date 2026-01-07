using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Carrito
    {
        //Si existe idproducto y idusuario en la tabla carrito devuelve true, sinó, false.
        public bool ExisteCarrito(int idusuario, int idproducto)
        {
            bool resultado  = true;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_ExisteCarrito", oconexion);
                    cmd.Parameters.AddWithValue("IdUsuario", idusuario);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }
            }
            catch (Exception ex)
            {
                resultado = false;
            }
            return resultado;
        }

        //Evalua si existe producto en carrito (bool), el stock (int) y sumar (bool)
        //Si existe en carrito le agrega uno, sinó, lo añade por primera vez (si hay stock disponible)
        //si sumar es true, sumamos al carrito y quimatos del stock, sinó, al revés
        public bool OperacionCarrito(int idusuario, int idproducto, bool sumar, out string Mensaje)
        {
            bool resultado = true;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_OperacionCarrito", oconexion);
                    cmd.Parameters.AddWithValue("IdUsuario", idusuario);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.AddWithValue("Sumar", sumar);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        //Devuelve la cantidad de productos que tenga el carrito
        public int CantidadEnCarrito(int idusuario)
        {
            int resultado = 0;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("select count(*) from CARRITO where IdUsuario = @IdUsuario", oconexion);
                    cmd.Parameters.AddWithValue("@IdUsuario", idusuario);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = Convert.ToInt32(cmd.ExecuteScalar());
                }
            }
            catch (Exception ex)
            {
                resultado = 0;
            }
            return resultado;
        }

        public List<Carrito> ListarProducto(int idusuario)
        {
            List<Carrito> list = new List<Carrito>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from fn_obtenerCarritoUsuario(@idUsuario)";

                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.Parameters.AddWithValue("@idUsuario", idusuario);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            list.Add(new Carrito()
                                {
                                    oProducto = new Producto() { 
                                        IdProducto = Convert.ToInt32(dr["Id"]),
                                        Nombre = dr["Nombre"].ToString(),
                                        oDetalle = new DetalleProducto() { PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]) },
                                        RutaImagen = dr["RutaImagen"].ToString(),
                                        NombreImagen = dr["NombreImagen"].ToString(),
                                        oMarca = new Marca()
                                        {
                                            Descripcion = dr["DesMarca"].ToString()
                                        }
                                    },
                                    Cantidad = Convert.ToInt32(dr["Cantidad"])
                            });
                        }
                    }
                }
            }
            catch
            {
                list = new List<Carrito>();
            }
            return list;
        }

        public bool EliminarCarrito(int idusuario, int idproducto)
        {
            bool resultado = true;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarCarrito", oconexion);
                    cmd.Parameters.AddWithValue("IdUsuario", idusuario);
                    cmd.Parameters.AddWithValue("IdProducto", idproducto);
                    cmd.Parameters.Add("Resultado", SqlDbType.Bit).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    resultado = Convert.ToBoolean(cmd.Parameters["Resultado"].Value);
                }
            }
            catch (Exception ex)
            {
                resultado = false;
            }
            return resultado;
        }

    }
}
