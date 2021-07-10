#if UNITY_EDITOR
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityObject = UnityEngine.Object;
using marijnz.EditorCoroutines;
using System.Collections;
using System.Diagnostics;
#if NET_2_0
using SearchMode = UnityEditor.SearchableEditorWindow.SearchMode;
#else
using static UnityEditor.SearchableEditorWindow;
#endif

namespace SearchPRO {
	[CustomEditor(typeof(InspectorRecentSO))]
	public class InspectorRecentSOEditor : Editor {

		private class Styles {

			public readonly Texture search_icon;

			public readonly GUIStyle window_background = (GUIStyle)"grey_border";

			public readonly GUIStyle label = EditorStyles.label;

			public readonly GUIStyle scroll_shadow;

			public readonly GUIStyle tag_button;

			public readonly GUIStyle search_bar;

			public readonly GUIStyle search_label;

			public readonly GUIStyle search_icon_item;

			public readonly GUIStyle search_title_item;

			public readonly GUIStyle search_description_item;

			public readonly GUIStyle on_search_title_item;

			public readonly GUIStyle on_search_description_item;

			public Styles() {
				search_icon = EditorGUIUtility.FindTexture("Search Icon");

				scroll_shadow = GlobalSkin.scrollShadow;

				tag_button = new GUIStyle(EditorStyles.miniButton);
				tag_button.richText = true;

				search_bar = GlobalSkin.searchBar;
				search_label = GlobalSkin.searchLabel;
				search_icon_item = GlobalSkin.searchIconItem;
				search_title_item = GlobalSkin.searchTitleItem;
				search_description_item = GlobalSkin.searchDescriptionItem;

				on_search_title_item = new GUIStyle(GlobalSkin.searchTitleItem);
				on_search_description_item = new GUIStyle(GlobalSkin.searchDescriptionItem);

				on_search_title_item.normal = GlobalSkin.searchTitleItem.onNormal;
				on_search_description_item.normal = GlobalSkin.searchDescriptionItem.onNormal;

				on_search_title_item.hover = GlobalSkin.searchTitleItem.onHover;
				on_search_description_item.hover = GlobalSkin.searchDescriptionItem.onHover;
			}
		}


		private static Styles styles;


		private const float WINDOW_HEAD_HEIGHT = 80.0f;

		private const float WINDOW_FOOT_OFFSET = 10.0f;

		private const double MAX_ELAPSED_MILLISECONDS = 25;

		private const string PREFS_ELEMENT_SIZE_SLIDER = "SearchPRO: SEW ElementSize Slider";

		private const string GUI_CONTROL_SEARCH_BOX = "GUIControlSearchBoxTextField";

		private readonly Color FOCUS_COLOR = new Color32(62, 125, 231, 255);

		private readonly Color STRIP_COLOR_DARK = new Color32(62, 62, 62, 255);

		private readonly Color STRIP_COLOR_LIGHT = new Color32(170, 170, 170, 255);

		private readonly Rect SEARCH_ICON_RECT = new Rect(20.0f, 13.0f, 23.0f, 23.0f);


		private static string search = "null";

		private static string new_search;

		private static bool init_refocus;

		private static bool need_refocus;

		private static bool drag_item;

		private static bool drag_scroll;

		private static bool enable_scroll;

		private static float scroll_pos;

		private static float element_list_height = 35;

		private static int gui_frame;

		private static int selected_index;

		private static EditorWindow inspector;

		private static InspectorRecentSO recent;

		private static SearchItem selected_item;

		private static List<SearchItem> main_list;

		private static Stopwatch search_stopwatch;

		private float elementSizeValue {
			get {
				return EditorPrefs.GetFloat(PREFS_ELEMENT_SIZE_SLIDER, 35);
			}
			set {
				EditorPrefs.SetFloat(PREFS_ELEMENT_SIZE_SLIDER, value);
			}
		}


#if NET_2_0
		private Rect position {
			get {
				return inspector.position;
			}
		}

		private bool hasSearch {
			get {
				return !search.IsNullOrEmpty();
			}
		}


		private int viewElementCapacity {
			get {
				return (int)((position.height - (WINDOW_HEAD_HEIGHT + (WINDOW_FOOT_OFFSET * 2))) / element_list_height) + 2;
			}
		}
#else
		private Rect position => inspector.position;

		private bool hasSearch => !search.IsNullOrEmpty();

