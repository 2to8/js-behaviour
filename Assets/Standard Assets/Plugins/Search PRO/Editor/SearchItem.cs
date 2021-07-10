using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityObject = UnityEngine.Object;
using UnityEditor;
using System;

namespace SearchPRO {
	[Serializable]
	public class SearchItem {

		public int id;

		public string title;

		public string description;

		public string path;

		public bool scene_object;

		public Type type;

		public SearchItem() { }

		public SearchItem(UnityObject obj) {
			this.id = obj.GetInstanceID();
			this.title = obj.name;
			type = obj.GetType();
			if (obj is GameObject) {
				GameObject go = (obj as GameObject);
				this.scene_object = go.activeInHierarchy;
				string scene_name = string.IsNullOrEmpty(go.scene.name) ? "Non-Saved" : go.scene.name;
#if NET_2_0
				this.description = string.Format("GameObject: {0} on '{1}' Scene", obj.name, scene_name);
#else
				this.description = $"GameObject: {obj.name} on '{scene_name}' Scene";
#endif
			}
			if (obj is Component) {
				GameObject go = (obj as Component).gameObject;
				this.scene_object = go.activeInHierarchy;
#if NET_2_0
				this.description = string.Format("Component: {0} in '{1}' GameObject", type.Name, go.name);
#else
				this.description = $"Component: '{type.Name}' in '{go.name}' GameObject";
#endif
			}
			if (!scene_object) {
				this.path = AssetDatabase.GetAssetPath(this.id);
				this.description = path;
			}
		}

		public SearchItem(string title, string description, int id, Type type = null) {
			this.id = id;
			this.title = title;
			this.description = description;
			this.type = type;
			this.scene_object = true;
		}

		public SearchItem(string title, string description, string path, Type type = null) {
			this.title = title;
			this.description = description;
			this.path = path;
			this.type = type;
		}

		public override bool Equals(object o) {
			return CompareBaseObjects(this, o as SearchItem);
		}

		public override int GetHashCode() {
			return this.id;
		}

		public static implicit operator bool(SearchItem exists) {
			return !CompareBaseObjects(exists, null);
		}

		public static bool operator ==(SearchItem x, SearchItem y) {
			return CompareBaseObjects(x, y);
		}

		public static bool operator !=(SearchItem x, SearchItem y) {
			return !CompareBaseObjects(x, y);
		}

		private static bool CompareBaseObjects(SearchItem lhs, SearchItem rhs) {
			bool flag1 = (object)lhs == null;
			bool flag2 = (object)rhs == null;
			if (flag2 && flag1) {
				return true;
			}
			if (flag1 || flag2) {
				return false;
			}
			return lhs.id == rhs.id;
		}
	}
}