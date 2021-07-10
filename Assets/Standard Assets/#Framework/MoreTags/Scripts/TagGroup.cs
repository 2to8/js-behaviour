using System.Linq;

namespace MoreTags {

public class TagGroup : TagName {

    public TagNames children => GetTagNames();
    public TagNames all => GetTagNames(true);
    public TagGroup(string n) : base(n) { }

    TagNames GetTagNames(bool recursive = false)
    {
        return new TagNames(TagSystem.AllTags().Where(tag => IsMatch(tag, recursive)));
    }

    bool IsMatch(string tag, bool recursive)
    {
        if (!tag.StartsWith(name + ".")) {
            return false;
        }
        return recursive || tag.IndexOf(".", name.Length + 1) == -1;
    }

    public static implicit operator TagGroup(string cat) => new TagGroup(cat);

}

}