		private int viewElementCapacity => (int)((position.height - (WINDOW_HEAD_HEIGHT + (WINDOW_FOOT_OFFSET * 2))) / element_list_height) + 2;
#endif

		[MenuItem("Window/Search PRO %SPACE")]
		public static void Init() {
			InspectorRecentSO instance = null;
			if (!instance) {
				instance = Resources.FindObjectsOfTypeAll<InspectorRecentSO>().FirstOrDefault();
				if (!instance) {
					instance = CreateInstance<InspectorRecentSO>();
				}
			}
			Selection.activeObject = instance;

			Type type = typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow");
			EditorWindow.FocusWindowIfItsOpen(type);
			CatchInspectorInstance();
			init_refocus = true;
			gui_frame = 0;
		}

		private void OnEnable() {
			CatchInspectorInstance();

			recent = (InspectorRecentSO)target;
			element_list_height = elementSizeValue;

			search_stopwatch = new Stopwatch();
			main_list = new List<SearchItem>(recent.items);

			drag_item = false;
			drag_scroll = false;
			need_refocus = true;

			EditorApplication.update += RefreshSearchControl;
		}

		static void CatchInspectorInstance() {
			if (!inspector) {
				Type type = typeof(EditorWindow).Assembly.GetType("UnityEditor.InspectorWindow");
				inspector = Resources.FindObjectsOfTypeAll<EditorWindow>().FirstOrDefault(win => win.titleContent.text == "Inspector");

				if (!inspector) {
					inspector = EditorWindow.GetWindow(type);
				}
				inspector.wantsMouseMove = true;
			}
		}

		private void OnDisable() {
			ClearSearch();
			EditorApplication.update -= RefreshSearchControl;
		}

		// Quick remove Inpector Header
		protected override void OnHeaderGUI() { }

