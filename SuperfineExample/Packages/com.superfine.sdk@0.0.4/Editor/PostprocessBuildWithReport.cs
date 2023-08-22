using UnityEditor.Build;
using UnityEditor.Build.Reporting;

public class PostprocessBuildWithReport : IPostprocessBuildWithReport
{
    public int callbackOrder => int.MaxValue;
    public void OnPostprocessBuild(BuildReport report)
    {
#if UNITY_STANDALONE_OSX
        if (UnityEditor.OSXStandalone.UserBuildSettings.createXcodeProject) return;

        UnityEditor.OSXStandalone.MacOSCodeSigning.CodeSignAppBundle(report.summary.outputPath + "/Contents/PlugIns/superfine-sdk-cpp.bundle");
        UnityEditor.OSXStandalone.MacOSCodeSigning.CodeSignAppBundle(report.summary.outputPath);
#endif
    }
}