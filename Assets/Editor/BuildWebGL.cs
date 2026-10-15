// Added for the LSTME case-study screenshots; not part of the original project.
using System.Linq;
using UnityEditor;

public static class BuildWebGL
{
    public static void Build()
    {
        var scenes = EditorBuildSettings.scenes.Where(s => s.enabled).Select(s => s.path).ToArray();
        if (scenes.Length == 0)
            scenes = AssetDatabase.FindAssets("t:Scene").Select(AssetDatabase.GUIDToAssetPath).ToArray();
        PlayerSettings.WebGL.compressionFormat = WebGLCompressionFormat.Disabled;
        var report = BuildPipeline.BuildPlayer(scenes, "Build/WebGL", BuildTarget.WebGL, BuildOptions.None);
        UnityEngine.Debug.Log("LSTME BUILD RESULT: " + report);
    }
}
