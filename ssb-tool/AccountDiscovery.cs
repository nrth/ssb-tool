using Newtonsoft.Json.Linq;
using SourceSchemaParser.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ssb_tool
{
    class AccountDiscovery
    {
        private String _userdataPath { get; set; }
        private String _localconfigPath = @"\config\localconfig.vdf";

        private Dictionary<String, String> _accounts;

        public AccountDiscovery(String userdataPath = 
                                @"C:\Program Files (x86)\Steam\userdata\")
        {
            _userdataPath = userdataPath;
            _accounts = new Dictionary<String, String>();
        }

        public void fetchAccounts()
        {
            _accounts.Clear();

            String[] accountDirectories = 
                Directory.GetFileSystemEntries(_userdataPath);

            foreach (var dir in accountDirectories)
            {
                String path = dir + _localconfigPath;

                if (!File.Exists(path))
                {
                    continue;
                }

                JObject localconfig = VDFConvert.ToJObject(
                                        File.ReadAllLines(path)
                                        );

                String personaName = localconfig["UserLocalConfigStore"]["friends"]["PersonaName"].ToString();

                _accounts.Add(
                    dir.Split('\\').Last(),
                    personaName
                );

            }
        }

        public Dictionary<String, String> getAccounts()
        {
            return _accounts;
        }
    }
}
