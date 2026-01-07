using CapaEntidad;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class CD_Productos
    {
        public List<Producto> Listar(bool soloActivos)
        {
            List<Producto> list = new List<Producto>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    //Es para leer la consulta con saltos de linea.
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("select p.Id, Nombre, p.Descripcion, ");
                    sb.AppendLine("m.Id[IdMarca], m.Descripcion[DesMarca], ");
                    sb.AppendLine("c.Id[IdCate], c.Descripcion[DesCate], ");
                    sb.AppendLine("Stock, dp.PrecioVenta[PrecioVenta], dp.PrecioCompra[PrecioCompra], dp.CantidadCompra, RutaImagen, NombreImagen, p.Activo ");
                    sb.AppendLine("from PRODUCTO p ");
                    sb.AppendLine("inner join MARCA m on m.Id = p.MarcaId ");
                    sb.AppendLine("inner join CATEGORIA c on c.Id = p.CategoriaId ");
                    sb.AppendLine("inner join DetalleProducto dp on dp.ProductoId = p.Id ");

                    if(soloActivos)
                        sb.AppendLine("where p.Activo = 1 ");


                    SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            list.Add(
                                new Producto()
                                {
                                    IdProducto = Convert.ToInt32(dr["Id"]),
                                    Nombre = dr["Nombre"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString(),
                                    oMarca = new Marca() { IdMarca = Convert.ToInt32(dr["IdMarca"]), Descripcion = dr["DesMarca"].ToString() },
                                    oCategoria = new Categoria() { IdCategoria = Convert.ToInt32(dr["IdCate"]), Descripcion = dr["DesCate"].ToString() },
                                    Stock = Convert.ToInt32(dr["Stock"]),
                                    oDetalle = new DetalleProducto() { 
                                        PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]), 
                                        PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"]), 
                                        CantidadCompra = Convert.ToInt32(dr["CantidadCompra"])
                                    },

                                    RutaImagen = dr["RutaImagen"].ToString(),
                                    NombreImagen = dr["NombreImagen"].ToString(),
                                    Activo = Convert.ToBoolean(dr["Activo"])
                                }

                                );
                        }
                    }
                }
            }
            catch
            {
                list = new List<Producto>();
            }
            return list;
        }

        public Producto Detalle(int Id)
        {
            Producto prod = new Producto();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    //Es para leer la consulta con saltos de linea.
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("select p.Id, Nombre, p.Descripcion, ");
                    sb.AppendLine("m.Id[IdMarca], m.Descripcion[DesMarca], ");
                    sb.AppendLine("c.Id[IdCate], c.Descripcion[DesCate], ");
                    //sb.AppendLine("Stock, dp.PrecioVenta[Precio], RutaImagen, NombreImagen, p.Activo ");
                    sb.AppendLine("Stock, dp.PrecioVenta[PrecioVenta], dp.PrecioCompra[PrecioCompra], dp.CantidadCompra, RutaImagen, NombreImagen, p.Activo ");
                    sb.AppendLine("from PRODUCTO p ");
                    sb.AppendLine("inner join MARCA m on m.Id = p.MarcaId ");
                    sb.AppendLine("inner join CATEGORIA c on c.Id = p.CategoriaId ");
                    sb.AppendLine("inner join DetalleProducto dp on dp.ProductoId = p.Id ");
                    sb.AppendLine("where p.Id = " + Id);


                    SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            prod = new Producto()
                            {
                                IdProducto = Convert.ToInt32(dr["Id"]),
                                Nombre = dr["Nombre"].ToString(),
                                Descripcion = dr["Descripcion"].ToString(),
                                oMarca = new Marca() { IdMarca = Convert.ToInt32(dr["IdMarca"]), Descripcion = dr["DesMarca"].ToString() },
                                oCategoria = new Categoria() { IdCategoria = Convert.ToInt32(dr["IdCate"]), Descripcion = dr["DesCate"].ToString() },
                                Stock = Convert.ToInt32(dr["Stock"]),
                                //oDetalle = new DetalleProducto() { PrecioVenta = Convert.ToDecimal(dr["Precio"]) },
                                oDetalle = new DetalleProducto()
                                {
                                    PrecioVenta = Convert.ToDecimal(dr["PrecioVenta"]),
                                    PrecioCompra = Convert.ToDecimal(dr["PrecioCompra"]),
                                    CantidadCompra = Convert.ToInt32(dr["CantidadCompra"])
                                },
                                RutaImagen = dr["RutaImagen"].ToString(),
                                NombreImagen = dr["NombreImagen"].ToString(),
                                Activo = Convert.ToBoolean(dr["Activo"])
                            };
                        }
                    }
                }
                

            }
            catch
            {
                prod = new Producto();
            }
            return prod;
        }

        public int Registrar(Producto obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarProducto", oconexion);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", obj.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.AddWithValue("PrecioCompra", obj.oDetalle.PrecioCompra);
                    cmd.Parameters.AddWithValue("PrecioVenta", obj.oDetalle.PrecioVenta);
                    cmd.Parameters.AddWithValue("CantidadCompra", obj.oDetalle.CantidadCompra);
                    cmd.Parameters.Add("Resultado", SqlDbType.Int).Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("Mensaje", SqlDbType.VarChar, 500).Direction = ParameterDirection.Output;
                    cmd.CommandType = CommandType.StoredProcedure;

                    oconexion.Open();

                    cmd.ExecuteNonQuery();

                    idautogenerado = Convert.ToInt32(cmd.Parameters["Resultado"].Value);
                    Mensaje = cmd.Parameters["Mensaje"].Value.ToString();
                }
            }
            catch (Exception ex)
            {
                idautogenerado = 0;
                Mensaje = ex.Message;
            }
            return idautogenerado;
        }

        public bool Editar(Producto obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarProducto", oconexion);
                    cmd.Parameters.AddWithValue("IdProducto", obj.IdProducto);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("IdMarca", obj.oMarca.IdMarca);
                    cmd.Parameters.AddWithValue("IdCategoria", obj.oCategoria.IdCategoria);
                    cmd.Parameters.AddWithValue("Stock", obj.Stock);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.AddWithValue("PrecioCompra", obj.oDetalle.PrecioCompra);
                    cmd.Parameters.AddWithValue("PrecioVenta", obj.oDetalle.PrecioVenta);
                    cmd.Parameters.AddWithValue("CantidadCompra", obj.oDetalle.CantidadCompra);
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

        public bool GuardarDatosImagen(Producto oProducto, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    string query = "update PRODUCTO set RutaImagen = @rutaimagen, NombreImagen = @nombreimagen where Id = @idproducto";
                                    
                    SqlCommand cmd = new SqlCommand(query, oconexion);
                    cmd.Parameters.AddWithValue("@rutaimagen", oProducto.RutaImagen);
                    cmd.Parameters.AddWithValue("@nombreimagen", oProducto.NombreImagen);
                    cmd.Parameters.AddWithValue("@idproducto", oProducto.IdProducto);
                    cmd.CommandType = CommandType.Text;

                    oconexion.Open();

                    if (cmd.ExecuteNonQuery() > 0)
                    {
                        resultado = true;
                    }
                    else
                    {
                        Mensaje = "No se pudo actualizar la imagen";
                    }
                    

                    
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        //Eliminación Definitiva mientras no esté relacionado a una venta
        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarProducto", oconexion);
                    cmd.Parameters.AddWithValue("IdProducto", id);
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

        
    }
}
