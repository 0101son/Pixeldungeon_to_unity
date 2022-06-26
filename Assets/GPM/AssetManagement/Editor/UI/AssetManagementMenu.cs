using UnityEngine;
using UnityEditor;


namespace Gpm.AssetManagement.Ui
{
    using Gpm.AssetManagement.AssetMap;
    using Gpm.AssetManagement.AssetFind.Ui;

    public static class AssetManagementMenu
    {
        private const string MENU_ASSET_MANAGAMENT_ENABLE = "Tools/GPM/AssetManagement/Enable";
        [MenuItem(MENU_ASSET_MANAGAMENT_ENABLE)]
        public static void ToggleEnable()
        {
            GpmAssetManagementManager.Enable = !GpmAssetManagementManager.Enable;

            if (GpmAssetManagementManager.Enable)
            {
                CacheAssetDataAll();
                PlayerPrefs.SetInt("useDependency", 1);
            }
            else
            {
                PlayerPrefs.DeleteKey("useDependency");
            }
            PlayerPrefs.Save();
        }

        [MenuItem(MENU_ASSET_MANAGAMENT_ENABLE, true)]
        public static bool ToggleUseDependencyValidate()
        {
            Menu.SetChecked(MENU_ASSET_MANAGAMENT_ENABLE, GpmAssetManagementManager.Enable);
            return true;
        }

        [MenuItem("Tools/GPM/AssetManagement/Cache AssetData")]
        static public void CacheAssetDataAll()
        {
            GpmAssetManagementManager.CacheAssetDataAll();
        }


        [MenuItem("Tools/GPM/AssetManagement/Show AssetMap")]
        private static void OpenAssetMap()
        {
            AssetManagementWindow.Show(Selection.activeObject);
        }

        [MenuItem("Tools/GPM/AssetManagement/Find Reference")]
        static public void OpenAssetFindWindow()
        {
            AssetFindWindow.Find(null);
        }

        [MenuItem("CONTEXT/Object/Gpm Find Reference", false, -1)]
        private static void OpenAssetFindContext(MenuCommand command)
        {
            if (command.context is AssetImporter importer)
            {
                AssetFindWindow.Find(AssetDatabase.LoadMainAssetAtPath(importer.assetPath));
            }
            else
            {
                AssetFindWindow.Find(command.context);
            }
        }
        
        
        [MenuItem("Assets/Gpm Find Reference", false, -1)]
        private static void OpenAssetFind()
        {
            AssetFindWindow.Find(Selection.activeObject);
        }

        [MenuItem("GameObject/Gpm Find Reference", false, -1)]
        private static void OpenAssetFindGo()
        {
            AssetFindWindow.Find(Selection.activeObject);
        }

        [MenuItem("Assets/AssetFindTest", false, -1)]
        private static void AssetFindTest()
        {

            string path = AssetDatabase.GetAssetPath(Selection.activeObject);

            System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
            watch.Start();

            var value = new Gpm.AssetManagement.GetDependencyInfo(path);

            watch.Stop();
            UnityEngine.Debug.Log(watch.ElapsedMilliseconds + " ms");

            foreach (string depGuid in value.dependencys)
            {
                Debug.Log(depGuid);
            }

            int unkonwnMissing = 0;
            string guid;
            long localid;
            foreach (int instanceID in value.missingInstanceIDs)
            {
                if (AssetDatabase.TryGetGUIDAndLocalFileIdentifier(instanceID, out guid, out localid) == true)
                {
                    Debug.Log("missing : " + guid);
                }
                else
                {
                    unkonwnMissing++;
                }
            }

            Debug.Log("unkonwnMissing : " + unkonwnMissing);

            watch.Restart();

            var dep = UnityEditor.AssetDatabase.GetDependencies(path, false);

            watch.Stop();
            UnityEngine.Debug.Log(watch.ElapsedMilliseconds + " ms");

            foreach (string p in dep)
            {
                Debug.Log(p);
                Debug.Log(AssetDatabase.AssetPathToGUID(p));
            }
        }
    }
}