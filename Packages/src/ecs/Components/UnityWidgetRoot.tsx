import { UnityEngine } from 'csharp';
import GameObject = UnityEngine.GameObject;

export class UnityWidgetRoot {
    nativePtr: GameObject | any;
    type: any;
    Added: Boolean;
    
    constructor(nativePtr: GameObject | any) {
        this.nativePtr = nativePtr;
    }
    
    appendChild(child) {
        if (!child.nativePtr) {
            child.getComp(this.nativePtr);
            child.mergeComp();
        } else {
            console.log(this.type, child.nativePtr.transform)
            child.nativePtr.transform.SetParent(this.nativePtr.transform)
        }
    }
    
    removeChild(child) {
        child.unbindAll();
        if (child.compPtr) {
            UnityEngine.Object.Destroy(child.compPtr);
        } else {
            child.nativePtr.transform.SetParent(null)
            UnityEngine.Object.Destroy(child.nativePtr);
        }
    }
    
    addToViewport(z) {
        if (!this.Added) {
            this.Added = true;
        }
    }
    
    removeFromViewport() {
        this.Added = false;//?????
    }
    
    getWidget() {
        return this.nativePtr;
    }
}