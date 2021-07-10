using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MoreTags {

[TestFixture, Category("MoreTags Tests")]
public class UnitTest {

    public static string[] s_SimpleTags = new string[]
        { "Test1", "Test2", "Test3", "Test4", "Test5" };

    public static string[] s_NotInSimpleTags = new string[]
        { "Test6", "Test7", "Test8", "Test9", "Test10" };

    public static string[] s_GroupTags = new string[]
        { "T1.Test1", "T1.Test2", "T1.Test1.S1", "T2.Test2", "T2.Test2.S1", "T2.Test2.S1.A1" };

    public static string[] s_NotInGroupTags = new string[]
        { "T3.Test1", "T3.Test3", "T3.Test4", "T1.Test1.S1", "T1.Test1.S2", "T1.Test1.S3" };

#region Test GameObject Tag
    [Test]
    public void GameObjectGetTag()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInSimpleTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_SimpleTags);
        var tags = Extensions.GetTags(go);
        foreach (var tag in s_SimpleTags) {
            Assert.Contains(tag, tags.ToList());
        }
        foreach (var tag in s_NotInSimpleTags) {
            Assert.IsFalse(tags.Contains(tag));
        }
        go.AddTag(s_GroupTags);
        tags = go.FindTags("T1.*");
        Assert.AreEqual(tags.ToArray().Length, 3);
        Assert.Contains("T1.Test1", tags.ToArray());
        Assert.Contains("T1.Test2", tags.ToArray());
        Assert.Contains("T1.Test1.S1", tags.ToArray());
        tags = go.FindTags("T2.?");
        Assert.AreEqual(tags.ToArray().Length, 1);
        Assert.Contains("T2.Test2", tags.ToArray());
        tags = go.FindTags("T3.*");
        Assert.IsEmpty(tags);
        tags = go.FindTags("*.S1");
        Assert.AreEqual(tags.ToArray().Length, 2);
        Assert.Contains("T1.Test1.S1", tags.ToArray());
        Assert.Contains("T2.Test2.S1", tags.ToArray());
        tags = go.FindTags("*.S1.?");
        Assert.AreEqual(tags.ToArray().Length, 1);
        Assert.Contains("T2.Test2.S1.A1", tags.ToArray());
        tags = go.FindTags("*.Test4");
        Assert.IsEmpty(tags);
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }

    [Test]
    public void GameObjectHasTag()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInSimpleTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_SimpleTags);
        foreach (var tag in s_SimpleTags) {
            Assert.IsTrue(go.HasTag(tag));
        }
        foreach (var tag in s_NotInSimpleTags) {
            Assert.IsFalse(go.HasTag(tag));
        }
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }

    [Test]
    public void GameObjectAnyTags()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInSimpleTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_SimpleTags);
        foreach (var tag in s_SimpleTags) {
            var list = s_NotInSimpleTags.ToList();
            list.Insert(Random.Range(0, s_SimpleTags.Length - 1), tag);
            Assert.IsTrue(go.AnyTags(list.ToArray()));
        }
        Assert.IsFalse(go.AnyTags(s_NotInSimpleTags));
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }

    [Test]
    public void GameObjectBothTags()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInSimpleTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_SimpleTags);
        for (var i = 1; i < s_SimpleTags.Length; i++) {
            var list = s_SimpleTags.Take(i).ToList();
            Assert.IsTrue(go.BothTags(list.ToArray()));
            list.Add(s_NotInSimpleTags.Skip(i - 1).FirstOrDefault());
            Assert.IsFalse(go.BothTags(list.ToArray()));
        }
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }
#endregion

#region Test Tag Pattern
    [Test]
    public void TagPatternWith()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInSimpleTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_SimpleTags);
        foreach (var tag in s_SimpleTags) {
            Assert.Contains(go, TagSystem.pattern.With(tag).GameObjects());
        }
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }

    [Test]
    public void TagPatternEither()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInSimpleTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_SimpleTags);
        foreach (var tag in s_SimpleTags) {
            var list = s_NotInSimpleTags.ToList();
            list.Insert(Random.Range(0, s_SimpleTags.Length - 1), tag);
            Assert.Contains(go, TagSystem.pattern.Either(list.ToArray()).GameObjects());
        }
        Assert.IsFalse(TagSystem.pattern.Either(s_NotInSimpleTags).GameObjects().Contains(go));
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }

    [Test]
    public void TagPatternBoth()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInSimpleTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_SimpleTags);
        for (var i = 1; i < s_SimpleTags.Length; i++) {
            var list = s_SimpleTags.Take(i).ToList();
            Assert.Contains(go, TagSystem.pattern.Both(list.ToArray()).GameObjects());
            list.Add(s_NotInSimpleTags.Skip(i - 1).FirstOrDefault());
            Assert.IsFalse(TagSystem.pattern.Both(list.ToArray()).GameObjects().Contains(go));
        }
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }
#endregion

