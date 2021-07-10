using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MoreTags {

public class TagNames : IEnumerable<string> {

    public TagPattern both => TagSystem.pattern.Both(names.ToArray());
    public TagPattern either => TagSystem.pattern.Either(names.ToArray());
    protected IEnumerable<string> names;

    public TagNames(params string[] n)
    {
        names = n;
    }

    public TagNames(IEnumerable<string> n)
    {
        names = n;
    }

    public void Add(TagNames n)
    {
        names = names.Union(n.names);
    }

    public void Remove(TagNames n)
    {
        names = names.Except(n.names);
    }

    public IEnumerator<string> GetEnumerator() => names.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => names.GetEnumerator();
    public static implicit operator TagNames(string str) => TagHelper.StringToTagNames(str);

    public static TagNames operator +(TagNames a, TagNames b) =>
        new TagNames(a.names.Union(b.names));

    public static TagNames operator -(TagNames a, TagNames b) =>
        new TagNames(a.names.Except(b.names));

}

}