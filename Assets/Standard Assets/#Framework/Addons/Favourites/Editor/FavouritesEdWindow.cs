using Sirenix.OdinInspector.Editor;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditor.SceneManagement;

namespace FavouritesEd
{
    public class FavouritesEdWindow : EditorWindow
    {
        private static readonly GUIContent GC_Add = new GUIContent("+", "Add category");
        private static readonly GUIContent GC_Reflash = new GUIContent("R", "Reflash category");
        private static readonly GUIContent GC_Remove = new GUIContent("-", "Remove selected");
        public static FavouritesEdWindow instance;

        [SerializeField, HideInInspector]
        private FavouritesAsset asset;

        [SerializeField, HideInInspector]
        private TreeViewState treeViewState;

        [SerializeField, HideInInspector]
        private FavouritesTreeView treeView;

        [SerializeField, HideInInspector]
        private SearchField searchField;

        // ------------------------------------------------------------------------------------------------------------------

        [MenuItem("Window/Favourites")]
        private static void ShowWindow()
        {
            GetWindow<FavouritesEdWindow>("Favourites").UpdateTreeview();
        }

        private void OnHierarchyChange()
        {
            if (Application.isPlaying) return;
            UpdateTreeview();
        }

        void OnEnable()
        {
            instance = this;
        }

        private void OnProjectChange()
        {
            UpdateTreeview();
        }

        bool firstUpdate;

        public void UpdateTreeview()
        {
            if (asset == null) {
                LoadAsset();
            }

            if (treeViewState == null) treeViewState = new TreeViewState();
            if (treeView == null) {
                searchField = null;
                treeView = new FavouritesTreeView(treeViewState);
            }

            if (searchField == null) {
                searchField = new SearchField();
                searchField.downOrUpArrowKeyPressed += treeView.SetFocusAndEnsureSelectedItem;
            }

            treeView?.LoadAndUpdate(asset);
            Repaint();
            asset.RefleshAction = () => { UpdateTreeview(); };
            if (!firstUpdate) {
                Debug.Log("Repaint");
                firstUpdate = true;
            }
        }

        // ------------------------------------------------------------------------------------------------------------------

        protected void OnGUI()
        {
            if (treeView == null) {
                UpdateTreeview();
            }

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                if (GUILayout.Button(GC_Reflash, EditorStyles.toolbarButton, GUILayout.Width(25))) {
                    Debug.Log("test");
                    UpdateTreeview();
                }

                treeView.searchString = searchField.OnToolbarGUI(treeView.searchString, GUILayout.ExpandWidth(true));
                GUILayout.Space(5);
                if (GUILayout.Button(GC_Add, EditorStyles.toolbarButton, GUILayout.Width(25))) {
                    TextInputWindow.ShowWindow("Favourites", "Enter category name", "", AddCategory, null);
                }

                GUI.enabled = treeView.Model.Data.Count > 0;
                if (GUILayout.Button(GC_Remove, EditorStyles.toolbarButton, GUILayout.Width(25))) {
                    RemoveSelected();
                }

                GUI.enabled = true;
            }
            EditorGUILayout.EndHorizontal();
            treeView.OnGUI();
        }

        // ------------------------------------------------------------------------------------------------------------------

        private void AddCategory(TextInputWindow wiz)
        {
            string s = wiz.Text;
            wiz.Close();
            if (string.IsNullOrEmpty(s)) return;
            AddCategory(s);
        }

        public void AddCategory(string s)
        {
            asset.AddCategory(s);
            EditorUtility.SetDirty(asset);
            Debug.Log("set dirty");
            UpdateTreeview();
            Repaint();
            Debug.Log("Repaint");
        }

        private void RemoveSelected()
        {
            IList<int> ids = treeView.GetSelection();
            if (ids.Count == 0) return;
            FavouritesTreeElement ele = treeView.Model.Find(ids[0]);
            if (ele == null) return;
            if (ele.category != null) {
                // remove elements from open scene. those in closed scenes will just
                // have to stay. they will not show up anyway if category is gone

                // remove from scene
                foreach (FavouritesContainer c in FavouritesEd.Containers) {
                    if (c == null || c.favs == null) continue;
                    for (int i = c.favs.Count - 1; i >= 0; i--) {
                        if (c.favs[i].categoryId == ele.category.id) {
                            c.favs.RemoveAt(i);
                            EditorSceneManager.MarkSceneDirty(c.gameObject.scene);
                            Debug.Log("set dirty");
                        }
                    }
                }

                // remove favourites linked to this category
                for (int i = asset.favs.Count - 1; i >= 0; i--) {
                    if (asset.favs[i].categoryId == ele.category.id) asset.favs.RemoveAt(i);
                }

                // remove category
                for (int i = 0; i < asset.categories.Count; i++) {
                    if (asset.categories[i].id == ele.category.id) {
                        asset.categories.RemoveAt(i);
                        break;
                    }
                }

                EditorUtility.SetDirty(asset);
                Debug.Log("set dirty");
            }
            else {
                bool found = false;
                for (int i = 0; i < asset.favs.Count; i++) {
                    if (asset.favs[i] == ele.fav) {
                        found = true;
                        asset.favs.RemoveAt(i);
                        EditorUtility.SetDirty(asset);
                        Debug.Log("set dirty");
                        break;
                    }
                }

                if (!found) {
                    foreach (FavouritesContainer c in FavouritesEd.Containers) {
                        if (c == null || c.favs == null) continue;
                        for (int i = 0; i < c.favs.Count; i++) {
                            if (c.favs[i] == ele.fav) {
                                found = true;
                                c.favs.RemoveAt(i);
                                EditorSceneManager.MarkSceneDirty(c.gameObject.scene);
                                Debug.Log("set dirty");
                                break;
                            }
                        }

                        if (found) break;
                    }
                }
            }

            UpdateTreeview();
            Repaint();
            Debug.Log("Repaint");
        }

        public FavouritesAsset LoadAsset()
        {
            string[] guids = AssetDatabase.FindAssets("t:FavouritesAsset");
            string fn = (guids.Length > 0
                ? AssetDatabase.GUIDToAssetPath(guids[0])
                : GetPackageFolder() + "FavouritesAsset.asset");
            asset = AssetDatabase.LoadAssetAtPath<FavouritesAsset>(fn);
            if (asset == null) {
                asset = CreateInstance<FavouritesAsset>();
                AssetDatabase.CreateAsset(asset, fn);
                AssetDatabase.SaveAssets();
            }

            return asset;
        }

        private string GetPackageFolder()
        {
            try {
                string[] res = System.IO.Directory.GetFiles(Application.dataPath, "FavouritesEdWindow.cs",
                    System.IO.SearchOption.AllDirectories);
                if (res.Length > 0)
                    return "Assets" + res[0].Replace(Application.dataPath, "").Replace("FavouritesEdWindow.cs", "")
                        .Replace("\\", "/");
            }
            catch (System.Exception ex) {
                Debug.LogException(ex);
            }

            return "Assets/";
        }

        // ------------------------------------------------------------------------------------------------------------------
    }
}