#region Test Tag Pattern Combine
    [Test]
    public void TagPatternCombineAnd()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInSimpleTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_SimpleTags);
        var pat = TagSystem.pattern.And()
            .With(s_SimpleTags[0])
            .With(s_SimpleTags[1])
            .With(s_SimpleTags[2])
            .With(s_SimpleTags[3])
            .With(s_SimpleTags[4]);
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.And().With(s_SimpleTags[0]).Both(s_SimpleTags.Skip(1).ToArray());
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.And().Both(s_SimpleTags.Take(4).ToArray()).With(s_SimpleTags[4]);
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.And()
            .Both(s_SimpleTags.Take(2).ToArray())
            .Both(s_SimpleTags.Skip(2).ToArray());
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.And()
            .With(s_SimpleTags[0])
            .With(s_SimpleTags[1])
            .With(s_SimpleTags[2])
            .With(s_SimpleTags[3])
            .With(s_NotInSimpleTags[4]);
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = TagSystem.pattern.And()
            .With(s_SimpleTags[0])
            .With(s_NotInSimpleTags[1])
            .With(s_NotInSimpleTags[2])
            .With(s_NotInSimpleTags[3])
            .With(s_NotInSimpleTags[4]);
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = TagSystem.pattern.And().Both(s_SimpleTags).With(s_NotInSimpleTags[4]);
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = TagSystem.pattern.And().With(s_SimpleTags[0]).Both(s_NotInSimpleTags);
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = TagSystem.pattern.And().Both(s_SimpleTags).Both(s_NotInSimpleTags);
        Assert.IsFalse(pat.GameObjects().Contains(go));
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }

    [Test]
    public void TagPatternCombineOr()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInSimpleTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_SimpleTags);
        var pat = TagSystem.pattern.Or()
            .With(s_SimpleTags[0])
            .With(s_SimpleTags[1])
            .With(s_SimpleTags[2])
            .With(s_SimpleTags[3])
            .With(s_SimpleTags[4]);
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.Or()
            .With(s_SimpleTags[0])
            .With(s_NotInSimpleTags[1])
            .With(s_NotInSimpleTags[2])
            .With(s_NotInSimpleTags[3])
            .With(s_NotInSimpleTags[4]);
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.Or()
            .With(s_NotInSimpleTags[0])
            .With(s_NotInSimpleTags[1])
            .With(s_NotInSimpleTags[2])
            .With(s_NotInSimpleTags[3])
            .With(s_SimpleTags[4]);
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.Or().With(s_SimpleTags[0]).Either(s_NotInSimpleTags);
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.Or().Either(s_SimpleTags).With(s_NotInSimpleTags[0]);
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.Or().Either(s_SimpleTags).Either(s_NotInSimpleTags);
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.Or()
            .With(s_NotInSimpleTags[0])
            .With(s_NotInSimpleTags[1])
            .With(s_NotInSimpleTags[2])
            .With(s_NotInSimpleTags[3])
            .With(s_NotInSimpleTags[4]);
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = TagSystem.pattern.Or()
            .With(s_NotInSimpleTags[0])
            .Either(s_NotInSimpleTags.Take(4).ToArray());
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = TagSystem.pattern.Or()
            .Either(s_NotInSimpleTags.Take(4).ToArray())
            .With(s_NotInSimpleTags[4]);
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = TagSystem.pattern.Or()
            .Either(s_NotInSimpleTags.Take(2).ToArray())
            .Either(s_NotInSimpleTags.Skip(2).ToArray());
        Assert.IsFalse(pat.GameObjects().Contains(go));
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }

    [Test]
    public void TagPatternCombineExclude()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInSimpleTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_SimpleTags);
        var pat = TagSystem.pattern.With(s_SimpleTags[0]).Exclude().With(s_NotInSimpleTags[0]);
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.Exclude()
            .With(s_NotInSimpleTags[0])
            .With(s_NotInSimpleTags[1])
            .With(s_NotInSimpleTags[2])
            .With(s_NotInSimpleTags[3])
            .With(s_NotInSimpleTags[4]);
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.Exclude().Both(s_NotInSimpleTags);
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.Exclude().Either(s_NotInSimpleTags);
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.Exclude().With(s_SimpleTags[0]);
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = TagSystem.pattern.Exclude()
            .With(s_SimpleTags[0])
            .With(s_SimpleTags[1])
            .With(s_SimpleTags[2])
            .With(s_SimpleTags[3])
            .With(s_SimpleTags[4]);
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = TagSystem.pattern.Exclude().Both(s_SimpleTags);
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = TagSystem.pattern.Exclude().Either(s_SimpleTags);
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = TagSystem.pattern.With(s_NotInSimpleTags[0]).Exclude().Both(s_SimpleTags[0]);
        Assert.IsFalse(pat.GameObjects().Contains(go));
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }

    [Test]
    public void TagPatternCombineMix()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInSimpleTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_SimpleTags);
        var pat = TagSystem.pattern.And()
            .With(s_SimpleTags[0])
            .With(s_SimpleTags[1])
            .Or()
            .With(s_SimpleTags[2])
            .With(s_SimpleTags[3])
            .With(s_SimpleTags[4]);
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.With(s_SimpleTags[0]).And().Either(s_SimpleTags.Skip(1).ToArray());
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.Either(s_SimpleTags.Take(4).ToArray()).And().With(s_SimpleTags[4]);
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.Either(s_SimpleTags.Take(2).ToArray())
            .And()
            .Either(s_SimpleTags.Skip(2).ToArray());
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.Or()
            .With(s_SimpleTags[0])
            .With(s_SimpleTags[1])
            .And()
            .With(s_SimpleTags[2])
            .With(s_SimpleTags[3])
            .With(s_SimpleTags[4]);
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.With(s_SimpleTags[0]).Or().Both(s_SimpleTags.Skip(1).ToArray());
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.Both(s_SimpleTags.Take(4).ToArray()).Or().With(s_SimpleTags[4]);
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.Both(s_SimpleTags.Take(2).ToArray())
            .Or()
            .Both(s_SimpleTags.Skip(2).ToArray());
        Assert.Contains(go, pat.GameObjects());
        pat = TagSystem.pattern.Or()
            .With(s_SimpleTags[0])
            .With(s_SimpleTags[1])
            .With(s_SimpleTags[2])
            .With(s_SimpleTags[3])
            .And()
            .With(s_NotInSimpleTags[4]);
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = TagSystem.pattern.Or()
            .With(s_SimpleTags[0])
            .And()
            .With(s_NotInSimpleTags[1])
            .With(s_NotInSimpleTags[2])
            .With(s_NotInSimpleTags[3])
            .With(s_NotInSimpleTags[4]);
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = TagSystem.pattern.Either(s_SimpleTags).And().With(s_NotInSimpleTags[4]);
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = TagSystem.pattern.With(s_SimpleTags[0]).And().Either(s_NotInSimpleTags);
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = TagSystem.pattern.Either(s_SimpleTags).And().Either(s_NotInSimpleTags);
        Assert.IsFalse(pat.GameObjects().Contains(go));
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }
#endregion

