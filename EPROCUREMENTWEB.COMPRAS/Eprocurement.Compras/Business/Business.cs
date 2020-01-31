using Eprocurement.Compras.ServiceReference1;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Eprocurement.Compras.Business
{
    public class Business
    {
        public void CreateVendor()
        {
            DXS_VendorPostServiceClient client = new DXS_VendorPostServiceClient();
            CallContext context = new CallContext();
            VendorResponse response;
            VendorAddressResponse addressResponse;
            VendorContactResponse contactResponse;
            List<VendorAddressContract> vendorAddresList = new List<VendorAddressContract>();
            List<VendorContactContract> vendorContactList = new List<VendorContactContract>();

            context.Company = "GAP";
            //client.ClientCredentials.Windows.ClientCredential.UserName = "compras1";
            //client.ClientCredentials.Windows.ClientCredential.Password = "cU3nt4zp4rApR0v3d0R1";

            client.ClientCredentials.Windows.ClientCredential.UserName = usuarioInfo.NombreUsuario;
            client.ClientCredentials.Windows.ClientCredential.Password = usuarioInfo.Password;

            //Creating vendor

            VendorContract vendor = new VendorContract();

            vendor.DataAreaId = "gap";

            vendor.AccountIdreference = "Ref13";

            vendor.DirPartyName = "Testing 13";

            vendor.GroupId = "01";

            vendor.OperationTypeMx = VendorOperationType_MX.Other;

            vendor.VendorTypeMx = VendorType_MX.ForeignVendor;

            vendor.TaxGroup = "01";

            vendor.LanguageId = "es-mx";

            vendor.PaymMode = "01";

            vendor.PaymTermId = "01";

            vendor.CurrencyCode = "MXN";



            //Creating adress vendor

            VendorAddressContract vendorAddress = new VendorAddressContract();

            vendorAddress.Description = "Headquarters";

            vendorAddress.CountryId = "MEX";

            vendorAddress.ZipCode = "44100";

            vendorAddress.State = "JAL";

            vendorAddress.City = "Guadalajara";

            vendorAddress.IsPrimary = NoYes.Yes;

            vendorAddresList.Add(vendorAddress);

            vendor.parmAddresList = vendorAddresList.ToArray();



            //Creating contact info vendor

            //email

            VendorContactContract vendorContact = new VendorContactContract();

            vendorContact.Description = "Correo headquarters";

            vendorContact.Locator = "correo@mail.com";

            vendorContact.Type = LogisticsElectronicAddressMethodType.Email;

            vendorContact.IsPrimary = NoYes.Yes;

            vendorContactList.Add(vendorContact);

            //phone

            VendorContactContract vendorContact2 = new VendorContactContract();

            vendorContact2.Description = "Telefono headquarters";

            vendorContact2.Locator = "3312345678";

            vendorContact2.Type = LogisticsElectronicAddressMethodType.Phone;

            vendorContact2.IsPrimary = NoYes.Yes;

            vendorContactList.Add(vendorContact2);

            vendor.parmContactList = vendorContactList.ToArray();

            //creating response

            response = client.createVendor(context, vendor);
            Console.WriteLine(response.ResponseMessageStr);
            Console.WriteLine(response.AccountNum);

            //New addres from a already created vendor



            vendorAddress.AccountNum = response.AccountNum; //this is mandatory when you create an address from an existing vendor

            vendorAddress.Company = vendor.DataAreaId; //this is mandatory when you create an address from an existing vendor

            vendorAddress.Description = "Headquarters 2";


            addressResponse = client.createAddressVendor(context, vendorAddress);
            Console.WriteLine(addressResponse.ResponseMessageStr);
            Console.WriteLine(addressResponse.PartyLocation);

            //New contact info for exixting vendor

            vendorContact.AccountNum = response.AccountNum; //this is mandatory when you create an contact info from an existing vendor

            vendorContact.Company = vendor.DataAreaId; //this is mandatory when you create an contact info from an existing vendor

            vendorContact.Description = "Headquarters info";

            vendorContact.Type = LogisticsElectronicAddressMethodType.Phone;

            vendorContact.IsPrimary = NoYes.Yes;

            vendorContact.Locator = "3312345678";


            contactResponse = client.createContactVendor(context, vendorContact);
            Console.WriteLine(contactResponse.ResponseMessageStr);
            Console.WriteLine(contactResponse.PartyLocation);
        }

        internal bool CreateVendor(UsuarioDTO usuarioInfo, ProveedorDTO proveedor)
        {
            bool respuesta = false;
            DXS_VendorPostServiceClient client = new DXS_VendorPostServiceClient();
            CallContext context = new CallContext();
            VendorResponse response;
            VendorAddressResponse addressResponse;
            VendorContactResponse contactResponse;
            List<VendorAddressContract> vendorAddresList = new List<VendorAddressContract>();
            List<VendorContactContract> vendorContactList = new List<VendorContactContract>();

            context.Company = "GAP";
            //client.ClientCredentials.Windows.ClientCredential.UserName = "compras1";
            //client.ClientCredentials.Windows.ClientCredential.Password = "cU3nt4zp4rApR0v3d0R1";

            client.ClientCredentials.Windows.ClientCredential.UserName = usuarioInfo.NombreUsuario;
            client.ClientCredentials.Windows.ClientCredential.Password = usuarioInfo.Password;

            //Creating vendor

            VendorContract vendor = new VendorContract();

            vendor.DataAreaId = "gap";

            vendor.AccountIdreference = "Ref13";

            vendor.DirPartyName = "Testing 13";

            vendor.GroupId = "01";

            vendor.OperationTypeMx = VendorOperationType_MX.Other;

            vendor.VendorTypeMx = VendorType_MX.ForeignVendor;

            vendor.TaxGroup = "01";

            vendor.LanguageId = "es-mx";

            vendor.PaymMode = "01";

            vendor.PaymTermId = "01";

            vendor.CurrencyCode = "MXN";



            //Creating adress vendor

            VendorAddressContract vendorAddress = new VendorAddressContract();

            vendorAddress.Description = "Dirección";

            vendorAddress.CountryId = "MEX";

            vendorAddress.ZipCode = proveedor.Direccion.CodigoPostal;

            vendorAddress.State = proveedor.Direccion.Estado;

            vendorAddress.City = proveedor.Direccion.Municipio;

            vendorAddress.IsPrimary = NoYes.Yes;

            vendorAddresList.Add(vendorAddress);

            vendor.parmAddresList = vendorAddresList.ToArray();



            //Creating contact info vendor

            //email

            VendorContactContract vendorContact = new VendorContactContract();

            vendorContact.Description = "Correo";

            vendorContact.Locator = proveedor.Contacto.Email;

            vendorContact.Type = LogisticsElectronicAddressMethodType.Email;

            vendorContact.IsPrimary = NoYes.Yes;

            vendorContactList.Add(vendorContact);

            //phone

            VendorContactContract vendorContact2 = new VendorContactContract();

            vendorContact2.Description = "Telefono";

            vendorContact2.Locator = proveedor.Contacto.TelefonoMovil;

            vendorContact2.Type = LogisticsElectronicAddressMethodType.Phone;

            vendorContact2.IsPrimary = NoYes.Yes;

            vendorContactList.Add(vendorContact2);

            vendor.parmContactList = vendorContactList.ToArray();

            //creating response

            response = client.createVendor(context, vendor);
            Console.WriteLine(response.ResponseMessageStr);
            Console.WriteLine(response.AccountNum);

            //New addres from a already created vendor



            vendorAddress.AccountNum = response.AccountNum; //this is mandatory when you create an address from an existing vendor

            vendorAddress.Company = vendor.DataAreaId; //this is mandatory when you create an address from an existing vendor

            vendorAddress.Description = "Headquarters 2";


            addressResponse = client.createAddressVendor(context, vendorAddress);
            Console.WriteLine(addressResponse.ResponseMessageStr);
            Console.WriteLine(addressResponse.PartyLocation);

            //New contact info for exixting vendor

            vendorContact.AccountNum = response.AccountNum; //this is mandatory when you create an contact info from an existing vendor

            vendorContact.Company = vendor.DataAreaId; //this is mandatory when you create an contact info from an existing vendor

            vendorContact.Description = "Headquarters info";

            vendorContact.Type = LogisticsElectronicAddressMethodType.Phone;

            vendorContact.IsPrimary = NoYes.Yes;

            vendorContact.Locator = "3312345678";


            contactResponse = client.createContactVendor(context, vendorContact);
            Console.WriteLine(contactResponse.ResponseMessageStr);
            Console.WriteLine(contactResponse.PartyLocation);

            return response.AccountNum != null && response.AccountNum.Length > 0;
        }
    }
}