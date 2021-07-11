
//declare var g: NodeJS.Global & typeof globalThis;

declare global
{
    // import { GlobalConfig } from 'Table/DemoData';
    // export var config: GlobalConfig;
    interface String
    {
        prependHello(): string;
    }
}

export {}