#region Test Tag Pattern Multi Combine
    [Test]
    public void TagPatternMultiCombineAnd()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInSimpleTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_SimpleTags);
        for (var i = 1; i < s_SimpleTags.Length; i++) {
            var list = s_SimpleTags.Take(i).ToList();
            var pat = TagSystem.pattern.And();
            foreach (var check in list) {
                pat.With(check);
            }
            Assert.Contains(go, pat.GameObjects());
            pat.With(s_NotInSimpleTags[i - 1]);
            Assert.IsFalse(pat.GameObjects().Contains(go));
        }
        {
            var pat = TagSystem.pattern.And();
            pat.With(s_SimpleTags[0]);
            pat.Both(s_SimpleTags.Skip(1).ToArray());
            Assert.Contains(go, pat.GameObjects());
            pat.Both(s_NotInSimpleTags);
            Assert.IsFalse(pat.GameObjects().Contains(go));
            pat = TagSystem.pattern.And();
            pat.Both(s_SimpleTags.Take(4).ToArray());
            pat.With(s_SimpleTags[4]);
            Assert.Contains(go, pat.GameObjects());
            pat.With(s_NotInSimpleTags[0]);
            Assert.IsFalse(pat.GameObjects().Contains(go));
        }
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }

    [Test]
    public void TagPatternMultiCombineOr()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInSimpleTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_SimpleTags);
        foreach (var tag in s_SimpleTags) {
            var list = s_NotInSimpleTags.ToList();
            list.Insert(Random.Range(0, s_SimpleTags.Length - 1), tag);
            var pat = TagSystem.pattern.Or();
            foreach (var check in list) {
                pat.With(check);
            }
            Assert.Contains(go, pat.GameObjects());
        }
        {
            var pat = TagSystem.pattern.Or();
            pat.With(s_SimpleTags[0]);
            pat.Either(s_SimpleTags.Skip(1).ToArray());
            Assert.Contains(go, pat.GameObjects());
            pat = TagSystem.pattern.Or();
            pat.Either(s_SimpleTags.Take(4).ToArray());
            pat.With(s_SimpleTags[4]);
            Assert.Contains(go, pat.GameObjects());
            pat = TagSystem.pattern.Or();
            foreach (var check in s_NotInSimpleTags) {
                pat = pat.With(check);
            }
            Assert.IsFalse(pat.GameObjects().Contains(go));
            pat = TagSystem.pattern.Or();
            pat.With(s_NotInSimpleTags[0]);
            pat.Either(s_NotInSimpleTags.Skip(1).ToArray());
            Assert.IsFalse(pat.GameObjects().Contains(go));
            pat = TagSystem.pattern.Or();
            pat.Either(s_NotInSimpleTags.Take(4).ToArray());
            pat.With(s_NotInSimpleTags[4]);
            Assert.IsFalse(pat.GameObjects().Contains(go));
        }
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }

    [Test]
    public void TagPatternMultiCombineExculde()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInSimpleTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_SimpleTags);
        foreach (var tag in s_SimpleTags) {
            var pat = TagSystem.pattern.With(tag).Exclude();
            foreach (var check in s_NotInSimpleTags) {
                pat.With(check);
                Assert.Contains(go, pat.GameObjects());
            }
            pat.With(tag);
            Assert.IsFalse(pat.GameObjects().Contains(go));
            pat = TagSystem.pattern.Exclude();
            foreach (var check in s_NotInSimpleTags) {
                pat.With(check);
                Assert.Contains(go, pat.GameObjects());
            }
            pat.With(tag);
            Assert.IsFalse(pat.GameObjects().Contains(go));
        }
        {
            var pat = TagSystem.pattern.Exclude();
            pat.With(s_NotInSimpleTags[0]);
            pat.Both(s_NotInSimpleTags.Skip(1).ToArray());
            Assert.Contains(go, pat.GameObjects());
            pat.Both(s_SimpleTags);
            Assert.IsFalse(pat.GameObjects().Contains(go));
            pat = TagSystem.pattern.Exclude();
            pat.Either(s_SimpleTags);
            Assert.IsFalse(pat.GameObjects().Contains(go));
            pat = TagSystem.pattern.Either(s_SimpleTags).Exclude();
            pat.With(s_NotInSimpleTags[0]);
            pat.Both(s_NotInSimpleTags.Skip(1).ToArray());
            Assert.Contains(go, pat.GameObjects());
            pat.Both(s_SimpleTags);
            Assert.IsFalse(pat.GameObjects().Contains(go));
        }
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }

    [Test]
    public void TagPatternMultiCombineMix()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInSimpleTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_SimpleTags);
        var pat = TagSystem.pattern;
        pat.Either(s_SimpleTags.Take(2).ToArray());
        pat.And().Either(s_SimpleTags.Skip(2).ToArray());
        Assert.Contains(go, pat.GameObjects());
        pat.Or().Either(s_NotInSimpleTags);
        Assert.Contains(go, pat.GameObjects());
        pat.And().Either(s_NotInSimpleTags);
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = TagSystem.pattern;
        pat.With(s_NotInSimpleTags[0]);
        pat.Or().With(s_SimpleTags[0]);
        Assert.Contains(go, pat.GameObjects());
        pat.Exclude().With(s_SimpleTags[0]);
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = TagSystem.pattern;
        pat.With(s_SimpleTags[0]);
        pat.Or().Either(s_NotInSimpleTags);
        Assert.Contains(go, pat.GameObjects());
        pat.Exclude().Both(s_NotInSimpleTags[0]);
        Assert.Contains(go, pat.GameObjects());
        pat.Exclude().Either(s_SimpleTags[0]);
        Assert.IsFalse(pat.GameObjects().Contains(go));
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }
#endregion

