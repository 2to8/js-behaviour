namespace MoreTags {

public class TagName {

    public readonly string name;
    public readonly string last;
    public readonly string[] parts;

    public TagName(string n)
    {
        name = n;
        parts = n.Split('.');
        last = parts[parts.Length - 1];
    }

    public static implicit operator TagName(string name) => new TagName(name);
    public static implicit operator string(TagName name) => name.name;
    public static implicit operator TagPattern(TagName name) => TagSystem.pattern.With(name.name);
    public static TagPattern operator &(TagName a, TagName b) => TagSystem.pattern.Both(a, b);
    public static TagPattern operator |(TagName a, TagName b) => TagSystem.pattern.Either(a, b);
    public static TagPattern operator -(TagName a, TagName b) => (TagPattern)a - b;
    public static TagPattern operator -(TagName a) => -(TagPattern)a;
    public static implicit operator TagNames(TagName name) => new TagNames(name.name);
    public static TagNames operator +(TagName a, TagName b) => new TagNames(a.name, b.name);

}

}