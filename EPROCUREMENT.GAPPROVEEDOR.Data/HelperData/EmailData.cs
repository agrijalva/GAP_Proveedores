using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EPROCUREMENT.GAPPROVEEDOR.Entities;

namespace EPROCUREMENT.GAPPROVEEDOR.Data
{
    public class EmailData
    {
        /// <summary>
        /// Envia el correo electronico a los destinatarios
        /// </summary>
        /// <param name="emailEntity">Representa la información del correo a enviar</param>
        public void EnviarEmailRegistro(int idProveedor, int idUsuario, int idEstatus, string comentarios)
        {
            string EmailOrigen = "GAPProveedoresTest@gmail.com";
            EmailDTO emailEntidad = new EmailDTO
            {
                Origin = EmailOrigen,
                Subject = "Estatus de Registro",
                Html = true,
                RecipientsList = new List<DireccionEmailDTO>(),
                Prioridad = EmailPrioridadDTO.Normal
            };

            ProveedorUsuarioDTO proveedorUsuario = null;
            proveedorUsuario = new ProveedorData().GetProvedorUsuarioItem(idProveedor, idUsuario);
            if (idEstatus == 4)
            {
                emailEntidad.Message = GetBodyAprobar(proveedorUsuario);
            } 
            else if(idEstatus == 2)
            {
                emailEntidad.Message = GetBodyRechazar(proveedorUsuario, comentarios);
            }
            emailEntidad.RecipientsList.Add(new DireccionEmailDTO { Address = proveedorUsuario.Email, DisplayName = proveedorUsuario.NombreEmpresa, UserIdentifier = 1 });             
            var mailMessage = ObtenerMensajeEmail(emailEntidad);
            var cliente = ObtenerClienteSmtp();
            try
            {
                cliente.Send(mailMessage);
            }
            catch (SmtpFailedRecipientException smtpFailedException)
            {
            }
        }

        //public void EnviarEmailRecuperar(int idProveedor, int idUsuario, int idEstatus, string comentarios)
        //{
        //    string EmailOrigen = "GAPProveedoresTest@gmail.com";
        //    EmailDTO emailEntidad = new EmailDTO
        //    {
        //        Origin = EmailOrigen,
        //        Subject = "Estatus de Registro",
        //        Html = true,
        //        RecipientsList = new List<DireccionEmailDTO>(),
        //        Prioridad = EmailPrioridadDTO.Normal
        //    };

        //    ProveedorUsuarioDTO proveedorUsuario = null;
        //    proveedorUsuario = new ProveedorData().GetProvedorUsuarioItem(idProveedor, idUsuario);
        //    if (idEstatus == 4)
        //    {
        //        emailEntidad.Message = GetBodyRecuperarPassword(proveedorUsuario);
        //    }
        //    else if (idEstatus == 2)
        //    {
        //        emailEntidad.Message = GetBodyRechazar(proveedorUsuario, comentarios);
        //    }
        //    emailEntidad.RecipientsList.Add(new DireccionEmailDTO { Address = proveedorUsuario.Email, DisplayName = proveedorUsuario.NombreEmpresa, UserIdentifier = 1 });
        //    var mailMessage = ObtenerMensajeEmail(emailEntidad);
        //    var cliente = ObtenerClienteSmtp();
        //    try
        //    {
        //        cliente.Send(mailMessage);
        //    }
        //    catch (SmtpFailedRecipientException smtpFailedException)
        //    {
        //    }
        //}
        public void EnviarEmailRecuperarPassword(UsuarioDTO usuario)
        {
            //string EmailOrigen = "GAPProveedoresTest@gmail.com";
            EmailDTO emailEntidad = new EmailDTO
            {
                Origin = ConfigurationManager.AppSettings["EmailOrigen"],
                Subject = "Recuperación de Contraseña",
                Html = true,
                RecipientsList = new List<DireccionEmailDTO>(),
                Prioridad = EmailPrioridadDTO.Normal
            };

            ProveedorUsuarioDTO proveedorUsuario = null;
            proveedorUsuario = new ProveedorData().GetProvedorUsuarioPorRFC(usuario.Email);
            emailEntidad.Message = GetBodyRecuperarPassword(proveedorUsuario, usuario.Token);
            emailEntidad.RecipientsList.Add(new DireccionEmailDTO { Address = proveedorUsuario.Email, DisplayName = proveedorUsuario.NombreEmpresa, UserIdentifier = 1 });
            var mailMessage = ObtenerMensajeEmail(emailEntidad);
            var cliente = ObtenerClienteSmtp();
            try
            {
                cliente.Send(mailMessage);
            }
            catch (SmtpFailedRecipientException smtpFailedException)
            {
            }
        }

