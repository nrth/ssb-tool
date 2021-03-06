﻿using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SourceSchemaParser.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace ssb_tool
{
    class ServerBrowserHistory
    {
        private String _userdataPath { get; set; }
        private String _historySubPath = @"\7\remote\serverbrowser_hist.vdf";

        public ServerBrowserHistory(String userdataPath = 
                                    @"C:\Program Files (x86)\Steam\userdata\")
        {
            _userdataPath = userdataPath;
        }

        public void Backup(String accountid, String backup_path)
        {
            StreamWriter output = new StreamWriter(backup_path);

            String path = getHistoryPath(accountid);

            dynamic hist = VDFConvert.ToJObject(File.ReadAllLines(path));
            output.WriteLine(hist.Filters.favorites);

            output.Close();
        }

        public void Import(String accountid, String backup_path)
        {
            String path = getHistoryPath(accountid);

            String import = File.ReadAllText(backup_path);
            String[] current = File.ReadAllLines(path);

            JObject imp = JObject.Parse(import);
            JObject cur = VDFConvert.ToJObject(current);

            JObject fav = (JObject)cur["Filters"]["favorites"];

            cur["Filters"]["favorites"] = merge(fav, imp);

            StreamWriter output = new StreamWriter(path);

            output.Write(Vdf.Convert(cur));

            output.Close();
            output.Dispose();

            touchFile(path);
        }

        public void Purge(String accountid)
        {
            String path = getHistoryPath(accountid);
           
            StreamWriter output = new StreamWriter(path);
            output.Write(Vdf.Convert(createEmptyList()));
            output.Close();
            output.Dispose();

            touchFile(path);
        }

        private JObject createEmptyList()
        {
            String empty = @"{Filters: {favorites: {}}}";
            return JObject.Parse(empty);
        }

        private void touchFile(String path)
        {
            File.SetLastWriteTimeUtc(path, DateTime.UtcNow);
        }

        private String getHistoryPath(String accountid)
        {
            return _userdataPath + accountid + _historySubPath;
        }

        private JObject merge(JObject current, JObject import)
        {
            Hashtable currentLookup = getHashTable(current);
            Hashtable importLookup = getHashTable(import);

            JObject output = new JObject();

            int propCounter = 1;

            foreach (DictionaryEntry i in currentLookup)
            {
                if (!importLookup.Contains(i.Key))
                {
                    JToken entry = current.GetValue(i.Value.ToString());
                    output.Add(new JProperty(propCounter.ToString(), entry));
                    propCounter++;
                }
            }

            foreach (DictionaryEntry i in importLookup)
            {
                JToken entry = import.GetValue(i.Value.ToString());
                output.Add(new JProperty(propCounter.ToString(), entry));
                propCounter++;
            }

            return output;
        }

        private Hashtable getHashTable(JObject servers)
        {
            Hashtable lookupTable = new Hashtable();

            foreach (var i in servers)
            {
                lookupTable.Add(i.Value["address"], i.Key);
            }

            return lookupTable;
        }
    }
}
