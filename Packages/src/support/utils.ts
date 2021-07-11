import { Constructor } from 'component/component-info-mgr';
import { $typeof } from 'puerts';
import TestBind from 'sandbox/TestBind';

export function uses(...c: Constructor[]) {
    c.forEach(type => {
        let pr = Object.getPrototypeOf(type.prototype);
        if ($typeof(pr.constructor) != null) {
            
            Object.getOwnPropertyNames(type.prototype).forEach(name => {
                console.log(`[check] ${ type.name }[${ $typeof(pr.constructor).FullName }] => ${ name }`)
                pr[name] = type.prototype[name];
            });
            (global.$providers as Map<string,any>).set($typeof(pr.constructor).FullName,type);
        }
    })
}