		public override void OnInspectorGUI() {
			if (styles == null) {
				styles = new Styles();
			}

			KeyboardInputGUI();

			// A little detail to make Cursor Flash
			GUISkin gui_skin = GUI.skin;
			GUI.skin = GlobalSkin.skin;

			GUILayout.Space(40.0f);

			Rect search_rect = new Rect(15.0f, 10.0f, position.width - 30.0f, 30.0f);

			// Search Bar
			GUI.SetNextControlName(GUI_CONTROL_SEARCH_BOX);
			new_search = GUI.TextField(search_rect, new_search, styles.search_bar);
			GUI.skin = gui_skin;

			// Fix: Should not be capturing when there is a hotcontrol
			if (need_refocus || (init_refocus && gui_frame++ >= 1)) {
				if (init_refocus) {
					EditorGUIUtility.hotControl = EditorGUIUtility.GetControlID(FocusType.Keyboard, GUILayoutUtility.GetLastRect());
				}
				GUI.FocusControl(GUI_CONTROL_SEARCH_BOX);
				TextEditor txt = (TextEditor)GUIUtility.GetStateObject(typeof(TextEditor), GUIUtility.keyboardControl);
				if (txt != null) {
					txt.MoveLineEnd();
					txt.SelectNone();
				}
				init_refocus = false;
				need_refocus = false;
			}

			if (hasSearch) {
				Rect search_log_rect = new Rect(15.0f, 40, 300.0f, 20.0f);
#if NET_2_0
				GUI.Label(search_log_rect, string.Format("About {0} results in {1}ms", main_list.Count, search_stopwatch.ElapsedMilliseconds), "minilabel");
#else
				GUI.Label(search_log_rect, $"About {main_list.Count} results in {search_stopwatch.ElapsedMilliseconds}ms", "minilabel");
#endif
			}
			else {
				// Create a ghost label while search box is empty
				GUI.Label(search_rect, "Type your search...", styles.search_label);
			}

			// A little hack to make Inspector Clip draw all elements
			GUILayout.BeginVertical(GUILayout.MinHeight(position.height - 52.0f));
			GUILayout.Label(GUIContent.none);
			GUILayout.EndVertical();

			// Draw Search Icon
			GUI.DrawTexture(SEARCH_ICON_RECT, styles.search_icon);

			int list_items_count = main_list == null ? 0 : main_list.Count;

			enable_scroll = viewElementCapacity - 2 < list_items_count;

			// Draw Search Icon
			Rect list_area = new Rect(1.0f, WINDOW_HEAD_HEIGHT, position.width - (enable_scroll ? 19.0f : 2.0f), position.height - (WINDOW_HEAD_HEIGHT + WINDOW_FOOT_OFFSET));

			if (enable_scroll) {
				scroll_pos = GUI.VerticalScrollbar(new Rect(position.width - 17.0f, WINDOW_HEAD_HEIGHT, 20.0f, list_area.height), scroll_pos, 1.0f, 0.0f, list_items_count - viewElementCapacity + 3);
			}
			else {
				scroll_pos = 0.0f;
			}
			scroll_pos = Mathf.Clamp(scroll_pos, 0.0f, list_items_count);

			Rect slider_rect = new Rect(position.width - 70.0f, 40.0f, 50.0f, 20.0f);
			element_list_height = Mathf.Round(GUI.HorizontalSlider(slider_rect, (int)element_list_height, 25, 50) / 5) * 5;

			// Clip elements to avoid overlapping the search box
			GUI.BeginClip(list_area);

			PreInputGUI();

			// Clip elements to avoid overlapping the search box
			int first_scroll_index = (int)Mathf.Max(scroll_pos, 0);
			int last_scroll_index = (int)Mathf.Min(scroll_pos + viewElementCapacity, list_items_count);

			int draw_index = 0;
			// The secret of the infinite list
			for (int id = first_scroll_index; id < last_scroll_index; id++) {
				SearchItem item = main_list[id];
				bool selected = false;

				float smooth_offset = (scroll_pos - id) * element_list_height;

				Rect layout_rect = new Rect(0.0f, draw_index - smooth_offset, list_area.width, element_list_height);

				if (id % 2 == 0) {
					EditorGUI.DrawRect(layout_rect, EditorGUIUtility.isProSkin ? STRIP_COLOR_DARK : STRIP_COLOR_LIGHT);
				}

				bool mouse_hover = layout_rect.Contains(Event.current.mousePosition);

				//Draw Selection Box
				if (selected_index == draw_index + first_scroll_index || (mouse_hover && Event.current.type == EventType.MouseMove)) {
					selected = true;
					selected_item = item;
					selected_index = draw_index + first_scroll_index;
					EditorGUI.DrawRect(layout_rect, FOCUS_COLOR);
				}

				var icon = AssetDatabase.GetCachedIcon(item.path);
				if (!icon && item.type != null) {
					icon = EditorGUIUtility.ObjectContent(null, item.type).image;
				}

				//Draw Element Button
				if (DrawElementList(layout_rect, new GUIContent(item.title, icon, item.description), selected, mouse_hover)) {
					DoItemFunction(item);
					break;
				}
				draw_index++;
			}
			PostInputGUI();

			GUI.EndClip();

			if (enable_scroll && scroll_pos != 0.0f) {
				Color gui_color = GUI.color;
				if (scroll_pos < 1.0f) {
					GUI.color = new Color(gui_color.r, gui_color.g, gui_color.b, gui_color.a * scroll_pos);
				}
				GUI.Box(new Rect(0.0f, WINDOW_HEAD_HEIGHT, position.width - 15.0f, 10.0f), GUIContent.none, styles.scroll_shadow);
				GUI.color = gui_color;
			}

			//Repaint();
		}

		void DoItemFunction(SearchItem item) {
			ClearSearch();

			if (item && item.scene_object) {
				Selection.activeObject = EditorUtility.InstanceIDToObject(item.id);
			}
			else {
				Selection.activeObject = AssetDatabase.LoadAssetAtPath<UnityObject>(item.path);
			}
		}

		void ClearSearch() {
			EditorCoroutines.StopCoroutine(StartFakeAsyncSearch(), this);
			main_list.Clear();
			search = string.Empty;
			new_search = string.Empty;
			SetSearchFilter(string.Empty, SearchMode.All, true, false);
			//Resources.UnloadUnusedAssets();
		}

		void SetSearchFilter(string filter, SearchMode mode = SearchMode.All, bool setAll = false, bool delayed = false) {
			foreach (SearchableEditorWindow window in Resources.FindObjectsOfTypeAll<SearchableEditorWindow>()) {
				var args = new object[] { filter, mode, setAll, delayed };
				typeof(SearchableEditorWindow).InvokeMethodFrom("SetSearchFilter", window, args);
			}
		}

