import { TestAction } from 'app/actions/TestAction';
import { Constructor } from 'component/component-info-mgr';
import { GameEngine, UnityEngine } from 'csharp';
import { $typeof } from 'puerts';
import TestBind from 'sandbox/TestBind';
import Strings = GameEngine.Extensions.Strings;
import Debug = UnityEngine.Debug;
import Component = UnityEngine.Component;

/**
 *** Object.appendChain(@object, @prototype)
 *
 * Appends the first non-native prototype of a chain to a new prototype.
 * Returns @object (if it was a primitive value it will transformed into an object).
 *
 *** Object.appendChain(@object [, "@arg_name_1", "@arg_name_2", "@arg_name_3", "..."], "@function_body")
 *** Object.appendChain(@object [, "@arg_name_1, @arg_name_2, @arg_name_3, ..."], "@function_body")
 *
 * Appends the first non-native prototype of a chain to the native Function.prototype object, then
 * appends a new Function(["@arg"(s)], "@function_body") to that chain. Returns the function.
 *
 **/

let appendChain = function(oChain, oProto) {
    if (arguments.length < 2) {
        throw new TypeError('Object.appendChain - Not enough arguments');
    }
    if (typeof oProto === 'number' || typeof oProto === 'boolean') {
        throw new TypeError('second argument to Object.appendChain must be an object or a string');
    }
    
    var oNewProto = oProto, oReturn, o2nd, oLast;
    
    oReturn = o2nd = oLast = new oChain.constructor(oChain);
    
    for (var o1st = this.getPrototypeOf(o2nd); o1st !== Object.prototype && o1st !== Function.prototype; o1st = this.getPrototypeOf(o2nd)) {
        o2nd = o1st;
    }
    
    if (oProto.constructor === String) {
        oNewProto = Function.prototype;
        oReturn = Function.apply(null, Array.prototype.slice.call(arguments, 1));
        this.setPrototypeOf(oReturn, oLast);
    }
    
    this.setPrototypeOf(o2nd, oNewProto);
    return oReturn;
}

export function uses(...c: Constructor[]) {
    c.forEach(type => {
        let pr = Object.getPrototypeOf(type.prototype);
        if ($typeof(pr.constructor) != null) {
            //Object.setPrototypeOf(pr,type.prototype);
            //pr.prototype = Object.create(Object.getPrototypeOf(type)) 
            //Object.setPrototypeOf(Object.getPrototypeOf(type.prototype), type.prototype);
            
            //appendChain(pr, new type());
            //Object.setPrototypeOf(pr,Object.getPrototypeOf(type) );
            
            Object.getOwnPropertyNames(type.prototype).forEach(name => {
                console.log(`[check] ${ type.name }[${ $typeof(pr.constructor).FullName }] => ${ name }`)
                pr[name] = type.prototype[name];
            });
            
            if (!$typeof(pr.constructor).IsAbstract && !$typeof(Component).IsAssignableFrom($typeof(pr.constructor))) {
                let TA: any = type;
                let N = new TA();
                for (const name of Object.keys(N)) {
                    console.log(name, N[name])
                    pr[name] = N[name];
                }
            }
            
            Debug.Log(Strings.ToBlue(type.name + ' : ' + JSON.stringify(Object.keys(type.prototype)) + ' : ' + Object.getOwnPropertyNames(type.prototype)))
            for (let name of Object.keys(Object.getPrototypeOf(type.prototype))) {
                if (type.prototype.hasOwnProperty(name)) {
                    console.log(Strings.ToYellow(`[check] ${ type.name }[${ $typeof(pr.constructor).FullName }] => ${ name }`))
                    pr[name] = type.prototype[name];
                }
                
            }
            
            // var getAllPropertyNames = (obj) => {
            //     let props = [];
            //     do {
            //         props = props.concat(Object.getOwnPropertyNames(obj));
            //     } while (obj = Object.getPrototypeOf(obj));
            //     return props;
            // };
            //
            // for (let prop of getAllPropertyNames(type.prototype)) {
            //     console.log(prop);
            // }
            
            // Object.keys(type.prototype).forEach(name => {
            //     pr[name] = type.prototype[name];
            // });
            
            (global.$providers as Map<string, any>).set($typeof(pr.constructor).FullName, type);
        }
    })
}