using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities
{
    public class ResetPasswordRequestDTO: RequestBaseDTO
    {
        public bool EsSolicitud { get; set; }
        public UsuarioDTO Usuario { get; set; }
    }
}
