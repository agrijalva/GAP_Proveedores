﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class ContactoRequestDTO: RequestBaseDTO
    {
        public int IdProveedor { get; set; }
        public ProveedorContactoDTO Contacto { get; set; }
    }
}
