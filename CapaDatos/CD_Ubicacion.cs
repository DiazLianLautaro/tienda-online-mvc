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
    public class CD_Ubicacion
    {
        public List<Departamento> ObtenerDepartamento()
        {
            List<Departamento> list = new List<Departamento>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from DEPARTAMENTO";

                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.CommandType = CommandType.Text;

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            list.Add(
                                new Departamento()
                                {
                                    IdDepartamento = dr["IdDepartamento"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString()
                                }

                                );
                        }
                    }
                }
            }
            catch
            {
                list = new List<Departamento>();
            }
            return list;
        }

        public List<Provincia> ObtenerProvincia(string IdDepartamento)
        {
            List<Provincia> list = new List<Provincia>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from PROVINCIA where IdDepartamento = @iddepartamento";

                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.Parameters.AddWithValue("@iddepartamento", IdDepartamento);
                    cmd.CommandType = CommandType.Text;
                    

                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            list.Add(
                                new Provincia()
                                {
                                    IdProvincia = dr["IdProvincia"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString()
                                }

                                );
                        }
                    }
                }
            }
            catch
            {
                list = new List<Provincia>();
            }
            return list;
        }

        public List<Distrito> ObtenerDistrito(string IdProvincia ,string IdDepartamento)
        {
            List<Distrito> list = new List<Distrito>();

            try
            {
                using (SqlConnection oConexion = new SqlConnection(Conexion.cn))
                {
                    string query = "select * from DISTRITO where IdProvincia = @idprovincia and IdDepartamento = @iddepartamento";

                    SqlCommand cmd = new SqlCommand(query, oConexion);
                    cmd.Parameters.AddWithValue("@idprovincia", IdProvincia);
                    cmd.Parameters.AddWithValue("@iddepartamento", IdDepartamento);
                    cmd.CommandType = CommandType.Text;


                    oConexion.Open();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {

                            list.Add(
                                new Distrito()
                                {
                                    IdDistrito = dr["IdDistrito"].ToString(),
                                    Descripcion = dr["Descripcion"].ToString()
                                }

                                );
                        }
                    }
                }
            }
            catch
            {
                list = new List<Distrito>();
            }
            return list;
        }
    }
}