        /// <summary>
        /// Prepara el detalle del email para logueo del proveedor
        /// </summary>
        /// <param name="nombreCompañia">Nombre de la compañia</param>
        /// <param name="urlLogin">Url para el logueo</param>
        /// <returns>La estructura del correo</returns>
        private string GetBodyAprobar(ProveedorUsuarioDTO proveedorUsuario)
        {
            string url = ConfigurationManager.AppSettings["UrlLogin"];
            string hrefUrl = "<a href='" + url + "'>Click para ingresar</a>";
            string layoutName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, App_GlobalResources.ResourceConstants.EmailLayout, "UssuarioAprobado.htm");
            string message = File.ReadAllText(layoutName);
            var details = new StringBuilder();
            message = message.Replace("<!--NombreCompania-->", proveedorUsuario.NombreEmpresa);
            message = message.Replace("<!--RFCCompania-->", proveedorUsuario.RFC);
            message = message.Replace("<!--ContaseñaCompania-->", proveedorUsuario.Password);
            message = message.Replace("<!--urlAction-->", hrefUrl);
            return message;
        }
        private string GetBodyRechazar(ProveedorUsuarioDTO proveedorUsuario, string comentarios)
        {
            string layoutName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, App_GlobalResources.ResourceConstants.EmailLayout, "UsuarioRechazado.htm");
            string message = File.ReadAllText(layoutName);
            var details = new StringBuilder();
            message = message.Replace("<!--NombreCompania-->", "Proveedor");
            message = message.Replace("<!--observaciones-->", comentarios);
            return message;
        }

        private string GetBodyRecuperarPassword(ProveedorUsuarioDTO proveedorUsuario, string tokenRecovery)
        {
            string url = ConfigurationManager.AppSettings["UrlrRecoveryPassword"] + tokenRecovery;
            string hrefUrl = "<a href='" + url + "'>Click para recuperar</a>";
            string layoutName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, App_GlobalResources.ResourceConstants.EmailLayout, "RecuperarPassword.htm");
            string message = File.ReadAllText(layoutName);
            var details = new StringBuilder();
            message = message.Replace("<!--NombreCompania-->", proveedorUsuario.NombreEmpresa);
            message = message.Replace("<!--urlAction-->", hrefUrl);
            return message;
        }

        /// <summary>
        /// Obtiene una instancia de la entidad SmtpClient
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        /// <returns></returns>
        private SmtpClient ObtenerClienteSmtp()
        {
            string EmailOrigen = ConfigurationManager.AppSettings["EmailOrigen"];//"GAPProveedoresTest@gmail.com";
            string password = "Kal08Test";
            SmtpClient oSmtpClient = new SmtpClient("smtp.gmail.com");
            oSmtpClient.EnableSsl = true;
            oSmtpClient.UseDefaultCredentials = false;
            oSmtpClient.Port = Convert.ToInt32(ConfigurationManager.AppSettings["PORT"]);
            oSmtpClient.Credentials = new System.Net.NetworkCredential(EmailOrigen, password);
            return oSmtpClient;
            //return new SmtpClient
            //{
            //    Host = ConfigurationManager.AppSettings["SERVER"],
            //    Port = Convert.ToInt32(ConfigurationManager.AppSettings["PORT"]),
            //    Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EmailOrigen"], "Kal08Test"),
            //    EnableSsl = true,
            //    UseDefaultCredentials = false
            //};
            //return new SmtpClient
            //{
            //    Host = ConfigurationManager.AppSettings["SERVER"],
            //    Port = Convert.ToInt32(ConfigurationManager.AppSettings["PORT"]),
            //    DeliveryMethod = SmtpDeliveryMethod.Network
            //};
        }

        /// <summary>
        /// Obtiene una instancia de la entidad MailMessage
        /// </summary>
        /// <param name="entidad"></param>
        /// <returns></returns>
        private MailMessage ObtenerMensajeEmail(EmailDTO entidad)
        {
            var mailMessage = new MailMessage();
            var mensaje = new StringBuilder();
            try
            {
                mailMessage.From = new MailAddress(ConfigurationManager.AppSettings["EmailOrigen"], ConfigurationManager.AppSettings["TituloEmail"]);
                mailMessage.Subject = entidad.Subject;
                mailMessage.Body = entidad.Message;
                mailMessage.IsBodyHtml = entidad.Html;

                if (entidad.RecipientsList != null && entidad.RecipientsList.Count > 0)
                {
                    foreach (var address in entidad.RecipientsList.Where(address => !string.IsNullOrWhiteSpace(address.Address)))
                    {
                        if (EsEmailValido(address.Address))
                        {
                            mailMessage.To.Add(new MailAddress(address.Address, address.DisplayName));
                        }
                    }
                }

                switch (entidad.Prioridad)
                {
                    case EmailPrioridadDTO.High:
                        mailMessage.Priority = MailPriority.High;
                        break;
                    case EmailPrioridadDTO.Normal:
                        mailMessage.Priority = MailPriority.Normal;
                        break;
                    case EmailPrioridadDTO.Low:
                        mailMessage.Priority = MailPriority.Low;
                        break;
                    default:
                        mailMessage.Priority = MailPriority.Normal;
                        break;
                }
            }
            catch (Exception ex)
            {
            }

            return mailMessage;
        }

        /// <summary>
        /// Expresion para validar el correo electronico
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public bool EsEmailValido(string email)
        {
            // Retorna true si es un email valido.
            return Regex.IsMatch(email, @"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        }

        
    }
}
