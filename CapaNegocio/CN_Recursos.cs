using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net.Mail;
using System.IO;
using System.Net;

namespace CapaNegocio
{
    public class CN_Recursos
    {
        //retorna clave de 6 dígitos aleatoria
        public static string GenerarClave()
        {
            string Clave = Guid.NewGuid().ToString("N").Substring(0,6);
            return Clave;
        }


        //encriptación de TEXTO en SHA256
        public static string ConvertSha256(string texto)
        {
            StringBuilder Sb = new StringBuilder();
            //USAR LA REFERENCIA DE "System.Security.Cryptography"
            using (SHA256 hash = SHA256.Create())
            {
                Encoding enc = Encoding.UTF8;
                byte[] result = hash.ComputeHash(enc.GetBytes(texto));

                foreach (byte b in result) 
                {
                    Sb.Append(b.ToString("x2"));
                }
            }
            return Sb.ToString();
        }

        public static bool EnviarCorreo(string correo, string asunto, string mensaje)
        {
            bool resultado = false;

            try
            {
                using (MailMessage mail = new MailMessage())
                using (var smtp = new SmtpClient())
                {

                    //programpruebas47@gmail.com
                    mail.From = new MailAddress("norespoder47@demo.com");  //desde quien (correo ficticio sin dominio)
                    mail.To.Add(correo);  //a quien
                    mail.Subject = asunto;
                    mail.Body = mensaje;
                    mail.IsBodyHtml = true;


                    smtp.UseDefaultCredentials = false; //IMPORTANTE debe ir antes de las credenciales 
                    smtp.Host = "sandbox.smtp.mailtrap.io";
                    smtp.Port = 2525;
                    //smtp.Port = 465;
                    smtp.EnableSsl = true; //Si o si para Mailtrap
                    smtp.Credentials = new NetworkCredential("dc2253c6c3f535", "e8e5991b265b68");

                    smtp.Send(mail);
                }
                ;
                resultado = true;
            }
            catch (Exception ex)
            {
                resultado = false;
            }
            return resultado;
        }

        public static bool EnviarCorreoContacto(string correo, string asunto, string mensaje)
        {
            bool resultado = false;

            try
            {
                using (MailMessage mail = new MailMessage())
                using (var smtp = new SmtpClient())
                {
                    mail.From = new MailAddress(correo);  //desde quien 
                    mail.To.Add("norespoder47@demo.com");                        //a quien (Pendiente: Pedir correo)
                    mail.Subject = asunto;
                    mail.Body = mensaje;
                    mail.IsBodyHtml = true;


                    smtp.UseDefaultCredentials = false; //IMPORTANTE debe ir antes de las credenciales 
                    smtp.Host = "sandbox.smtp.mailtrap.io";
                    smtp.Port = 2525;
                    //smtp.Port = 465;
                    smtp.EnableSsl = true; //Si o si para Mailtrap
                    smtp.Credentials = new NetworkCredential("dc2253c6c3f535", "e8e5991b265b68");

                    smtp.Send(mail);
                }
                
                resultado = true;
            }
            catch (Exception ex)
            {
                resultado = false;
            }
            return resultado;
        }


        public static string ConvertirBase64(string ruta, out bool conversion)
        {
            string textoBase64 = string.Empty;
            conversion = true;

            try
            {
                byte[] bytes = File.ReadAllBytes(ruta);
                textoBase64 = Convert.ToBase64String(bytes);
            }
            catch (Exception)
            {
                conversion = false;
            }

            return textoBase64;
        }
    }
}
