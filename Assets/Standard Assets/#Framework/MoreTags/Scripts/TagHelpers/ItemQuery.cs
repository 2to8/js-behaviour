using GameEngine.Extensions;
using Sirenix.Utilities;
using System.Collections.Generic;
using System.Linq; //using ReactECS.Extensions;

namespace MoreTags.TagHelpers {

public enum FilterType {

    All, Any, None

}


public class ItemQuery {

    static List<TagData> TagDatas = new List<TagData>();
    public Dictionary<FilterType, List<List<int>>> items;

    public ItemQuery()
    {
        if (!TagDatas.Any()) {
            TagDatas = TagData.table.FetchAll();
        }
        items = new Dictionary<FilterType, List<List<int>>>() {
            [FilterType.All] = new List<List<int>>(),
            [FilterType.Any] = new List<List<int>>(),
            [FilterType.None] = new List<List<int>>()
        };
    }

    public static ItemQuery NewQuery() => new ItemQuery();

    public static List<List<string>> getList() => new List<List<string>>();

    public ItemQuery Add(FilterType type, List<List<string>> taglist)
    {
        taglist = taglist.Select(t => t.Select(tn => tn._TagKey()).ToList()).ToList();
        taglist.ForEach(t => TagSystem.AddTag(t.ToArray()));

        if (!items.ContainsKey(type)) {
            items[type] = new List<List<int>>();
        }
        taglist.ForEach(tags => {
            if (!items[type]
                .Any(t => {
                    var result = t.Count == tags.Count;

                    if (result) {
                        t.ForEach((s, i) => {
                            if (s != TagDatas.Where(ts => ts.name == tags[i]).Select(ts => ts.Id).FirstOrDefault()) {
                                result = false;
                            }
                        });
                    }

                    return result;
                })) {
                items[type]
                    .Add(tags.Select(tn => TagDatas.Where(ts => ts.name == tn).Select(ts => ts.Id).FirstOrDefault())
                        .ToList());
            }
        });

        return this;
    }

    public List<Tags> FetchAll(ItemQuery data)
    {
        //var tagList = query.ToComponentArray<Tags>();
        var final = new List<Tags>();

        // 每次重新检索 entities, 因此上个系统可以实时修改状态
        //using (var list = GetEntityQuery(typeof(ReactTags)).ToEntityArray(Allocator.Temp)) {
        var allTags = new List<Tags>();
        TagSystem.refs.Select(tag => tag.Value.gameObjects)
            .ForEach(t => {
                t.ForEach(go => {
                    var tags = go?.GetComponent<Tags>();

                    if (tags != null) {
                        allTags.Add(tags);
                    }
                });
            });

        List<Tags> ParentContains(List<int> find)
        {
            var ret = find.Any() ? allTags.Where(t => t.ids.Contains(find.Last())).ToList() : new List<Tags>();

            if (!find.Any()) return ret;

            if (find.Count > 1) {
                var nw = find.ToList();
                nw.Reverse();
                ret.ToList()
                    .ForEach(t => {
                        var check = true;
                        t.GetComponentsInParent<Tags>(true)
                            .ForEach((c, i) => {
                                if (!c.ids.Contains(nw[i])

                                    // || i < nw.Count - 1 && c.ids.Contains(nw[i + 1])) { } else
                                ) {
                                    check = false;
                                }
                            });

                        if (!check) {
                            ret.Remove(t);
                        }
                    });
            }

            return ret;
        }

        var has = data.items[FilterType.All].Any();

        if (has) {
            data.items[FilterType.All]
                .ForEach((t, i) => {
                    var r = ParentContains(t);

                    if (i == 0) {
                        final = r; // 任何一项结果都不能为空,否则结果为空
                    } else if (r.Any()) {
                        final.RemoveAll(t1 => !r.Contains(t1));
                    } else {
                        final.Clear(); // 任何一项结果都不能为空,否则结果为空
                    }
                });
        }

        if (data.items[FilterType.Any].Any()) {
            var tmp = new List<Tags>();
            data.items[FilterType.Any].ForEach((t, i) => tmp = tmp.Concat(ParentContains(t)).Distinct().ToList());

            if (has && tmp.Any()) {
                // 有 all 和 any 的时候两个都要满足, any 必须存在一个
                final.RemoveAll(t => !tmp.Contains(t));
            } else if (!has) {
                // 没有 all 只有 any 的时候, 取 any 的匹配
                final = tmp;
            }
        }

        if (data.items[FilterType.None].Any() && final.Any()) {
            data.items[FilterType.None].ForEach(t => final.RemoveAll(t1 => ParentContains(t).Contains(t1)));
        }

        //}
        return final;
    }

    // public List<Tags> FetchAll()
    // {
    //     return null;
    //
    //     //return RouteProxy.instance.FetchAll(this);
    // }

}

}