		void RefreshSearchControl() {
			if (search != new_search) {
				EditorCoroutines.StopCoroutine(StartFakeAsyncSearch(), this);
				main_list.Clear();
				if (new_search.IsNullOrEmpty()) {
					main_list = new List<SearchItem>(recent.items);
				}
				else {
					EditorCoroutines.StartCoroutine(StartFakeAsyncSearch(), this);
				}

				SetSearchFilter(new_search, SearchMode.All);
				search = new_search;
			}
		}

		IEnumerator StartFakeAsyncSearch() {
			string escape = Regex.Escape(new_search);
			Stopwatch stop_watch = new Stopwatch();
			stop_watch.Start();
#if NET_2_0
			search_stopwatch.Reset();
			search_stopwatch.Start();
#else
			search_stopwatch.Restart();
#endif

			bool has_filter = new_search.Contains(":");

			GameObject[] objects = FindObjectsOfType<GameObject>();
			foreach (GameObject obj in objects) {
				if (obj.activeInHierarchy) {
					if ((has_filter && Regex.IsMatch(escape, "Object", RegexOptions.IgnoreCase)) || Regex.IsMatch(obj.name, escape, RegexOptions.IgnoreCase)) {
						main_list.Add(new SearchItem(obj));
					}
					if (stop_watch.ElapsedMilliseconds >= MAX_ELAPSED_MILLISECONDS) {
						yield return null;
#if NET_2_0
						stop_watch.Reset();
						stop_watch.Start();
#else
						stop_watch.Restart();
#endif
					}

					foreach (Component component in obj.GetComponents<Component>()) {
						Type type = component.GetType();
						string name = type.Name;
						if (Regex.IsMatch(name, escape, RegexOptions.IgnoreCase)) {
							main_list.Add(new SearchItem(component));
						}
						if (stop_watch.ElapsedMilliseconds >= MAX_ELAPSED_MILLISECONDS) {
							yield return null;
#if NET_2_0
							stop_watch.Reset();
							stop_watch.Start();
#else
							stop_watch.Restart();
#endif
						}
					}
				}
			}

			foreach (string path in has_filter ? AssetDatabase.FindAssets(new_search.ToLower()).Select(guid => AssetDatabase.GUIDToAssetPath(guid)) : AssetDatabase.GetAllAssetPaths()) {
				string[] split = path.Split('/');
				string title = split[split.Length - 1];
				if (has_filter || Regex.IsMatch(title, escape, RegexOptions.IgnoreCase) || Regex.IsMatch(path, escape, RegexOptions.IgnoreCase)) {
					string description = path;
					if (!AssetDatabase.IsValidFolder(path)) {
						long file_size = new FileInfo(path.Replace(Application.dataPath, "Assets").Replace("\\", "/")).Length;
						description = string.Format("{0} ({1})", path, EditorUtility.FormatBytes(file_size));
					}

					main_list.Add(new SearchItem(title, description, path));
				}

				if (stop_watch.ElapsedMilliseconds >= MAX_ELAPSED_MILLISECONDS) {
					yield return null;
#if NET_2_0
					stop_watch.Reset();
					stop_watch.Start();
#else
					stop_watch.Restart();
#endif
				}
			}
			search_stopwatch.Stop();
		}

		public string HighlightText(string text) {
			if (text.IsNullOrEmpty() || !hasSearch) return text;
#if NET_2_0
			return Regex.Replace(text, Regex.Escape(search), (match) => "<color=#FFFF00><b>" + match + "</b></color>", RegexOptions.IgnoreCase);
#else
			return Regex.Replace(text, Regex.Escape(search), (match) => $"<color=#FFFF00><b>{match}</b></color>", RegexOptions.IgnoreCase);
#endif
		}

		public bool DrawElementList(Rect layout_rect, GUIContent content, bool selected, bool mouse_over) {
			bool trigger = false;
			Event current = Event.current;

			// My custom button =]
			if (mouse_over && current.type == EventType.MouseUp) {
				trigger = true;
				current.Use();
			}

			Rect icon_rect = new Rect(layout_rect.x + 10.0f, layout_rect.y, element_list_height, element_list_height);
			Rect title_rect = new Rect(element_list_height + 5.0f, layout_rect.y, layout_rect.width - element_list_height - 10.0f, layout_rect.height);
			Rect subtitle_rect = new Rect(title_rect);

			GUI.Label(icon_rect, content.image, styles.search_icon_item);
			if (!search.IsNullOrEmpty()) {
				string title = HighlightText(content.text);
				EditorGUI.LabelField(title_rect, title, selected ? styles.on_search_title_item : styles.search_title_item);

				if (element_list_height > 30) {
					string subtitle = HighlightText(content.tooltip);
					EditorGUI.LabelField(subtitle_rect, subtitle, selected ? styles.on_search_description_item : styles.search_description_item);
				}
			}
			else {
				EditorGUI.LabelField(title_rect, content.text, selected ? styles.on_search_title_item : styles.search_title_item);
				if (element_list_height > 30) {
					EditorGUI.LabelField(subtitle_rect, content.tooltip, selected ? styles.on_search_description_item : styles.search_description_item);
				}
			}

			return !drag_scroll && !drag_item && trigger;
		}

