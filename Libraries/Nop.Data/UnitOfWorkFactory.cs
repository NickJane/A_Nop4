using Nop.Core.Infrastructure;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace Nop.Data
{
    public class UnitOfWorkFactory
    {
        private static string CONTEXT_KEY = "Ef_DbContext_Static_Key";
        public static IUnitOfWork CurrentUnitOfWork
        {

            get
            {
                if (IsInWebContext())
                {
                    if (HttpContext.Current.Items[CONTEXT_KEY] == null)
                    {

                        HttpContext.Current.Items[CONTEXT_KEY] = EngineContext.Current.Resolve<IUnitOfWork>();
                    }

                    return (IUnitOfWork)HttpContext.Current.Items[CONTEXT_KEY];
                }
                else
                {

                    if (CallContext.GetData(CONTEXT_KEY) == null)
                    {
                        CallContext.SetData(CONTEXT_KEY, EngineContext.Current.Resolve<IUnitOfWork>());
                    }
                    return (IUnitOfWork)CallContext.GetData(CONTEXT_KEY);
                }
            }

        }
        public static void DisposeUnitOfWork()
        {
            if (IsInWebContext())
            {
                HttpContext.Current.Items[CONTEXT_KEY] = null;
            }
            else
            {
                CallContext.SetData(CONTEXT_KEY, null);
                CallContext.FreeNamedDataSlot(CONTEXT_KEY);
            }
        }
        public static bool HasContextOpen()
        {
            if (HttpContext.Current.Items[CONTEXT_KEY] != null)
            {
                return true;
            }
            return false;
        }
        public static bool IsInWebContext()
        {
            return HttpContext.Current != null;
        }
    }
}