#region Test Tag Pattern Combine Pattern
    [Test]
    public void TagPatternCombinePattern()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInSimpleTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_SimpleTags);
        var pat = TagSystem.pattern.With(s_SimpleTags[0]).With(s_SimpleTags[1]);
        var pat2 = TagSystem.pattern.With(s_SimpleTags[2])
            .With(s_SimpleTags[3])
            .With(s_SimpleTags[4])
            .And(pat);
        Assert.Contains(go, pat2.GameObjects());
        pat2 = TagSystem.pattern.With(s_SimpleTags[4]).Exclude(pat);
        Assert.IsFalse(pat2.GameObjects().Contains(go));
        pat = TagSystem.pattern.Both(s_SimpleTags.Take(4).ToArray());
        pat2 = TagSystem.pattern.With(s_SimpleTags[4]).And(pat);
        Assert.Contains(go, pat2.GameObjects());
        pat2 = TagSystem.pattern.With(s_SimpleTags[4]).Or(pat);
        Assert.Contains(go, pat2.GameObjects());
        pat = TagSystem.pattern.Both(s_NotInSimpleTags);
        pat2 = TagSystem.pattern.With(s_SimpleTags[0]).Exclude(pat);
        Assert.Contains(go, pat2.GameObjects());
        pat2.Or(pat);
        Assert.Contains(go, pat2.GameObjects());
        pat2.And(pat);
        Assert.IsFalse(pat2.GameObjects().Contains(go));
        pat = TagSystem.pattern.Either(s_SimpleTags.Take(2).ToArray());
        pat2 = TagSystem.pattern.Both(s_SimpleTags.Skip(2).ToArray()).And(pat);
        Assert.Contains(go, pat2.GameObjects());
        pat2 = TagSystem.pattern.Both(s_SimpleTags.Skip(2).ToArray()).Or(pat);
        Assert.Contains(go, pat2.GameObjects());
        pat2 = TagSystem.pattern.Exclude(pat);
        Assert.IsFalse(pat2.GameObjects().Contains(go));
        pat = TagSystem.pattern.Either(s_SimpleTags);
        pat2 = TagSystem.pattern.Exclude(pat);
        Assert.IsFalse(pat2.GameObjects().Contains(go));
        pat = TagSystem.pattern.Either(s_NotInSimpleTags);
        pat2 = TagSystem.pattern.All().Exclude(pat);
        Assert.Contains(go, pat2.GameObjects());
        pat = TagSystem.pattern;
        pat2 = TagSystem.pattern.Or(pat);
        Assert.IsFalse(pat2.GameObjects().Contains(go));
        pat2 = TagSystem.pattern.And(pat);
        Assert.IsFalse(pat2.GameObjects().Contains(go));
        pat2 = TagSystem.pattern.Exclude(pat);
        Assert.Contains(go, pat2.GameObjects());
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }
#endregion

