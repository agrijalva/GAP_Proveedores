using System;

namespace EPROCUREMENT.GAPPROVEEDOR.Data
{
    /// <summary>
    /// Clase de TryCatch personalizado
    /// </summary>
    public class TryCatchExecutor
    {
        /// <summary>
        /// Metodo de try Catch personalizado
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public T SafeExecutor<T>(Func<T> action)
        {
            try
            {
                return action();
            }
            catch (Exception ex)
            {
                //Aqui se controlara el error
                return action();
            }

            return default(T);
        }
    }
}
