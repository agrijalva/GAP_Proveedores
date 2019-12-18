using EPROCUREMENT.GAPPROVEEDOR.Data;
using EPROCUREMENT.GAPPROVEEDOR.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPROCUREMENT.GAPPROVEEDOR.Business.OrdenCompra
{
    public class HandlerOrdenCompra
    {
        private readonly OrdenCompraData ordenCompraData;

        public HandlerOrdenCompra()
        {
            ordenCompraData = new OrdenCompraData();
        }

        public OrdenCompraResponseDTO GetOrdenCompraList(OrdenCompraRequestDTO request)
        {
            var response = ordenCompraData.GetOrdenCompraList(request);

            if (!response.Success)
            {
                response.ErrorList = new List<ErrorDTO> { new ErrorDTO { Codigo = "", Mensaje = string.Format("No fue posible recuperar datos disponibles o no se encontro alguna solicitud en proceso") } };
            }
            return response;
        }
    }
}