#region Test Tag Helper
    [Test]
    public void TestTagName()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInSimpleTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_SimpleTags);
        TagPattern pat = (TagName)"Test1";
        Assert.Contains(go, pat.GameObjects());
        pat = pat & (TagName)"Test2";
        Assert.Contains(go, pat.GameObjects());
        pat = pat | (TagName)"Test6";
        Assert.Contains(go, pat.GameObjects());
        pat = pat - (TagName)"Test8";
        Assert.Contains(go, pat.GameObjects());
        pat = (TagName)"Test1" & (TagName)"Test2";
        Assert.Contains(go, pat.GameObjects());
        pat &= (TagName)"Test3";
        Assert.Contains(go, pat.GameObjects());
        pat |= (TagName)"Test7";
        Assert.Contains(go, pat.GameObjects());
        pat -= (TagName)"Test8";
        Assert.Contains(go, pat.GameObjects());
        pat &= (TagName)"Test9";
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = ((TagName)"Test1" & (TagName)"Test2") | ((TagName)"Test6" - (TagName)"Test8");
        Assert.Contains(go, pat.GameObjects());
        pat -= (TagName)"Test4";
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat |= (TagName)"Test5";
        Assert.Contains(go, pat.GameObjects());
        pat = -(TagName)"Test6";
        Assert.Contains(go, pat.GameObjects());
        pat = -(TagName)"Test5";
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = ((TagName)"Test1" & (TagName)"Test2") | ((TagName)"Test6" & (TagName)"Test8");
        Assert.Contains(go, pat.GameObjects());
        pat = ((TagName)"Test1" | (TagName)"Test6") & ((TagName)"Test2" | (TagName)"Test7");
        Assert.Contains(go, pat.GameObjects());
        pat = ((TagName)"Test1" | (TagName)"Test6") & ((TagName)"Test8" | (TagName)"Test10");
        Assert.IsFalse(pat.GameObjects().Contains(go));
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }

    [Test]
    public void TestTagNames()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInSimpleTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_SimpleTags);
        var names = (TagName)"Test1" + (TagName)"Test2";
        Assert.Contains(go, names.both.GameObjects());
        Assert.Contains(go, names.either.GameObjects());
        names += (TagName)"Test6";
        Assert.IsFalse(names.both.GameObjects().Contains(go));
        Assert.Contains(go, names.either.GameObjects());
        names = names + (TagName)"Test3" - (TagName)"Test6";
        Assert.Contains(go, names.both.GameObjects());
        Assert.Contains(go, names.either.GameObjects());
        names -= (TagName)"Test3" + (TagName)"Test1" + (TagName)"Test2";
        Assert.IsFalse(names.both.GameObjects().Contains(go));
        Assert.IsFalse(names.either.GameObjects().Contains(go));
        TagSystem.RemoveTag(s_NotInGroupTags);
        TagSystem.AddTag(s_GroupTags);
        names = "T1.*";
        Assert.AreEqual(names.Count(), 3);
        Assert.Contains("T1.Test1", names.ToArray());
        Assert.Contains("T1.Test2", names.ToArray());
        Assert.Contains("T1.Test1.S1", names.ToArray());
        names = "*.S1+*.A1";
        Assert.AreEqual(names.Count(), 3);
        Assert.Contains("T1.Test1.S1", names.ToArray());
        Assert.Contains("T2.Test2.S1", names.ToArray());
        Assert.Contains("T2.Test2.S1.A1", names.ToArray());
        names = "*.Test2-T2.*+T1.Test1";
        Assert.AreEqual(names.Count(), 2);
        Assert.Contains("T1.Test1", names.ToArray());
        Assert.Contains("T1.Test2", names.ToArray());
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }

    [Test]
    public void TestTagGroup()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInGroupTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_GroupTags);
        var pat = ((TagName)"T1.Test1" | (TagName)"T3") & ((TagName)"T2.Test2" | (TagName)"T1");
        Assert.Contains(go, pat.GameObjects());
        var cat = (TagGroup)"T1";
        Assert.Contains(go, cat.children.both.GameObjects());
        Assert.Contains(go, cat.children.either.GameObjects());
        Assert.IsFalse(cat.all.both.GameObjects().Contains(go));
        Assert.Contains(go, cat.all.either.GameObjects());
        pat = -cat.children.either;
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = -cat.all.both;
        Assert.Contains(go, pat.GameObjects());
        pat = (((TagGroup)"T1.Test1").children - (TagName)"T1.Test1.S2" - (TagName)"T1.Test1.S3")
            .both;
        Assert.Contains(go, pat.GameObjects());
        cat = "T3";
        Assert.IsFalse(cat.children.both.GameObjects().Contains(go));
        Assert.IsFalse(cat.children.either.GameObjects().Contains(go));
        Assert.IsFalse(cat.all.both.GameObjects().Contains(go));
        Assert.IsFalse(cat.all.either.GameObjects().Contains(go));
        pat = -cat;
        Assert.Contains(go, pat.GameObjects());
        pat = -cat.all.either;
        Assert.Contains(go, pat.GameObjects());
        var child = (TagChildren)"Test1";
        Assert.IsFalse(child.both.GameObjects().Contains(go));
        Assert.Contains(go, child.either.GameObjects());
        Assert.IsFalse(child.all.both.GameObjects().Contains(go));
        Assert.Contains(go, child.all.either.GameObjects());
        Assert.IsFalse(child.children.both.GameObjects().Contains(go));
        Assert.Contains(go, child.children.either.GameObjects());
        pat = (child - cat.all).both;
        Assert.Contains(go, pat.GameObjects());
        child = "Test2";
        Assert.Contains(go, child.both.GameObjects());
        Assert.Contains(go, child.either.GameObjects());
        Assert.Contains(go, child.all.both.GameObjects());
        Assert.Contains(go, child.all.either.GameObjects());
        Assert.Contains(go, child.children.both.GameObjects());
        Assert.Contains(go, child.children.either.GameObjects());
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }
#endregion

