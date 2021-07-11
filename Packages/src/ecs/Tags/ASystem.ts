import { MoreTags, UnityEngine } from 'csharp';
import React from 'react';
import Tags = MoreTags.Tags;
import GameObject = UnityEngine.GameObject;
import Transform = UnityEngine.Transform;

export class ASystem extends React.Component<{ tags: Tags }> {
    result: React.ReactNode = null;
    
    get gameObject(): GameObject {
        return this.props.tags.gameObject;
    }
    
    get transform(): Transform {
        return this.props.tags.transform;
    }
    
    onStart() {
    
    }
    
    public render(): React.ReactNode {
        if (this.props.tags == null) return null;
        this.onStart();
        return this.result;
    }
}
