using System.IO;
using Source.TiltBoard.Ball.Util;
using Source.TiltBoard.Pool.Util;
using Source.TiltBoard.Util.Config;
using UnityEditor;
using UnityEngine;

namespace Source.Editor.Util
{
    public class ConfigHandler
    {
        private const string CONFIG_FILE_PATH = "Assets/Resources/Configurations/";

        [MenuItem("TiltBoard/Create Config/PoolConfiguration", false, 0)]
        public static void CreatePoolConfiguration()
        {
            createConfiguration<PoolConfiguration>("PoolConfiguration.asset");
        }

        [MenuItem("TiltBoard/Create Config/GameConfiguration", false, 1)]
        public static void CreateGameConfiguration()
        {
            createConfiguration<GameConfiguration>("GameConfiguration.asset");
        }

        [MenuItem("TiltBoard/Create Config/BallConfiguration", false, 2)]
        public static void CreateBallConfiguration()
        {
            createConfiguration<BallConfiguration>("BallConfiguration.asset");
        }

        /// <summary>
        /// Helper method which creates a configuration object with a given filename
        /// </summary>
        /// <param name="config">The custom configuration.</param>
        /// <param name="filename">The filename.</param>
        private static void createConfiguration<T>(string filename, string filePath = null) where T : ScriptableObject
        {
            // path to the location where config file is to be saved
            string configFilePath = $"{(string.IsNullOrEmpty(filePath) ? CONFIG_FILE_PATH : filePath)}{filename}";

            // create the directory if it doesn't already exist
            Directory.CreateDirectory(CONFIG_FILE_PATH);

            // create instance of the scriptable object
            T configObject = ScriptableObject.CreateInstance<T>();

            // create the asset at the given path
            AssetDatabase.CreateAsset(configObject, configFilePath);

            // set the instance to dirty so that it is saved to disk
            EditorUtility.SetDirty(configObject);

            // force all dirty assets to be saved
            AssetDatabase.SaveAssets();

            // set the active object 
            Selection.activeObject = configObject;

            Debug.Log($"Created '{configObject.name}' at '{configFilePath}' ");

            EditorUtility.FocusProjectWindow();
        }
    }
}