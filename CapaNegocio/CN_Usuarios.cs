using CapaDatos;
using CapaEntidad;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace CapaNegocio
{
    public class CN_Usuarios
    {
        private CD_Usuarios objCapaDato = new CD_Usuarios();

        public List<Usuario> Listar()
        {
            return objCapaDato.Listar();
        }

        public int Registrar(Usuario obj, out string Mensaje)
        {
            Mensaje = string.Empty;

            //Validaciones a campos nulos o vacios.
            if(string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El Nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellido) || string.IsNullOrWhiteSpace(obj.Apellido))
            {
                Mensaje = "El Apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Email) || string.IsNullOrWhiteSpace(obj.Email))
            {
                Mensaje = "El Correo del usuario no puede ser vacio";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {
                //string clave = CN_Recursos.GenerarClave();
                //string asunto = "Creación de la Cuenta";
                //string mensajeCorreo = "<h3>Su cuenta fue creada correctamente</h3></br><p>Su contraseña para acceder es: !clave</p>";
                //mensajeCorreo = mensajeCorreo.Replace("!clave", clave);

                //bool respuesta = CN_Recursos.EnviarCorreo(obj.Email, asunto, mensajeCorreo);

                //if (respuesta) 
                //{
                    obj.Clave = CN_Recursos.ConvertSha256(obj.Clave);
                    return objCapaDato.Registrar(obj, out Mensaje);
                //}
                //else
                //{
                //    Mensaje = "No se puede enviar el correo";
                //    return 0;
                //}


            }
            else 
            {
                return 0;
            }
        }

        public bool Editar(Usuario obj,bool adm ,out string Mensaje)
        {
            Mensaje = string.Empty;

            //Validaciones a campos nulos o vacios.
            if (string.IsNullOrEmpty(obj.Nombre) || string.IsNullOrWhiteSpace(obj.Nombre))
            {
                Mensaje = "El Nombre del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Apellido) || string.IsNullOrWhiteSpace(obj.Apellido))
            {
                Mensaje = "El Apellido del usuario no puede ser vacio";
            }
            else if (string.IsNullOrEmpty(obj.Email) || string.IsNullOrWhiteSpace(obj.Email))
            {
                Mensaje = "El Correo del usuario no puede ser vacio";
            }


            if (string.IsNullOrEmpty(Mensaje))
            {
                if (adm)
                {
                    return objCapaDato.EditarAdmin(obj, out Mensaje);
                }
                else
                    return objCapaDato.Editar(obj, out Mensaje);
            }
            else
            {
                return false;
            }

        }

        public bool Eliminar(int id, out string Mensaje)
        {
            return objCapaDato.Eliminar(id, out Mensaje);
        }


        public bool CambiarClave(int idUsuario, string nuevaClave, out string Mensaje)
        {
            return objCapaDato.CambiarClave(idUsuario, nuevaClave, out Mensaje);
        }

        public bool ReestablecerClave(int idUsuario, string correo, out string Mensaje)
        {
            try
            {
                Mensaje = string.Empty;
                string nuevaClave = CN_Recursos.GenerarClave();
                bool resultado = objCapaDato.ReestablecerClave(idUsuario, CN_Recursos.ConvertSha256(nuevaClave), out Mensaje);

                if (resultado)
                {
                    string asunto = "Contraseña Reestablecida";
                    string mensajeCorreo = "<h3>Su cuenta fue reestablecida correctamente</h3></br><p>Su contraseña para acceder ahora es: !clave</p>";
                    mensajeCorreo = mensajeCorreo.Replace("!clave", nuevaClave);


                    bool respuesta = CN_Recursos.EnviarCorreo(correo, asunto, mensajeCorreo);

                    if (respuesta)
                    {
                        return true;
                    }
                    else
                    {
                        Mensaje = "No se puede enviar el correo";
                        return false;
                    }
                }
                else
                {
                    Mensaje = "No se pued reestablecer la contraseña";
                    return false;
                }


            }
            catch (Exception ex)
            {
                Mensaje = "No se puedo realizar la operación";
                return false;
            }




        }
    }
}
