using UnityEngine.SceneManagement;

namespace FrikLib.Unity
{
    public class SceneFunc
    {
        public static bool HasSceneLoaded(string name)
        {
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                if (SceneManager.GetSceneAt(i).name == name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
