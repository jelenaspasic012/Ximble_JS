using System;
using System.Web.Http;

namespace Ximble_JS.Helpers
{
    public interface IPurchasingRepository : IDisposable
    {
        IHttpActionResult Get(DateTime startDate, DateTime endDate);
    }
}