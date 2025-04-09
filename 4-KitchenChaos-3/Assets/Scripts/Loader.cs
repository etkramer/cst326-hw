using UnityEngine;
using UnityEngine.SceneManagement;

public static class Loader
{
    public enum Scene
    {
        MainMenuScene,
        GameScene,
        LoadingScene,
    }

    private static Scene targetScene;

    public static void Load(Scene targetScene)
    {
        // Unlock mouse on any scene load (assuming scenes will re-lock on their own as needed)
        Cursor.lockState = CursorLockMode.None;

        Loader.targetScene = targetScene;
        SceneManager.LoadScene(Scene.LoadingScene.ToString());
    }

    public static void LoaderCallback()
    {
        SceneManager.LoadScene(targetScene.ToString());
    }
}
