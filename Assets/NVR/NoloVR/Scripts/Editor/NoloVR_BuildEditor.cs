using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;

public class NoloVR_BuildEditor {

    //PC平台打包时，将NoloServer打包进Plugins文件夹下面
    public class MyBuildPostprocessor
    {
        [PostProcessBuildAttribute(1)]
        public static void OnPostprocessBuild(BuildTarget target, string path)
        {
#if UNITY_STANDALONE_WIN
            var strPathFrom = UnityEngine.Application.dataPath + "/Plugins/x86_64/NoloServer.exe";
            var nIdxSlash = path.LastIndexOf('/');
            var nIdxDot = path.LastIndexOf('.');
            var strRootTarget = path.Substring(0, nIdxSlash);
            var strPathTargetFile = strRootTarget + path.Substring(nIdxSlash, nIdxDot - nIdxSlash) + "_Data/Plugins/NoloServer.exe";
            System.IO.File.Copy(strPathFrom, strPathTargetFile);
#endif
        }
    }
}
