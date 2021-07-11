import { GameEngine, MoreTags, UnityEngine } from 'csharp';
import React from 'react';
import { ASystem } from 'Tags/ASystem';
import { $ } from 'types';
import { id } from 'Widget/id';
import Debug = UnityEngine.Debug;
import Strings = GameEngine.Extensions.Strings;
import TagSystem = MoreTags.TagSystem;

export class HealthSystem extends ASystem {
    public render(): React.ReactNode {
        if (this.props.tags == null) return null;
        Debug.Log(Strings.ToBlue(`${ this.gameObject.name } => ${ this.constructor.name }, ${ user.health }, ${ user.healthTotal }`))
        
        TagSystem.SetText(this.gameObject, user.health / user.healthTotal * 100)
        $(id.HealthTotal, (t, go) => {
            TagSystem.SetText(go, `${ user.health } / ${ user.healthTotal }`)
        })
        $(id.LevelPoint, (t, go) => {
            TagSystem.SetText(go, user.levelPoint / user.levelPointTotal * 100)
        })
        return null;
    }
}