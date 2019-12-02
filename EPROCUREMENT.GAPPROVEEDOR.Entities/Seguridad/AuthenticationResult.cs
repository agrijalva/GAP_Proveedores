using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Entities.Seguridad
{
    public class AuthenticationResult
    {
        public AuthenticationResult(string errorMessage = null)
        {
            ErrorMessage = errorMessage;
        }

        public String ErrorMessage { get; private set; }
        public Boolean IsSuccess => String.IsNullOrEmpty(ErrorMessage);
    }
}
