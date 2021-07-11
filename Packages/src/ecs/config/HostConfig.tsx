import { UnityWidget } from '@/Components/UnityWidget';
import { UnityEngine } from 'csharp';

function deepEquals(x, y) {
    if (x === y) return true;
    const xEqual = x == null ? null : x.Equals;
    const yEqual = y == null ? null : y.Equals;
    if (xEqual || yEqual) {
        return xEqual ? x.Equals(y) : y.Equals(x);
    }
    if (!(x instanceof Object) || !(y instanceof Object)) return false;
    for (let p in x) { // all x[p] in y
        if (p === 'children') continue;
        if (!deepEquals(x[p], y[p])) return false;
    }
    
    for (let p in y) {
        if (p === 'children') continue;
        if (!x.hasOwnProperty(p)) return false;
    }
    
    return true;
}

export const HostConfig = {
    getRootHostContext() {
        return {};
    }, //CanvasPanel()的parentHostContext是getRootHostContext返回的值
    getChildHostContext(parentHostContext) {
        
        return parentHostContext;//no use, share one
    },
    appendInitialChild(parent, child) {
        parent.appendChild(child);
    },
    appendChildToContainer(container, child) {
        container.appendChild(child);
    },
    appendChild(parent, child) {
        parent.appendChild(child);
    },
    createInstance(type, props) {
        return new UnityWidget(type, props);
    },
    createTextInstance(text) {
        return new UnityWidget('UnityEngine.UI.Text', {
            text: text,
            font: UnityEngine.Font.CreateDynamicFontFromOSFont('Arial', 12),
        });
    },
    finalizeInitialChildren() {
        return false
    },
    getPublicInstance(instance) {
        console.warn('getPublicInstance');
        return instance
    },
    now: Date.now,
    prepareForCommit() {
        //log('prepareForCommit');
        
    },
    resetAfterCommit(container) {
        container.addToViewport(0);
        
    },
    resetTextContent() {
        console.error('resetTextContent not implemented!');
    },
    shouldSetTextContent(type, props) {
        return false
    },
    
    commitTextUpdate(textInstance, oldText, newText) {
        if (oldText !== newText) {
            textInstance.update({}, { Text: newText })
        }
    },
    
    //return false表示不更新，真值将会出现到commitUpdate的updatePayload里头
    prepareUpdate(instance, type, oldProps, newProps) {
        try {
            return !deepEquals(oldProps, newProps);
        } catch (e) {
            console.error(e.message);
            return true;
        }
    },
    commitUpdate(instance, updatePayload, type, oldProps, newProps) {
        try {
            instance.update(oldProps, newProps);
        } catch (e) {
            console.error('commitUpdate fail!, ' + e);
        }
    },
    removeChildFromContainer(container, child) {
        container.removeChild(child);
    },
    removeChild(parent, child) {
        parent.removeChild(child);
    },
    insertBefore(parentInstance, child, beforeChild) {
        parentInstance.appendChild(child);
        child.nativePtr.transform.SetSiblingIndex(beforeChild.nativePtr.transform.GetSiblingIndex())
    }, //useSyncScheduling: true,
    supportsMutation: true,
    isPrimaryRenderer: true,
    supportsPersistence: false,
    supportsHydration: false,
    
    clearContainer(container) {
        console.log('[clear container]', container)
    },
    
    shouldDeprioritizeSubtree: undefined,
    setTimeout: undefined,
    clearTimeout: undefined,
    cancelDeferredCallback: undefined,
    noTimeout: undefined,
    scheduleDeferredCallback: undefined,
}
