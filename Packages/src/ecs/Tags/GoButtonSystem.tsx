import { MoreTags, UnityEngine } from 'csharp';
import { stat } from 'fs';
import { find, fixDict, fixList } from 'libs/Dictionary';
import { number } from 'prop-types';
import { $typeof } from 'puerts';
import React from 'react';
import { $ } from 'types';
import { id } from 'Widget/id';
import Tags = MoreTags.Tags;
import Button = UnityEngine.UI.Button;
import TagSystem = MoreTags.TagSystem;
import GameObject = UnityEngine.GameObject;

export class GoButtonSystem extends React.Component<{ tags: Tags }> {
    
    componentWillUnmount() {
        console.log("GoButtonSystem unmount")
        // this.off('authChange', this.authChange);
        // this.authChange = null;
    }
    
    public render(): React.ReactNode {
        if (this.props.tags == null) return null;
        
        console.log('go button started')
        let button = this.props.tags.GetComponent($typeof(Button)) as Button;
        button?.onClick.AddListener(() => {
            console.log('go button clicked 123')
            fixList(TagSystem.query.tags('Id.Stages').result)?.forEach(t => t?.SetActive(false));
            fixList(TagSystem.query.tags('Id.Choice').result)?.forEach(t => t?.SetActive(true));
            this.props.tags.gameObject.SetActive(false)
            //$(id.GoBack, Button)?.onClick
        })
        
        return null;
    }
}