		void PreInputGUI() {
			Event current = Event.current;

			switch (current.type) {
				case EventType.MouseDown:
				drag_item = false;
				drag_scroll = false;
				break;
				case EventType.ScrollWheel:
				drag_item = false;
				drag_scroll = true;
				scroll_pos += current.delta.y;
				current.Use();
				break;
				case EventType.MouseDrag:
				if (!drag_scroll && !drag_item && (!enable_scroll || Mathf.Abs(current.delta.x) > 0.8f && Mathf.Abs(current.delta.y) < 0.5f)) {
					if (selected_item) {
						DragAndDrop.PrepareStartDrag();
						UnityObject obj = null;
						if (selected_item.scene_object) {
							obj = EditorUtility.InstanceIDToObject(selected_item.id);
						}
						else {
							obj = AssetDatabase.LoadAssetAtPath<UnityObject>(selected_item.path);
						}
						DragAndDrop.objectReferences = new UnityObject[] { obj };
						DragAndDrop.StartDrag("Dragging " + selected_item.title);
						Event.current.Use();
						drag_item = true;
					}
					else {
						drag_item = false;
						drag_scroll = true;
					}
				}
				if (!drag_item) {
					drag_scroll = true;
					scroll_pos -= current.delta.y / element_list_height;
					current.Use();
				}
				break;
			}
		}

		void PostInputGUI() {
			Event current = Event.current;

			switch (current.type) {
				case EventType.KeyDown:
				drag_item = false;
				break;
				case EventType.MouseUp:
				drag_item = false;
				drag_scroll = false;
				break;
			}
		}

		void KeyboardInputGUI() {
			Event current = Event.current;

			switch (current.type) {
				case EventType.MouseUp:
				if (element_list_height != elementSizeValue) {
					elementSizeValue = element_list_height;
				}
				break;
				case EventType.KeyDown:
				//new_search += current.character;
				if (current.keyCode == KeyCode.Escape) {
					ClearSearch();
					Selection.activeObject = recent.last;
				}
				if (!current.control) {
					if (current.keyCode == KeyCode.Home) {
						selected_index = 0;
						scroll_pos = 0.0f;
						current.Use();
					}
					else if (current.keyCode == KeyCode.End) {
						selected_index = main_list.Count - 1;
						scroll_pos = main_list.Count;
						current.Use();
					}
					else if (current.keyCode == KeyCode.PageDown) {
						selected_index += viewElementCapacity;
						scroll_pos += viewElementCapacity;
						if (selected_index >= main_list.Count) {
							selected_index = 0;
							scroll_pos = 0.0f;
						}
						current.Use();
					}
					else if (current.keyCode == KeyCode.PageUp) {
						selected_index -= viewElementCapacity;
						scroll_pos -= viewElementCapacity;
						if (selected_index < 0) {
							selected_index = main_list.Count - 1;
							scroll_pos = main_list.Count;
						}
						current.Use();
					}
					else if (current.keyCode == KeyCode.DownArrow) {
						selected_index++;
						if (selected_index >= scroll_pos + viewElementCapacity - 2) {
							scroll_pos++;
						}
						if (selected_index >= main_list.Count) {
							selected_index = 0;
							scroll_pos = 0.0f;
						}
						current.Use();
					}
					else if (current.keyCode == KeyCode.UpArrow) {
						selected_index--;
						if (selected_index < scroll_pos) {
							scroll_pos--;
						}
						if (selected_index < 0) {
							selected_index = main_list.Count - 1;
							scroll_pos = main_list.Count;
						}
						current.Use();
					}
					else if ((current.keyCode == KeyCode.Return) || (current.keyCode == KeyCode.KeypadEnter)) {
						DoItemFunction(selected_item);
						current.Use();
					}
				}
				break;
			}
		}
	}
}
#endif
