using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace MoreTags {

[Serializable, TableList]
public class TagState {

    //[ToggleGroup(nameof(@on))]
    [TableColumnWidth(20, false)]
    public bool on;

    //[ToggleGroup(nameof(@on))]
    //public int order;

    //[ToggleGroup(nameof(@on))]
    public Component component;

}

}