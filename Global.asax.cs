using System;
using System.Linq;
using System.Web.Mvc;

namespace Alloy
{
    public class EPiServerApplication : EPiServer.Global
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            ViewEngines.Engines.Remove(ViewEngines.Engines.OfType<WebFormViewEngine>().First());
            ViewEngines.Engines.Add(new FeaturesLayoutViewEngine());
            //Tip: Want to call the EPiServer API on startup? Add an initialization module instead (Add -> New Item.. -> EPiServer -> Initialization Module)
        }
    }
}