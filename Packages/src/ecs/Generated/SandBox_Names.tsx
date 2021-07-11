import React from "react";
import { NodeCanvas } from 'csharp';
import { DebugPage_Main } from 'Prefabs/SandBox/DebugPage/DebugPage_Main';
import { BootUI_Main } from 'Scenes/BootScene/BootUI/BootUI_Main';
import BindScript = NodeCanvas.Tasks.Actions.BindScript;

interface INames {
    [name: string]: (...args: any) => JSX.Element,
}

export const SandBox_Names: INames = {
    DebugPage_Main: args => <DebugPage_Main { ...args }/>,
    BootUI_Main: (target: BindScript) => <BootUI_Main target={ target }/>,
}