using Newtonsoft.Json.Linq;
using SourceSchemaParser.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ssb_tool
{
    class AccountDiscovery
    {
        private String _userdataPath { get; set; }
        private String _localconfigPath = @"\config\localconfig.vdf";

        private Dictionary<String, String> _accounts = 
            new Dictionary<String, String>();

        public AccountDiscovery(String userdataPath = 
                                @"C:\Program Files (x86)\Steam\userdata\")
        {
            _userdataPath = userdataPath;
        }

        public void fetchAccounts()
        {
            _accounts.Clear();

            String[] accountDirectories = 
                Directory.GetFileSystemEntries(_userdataPath);

            foreach (var dir in accountDirectories)
            {
                try
                {
                    String path = dir + _localconfigPath;

                    dynamic localconfig = VDFConvert.ToJObject(
                                            File.ReadAllLines(path)
                                          );
                    
                    String personaName = localconfig.UserLocalConfigStore.
                                         friends.PersonaName.ToString();
                    _accounts.Add(
                        personaName,
                        dir.Split('\\').Last()
                    );

                } catch
                {
                    Console.WriteLine("Can not access localconfig"
                                    + "for account directory " + dir);
                }
            }
        }

        public List<String> getPersonaList()
        {
            return _accounts.Keys.ToList();
        }

        public String getID3(String personaname)
        {
            String ID64 = "";
            _accounts.TryGetValue(personaname, out ID64);
            return ID64;
        }
    }
}
