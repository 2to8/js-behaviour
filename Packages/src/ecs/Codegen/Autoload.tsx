///<import>
import { HelloWorldSystem } from '@/Manager/HelloWorldSystem';
import { SettingSystem } from '@/Manager/SettingSystem';
///</import>
import React from 'react';

export class Autoload extends React.Component<{}, {}> {
    public constructor(props) {
        super(props);
    }
    
    public render(): React.ReactNode {
        return (///<load>
        <>
            <HelloWorldSystem.HelloWorldSystem tid={ 3 } tag={ 'Manager.HelloWorldSystem' } />
            <SettingSystem.SettingSystem tid={ 5 } tag={ 'Manager.SettingSystem' } />
        </>
        ///</load>
        )
    }
}

export enum Tag {
    ///<tags>

    ///</tags>
}
