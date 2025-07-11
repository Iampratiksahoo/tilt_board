using UnityEngine;

namespace Source.TiltBoard.Util
{
    public class TBUtility
    {
        private const string CONFIG_FILE_PATH_IN_RESOURCES = "Configurations/";

        public static T LoadConfiguration<T>(string filename) where T : ScriptableObject
        {
            // return object
            T config = null;

            string configFilePath = CONFIG_FILE_PATH_IN_RESOURCES + filename;

            // try to load from the resources folder
            config = Resources.Load<T>( configFilePath );

            // return to caller 
            return config;
        }
    }
}