#region Test String To Pattern
    [Test]
    public void TestStringToPattern()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInSimpleTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_SimpleTags);
        var pat = (TagPattern)"Test1";
        Assert.Contains(go, pat.GameObjects());
        pat = pat & "Test2";
        Assert.Contains(go, pat.GameObjects());
        pat = pat | "Test6";
        Assert.Contains(go, pat.GameObjects());
        pat = pat - "Test8";
        Assert.Contains(go, pat.GameObjects());
        pat = "Test1 & Test2";
        Assert.Contains(go, pat.GameObjects());
        pat &= "Test3";
        Assert.Contains(go, pat.GameObjects());
        pat |= "Test7";
        Assert.Contains(go, pat.GameObjects());
        pat -= "Test8";
        Assert.Contains(go, pat.GameObjects());
        pat &= "Test9";
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = "Test1 & Test2 | Test6 - Test8";
        Assert.Contains(go, pat.GameObjects());
        pat -= "Test4";
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat |= "Test5";
        Assert.Contains(go, pat.GameObjects());
        pat = "-Test6";
        Assert.Contains(go, pat.GameObjects());
        pat = "-Test5";
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = "(Test1 & Test2) | (Test6 & Test8)";
        Assert.Contains(go, pat.GameObjects());
        pat = "(Test1 | Test6) & (Test2 | Test7)";
        Assert.Contains(go, pat.GameObjects());
        pat = "(Test1 | Test6) & (Test8 | Test10)";
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = "both(Test1 + Test2)";
        Assert.Contains(go, pat.GameObjects());
        pat = "either(Test1 + Test2)";
        Assert.Contains(go, pat.GameObjects());
        pat = "both(Test1 + Test2 + Test6)";
        Assert.IsFalse(pat.GameObjects().Contains(go));
        pat = "either(Test1 + Test2 + Test6)";
        Assert.Contains(go, pat.GameObjects());
        pat = "both(Test1 + Test2 + Test6 + Test3 - Test6)";
        Assert.Contains(go, pat.GameObjects());
        pat = "either(Test1 + Test2 + Test6 + Test3 - Test6)";
        Assert.Contains(go, pat.GameObjects());
        pat = "either(Test1 + Test2 + Test6 - Test1 - Test6 - Test2)";
        Assert.IsFalse(pat.GameObjects().Contains(go));
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }

    [Test]
    public void TestStringToPatternGroup()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInGroupTags);
        var go = TagHelper.CreateGameObject();
        go.AddTag(s_GroupTags);
        Assert.Contains(go, ((TagPattern)"(T1.Test1 | T3) & (T2.Test2 | T1)").GameObjects());
        Assert.Contains(go, ((TagPattern)"T2.?").GameObjects());
        Assert.Contains(go, ((TagPattern)"*.S1").GameObjects());
        Assert.Contains(go, ((TagPattern)"*.Test2.?").GameObjects());
        Assert.IsFalse(((TagPattern)"T3.*").GameObjects().Contains(go));
        Assert.IsFalse(((TagPattern)"*.S3").GameObjects().Contains(go));
        Assert.IsFalse(((TagPattern)"*.Test4").GameObjects().Contains(go));
        Assert.Contains(go, ((TagPattern)"both(T1.?)").GameObjects());
        Assert.Contains(go, ((TagPattern)"either(T1.?)").GameObjects());
        Assert.IsFalse(((TagPattern)"both(T1.*)").GameObjects().Contains(go));
        Assert.Contains(go, ((TagPattern)"either(T1.*)").GameObjects());
        Assert.IsFalse(((TagPattern)"-either(T1.?)").GameObjects().Contains(go));
        Assert.Contains(go, ((TagPattern)"-both(T1.*)").GameObjects());
        Assert.Contains(go,
            ((TagPattern)"both(T1.Test1.? - T1.Test1.S2 - T1.Test1.S3)").GameObjects());
        Assert.IsFalse(((TagPattern)"both(T3.?)").GameObjects().Contains(go));
        Assert.IsFalse(((TagPattern)"either(T3.?)").GameObjects().Contains(go));
        Assert.IsFalse(((TagPattern)"both(T3.*)").GameObjects().Contains(go));
        Assert.IsFalse(((TagPattern)"either(T3.*)").GameObjects().Contains(go));
        Assert.Contains(go, ((TagPattern)"-T3").GameObjects());
        Assert.Contains(go, ((TagPattern)"-either(T3.*)").GameObjects());
        Assert.IsFalse(((TagPattern)"both(*.Test1)").GameObjects().Contains(go));
        Assert.Contains(go, ((TagPattern)"either(*.Test1)").GameObjects());
        Assert.IsFalse(((TagPattern)"both(*.Test1.*)").GameObjects().Contains(go));
        Assert.Contains(go, ((TagPattern)"either(*.Test1.*)").GameObjects());
        Assert.IsFalse(((TagPattern)"both(*.Test1.?)").GameObjects().Contains(go));
        Assert.Contains(go, ((TagPattern)"either(*.Test1.?)").GameObjects());
        Assert.Contains(go, ((TagPattern)"both(*.Test1-T3.*)").GameObjects());
        Assert.Contains(go, ((TagPattern)"both(*.Test2)").GameObjects());
        Assert.Contains(go, ((TagPattern)"either(*.Test2)").GameObjects());
        Assert.Contains(go, ((TagPattern)"both(*.Test2.*)").GameObjects());
        Assert.Contains(go, ((TagPattern)"either(*.Test2.*)").GameObjects());
        Assert.Contains(go, ((TagPattern)"both(*.Test2.?)").GameObjects());
        Assert.Contains(go, ((TagPattern)"either(*.Test2.?)").GameObjects());
        TagHelper.DestroyGameObject(go);
        TagSystem.RemoveNullGameObject();
    }
