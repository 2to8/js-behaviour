using UnityEngine;

namespace MoreTags {

public abstract class TagPattern {

    public abstract GameObject GameObject();

    public abstract GameObject[] GameObjects();

    public abstract int Count();

    public abstract TagPattern And();

    public abstract TagPattern Or();

    public abstract TagPattern Exclude();

    public abstract TagPattern And(TagPattern pattern);

    public abstract TagPattern Or(TagPattern pattern);

    public abstract TagPattern Exclude(TagPattern pattern);

    public abstract TagPattern Combine(TagPattern pattern);

    public abstract TagPattern All();

    public abstract TagPattern With(string tag);

    public abstract TagPattern Both(params string[] tags);

    public abstract TagPattern Either(params string[] tags);

    public static implicit operator TagPattern(string pattern) => TagHelper.StringToPattern(pattern);

    public static TagPattern operator &(TagPattern a, TagPattern b) => a.And(b);

    public static TagPattern operator |(TagPattern a, TagPattern b) => a.Or(b);

    public static TagPattern operator -(TagPattern a, TagPattern b) => a.Exclude(b);

    public static TagPattern operator -(TagPattern a) => TagSystem.pattern.All().Exclude(a);

    public static TagPattern operator &(TagName a, TagPattern b) => (TagPattern)a & b;

    public static TagPattern operator |(TagName a, TagPattern b) => (TagPattern)a | b;

    public static TagPattern operator -(TagName a, TagPattern b) => (TagPattern)a - b;

}

}