import { Component } from 'component/component-base';
import { component, property } from 'component/component-decoration';
import { Sandbox, System, UnityEngine } from 'csharp';
import TestScript = Sandbox.TestScript;

@component('User')
export class UserBehaviour1 {
}

@component('User')
export class UserBehaviour2 {
}

@component('User')
export class UserBehaviour3 {
}

@component('System')
export class SystemBehaviour1 {
}

@component('System')
export class SystemBehaviour2 {
}

@component('System')
export class SystemBehaviour3 {
}

/**
 * 使用component修饰器定义TestBehaviour为Js组件
 */
@component()
export  class TestBehaviour extends Component {
    /**
     * 使用property修饰器定义需要在Inspector上显示的属性及其类型
     */
    @property(UnityEngine.GameObject) prop1: UnityEngine.GameObject;
    
    /**
     * editable未实现，仅演示功能扩展模式
     */
    @property({
        type: System.Single, editable: true,
    }) prop2: number;
    
    /**
     * 数组的几种定义形式
     */
    @property({
        type: UnityEngine.GameObject, isArray: true,
    }) prop3: UnityEngine.GameObject[];
    @property([ UnityEngine.Vector3 ]) prop4: UnityEngine.Vector3[];
    @property({
        type: [ System.UInt32 ],
    }) prop5: number[];
    
    @property(UnityEngine.Camera) prop6: UnityEngine.Camera;
    
    @property(UnityEngine.Vector3) prop7: UnityEngine.Vector3;
    
    public Awake() {
        
        //  let _功夫="如来神掌";
        // gf
        // logging_trace_with_error.js
        let test = {
            add(x, y) {
                console.log(new Error().stack);
                return x + y;
            },
            
            calc() {
                return this.add(8, 11) + this.add(9, 14);
            },
            
            main() {
                let x = this.add(2, 3);
                let y = this.calc();
            },
        }
        
        global.testScript = (mb: TestScript) => {
            mb.script = this as any;
            console.log('test hello');
            (mb.script as unknown as TestBehaviour).SayHello();
        }
        
        test.main();
        console.log('Awake');
        
        //console.log(xxx.xxx);
        console.log(`prop1 = ${ this.prop1 }`);
        console.log(`prop2 = ${ this.prop2 }`);
        console.log(`prop3 = ${ this.prop3 }`);
        console.log(`prop4 = ${ this.prop4.length }`);
        console.log(`prop5 = ${ this.prop5 }`);
        console.log(`prop6 = ${ this.prop6 }`);
        console.log(`prop7 = {${ this.prop7.x }, ${ this.prop7.y }, ${ this.prop7.z }}`);
    }
    
    public Start() {
        console.log('Start');
        
    }
    
    public SayHello() {
        console.log('Hello, not bad!')
    }
    
    public OnEnable() {
        console.log('OnEnable');
    }
    
    public OnDisable() {
        console.log('OnDisable');
    }
    
    public OnDestroy() {
        console.log('OnDestroy');
    }
}