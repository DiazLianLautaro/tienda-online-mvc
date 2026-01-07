using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CapaEntidad;
using System.Data.SqlClient;
using System.Data;

namespace CapaDatos
{
    public class CD_Usuarios
    {
        public List<Usuario> Listar() 
        {
            List<Usuario> list = new List<Usuario>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select IdUsuario, Nombre, Apellido, Email, Clave, Telefono, Reestablecer, Activo, EsAdmin from USUARIO";

                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while(dr.Read())
                        {

                            list.Add(
                                new Usuario()
                                {
                                    IdUsuario = Convert.ToInt32(dr["IdUsuario"]),
                                    Nombre = dr["Nombre"].ToString(),
                                    Apellido = dr["Apellido"].ToString(),
                                    Email = dr["Email"].ToString(),
                                    Clave = dr["Clave"].ToString(),
                                    Telefono = dr["Telefono"].ToString(),
                                    Reestablecer = Convert.ToBoolean(dr["Reestablecer"]),
                                    Activo = Convert.ToBoolean(dr["Activo"]),
                                    EsAdmin = Convert.ToBoolean(dr["EsAdmin"])
                                }

                                );
                        }
                    }
                }
            }
            catch 
            {
                list = new List<Usuario>();
            }
            return list;
        }

        public int Registrar(Usuario obj, out string Mensaje)
        {
            int idautogenerado = 0;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", oconexion);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Apellido", obj.Apellido);
                    cmd.Parameters.AddWithValue("Email", obj.Email);
                    cmd.Parameters.AddWithValue("Clave", obj.Clave);
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

        public bool Editar (Usuario obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarUsuario", oconexion);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Apellido", obj.Apellido);
                    cmd.Parameters.AddWithValue("Email", obj.Email);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.AddWithValue("Telefono", obj.Telefono);
                    cmd.Parameters.AddWithValue("EsAdmin", obj.EsAdmin);
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

        public bool EditarAdmin(Usuario obj, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("sp_EditarUsuarioAdmin", oconexion);
                    cmd.Parameters.AddWithValue("IdUsuario", obj.IdUsuario);
                    cmd.Parameters.AddWithValue("Nombre", obj.Nombre);
                    cmd.Parameters.AddWithValue("Apellido", obj.Apellido);
                    cmd.Parameters.AddWithValue("Email", obj.Email);
                    cmd.Parameters.AddWithValue("Activo", obj.Activo);
                    cmd.Parameters.AddWithValue("EsAdmin", obj.EsAdmin);
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
                    SqlCommand cmd = new SqlCommand("delete top (1) from USUARIO where IdUsuario = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public bool CambiarClave(int idUsuario,string nuevaClave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update USUARIO set clave = @nuevaClave, reestablecer = 0 where idUsuario = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", idUsuario);
                    cmd.Parameters.AddWithValue("@nuevaClave", nuevaClave);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                resultado = false;
                Mensaje = ex.Message;
            }
            return resultado;
        }

        public bool ReestablecerClave(int idUsuario, string Clave, out string Mensaje)
        {
            bool resultado = false;
            Mensaje = string.Empty;
            try
            {
                using (SqlConnection oconexion = new SqlConnection(Conexion.cn))
                {
                    SqlCommand cmd = new SqlCommand("update USUARIO set clave = @Clave, reestablecer = 1 where idUsuario = @id", oconexion);
                    cmd.Parameters.AddWithValue("@id", idUsuario);
                    cmd.Parameters.AddWithValue("@Clave", Clave);
                    cmd.CommandType = CommandType.Text;
                    oconexion.Open();
                    resultado = cmd.ExecuteNonQuery() > 0 ? true : false;
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
