#if UNITY_EDITOR
using System.Linq;
using UnityEngine;
using UnityEditor;

namespace SearchPRO {
	[InitializeOnLoad]
	public static class InspectorInitialize {

		private static InspectorRecentSO instance;

		static InspectorInitialize() {
			EditorApplication.update += ForceRecentSelection;
			Selection.selectionChanged += OnSelectionChange;
		}

		static void OnSelectionChange() {
			if (Selection.activeObject && Selection.activeObject != instance) {
				instance?.OnSelectionChange();
			}
		}

		public static void ForceRecentSelection() {
			if (!instance) {
				instance = Resources.FindObjectsOfTypeAll<InspectorRecentSO>().FirstOrDefault();
				if (!instance) {
					instance = ScriptableObject.CreateInstance<InspectorRecentSO>();
				}
			}

			if (Selection.activeObject == instance) return;

			if (!Selection.activeObject) {
				Selection.activeObject = instance;
			}
		}
	}
}
#endif
