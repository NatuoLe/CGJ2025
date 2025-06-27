#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityToolbarExtender;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

public class UtilEditor
{
    static string batGenAllFilePath = "gen.bat";
    static string batGenConfigFilePath = "genConfig.bat";

    [InitializeOnLoad]
    public class SceneSwitchLeftButton
    {
        static SceneSwitchLeftButton()
        {
            ToolbarExtender.RightToolbarGUI.Add(OnToolbarGUI);
        }

        public class EditorSceneChoice : OdinMenuEditorWindow
        {
            private string lastPath;

            public static void OpenWindow()
            {
                var windows = GetWindow<EditorSceneChoice>();
                windows.titleContent = new GUIContent("选择场景");
                windows.minSize = new Vector2(800, 800);
                windows.maxSize = new Vector2(800, 800);
                windows.maximized = false;
                windows.ResizableMenuWidth = false;
                windows.Show();
            }

            protected override OdinMenuTree BuildMenuTree()
            {
                var tree = new OdinMenuTree(false);
                var guids = AssetDatabase.FindAssets("t:scene");

                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    tree.Add(path, path);
                }

                tree.EnumerateTree().AddThumbnailIcons();
                tree.Config.DrawSearchToolbar = true;
                MenuWidth = 800;
                return tree;
            }

            [Obsolete("Obsolete")]
            protected override void OnGUI()
            {
                base.OnGUI();
                if (MenuTree?.Selection.SelectedValue == null) return;
                var selectionPath = MenuTree.Selection.SelectedValue.ToString();
                if (string.IsNullOrEmpty(lastPath))
                {
                    lastPath = selectionPath;
                    return;
                }

                if (lastPath != selectionPath)
                {
                    lastPath = selectionPath;
                    EditorSceneManager.OpenScene(selectionPath);
                    Close();
                }
            }

            public void OnFocus()
            {
                ForceMenuTreeRebuild();
            }
        }

        public class EditorDialogueChoice : OdinMenuEditorWindow
        {
            private string lastPath;

            public static void OpenWindow()
            {
                var windows = GetWindow<EditorDialogueChoice>();
                windows.titleContent = new GUIContent("选择场景");
                windows.minSize = new Vector2(800, 800);
                windows.maxSize = new Vector2(800, 800);
                windows.maximized = false;
                windows.ResizableMenuWidth = false;
                windows.Show();
            }

            protected override OdinMenuTree BuildMenuTree()
            {
                var tree = new OdinMenuTree(false);
                var guids = AssetDatabase.FindAssets("t:scene");

                foreach (var guid in guids)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    tree.Add(path, path);
                }

                tree.EnumerateTree().AddThumbnailIcons();
                tree.Config.DrawSearchToolbar = true;
                MenuWidth = 800;
                return tree;
            }

            [Obsolete("Obsolete")]
            protected override void OnGUI()
            {
                base.OnGUI();
                if (MenuTree?.Selection.SelectedValue == null) return;
                var selectionPath = MenuTree.Selection.SelectedValue.ToString();
                if (string.IsNullOrEmpty(lastPath))
                {
                    lastPath = selectionPath;
                    return;
                }

                if (lastPath != selectionPath)
                {
                    lastPath = selectionPath;
                    EditorSceneManager.OpenScene(selectionPath);
                    Close();
                }
            }

            public void OnFocus()
            {
                ForceMenuTreeRebuild();
            }
        }

        static void OnToolbarGUI()
        {
            var style = new GUIStyle(EditorStyles.toolbarButton);
            style.fixedWidth = 35;
            var styleBig = new GUIStyle(EditorStyles.toolbarButton);
            styleBig.fixedWidth = 65;
            var styleBigger = new GUIStyle(EditorStyles.toolbarButton);
            styleBigger.fixedWidth = 90;
            if (GUILayout.Button(EditorGUIUtility.ObjectContent(null, typeof(SceneAsset)).image, style))
            {
                EditorSceneChoice.OpenWindow();
            }

            if (GUILayout.Button(new GUIContent("Launcher"), styleBig)) OpenExcelDic();
            if (GUILayout.Button(new GUIContent("Combat"), styleBig)) OpenCombat();
            if (GUILayout.Button(new GUIContent("Map"), styleBig)) OpenMap();
            if (GUILayout.Button(new GUIContent("ClearPrefs"), styleBigger)) ClearPlayerPrefs();
            style.fontStyle = FontStyle.Bold;
            GUILayout.FlexibleSpace();
        }
    }


    private static void OpenExcelDic()
    {
        //Debug.Log("施工中");
        // 指定要打开的场景路径
        string scenePath = "Assets/Scenes/Luancher.unity"; // 替换为你的场景路径
        OpenScene(scenePath);
    }
    private static void OpenCombat()
    {
        //Debug.Log("施工中");
        // 指定要打开的场景路径
        string scenePath = "Assets/Scenes/Combat.unity"; // 替换为你的场景路径
        OpenScene(scenePath);
    }
    private static void OpenMap()
    {
        // 指定要打开的场景路径
        string scenePath = "Assets/_DynamicGroups/Scenes/Alpha/Map.unity"; // 替换为你的场景路径
        OpenScene(scenePath);
    }
    private static void OpenVFXScene()
    {
        //Debug.Log("施工中");
        // 指定要打开的场景路径
        string scenePath = "Assets/App/Scenes/Alpha/VfxTest.unity"; // 替换为你的场景路径
        OpenScene(scenePath);
    }



    // 定义一个方法，用于打开指定的场景
    private static void OpenScene(string scenePath)
    {
        // 使用 EditorApplication.LoadLevel 来加载场景
        EditorSceneManager.OpenScene(scenePath);
    }

    private static void OpenMaps()
    {
        string folderPath = "Assets/App/Res/_DynamicGroups/Prefabs/Maps";
        // 确保路径是有效的
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("Invalid folder path: " + folderPath);
            return;
        }

        // 将文件夹路径转换为 Unity 的 Assets 路径
        string relativePath = folderPath.Replace(Application.dataPath, "Assets");
        // 刷新 AssetDatabase 以确保路径被正确识别
        AssetDatabase.Refresh();

        // 在 Project 面板中打开指定路径
        EditorSceneManager.SetActiveScene(EditorSceneManager.GetActiveScene()); // 刷新编辑器
        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(relativePath));
    }

    private static void Open(string file)
    {
        string folderPath = "Assets/App/Res/_DynamicGroups/Prefabs/" + file;
        // 确保路径是有效的
        if (!Directory.Exists(folderPath))
        {
            Debug.LogError("Invalid folder path: " + folderPath);
            return;
        }

        // 将文件夹路径转换为 Unity 的 Assets 路径
        string relativePath = folderPath.Replace(Application.dataPath, "Assets");
        // 刷新 AssetDatabase 以确保路径被正确识别
        AssetDatabase.Refresh();

        // 在 Project 面板中打开指定路径
        EditorSceneManager.SetActiveScene(EditorSceneManager.GetActiveScene()); // 刷新编辑器
        EditorGUIUtility.PingObject(AssetDatabase.LoadAssetAtPath<Object>(relativePath));
    }

    private static void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        Debug.Log("PlayerPrefs 已全部清除！");
    }
}
#endif