#endregion

#region Test Search From
    [Test]
    public void TestSearchFrom()
    {
        TagSystem.SearchFrom();
        TagSystem.AddTag(s_NotInSimpleTags);
        var list = new List<GameObject>();
        for (var i = 0; i < 10; i++) {
            var go = TagHelper.CreateGameObject();
            go.AddTag(s_SimpleTags);
            if (i >= 5) {
                go.AddTag(s_GroupTags);
            }
            list.Add(go);
        }
        foreach (var tag in s_SimpleTags) {
            TagSystem.SearchFrom(list.Take(3).ToArray());
            TagPattern pat = "(Test1 & Test2) | (Test6 & Test8)";
            foreach (var go in list.Take(3)) {
                Assert.Contains(go, pat.GameObjects());
            }
            foreach (var go in list.Skip(3)) {
                Assert.IsFalse(pat.GameObjects().Contains(go));
            }
            TagSystem.SearchFrom(list.Take(6).ToArray());
            pat = "(Test1 | Test6) & (Test2 | Test7)";
            foreach (var go in list.Take(6)) {
                Assert.Contains(go, pat.GameObjects());
            }
            foreach (var go in list.Skip(6)) {
                Assert.IsFalse(pat.GameObjects().Contains(go));
            }
            pat = "(Test1 | Test6) & (Test8 | Test10)";
            foreach (var go in list) {
                Assert.IsFalse(pat.GameObjects().Contains(go));
            }
            TagSystem.SearchFrom(list.Skip(3).Take(4).ToArray());
            pat = "*.S1";
            foreach (var go in list.Take(5)) {
                Assert.IsFalse(pat.GameObjects().Contains(go));
            }
            foreach (var go in list.Skip(5).Take(2)) {
                Assert.Contains(go, pat.GameObjects());
            }
            foreach (var go in list.Skip(7)) {
                Assert.IsFalse(pat.GameObjects().Contains(go));
            }
        }
        TagSystem.SearchFrom();
        foreach (var go in list) {
            TagHelper.DestroyGameObject(go);
        }
        TagSystem.RemoveNullGameObject();
    }
#endregion

