#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;

namespace SearchPRO {
	public static class GlobalSkin {

		private static GUISkin m_skin;

		public static GUISkin skin {
			get {
				if (!m_skin) return m_skin = FindAssetByName<GUISkin>(EditorGUIUtility.isProSkin ? "SPDarkSkin" : "SPLightSkin");
				return m_skin;
			}
		}

		public static GUIStyle scrollShadow {
			get {
				return GetStyle("Scroll Shadow");
			} 
		}

		public static GUIStyle searchBar {
			get {
				return GetStyle("Search Bar");
			}
		}

		public static GUIStyle searchLabel {
			get {
				return GetStyle("Search Label");
			}
		}

		public static GUIStyle searchIconItem {
			get {
				return GetStyle("Search Icon Item");
			}
		}

		public static GUIStyle searchTitleItem {
			get {
				return GetStyle("Search Title Item");
			}
		}

		public static GUIStyle searchDescriptionItem {
			get {
				return GetStyle("Search Description Item");
			}
		}

		public static GUIStyle GetStyle(string style_name) {
			return skin.FindStyle(style_name);
		}

		public static T FindAssetByName<T>(string name) where T : UnityObject {
			if (name == string.Empty) return null;
			T result = Resources.Load<T>(name);
			if (result) {
				return result;
			}

			T[] assets = (T[])Resources.FindObjectsOfTypeAll<T>();
			foreach (T asset in assets) {
				if (asset.name == name) {
					return asset;
				}
			}
			string[] GUIDs = AssetDatabase.FindAssets(name);
			foreach (string GUID in GUIDs) {
				if (AssetDatabase.GUIDToAssetPath(GUID).Contains(name)) {
					return AssetDatabase.LoadAssetAtPath<T>(AssetDatabase.GUIDToAssetPath(GUID));
				}
			}
			return null;
		}
	}
}
#endif