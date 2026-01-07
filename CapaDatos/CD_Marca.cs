using CapaEntidad;
using System;
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
    public class CD_Marca
    {
        public List<Marca> Listar()
        {
            List<Marca> list = new List<Marca>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select Id, Descripcion, Activo From Marca";

                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            list.Add(
                                new Marca()
                                {
                                    IdMarca = Convert.ToInt32(dr["Id"]),
                                    Descripcion = dr["Descripcion"].ToString(),
                                    Activo = Convert.ToBoolean(dr["Activo"])
                                }

                                );
                        }
                    }
                }
            }
            catch
            {
                list = new List<Marca>();
            }
            return list;
        }




        public int Registrar(Marca obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarMarca", oconexion);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
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



        public bool Editar(Marca obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarMarca", oconexion);
                    cmd.Parameters.AddWithValue("IdMarca", obj.IdMarca);
                    cmd.Parameters.AddWithValue("Descripcion", obj.Descripcion);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
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

        public bool Eliminar(int id, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EliminarMarca", oconexion);
                    cmd.Parameters.AddWithValue("IdMarca", id);
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


        public List<Marca> ListarMarcaPorCategoria(int idcategoria)
        {
            List<Marca> list = new List<Marca>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    StringBuilder sb = new StringBuilder();

                    sb.AppendLine("select distinct m.Id, m.Descripcion from PRODUCTO p");
                    sb.AppendLine("inner join CATEGORIA c on c.Id = p.CategoriaId");
                    sb.AppendLine("inner join MARCA m on m.Id = p.MarcaId and m.Activo = 1");
                    sb.AppendLine("where c.Id = iif(@idcategoria = 0, c.Id, @idcategoria)");


                    SqlCommand cmd = new SqlCommand(sb.ToString(), oConexion);
                    cmd.Parameters.AddWithValue("@idcategoria", idcategoria);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            list.Add(
                                new Marca()
                                {
                                    IdMarca = Convert.ToInt32(dr["Id"]),
                                    Descripcion = dr["Descripcion"].ToString()
                                }

                                );
                        }
                    }
                }
            }
            catch
            {
                list = new List<Marca>();
            }
            return list;
        }
    }
}
