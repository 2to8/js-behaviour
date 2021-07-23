using Common;
using GameEngine.Extensions;
using UnityEngine;
using UnityEngine.UI;

namespace Presets
{
    public class GenMapButton : View<GenMapButton>
    {
        Button btn;
        GenMapManager manager;

        public static void RefreshLayoutGroupsImmediateAndRecursive(GameObject root)
        {
            foreach (var layoutGroup in root.GetComponentsInParent<LayoutGroup>())
                LayoutRebuilder.ForceRebuildLayoutImmediate(layoutGroup.GetComponent<RectTransform>());
        }

        public override void Start()
        {
            base.Start();
            btn = GetComponent<Button>();
            if (btn == null) {
                Debug.Log("not contain button", gameObject);
                return;
            }

            manager = GenMapManager.instance;
            var data = manager.map ?? manager.originMapData;
            if (data == null) return;
            var index = transform.GetSiblingIndex();
            var y = index / 9;
            var x = index % 9;
            if (data.data[x, y] > 0) {
                var color = data.colors[data.data[x, y] % data.colors.Count];
                color.a = 1f;
                btn.GetComponent<Graphic>()?.Tap(t => t.color = color); //.color = color;

                // btn.GetComponentInChildren<Image>().color =
                //     data.colors[data.data[x, y] % data.colors.Count];
                btn.GetComponentInChildren<Text>().text = data.data[x, y].ToString();
            }
            else {
                btn.GetComponent<Graphic>().color = Color.white;
                btn.GetComponentInChildren<Text>().text = data.data[x, y] == 0 ? $"({x},{y})" : $"{data.data[x, y]}";
            }

            btn.onClick.AddListener(() => {
                if (data.locked) return;
                Debug.Log(
                    $"x: {x}, y: {y} current: {data.current} id: {data.last} color: {data.colors[data.data[x, y] % data.colors.Count]}");
                if (data.data[x, y] == 0) {
                    data.current += 1;
                    if (data.last <= 0) data.last = 1;
                    if (data.current > 4) {
                        data.last += 1;
                        data.current = 1;
                    }

                    data.data[x, y] = data.last;

                    // var colors = GetComponent<Button>().colors;
                    // colors.normalColor = data.colors[data.last % data.colors.Count];
                    // GetComponent<Button>().colors = colors;
                    var color = data.colors[data.data[x, y] % data.colors.Count];
                    color.a = 1f;
                    btn.GetComponent<Graphic>().color = color;

                    // btn.GetComponentInChildren<Image>().color =
                    //     data.colors[data.last % data.colors.Count];
                    btn.GetComponentInChildren<Text>().text = data.data[x, y].ToString();
                }
                else {
                    data.current -= 1;
                    if (data.current <= 0) {
                        if (data.last > 0) data.last -= 1;
                        data.current = 4;
                    }

                    data.data[x, y] = 0;
                    btn.GetComponent<Graphic>().color = Color.white;

                    // var colors = GetComponent<Button>().colors;
                    // colors.normalColor = Color.white;
                    // GetComponent<Button>().colors = colors;
                    // btn.GetComponentInChildren<Image>().color = Color.white;
                    btn.GetComponentInChildren<Text>().text = "";
                }

                //RefreshLayoutGroupsImmediateAndRecursive(gameObject);
            });
        }
    }
}