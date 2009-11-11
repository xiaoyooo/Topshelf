// Copyright 2007-2008 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace Topshelf.Configuration
{
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.ServiceProcess;

    public class WinServiceSettings
    {
        private const string _instanceChar = "$";
        
        public WinServiceSettings()
        {
            StartMode = ServiceStartMode.Automatic;
            Dependencies = new List<string>();
        }

        public ServiceStartMode StartMode { get; set; }
        

        public string ServiceName { private get; set; }
        public string DisplayName { private get; set; }
        public string Description { get; set; }
        public string InstanceName { get; set; }
        public Credentials Credentials { get; set; }

        public string FullServiceName
        {
            get
            {
                return InstanceName == null
                           ? ServiceName : "{0}{1}{2}".FormatWith(ServiceName, _instanceChar, InstanceName);
            }
        }

        public string FullDisplayName
        {
            get
            {
                return InstanceName == null
                           ? DisplayName : "{0} (Instance: {1})".FormatWith(DisplayName, InstanceName);
            }
        }

        public List<string> Dependencies { get; set; }

        public static WinServiceSettings DotNetConfig
        {
            get
            {
                var settings = new WinServiceSettings
                               {
                                   ServiceName = ConfigurationManager.AppSettings["serviceName"],
                                   DisplayName = ConfigurationManager.AppSettings["displayName"],
                                   Description = ConfigurationManager.AppSettings["description"],
                               };

                settings.Dependencies.AddRange(ConfigurationManager.AppSettings["dependencies"].Split(','));
                return settings;
            }
        }

        public string ImagePath
        {
            get
            {
                return InstanceName == null ? " -service" : " -service -instance:{0}".FormatWith(InstanceName);
            }
        }

        public static WinServiceSettings Custom(string serviceName, string displayName, string description, params string[] dependencies)
        {
            var settings = new WinServiceSettings
                           {
                               ServiceName = serviceName,
                               DisplayName = displayName,
                               Description = description,
                           };
            settings.Dependencies.AddRange(dependencies);
            return settings;
        }
    }
}