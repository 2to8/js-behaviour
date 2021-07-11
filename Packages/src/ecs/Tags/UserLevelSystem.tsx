import { GameEngine, MoreTags, UnityEngine } from 'csharp';
import React from 'react';
import { ASystem } from 'Tags/ASystem';
import Debug = UnityEngine.Debug;
import Strings = GameEngine.Extensions.Strings;
import TagSystem = MoreTags.TagSystem;

export class UserLevelSystem extends ASystem {
    public render(): React.ReactNode
    {
        if (this.props.tags == null) return null;
        Debug.Log(Strings.ToBlue(`${this.gameObject.name} => ${this.constructor.name}, ${user.userLevel}`))
        
        TagSystem.SetText(this.gameObject, user.userLevel)
        return null;
    }
}