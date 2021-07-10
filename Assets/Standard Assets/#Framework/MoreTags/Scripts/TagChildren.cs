using System.Linq;

namespace MoreTags {

public class TagChildren : TagNames {

    public readonly string name;
    public TagNames children => GetTagNames();
    public TagNames all => GetTagNames(true);

    public TagChildren(string n)
    {
        name = n;
        names = TagSystem.AllTags().Where(tag => tag.EndsWith("." + name));
    }

    TagNames GetTagNames(bool recursive = false)
    {
        return new TagNames(TagSystem.AllTags().Where(tag => IsMatch(tag, recursive)));
    }

    bool IsMatch(string tag, bool recursive)
    {
        var idx = tag.IndexOf("." + name + ".");
        if (idx == -1) {
            return false;
        }
        return recursive || tag.IndexOf(".", idx + name.Length + 2) == -1;
    }

    public static implicit operator TagChildren(string child) => new TagChildren(child);

}

}