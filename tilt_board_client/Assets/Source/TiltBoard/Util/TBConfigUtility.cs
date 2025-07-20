using System.Collections.Generic;
using UnityEngine;

namespace Source.TiltBoard.Util
{
    public class TBConfigUtility
    {
        private const string CONFIG_FILE_PATH_IN_RESOURCES = "Configurations/";
        private static Dictionary<string, ScriptableObject> _configMap = new Dictionary<string, ScriptableObject>();

        public static T LoadConfiguration<T>(string filename) where T : ScriptableObject
        {
            // return object
            T config;

            // check if the file is already loaded 
            if (_configMap.ContainsKey(filename))
            {
                // then send the cached files 
                config = _configMap[filename] as T;
            }
            else
            {
                // else load the config file from resources 
                // the final configuration file 
                string configFilePath = CONFIG_FILE_PATH_IN_RESOURCES + filename;

                // try to load from the resources folder
                config = Resources.Load<T>(configFilePath);

                // cache it in the map 
                _configMap.Add(filename, config);
            }

            // return to caller 
            return config;
        }
    }
}