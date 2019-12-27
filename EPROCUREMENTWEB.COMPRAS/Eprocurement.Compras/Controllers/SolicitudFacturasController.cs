using Eprocurement.Compras.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Eprocurement.Compras.Controllers
{
    public class SolicitudFacturasController : Controller
    {
        OrdenDTO orden = null;
        OrdenCompraDTO ordenCompra = null;
        SolicitudLineasDTO solicitudLineas = null;
        SolicitudFacturaDTO solicitud = null;
        private int idProveedor;
        private int idSolicitud = 0;
        List<EstatusSolicitud> estatusList;

        private string stringConnection = "Data Source=172.16.1.48;Initial Catalog=GAPProveedores;Persist Security Info=True;User ID=axconsulta;Password=LE89jf770Fx";
        
        // GET: SolicitudFacturas
        public ActionResult Index(string noOrden)
        {
            noOrden = noOrden ?? "AGU-OC-0000029";
            try
            {
                using (var conexion = new SqlConnection(stringConnection))
                {
                    conexion.Open();

                    //Obtenemos los datos de la proveedor de la ordenn 
                    var cmdOrden = new SqlCommand("SELECT TOP 1 * FROM vPURCHTABLE v" +
                        " left join Proveedor p on v.NoProveedor = p.AXNumeroProveedor" +
                        " WHERE NoOrdenCompra = @orden", conexion);

                    cmdOrden.Parameters.Add("@orden", SqlDbType.VarChar);
                    cmdOrden.Parameters["@orden"].Value = noOrden;
                    
                    using (SqlDataReader reader = cmdOrden.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                orden = new OrdenDTO();
                                idProveedor = int.Parse(reader["IdProveedor"].ToString());
                                orden.NoOrdenCompra = reader["NoOrdenCompra"].ToString();
                                orden.IdProveedor = int.Parse(reader["IdProveedor"].ToString());
                                orden.Descripcion = reader["Descripcion"].ToString();
                                orden.RazonSocial = reader["RazonSocial"].ToString();
                            }
                        }
                    }
                    
                    //Obtenemos los datos de las solicitudes
                    var cmdSolicitudFact = new SqlCommand("select f.IdSolicitudFactura, f.IdProveedor, f.NoRecepcion, f.FechaSolicitud, e.EstatusNombre, f.PDF, f.XML" +
                    " from SolicitudFactura f" + 
                    " left join (select IdSolicitudFactura, IdEstatusSolicitud from HistoricoEstatusSolicitud group by IdSolicitudFactura, IdEstatusSolicitud) as h" + 
                    " on f.IdSolicitudFactura = h.IdSolicitudFactura" +
                    " left join EstatusSolicitud e" + 
                    " on h.IdEstatusSolicitud = e.IdEstatusSolicitud" +
                    " WHERE f.IdProveedor = @proveedorId group by f.IdSolicitudFactura, f.IdProveedor, f.NoRecepcion, f.FechaSolicitud, e.EstatusNombre, f.PDF, f.XML", conexion);

                    cmdSolicitudFact.Parameters.Add("@proveedorId", SqlDbType.Int);
                    cmdSolicitudFact.Parameters["@proveedorId"].Value = idProveedor;

                    orden.solicitud = new List<SolicitudFacturaDTO>();
                    using (SqlDataReader readerS = cmdSolicitudFact.ExecuteReader())
                    {
                        if (readerS.HasRows)
                        { 
                            while (readerS.Read())
                            {
                                if (readerS["IdSolicitudFactura"] != DBNull.Value) {
                                    solicitud = new SolicitudFacturaDTO();
                                    solicitud.IdSolicitudFactura = int.Parse(readerS["IdSolicitudFactura"].ToString());
                                    solicitud.IdProveedor = int.Parse(readerS["IdProveedor"].ToString());
                                    solicitud.NoRecepcion = readerS["NoRecepcion"].ToString();
                                    solicitud.FechaSolicitud = readerS["FechaSolicitud"].ToString();
                                    solicitud.Estatus = readerS["EstatusNombre"].ToString();
                                    solicitud.PDF = readerS["PDF"].ToString();
                                    solicitud.XML = readerS["XML"].ToString();
                                    
                                    orden.solicitud.Add(solicitud);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
            }
            var ordenList = orden;
            return View(orden);
        }


        public ActionResult Create(string noOrden, string accion)
        {
            noOrden = noOrden ?? "AGU-OC-0001029";
            accion = accion ?? "ver";
            bool ExistenRecepciones = false;
            
            List<SolicitudLineasDTO> lineasSolicitud = new List<SolicitudLineasDTO>();
            orden = new OrdenDTO();
            ordenCompra = new OrdenCompraDTO();
            try
            {
                using (var conexion = new SqlConnection(stringConnection))
                {
                    conexion.Open();

                    //Obtenemos los datos de la orden
                    var cmdOrdenCompra = new SqlCommand("SELECT * FROM vOrdenCompra WHERE OrdenCompra = @orden", conexion);
                    cmdOrdenCompra.Parameters.Add("@orden", SqlDbType.VarChar);
                    cmdOrdenCompra.Parameters["@orden"].Value = noOrden;

                    using (SqlDataReader reader = cmdOrdenCompra.ExecuteReader())
                    {

                        while (reader.Read())
                        {
                            ordenCompra = new OrdenCompraDTO();
                            ordenCompra.OrdenCompra = reader["OrdenCompra"].ToString();
                            ordenCompra.Agrupador = reader["Agrupador"].ToString();
                            ordenCompra.CentroCosto = reader["CentroCosto"].ToString();
                            ordenCompra.TipoPresupuesto = reader["TipoPresupuesto"].ToString();
                            ordenCompra.Total = decimal.Parse(reader["Total"].ToString());
                            ordenCompra.AnioPresupuesto1 = decimal.Parse(reader["AnioPresupuesto1"].ToString());
                            ordenCompra.AnioPresupuesto2 = decimal.Parse(reader["AnioPresupuesto2"].ToString());
                            ordenCompra.AnioPresupuesto3 = decimal.Parse(reader["AnioPresupuesto3"].ToString());
                            ordenCompra.AnioPresupuesto4 = decimal.Parse(reader["AnioPresupuesto4"].ToString());
                            ordenCompra.AnioPresupuesto5 = decimal.Parse(reader["AnioPresupuesto5"].ToString());
                        }

                    }

                    //Si ya existe una recepcion de la orden la Obtenemos
                    /* var cmdRecepciones = new SqlCommand("select o.OrdenCompra, r.Linea, o.Categoria 'Producto', o.Producto 'Texto', o.Cantidad, r.CantidadRecibida,r.CantidadTotal 'TotalRecibido'," +
                         " r.CantidadEjecutar 'CantidadXEjecutar',  r.Precio, r.Monto 'MontoNeto', o.Agrupador, o.CentroCosto, o.TipoPresupuesto, r.RECID, r.IATA " +
                         " from Recepciones r " +
                         " left join [GestorCompras].dbo.OrdenCompra o" +
                         " on  r.OrdenCompra = o.OrdenCompra" +
                         " and r.IATA = o.Empresa and r.Linea = o.Linea" +
                         " WHERE o.OrdenCompra = @orden", conexion);*/
                    var cmdRecepciones = new SqlCommand("select o.OrdenCompra, r.Linea, o.Categoria 'Producto', o.Producto 'Texto', o.Cantidad, sum(r.CantidadRecepcion)" +
                        " 'CantidadRecibida',r.CantidadTotal 'TotalRecibido',  (o.Cantidad - sum(r.CantidadRecibida)) 'CantidadXEjecutar', r.Precio, r.Monto 'MontoNeto', o.Agrupador," +
                        " o.CentroCosto, o.TipoPresupuesto, r.RECID, r.IATA from Recepciones r left join [GestorCompras].dbo.OrdenCompra o" +
                        " on  r.OrdenCompra = o.OrdenCompra and r.IATA = o.Empresa and r.Linea = o.Linea" +
                        " WHERE o.OrdenCompra = @orden group by  o.OrdenCompra, r.Linea, o.Categoria, o.Producto, o.Cantidad,r.CantidadTotal," +
                        " r.CantidadEjecutar, r.Precio, r.Monto, o.Agrupador, o.CentroCosto, o.TipoPresupuesto, r.RECID, r.IATA ", conexion);


                    cmdRecepciones.Parameters.Add("@orden", SqlDbType.VarChar);
                    cmdRecepciones.Parameters["@orden"].Value = noOrden;
                    
                    using (SqlDataReader readerR = cmdRecepciones.ExecuteReader())
                    {
                        if (readerR.HasRows)
                        {
                            while (readerR.Read())
                            {
                                solicitudLineas = new SolicitudLineasDTO();
                                solicitudLineas.OrdenCompra = readerR["OrdenCompra"].ToString();
                                solicitudLineas.Linea = int.Parse(readerR["Linea"].ToString());
                                solicitudLineas.Producto = readerR["Producto"].ToString();
                                solicitudLineas.Texto = readerR["Texto"].ToString();
                                solicitudLineas.Cantidad = decimal.Parse(readerR["Cantidad"].ToString());
                                solicitudLineas.CantidadRecibida = decimal.Parse(readerR["CantidadRecibida"].ToString());
                                solicitudLineas.TotalRecibido = decimal.Parse(readerR["TotalRecibido"].ToString());
                                solicitudLineas.CantidadXEjecutar = decimal.Parse(readerR["CantidadXEjecutar"].ToString());
                                solicitudLineas.Precio = decimal.Parse(readerR["Precio"].ToString());
                                solicitudLineas.MontoNeto = decimal.Parse(readerR["MontoNeto"].ToString());
                                solicitudLineas.Agrupador = readerR["Agrupador"].ToString();
                                solicitudLineas.CentroCosto = readerR["CentroCosto"].ToString();
                                solicitudLineas.TipoPresupuesto = readerR["TipoPresupuesto"].ToString();
                                solicitudLineas.RECID = readerR["RECID"].ToString();
                                solicitudLineas.IATA = readerR["IATA"].ToString();

                                lineasSolicitud.Add(solicitudLineas);
                            }
                            ExistenRecepciones = true;
                        }

                    }
                    
                    //Si no existen recepciones obtenemos los datos de la orden de compra
                    if (ExistenRecepciones == false)
                    {
                        //Obtenemos las lineas
                        var cmdSolicitudLineas = new SqlCommand("SELECT * FROM vSolicitudLineas WHERE OrdenCompra = @orden", conexion);

                        cmdSolicitudLineas.Parameters.Add("@orden", SqlDbType.VarChar);
                        cmdSolicitudLineas.Parameters["@orden"].Value = noOrden;

                        using (SqlDataReader readerS = cmdSolicitudLineas.ExecuteReader())
                        {
                                while (readerS.Read())
                                {
                                    solicitudLineas = new SolicitudLineasDTO();
                                    solicitudLineas.OrdenCompra = readerS["OrdenCompra"].ToString();
                                    solicitudLineas.Linea = int.Parse(readerS["Linea"].ToString());
                                    solicitudLineas.Producto = readerS["Producto"].ToString();
                                    solicitudLineas.Texto = readerS["Texto"].ToString();
                                    solicitudLineas.Cantidad = decimal.Parse(readerS["Cantidad"].ToString());
                                    solicitudLineas.CantidadRecibida = decimal.Parse(readerS["CantidadRecibida"].ToString());
                                    solicitudLineas.TotalRecibido = decimal.Parse(readerS["TotalRecibido"].ToString());
                                    solicitudLineas.CantidadXEjecutar = decimal.Parse(readerS["CantidadXEjecutar"].ToString());
                                    solicitudLineas.Precio = decimal.Parse(readerS["Precio"].ToString());
                                    solicitudLineas.MontoNeto = decimal.Parse(readerS["MontoNeto"].ToString());
                                    solicitudLineas.Agrupador = readerS["Agrupador"].ToString();
                                    solicitudLineas.CentroCosto = readerS["CentroCosto"].ToString();
                                    solicitudLineas.TipoPresupuesto = readerS["TipoPresupuesto"].ToString();
                                    solicitudLineas.RECID = readerS["RECID"].ToString();
                                    solicitudLineas.IATA = readerS["IATA"].ToString();

                                    lineasSolicitud.Add(solicitudLineas);
                                }
                        }
                    }
                    
                    ordenCompra.lineas = lineasSolicitud.ToList();
                    ViewBag.Modo = accion;
                }
            }
            catch (Exception exception)
            {

            }
            //Devolvemos la colección con el cabecero y las lineas
            var dataSolitud = ordenCompra;
            return View(ordenCompra);
        }

        //public ActionResult GuardaSolicitudes(List<OrdenCompraDTO> listaSolicitudes,string estatus)
        public ActionResult GuardaSolicitudes(List<listaSolicitudes> listaSolicitudes, string noOrden, string estatus)
        {
            int idEstatus;
            DateTime fechaActual = DateTime.Now;
            //Obtenemos la lista de estatus
            getEstatus();
            
            try
            {
                using (var conexion = new SqlConnection(stringConnection))
                {
                    conexion.Open();
                    var cmdOrden = new SqlCommand("SELECT TOP 1 * FROM vPURCHTABLE v" +
                       " left join Proveedor p on v.NoProveedor = p.AXNumeroProveedor" +
                       " WHERE NoOrdenCompra = @orden", conexion);
                    cmdOrden.Parameters.Add("@orden", SqlDbType.VarChar);
                    cmdOrden.Parameters["@orden"].Value = noOrden;

                    using (SqlDataReader reader = cmdOrden.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            orden = new OrdenDTO();
                            idProveedor = int.Parse(reader["IdProveedor"].ToString());
                           
                        }
                    }
                
                        var insSolicitud = new SqlCommand("INSERT INTO SolicitudFactura (IdProveedor,NoRecepcion,FechaSolicitud,PDF,XML)" +
                        " VALUES(@IdProveedor,@NoRecepcion,@FechaSolicitud,@PDF,@XML)", conexion);

                        insSolicitud.Parameters.Add("@IdProveedor", System.Data.SqlDbType.Int).Value = idProveedor;//item.IdSolicitud;
                        insSolicitud.Parameters.Add("@NoRecepcion", System.Data.SqlDbType.NVarChar,20).Value = "EJE12345678915";
                        insSolicitud.Parameters.Add("@FechaSolicitud", System.Data.SqlDbType.DateTime).Value = fechaActual;
                        insSolicitud.Parameters.Add("@PDF", System.Data.SqlDbType.NVarChar, 350).Value = "";
                        insSolicitud.Parameters.Add("@XML", System.Data.SqlDbType.NVarChar, 350).Value = "";
                    
                    int insertResult = insSolicitud.ExecuteNonQuery();
                    if (insertResult > 0)
                    {
                        int solicitud = getSolicitudInserted(conexion, idProveedor);

                        if (solicitud != 0)
                        {
                            //var lista = estatusList.ToList();
                            idEstatus = estatusList.Where(e => e.EstatusNombre.Trim() == "Pendiente").FirstOrDefault().IdEstatusSolicitud;
                            int succesInsert = guardaHistoricoSolicitud(conexion, solicitud, idEstatus, fechaActual);

                            if (succesInsert != 0)
                            {
                                int succesRecepcion = guardaRecepcion(conexion, solicitud, listaSolicitudes, fechaActual, noOrden);
                                if (succesRecepcion != 0)
                                {
                                    guardaResumen(conexion, solicitud, listaSolicitudes, fechaActual, noOrden);
                                }
                            }
                        }
                    }
                   
                    conexion.Close();

                    //using (TransactionScope transactionScope = new TransactionScope())

                    return new HttpStatusCodeResult(200, "Datos guardados");
                }

            }        //Logica guardado de datos
            catch (Exception exception)
            {
            }

            return new HttpStatusCodeResult(400,"Ocurrio un error");

        }

        private void getEstatus()
        {
            estatusList = new List<EstatusSolicitud>();
            EstatusSolicitud estatus;

            try
            {
                using (var conexion = new SqlConnection(stringConnection))
                {
                    conexion.Open();
                
                    var cmdEstatus = new SqlCommand("select * from EstatusSolicitud", conexion);

                    using (SqlDataReader readerE = cmdEstatus.ExecuteReader())
                    {
                        while (readerE.Read())
                        {
                            estatus = new EstatusSolicitud();
                            estatus.IdEstatusSolicitud = int.Parse(readerE["IdEstatusSolicitud"].ToString());
                            estatus.EstatusNombre = readerE["EstatusNombre"].ToString();

                            estatusList.Add(estatus);
                        }
                    }
                    conexion.Close();
                    //return new HttpStatusCodeResult(200, "Datos guardados");
                }
            }
            catch (Exception exception)
            {
            }
            //return new HttpStatusCodeResult(400, "Ocurrio un error");
        }

        private int getSolicitudInserted(SqlConnection conexion, int idProveedor)
        {
            try { 
                var cmdIdSolicitud = new SqlCommand("select TOP 1 IdSolicitudFactura from SolicitudFactura" +
                " WHERE IdProveedor = @IdProveedor and NoRecepcion = @NoRecepcion order by IdSolicitudFactura desc", conexion);
                cmdIdSolicitud.Parameters.Add("@IdProveedor", SqlDbType.Int);
                cmdIdSolicitud.Parameters["@IdProveedor"].Value = idProveedor;
                cmdIdSolicitud.Parameters.Add("@NoRecepcion", SqlDbType.NVarChar,20);
                cmdIdSolicitud.Parameters["@NoRecepcion"].Value = "EJE12345678915";

                using (SqlDataReader readerS = cmdIdSolicitud.ExecuteReader())
                {
                    while (readerS.Read())
                    {
                        idSolicitud = int.Parse(readerS["IdSolicitudFactura"].ToString());
                    }

                }

            }catch(Exception ex) {

            }
            return idSolicitud;
        }

        private int guardaHistoricoSolicitud(SqlConnection conexion, int solicitud, int idEstatus, DateTime fechaActual)
        {
            var insHistorico = new SqlCommand("INSERT INTO HistoricoEstatusSolicitud (IdSolicitudFactura,IdEstatusSolicitud,FechaRegistro)" +
                       " VALUES(@IdSolicitudFactura,@IdEstatusSolicitud,@FechaRegistro)", conexion);

            insHistorico.Parameters.Add("@IdSolicitudFactura", System.Data.SqlDbType.Int).Value = solicitud;//item.IdSolicitud;
            insHistorico.Parameters.Add("@IdEstatusSolicitud", System.Data.SqlDbType.NVarChar, 10).Value = idEstatus;
            insHistorico.Parameters.Add("@FechaRegistro", System.Data.SqlDbType.DateTime, 250).Value = fechaActual;

            int resultSucces = insHistorico.ExecuteNonQuery();
            return resultSucces;
        }

        private int guardaRecepcion(SqlConnection conexion, int solicitud, List<listaSolicitudes> listaSolicitudesV, DateTime fechaActual, string noOrden)
        {
            int resultSucces = 0;
            List<SolicitudLineasDTO> lineasSolicitud = new List<SolicitudLineasDTO>();
            try
            {
                foreach (var itemList in listaSolicitudesV)
                {
                    if (itemList.Monto > 0 && itemList.CantidadRecibir > 0)
                    {
                        var selDatos = new SqlCommand("SELECT * FROM vSolicitudLineas" +
                 " WHERE OrdenCompra = @orden and Linea = @linea", conexion);

                        selDatos.Parameters.Add("@orden", SqlDbType.VarChar);
                        selDatos.Parameters["@orden"].Value = noOrden;
                        selDatos.Parameters.Add("@linea", SqlDbType.VarChar);
                        selDatos.Parameters["@linea"].Value = itemList.Linea;

                        using (SqlDataReader readerS = selDatos.ExecuteReader())
                        {
                            while (readerS.Read())
                            {
                                solicitudLineas = new SolicitudLineasDTO();
                                solicitudLineas.OrdenCompra = readerS["OrdenCompra"].ToString();
                                solicitudLineas.Linea = int.Parse(readerS["Linea"].ToString());
                                solicitudLineas.Producto = readerS["Producto"].ToString();
                                solicitudLineas.Texto = readerS["Texto"].ToString();
                                solicitudLineas.Cantidad = decimal.Parse(readerS["Cantidad"].ToString());
                                solicitudLineas.CantidadRecibir = itemList.CantidadRecibir;
                                solicitudLineas.CantidadRecibida = itemList.CantidadRecibir;//decimal.Parse(readerS["CantidadRecibida"].ToString());
                                solicitudLineas.TotalRecibido = decimal.Parse(readerS["TotalRecibido"].ToString());
                                solicitudLineas.CantidadXEjecutar = decimal.Parse(readerS["CantidadXEjecutar"].ToString());
                                solicitudLineas.Precio = decimal.Parse(readerS["Precio"].ToString());
                                solicitudLineas.MontoNeto = itemList.Monto;//decimal.Parse(readerS["MontoNeto"].ToString());
                                solicitudLineas.Agrupador = readerS["Agrupador"].ToString();
                                solicitudLineas.CentroCosto = readerS["CentroCosto"].ToString();
                                solicitudLineas.TipoPresupuesto = readerS["TipoPresupuesto"].ToString();
                                solicitudLineas.RECID = readerS["RECID"].ToString();
                                solicitudLineas.IATA = readerS["IATA"].ToString();

                                lineasSolicitud.Add(solicitudLineas);
                            }
                        }
                    }
                }

                var cfdi = 480;
                if (lineasSolicitud.Count() > 0)
                {
                    foreach (var item in lineasSolicitud)
                    {
                        cfdi++;
                        var insRecepcion = new SqlCommand("INSERT INTO Recepciones(IdSolicitudFactura,OrdenCompra,Fecha,CFDI,Linea,CantidadPedido," +
              "CantidadRecepcion,CantidadRecibida,CantidadTotal,CantidadEjecutar,Precio,Monto,RECID,IATA,Estado,PARMID)" +
              " VALUES(@IdSolicitudFactura,@OrdenCompra,@Fecha,@CFDI,@Linea,@CantidadPedido,@CantidadRecepcion,@CantidadRecibida," +
              "@CantidadTotal,@CantidadEjecutar,@Precio,@Monto,@RECID,@IATA,@Estado,@PARMID)", conexion);

                        insRecepcion.Parameters.Add("@IdSolicitudFactura", System.Data.SqlDbType.Int).Value = solicitud;
                        insRecepcion.Parameters.Add("@OrdenCompra", System.Data.SqlDbType.NVarChar, 20).Value = noOrden;//item.OrdenCompra;
                        insRecepcion.Parameters.Add("@Fecha", System.Data.SqlDbType.DateTime).Value = fechaActual;
                        insRecepcion.Parameters.Add("@CFDI", System.Data.SqlDbType.NVarChar, 20).Value = "CFDI / " + cfdi.ToString(); // 448";//item.NoRecepcion;
                        insRecepcion.Parameters.Add("@Linea", System.Data.SqlDbType.Int).Value = item.Linea;
                        insRecepcion.Parameters.Add("@CantidadPedido", System.Data.SqlDbType.Decimal).Value = item.Cantidad;
                        insRecepcion.Parameters.Add("@CantidadRecepcion", System.Data.SqlDbType.Decimal).Value = item.CantidadRecibir;
                        insRecepcion.Parameters.Add("@CantidadRecibida", System.Data.SqlDbType.Decimal).Value = item.CantidadRecibida;
                        insRecepcion.Parameters.Add("@CantidadTotal", System.Data.SqlDbType.Decimal).Value = item.TotalRecibido;
                        insRecepcion.Parameters.Add("@CantidadEjecutar", System.Data.SqlDbType.Decimal).Value = item.CantidadXEjecutar;
                        insRecepcion.Parameters.Add("@Precio", System.Data.SqlDbType.Decimal).Value = item.Precio;
                        insRecepcion.Parameters.Add("@Monto", System.Data.SqlDbType.Decimal).Value = item.MontoNeto;
                        insRecepcion.Parameters.Add("@RECID", System.Data.SqlDbType.NVarChar, 50).Value = item.RECID;
                        insRecepcion.Parameters.Add("@IATA", System.Data.SqlDbType.NVarChar).Value = item.IATA;
                        insRecepcion.Parameters.Add("@Estado", System.Data.SqlDbType.Int).Value = 1;
                        insRecepcion.Parameters.Add("@PARMID", System.Data.SqlDbType.NVarChar, 20).Value = "";

                        resultSucces = insRecepcion.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            return resultSucces;
        }

        private void guardaResumen(SqlConnection conexion, int solicitud, List<listaSolicitudes> listaSolicitudes, DateTime fechaActual, string noOrden)
        {
            int resultSucces;
            var antAgrupador = "";
            var antCentroCosto = "";
            var antTipoPresupuesto = "";
            try
            {
                List<SolicitudLineasDTO> lineasSolicitud = new List<SolicitudLineasDTO>();

                foreach (var itemResum in listaSolicitudes)
                {
                    var selDatos = new SqlCommand("select r.OrdenCompra, r.Recepcion, r.PendienteAntes, r.Precio, r.Monto, o.Agrupador, o.CentroCosto," +
                        " o.TipoPresupuesto from(select r.OrdenCompra, o.Agrupador, o.CentroCosto, o.TipoPresupuesto, sum(r.Monto) 'Recepcion'," +
                        " sum(r.CantidadRecibida) 'PendienteAntes', sum(r.Precio) 'Precio', SUM(r.Monto) 'Monto' from Recepciones r left join vOrdenCompra" +
                        " o on r.OrdenCompra = o.OrdenCompra group by r.OrdenCompra, o.Agrupador, o.CentroCosto, o.TipoPresupuesto) r" +
                        " left join vOrdenCompra o on r.OrdenCompra = o.OrdenCompra and r.Agrupador = o.Agrupador" +
                        " and r.CentroCosto = o.CentroCosto and r.TipoPresupuesto = o.TipoPresupuesto WHERE r.OrdenCompra = @orden group by  r.OrdenCompra," +
                        " r.Recepcion, r.PendienteAntes, r.Precio, r.Monto, o.Agrupador, o.CentroCosto, o.TipoPresupuesto", conexion);

                    selDatos.Parameters.Add("@orden", SqlDbType.VarChar);
                    selDatos.Parameters["@orden"].Value = noOrden;
                 
                    using (SqlDataReader readerS = selDatos.ExecuteReader())
                    {
                        while (readerS.Read())
                        {
                            solicitudLineas = new SolicitudLineasDTO();
                            solicitudLineas.OrdenCompra = readerS["OrdenCompra"].ToString();
                            //solicitudLineas.RECID = readerS["RECID"].ToString();
                            solicitudLineas.MontoNeto = itemResum.Monto;
                            solicitudLineas.CantidadRecibida = decimal.Parse(readerS["CantidadRecibida"].ToString());//---itemResum.CantidadRecibir;//decimal.Parse(readerS["CantidadRecibida"].ToString());
                            solicitudLineas.Linea = itemResum.Linea;
                            solicitudLineas.Precio = decimal.Parse(readerS["Precio"].ToString());
                            lineasSolicitud.Add(solicitudLineas);
                        }

                    }
                }

                var insResumen = new SqlCommand("INSERT INTO ResumenRecepcion(OrdenCompra,RECID,Recepcion,PendienteAntes,Linea," +
                    "Precio,Monto,CFDI,PARMID,IdSolicitudFactura)" +
                    " VALUES(@OrdenCompra,@RECID,@Recepcion,@PendienteAntes,@Linea," +
                    "@Precio,@Monto,@CFDI,@PARMID,@IdSolicitudFactura)", conexion);

                foreach (var item in lineasSolicitud)
                {
                    insResumen.Parameters.Add("@OrdenCompra", System.Data.SqlDbType.NVarChar, 50).Value = noOrden;
                    insResumen.Parameters.Add("@RECID", System.Data.SqlDbType.NVarChar, 50).Value = "5637692617";//Validar
                    insResumen.Parameters.Add("@Recepcion", System.Data.SqlDbType.Decimal).Value = item.MontoNeto;
                    insResumen.Parameters.Add("@PendienteAntes", System.Data.SqlDbType.Decimal).Value = item.CantidadRecibida;//item.NoRecepcion;
                    insResumen.Parameters.Add("@Linea", System.Data.SqlDbType.Int).Value = item.Linea;
                    insResumen.Parameters.Add("@Precio", System.Data.SqlDbType.Decimal).Value = item.Precio;
                    insResumen.Parameters.Add("@Monto", System.Data.SqlDbType.Decimal).Value = item.MontoNeto;
                    insResumen.Parameters.Add("@CFDI", System.Data.SqlDbType.NVarChar, 50).Value = "CFDI / 450";
                    insResumen.Parameters.Add("@PARMID", System.Data.SqlDbType.NVarChar, 20).Value = "";
                    insResumen.Parameters.Add("@IdSolicitudFactura", System.Data.SqlDbType.Int).Value = solicitud;
                    resultSucces = insResumen.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {

            }
        }
        public ActionResult OrdenesAxTest()
        {
            return View();
        }
        [HttpPost]
        public JsonResult GetOpcionesEstatus()
        {
            List<object> result = new List<object>();
            try
            {
                using (var conexion = new SqlConnection(stringConnection))
                {
                    conexion.Open();
                    var cmdEstatus = new SqlCommand("select * from EstatusSolicitud", conexion);

                    using (SqlDataReader readerS = cmdEstatus.ExecuteReader())
                    {
                        while (readerS.Read())
                        {
                            result.Add(new
                            {
                                Id = int.Parse(readerS["IdEstatusSolicitud"].ToString()),
                                Nombre = readerS["EstatusNombre"].ToString()
                            });

                        }

                    }
                }
            }
            catch (Exception ex)
            {

            }

            return Json(new
            {
                option = result
            });
        }
    }
}