using System;
using System.Web.Http;

namespace Ximble_JS.Helpers
{
    public interface IProductionRepository: IDisposable
    {
        IHttpActionResult Get(string name = null, DateTime? selDate = null, string keywords = null);   
    }
}