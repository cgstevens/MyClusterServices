using System;
using System.Configuration;

namespace MyServices.Shared.Helpers
{
    public class AppSettings
    {
        public static string GetIpAddressFromConfig()
        {
            var ipAddress = StaticMethods.GetHostIpAddress();
            string port = ServerPort;
            
            var selfAddress = string.Format("{0}:{1}", ipAddress, port);
            return selfAddress;
        }

        public static string ConnectionString
        {
            get
            {
                var val = ConfigurationManager.ConnectionStrings["myconnection"].ConnectionString;
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'myconnection' ConnectionString in config file.");
                return val;
            }
        }

        public static string Log4NetConnectionString
        {
            get
            {
                var val = ConfigurationManager.ConnectionStrings["Logging"].ConnectionString;
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'Logging' ConnectionString in config file.");
                return val;
            }
        }

        public static string ApiPort
        {
            get
            {
                var val = ConfigurationManager.AppSettings["ApiPort"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'ApiPort' appSetting in config file.");
                return val;
            }
        }

        public static string HubAccessUri
        {
            get
            {
                var val = ConfigurationManager.AppSettings["HubAccessUri"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'HubAccessUri' appSetting in config file.");
                return val;
            }
        }

        public static string GrantType
        {
            get
            {
                var val = ConfigurationManager.AppSettings["GrantType"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'GrantType' appSetting in config file.");
                return val;
            }
        }

        public static string UserName
        {
            get
            {
                var val = ConfigurationManager.AppSettings["UserName"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'UserName' appSetting in config file.");
                return val;
            }
        }

        public static string Password
        {
            get
            {
                var val = ConfigurationManager.AppSettings["Password"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'Password' appSetting in config file.");
                return val;
            }
        }

        public static string ClientId
        {
            get
            {
                var val = ConfigurationManager.AppSettings["ClientId"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'ClientId' appSetting in config file.");
                return val;
            }
        }

        public static string ClientSecret
        {
            get
            {
                var val = ConfigurationManager.AppSettings["ClientSecret"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'Password' appSetting in config file.");
                return val;
            }
        }

        public static string DealerShareUrl
        {
            get
            {
                var val = ConfigurationManager.AppSettings["DealerShareUrl"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'DealerShareUrl' appSetting in config file.");
                return val;
            }
        }

        public static string InvoiceServiceEndPoint
        {
            get
            {
                var val = ConfigurationManager.AppSettings["InvoiceServiceEndPoint"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'InvoiceServiceEndPoint' appSetting in config file.");
                return val;
            }
        }

        public static string CompanyId
        {
            get
            {
                var val = ConfigurationManager.AppSettings["CompanyId"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'CompanyId' appSetting in config file.");
                return val;
            }
        }

        public static string HubPullDateTime
        {
            get
            {
                var val = ConfigurationManager.AppSettings["HubPullDateTime"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'HubPullDateTime' appSetting in config file.");
                return val;
            }
        }

        public static string JobTaskerScheduleDelay
        {
            get
            {
                var val = ConfigurationManager.AppSettings["JobTaskerScheduleDelay"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'JobTaskerScheduleDelay' appSetting in config file.");
                return val;
            }
        }

        public static string HowManyMarketsToRun
        {
            get
            {
                var val = ConfigurationManager.AppSettings["HowManyMarketsToRun"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'HowManyMarketsToRun' appSetting in config file.");
                return val;
            }
        }

        public static string InvoiceProcessDelayOffSet
        {
            get
            {
                var val = ConfigurationManager.AppSettings["InvoiceProcessDelayOffSet"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'InvoiceProcessDelayOffSet' appSetting in config file.");
                return val;
            }
        }

        public static string MarketJobDelayOffSet
        {
            get
            {
                var val = ConfigurationManager.AppSettings["MarketJobDelayOffSet"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'MarketJobDelayOffSet' appSetting in config file.");
                return val;
            }
        }

        public static string MarketRefreshOffSet
        {
            get
            {
                var val = ConfigurationManager.AppSettings["MarketRefreshOffSet"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'MarketRefreshOffSet' appSetting in config file.");
                return val;
            }
        }

        public static string RemoteActorTimeout
        {
            get
            {
                var val = ConfigurationManager.AppSettings["RemoteActorTimeout"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'RemoteActorTimeout' appSetting in config file.");
                return val;
            }
        }

        public static string AkkaWorkerNodes
        {
            get
            {
                var val = ConfigurationManager.AppSettings["AkkaWorkerNodes"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'AkkaWorkerNodes' appSetting in config file.");
                return val;
            }
        }

        public static string IsCommissionsWorkersEnabled
        {
            get
            {
                var val = ConfigurationManager.AppSettings["IsCommissionsWorkersEnabled"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'IsCommissionsWorkersEnabled' appSetting in config file.");
                return val;
            }
        }

        public static string ServiceWorkerName
        {
            get
            {
                var val = ConfigurationManager.AppSettings["ServiceWorkerName"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'ServiceWorkerName' appSetting in config file.");
                return val;
            }
        }

        public static string ActorSystemType
        {
            get
            {
                var val = ConfigurationManager.AppSettings["ActorSystemType"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'ActorSystemType' appSetting in config file.");
                return val;
            }
        }
        public static string InstrumentationKey
        {
            get
            {
                var val = ConfigurationManager.AppSettings["InstrumentationKey"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'InstrumentationKey' appSetting in config file.");
                return val;
            }
        }

        public static string DisableTelemetry
        {
            get
            {
                var val = ConfigurationManager.AppSettings["DisableTelemetry"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'DisableTelemetry' appSetting in config file.");
                return val;
            }
        }

        public static bool DisableActorPerformanceCounters
        {
            get
            {
                var val = ConfigurationManager.AppSettings["DisableActorPerformanceCounters"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'DisableActorPerformanceCounters' appSetting in config file.");
                
                bool retVal = true;
                bool result = bool.TryParse(val, out retVal);
                return retVal;
            }
        }

        public static bool TrackMessagesReceived
        {
            get
            {
                var val = ConfigurationManager.AppSettings["TrackMessagesReceived"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'TrackMessagesReceived' appSetting in config file.");

                bool retVal = true;
                bool result = bool.TryParse(val, out retVal);
                return retVal;
            }
        }
        
        public static string ServerIpList
        {
            get
            {
                var val = ConfigurationManager.AppSettings["ServerIpList"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'ServerIpList' appSetting in config file.");
                return val;
            }
        }

        public static string ServerPort
        {
            get
            {
                var val = ConfigurationManager.AppSettings["ServerPort"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'ServerPort' appSetting in config file.");
                return val;
            }
        }
        
        public static bool IsSignalRBackPlaneEnabled
        {
            get
            {
                var val = ConfigurationManager.AppSettings["IsSignalRBackPlaneEnabled"];
                if (string.IsNullOrWhiteSpace(val))
                    throw new ConfigurationErrorsException("Missing 'IsSignalRBackPlaneEnabled' appSetting in config file.");

                bool retVal = false;
                bool result = Boolean.TryParse(val, out retVal);
                return retVal;
            }
        }
    }
}