#region Test Auto Class
    //[Test]
    //public void TestAutoClass()
    //{
    //    TagSystem.SearchFrom();
    //    TagSystem.AddTag(s_NotInSimpleTags);
    //    var go = TagHelper.CreateGameObject();
    //    go.AddTag(s_SimpleTags);

    //    ITagPattern pat = Tag.Test1;
    //    Assert.Contains(go, pat.GameObjects());
    //    pat = pat & Tag.Test2;
    //    Assert.Contains(go, pat.GameObjects());
    //    pat = pat | Tag.Test6;
    //    Assert.Contains(go, pat.GameObjects());
    //    pat = pat - Tag.Test8;
    //    Assert.Contains(go, pat.GameObjects());
    //    pat = Tag.Test1 & Tag.Test2;
    //    Assert.Contains(go, pat.GameObjects());
    //    pat &= Tag.Test3;
    //    Assert.Contains(go, pat.GameObjects());
    //    pat |= Tag.Test7;
    //    Assert.Contains(go, pat.GameObjects());
    //    pat -= Tag.Test8;
    //    Assert.Contains(go, pat.GameObjects());
    //    pat &= Tag.Test9;
    //    Assert.IsFalse(pat.GameObjects().Contains(go));
    //    pat = Tag.Test1 & Tag.Test2 | Tag.Test6 - Tag.Test8;
    //    Assert.Contains(go, pat.GameObjects());
    //    pat -= Tag.Test4;
    //    Assert.IsFalse(pat.GameObjects().Contains(go));
    //    pat |= Tag.Test5;
    //    Assert.Contains(go, pat.GameObjects());
    //    pat = -Tag.Test6;
    //    Assert.Contains(go, pat.GameObjects());
    //    pat = -Tag.Test5;
    //    Assert.IsFalse(pat.GameObjects().Contains(go));

    //    pat = (Tag.Test1 & Tag.Test2) | (Tag.Test6 & Tag.Test8);
    //    Assert.Contains(go, pat.GameObjects());
    //    pat = (Tag.Test1 | Tag.Test6) & (Tag.Test2 | Tag.Test7);
    //    Assert.Contains(go, pat.GameObjects());
    //    pat = (Tag.Test1 | Tag.Test6) & (Tag.Test8 | Tag.Test10);
    //    Assert.IsFalse(pat.GameObjects().Contains(go));

    //    var names = (TagNames)Tag.Test1 + Tag.Test2;
    //    Assert.Contains(go, names.both.GameObjects());
    //    names = Tag.Test1 + Tag.Test2;
    //    Assert.Contains(go, names.either.GameObjects());
    //    names = Tag.Test1 + Tag.Test2 + Tag.Test6;
    //    Assert.IsFalse(names.both.GameObjects().Contains(go));
    //    names = Tag.Test1 + Tag.Test2 + Tag.Test6;
    //    Assert.Contains(go, names.either.GameObjects());
    //    names = Tag.Test1 + Tag.Test2 + Tag.Test6 + Tag.Test3 - Tag.Test6;
    //    Assert.Contains(go, names.both.GameObjects());
    //    names = Tag.Test1 + Tag.Test2 + Tag.Test6 + Tag.Test3 - Tag.Test6;
    //    Assert.Contains(go, names.either.GameObjects());
    //    names = Tag.Test1 + Tag.Test2 + Tag.Test6 - Tag.Test1 - Tag.Test6 - Tag.Test2;
    //    Assert.IsFalse(names.either.GameObjects().Contains(go));

    //    TagSystem.AddTag(s_NotInGroupTags);
    //    go.AddTag(s_GroupTags);
    //    pat = (Tag.T1.Test1 | Tag.T3) & (Tag.T2.Test2 | Tag.T1);
    //    Assert.Contains(go, pat.GameObjects());

    //    Assert.Contains(go, Tag.T1.children.both.GameObjects());
    //    Assert.Contains(go, Tag.T1.children.either.GameObjects());
    //    Assert.IsFalse(Tag.T1.all.both.GameObjects().Contains(go));
    //    Assert.Contains(go, Tag.T1.all.either.GameObjects());
    //    Assert.Contains(go, (-Tag.T1.all.both).GameObjects());
    //    Assert.Contains(go, (Tag.T1.Test1.children - Tag.T1.Test1.S2 - Tag.T1.Test1.S3).both.GameObjects());

    //    Assert.IsFalse(Tag.T3.children.both.GameObjects().Contains(go));
    //    Assert.IsFalse(Tag.T3.children.either.GameObjects().Contains(go));
    //    Assert.IsFalse(Tag.T3.all.both.GameObjects().Contains(go));
    //    Assert.IsFalse(Tag.T3.all.either.GameObjects().Contains(go));
    //    Assert.Contains(go, (-Tag.T3).GameObjects());
    //    Assert.Contains(go, (-Tag.T3.all.either).GameObjects());

    //    Assert.IsFalse(Tag.all.Test1.both.GameObjects().Contains(go));
    //    Assert.Contains(go, Tag.all.Test1.either.GameObjects());
    //    Assert.IsFalse(Tag.all.Test1.all.both.GameObjects().Contains(go));
    //    Assert.Contains(go, Tag.all.Test1.all.either.GameObjects());
    //    Assert.IsFalse(Tag.all.Test1.children.both.GameObjects().Contains(go));
    //    Assert.Contains(go, Tag.all.Test1.children.either.GameObjects());
    //    Assert.Contains(go, (Tag.all.Test1 - Tag.T3.all).both.GameObjects());

    //    Assert.Contains(go, Tag.all.Test2.both.GameObjects());
    //    Assert.Contains(go, Tag.all.Test2.either.GameObjects());
    //    Assert.Contains(go, Tag.all.Test2.all.both.GameObjects());
    //    Assert.Contains(go, Tag.all.Test2.all.either.GameObjects());
    //    Assert.Contains(go, Tag.all.Test2.children.both.GameObjects());
    //    Assert.Contains(go, Tag.all.Test2.children.either.GameObjects());

    //    TagHelper.DestroyGameObject(go);
    //    TagSystem.RemoveNullGameObject();
    //}
#endregion

}

}