using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;

namespace Source.Editor.Util
{
    public class SceneSwitcher
    {
        [MenuItem("TiltBoard/Scene/Boot", false, 0)]
        public static void SwitchToBoot()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/boot.unity");
        }

        [MenuItem("TiltBoard/Scene/Main", false, 1)]
        public static void SwitchToMain()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/main.unity");
        }   
        
        [MenuItem("TiltBoard/Scene/Game", false, 2)]
        public static void SwitchToGame()
        {
            EditorSceneManager.OpenScene("Assets/Scenes/game.unity");
        }   
    }
}