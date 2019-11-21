﻿using System.Threading;
using System.Configuration;
using System.Web.Configuration;

namespace EprocurementWeb.Helpers
{
    public class GlobalHelper
    {
        public static string CurrentCulture
        {
            get
            {
                return Thread.CurrentThread.CurrentUICulture.Name;
            }
        }
        public static string DefaultCulture
        {
            get
            {
                Configuration config = WebConfigurationManager.OpenWebConfiguration("/");
                GlobalizationSection section = (GlobalizationSection)config.GetSection("system.web/globalization");
                return section.UICulture;
            }
        }     
    }
}