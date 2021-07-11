import { GameEngine, UnityEngine, Utils } from 'csharp';
import { $typeof } from 'puerts';
import React from 'react';
import { LevelCell } from 'Table/LevelTable';
import { ASystem } from 'Tags/ASystem';
import Transforms = GameEngine.Extensions.Transforms;
import Color = UnityEngine.Color;
import GameObject = UnityEngine.GameObject;
import RectTransform = UnityEngine.RectTransform;
import UI = UnityEngine.UI;
import LayoutRebuilder = UnityEngine.UI.LayoutRebuilder;
import RndReference = Utils.RndReference;

export class LevelSystem extends ASystem {
    public onStart(): void {
        super.onStart();
        // if (level.data.length == 0) {
        console.log('init level items')
        let c = this.gameObject.GetComponents($typeof(RndReference))?.toArray() as RndReference[];
        if(c == null) {
            console.log("rndreference not found")
            return;
        }
        let icons = c.filter(t => t.mainLabel.labelString == 'LevelIcon')[0];
        let emptyIcon = icons.GetItem('EmptyIcon');

        let prefab = c.filter(t => t.mainLabel.labelString == 'LevelBtn')[0];
        let levelBtn = prefab.GetItem('LevelBtn').gameObject;
        Transforms.ClearChildTransforms(this.transform);

        for (let i = 0; i < level.height; i++) {
            for (let j = 0; j < level.width; j++) {
                let btn = UnityEngine.Object.Instantiate(levelBtn, this.transform) as GameObject;
                let image = btn.GetComponent($typeof(UI.Image)) as UI.Image;
                if (j == 0 || j == level.width - 1 || (i == level.height - 1 && (j < 2 || j >= level.width - 2))) {
                    //Debug.Log(Strings.ToYellow(`empty icon: ${ emptyIcon.GetType().FullName
                    // }`))
                    image.sprite = emptyIcon.sprite;
                } else {
                    let types = [
                        'EmptyIcon',
                        'FlashIcon',
                        'CamIcon',
                        'HPIcon',
                        'MpBottleIcon',
                        'UnknownIcon',
                    ];
                    let icon = icons.GetItem(...types);
                    while (i == level.height - 1 && (icon.label.labelString == 'EmptyIcon' || icon.label.labelString == 'HPIcon')) {
                        icon = icons.GetItem(...types);
                    }
                    image.sprite = icon.sprite;
                    if (icon.label.labelString != 'EmptyIcon') {
                        image.color = Color.white;
                    }

                    //Debug.Log(Strings.ToYellow(`icon: ${ icon.GetType().FullName }`))
                    //image.sprite = icon as Sprite;
                    // if (icon instanceof Texture2D) {
                    //     let texture = icon as Texture2D;
                    //     image.overrideSprite = Sprite.Create(texture, new Rect(0, 0,
                    // texture.width, texture.height), Vector2.zero); } else if (icon
                    // instanceof Sprite) { image.sprite = icon; }

                }
                if (level.data[i] == null) {
                    level.data[i] = [];
                }
                if (level.data[i][j] == null) {
                    let item = new LevelCell();

                    level.data[i].push(item)
                }

            }
        }

        let sr = this.gameObject.GetComponentInParent($typeof(UI.ScrollRect)) as UI.ScrollRect;
        if (level.width == 7) {
            sr.horizontal = false;
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(this.gameObject.GetComponent($typeof(RectTransform)) as RectTransform);
        sr.vertical = true;
        sr.horizontalNormalizedPosition = 0.5;
        sr.verticalNormalizedPosition = 0;

        //let refer = c.
        //let emptyIcon = refer.GetItem("EmptyIcon")
        // }
    }
}
