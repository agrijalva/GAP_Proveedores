using Eprocurement.Compras.App_Start;
using Eprocurement.Compras.Business;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using EPROCUREMENT.GAPPROVEEDOR.Entities.Seguridad;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using System;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace Eprocurement.Compras.Service
{
    public class AuthenticationService
    {
        private string GapActiveDirectory = ConfigurationManager.AppSettings["GapActiveDirectory"].ToString();

        private readonly IAuthenticationManager authenticationManager;

        public AuthenticationService(IAuthenticationManager authenticationManager)
        {
            this.authenticationManager = authenticationManager;
        }

        public AuthenticationResult SignIn(String username, String password)
        {
            string error = "";

            try
            {
                using (PrincipalContext principalContext = new PrincipalContext(ContextType.Domain, GapActiveDirectory, username, password))
                {
                    var userPrincipal = UserPrincipal.FindByIdentity(principalContext, IdentityType.SamAccountName, username);
                    var groupsUser = userPrincipal.GetGroups(principalContext);

                    if (groupsUser.Any(c => c.Name.Equals("Compras SIAP")))
                    {
                        if (userPrincipal.IsAccountLockedOut())
                        {
                            error = "Acceso Invalido 102";
                        }
                        else
                        {
                            UsuarioDTO usuarioDTO = new BusinessLogic().LoginUsuarioItem(username, password);
                            if (usuarioDTO != null)
                            {
                                var identity = CreateIdentity(username, usuarioDTO.IdUsuarioRol.ToString(), JsonConvert.SerializeObject(usuarioDTO));
                                authenticationManager.SignOut(LoginAuthentication.ApplicationCookie);
                                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
                            }
                            else
                            {
                                UsuarioDTO usuario = new BusinessLogic().AddUsuarioItem(username, password, 2);
                                if (usuarioDTO != null)
                                {
                                    UsuarioDTO u = new BusinessLogic().LoginUsuarioItem(username, password);
                                    if (u != null)
                                    {
                                        var identity = CreateIdentity(username, u.IdUsuarioRol.ToString(), JsonConvert.SerializeObject(u));
                                        authenticationManager.SignOut(LoginAuthentication.ApplicationCookie);
                                        authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
                                    }
                                    else
                                    {
                                        error = "Acceso Invalido, Usuario local no existente";                                        
                                    }
                                }
                                else
                                {
                                    error = "Acceso Invalido, Usuario local no existente";
                                }
                            }
                        }
                    }
                    else
                    {
                        if (userPrincipal.IsAccountLockedOut())
                        {
                            error = "Acceso Invalido 102";
                        }
                        else
                        {
                            UsuarioDTO usuarioDTO = new BusinessLogic().LoginUsuarioItem(username, password);
                            if (usuarioDTO != null)
                            {
                                var identity = CreateIdentity(username, usuarioDTO.IdUsuarioRol.ToString(), JsonConvert.SerializeObject(usuarioDTO));
                                authenticationManager.SignOut(LoginAuthentication.ApplicationCookie);
                                authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
                            }
                            else
                            {
                                UsuarioDTO usuario = new BusinessLogic().AddUsuarioItem(username, password, 3);
                                if (usuarioDTO != null)
                                {
                                    UsuarioDTO u = new BusinessLogic().LoginUsuarioItem(username, password);
                                    if (u != null)
                                    {
                                        var identity = CreateIdentity(username, u.IdUsuarioRol.ToString(), JsonConvert.SerializeObject(u));
                                        authenticationManager.SignOut(LoginAuthentication.ApplicationCookie);
                                        authenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = false }, identity);
                                    }
                                    else
                                    {
                                        error = "Acceso Invalido, Usuario local no existente";
                                    }
                                }
                                else
                                {
                                    error = "Acceso Invalido, Usuario local no existente";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                error = "Acceso Invalido: No se pudo validar con LDAP";
            }
            return new AuthenticationResult(error);
        }

        private ClaimsIdentity CreateIdentity(string userPrincipal, string rol, string propiedadesUsuario)
        {
            var identity = new ClaimsIdentity(LoginAuthentication.ApplicationCookie, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            identity.AddClaim(new Claim(ClaimTypes.Name, userPrincipal));
            identity.AddClaim(new Claim(ClaimTypes.Role, rol));
            identity.AddClaim(new Claim(ClaimTypes.UserData, propiedadesUsuario));
            identity.AddClaim(new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", userPrincipal));
            identity.AddClaim(new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", userPrincipal));

            return identity;
        }
    }
}