using UnityEngine;
using static System.IO.Directory;
using static System.IO.Path;
using static UnityEditor.AssetDatabase;

public static class Setup
{
    [UnityEditor.MenuItem("Tools/Setup/Create Default Folders")]
    public static void CreateDefaultFolders()
    {
        Folders.CreateDefault("_Project", "Scripts", "Prefabs", "Scenes", "Materials", "Textures", "Models", "Animations", "Audio", "Fonts", "Shaders", "Resources", "Plugins",
            "Editor");
        Refresh();
    }

    private static class Folders
    {
        public static void CreateDefault(string root, params string[] folders)
        {
            string fullPath = Combine(Application.dataPath, root);
            foreach (string folder in folders)
            {
                string path = Combine(fullPath, folder);
                if (!Exists(path))
                    CreateDirectory(path);
            }
        }
    }
}