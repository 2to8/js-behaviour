#if UNITY_EDITOR
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;
using UnityEditor;

namespace SearchPRO {
	public class InspectorRecentSO : ScriptableObject {

		public UnityObject last;

		private const int max_itens = 10;

		public List<SearchItem> items = new List<SearchItem>();

		public void OnSelectionChange() {
			if (!items.FirstOrDefault(item => item.id == Selection.activeObject.GetInstanceID())) {
				items.Insert(0, new SearchItem(Selection.activeObject));
				if (items.Count > max_itens) {
					items.RemoveAt(items.Count - 1);
				}
			}
			if (Selection.activeObject && Selection.activeObject != this) {
				last = Selection.activeObject;
			}
		}
	}
}
#endif

