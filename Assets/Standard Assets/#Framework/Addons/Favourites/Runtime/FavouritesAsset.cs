using GameEngine.Attributes;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace FavouritesEd {

[PreloadSetting]
public class FavouritesAsset : ScriptableObject {

    public List<FavouritesElement> favs = new List<FavouritesElement>();
    public List<FavouritesCategory> categories = new List<FavouritesCategory>();

    public static FavouritesAsset instance;
    public Action RefleshAction;

    void OnEnable()
    {
        Debug.Log("Favourites loaded");
        instance = this;
    }

    [SerializeField]
    private int nextCategoryId = 0;

    public string AddObject(string category, Object target)
    {
        if (category.IsNullOrWhitespace() || target == null) return null;
        var obj = favs.FirstOrDefault(t => t.obj == target);

        if (obj != null) {
            return categories.FirstOrDefault(t => t.id == obj.categoryId)?.name;
        }
        var cat = AddCategory(category);
        favs.Add(new FavouritesElement() {
            categoryId = cat.id,
            obj = target
        });

        RefleshAction?.Invoke();

    #if UNITY_EDITOR
        EditorUtility.SetDirty(this);
        AssetDatabase.SaveAssets();
    #endif

        return cat.name;
    }

    public FavouritesCategory AddCategory(string catName)
    {
        var exists = categories.FirstOrDefault(t => t.name == catName);

        if (exists != null) {
            return exists;
        }
        FavouritesCategory c = new FavouritesCategory() {
            id = nextCategoryId,
            name = catName,

            // proto = proto,
        };

        // #if UNITY_EDITOR
        //     c.path = UnityEditor.AssetDatabase.GetAssetPath(proto);
        // #endif
        nextCategoryId++;
        categories.Add(c);

        return c;
    }

    // ------------------------------------------------------------------------------------------------------------

}

}