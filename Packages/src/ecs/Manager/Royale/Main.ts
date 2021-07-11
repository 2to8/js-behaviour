import { $ } from '@/types';
import { el } from '@/Widget/el';
import { health } from '@/Widget/health';
import { Item } from '@/Widget/Item';
import { speed } from '@/Widget/speed';
import { UnityEngine } from 'csharp';
import Component = UnityEngine.Component;
import Debug = UnityEngine.Debug;

//  interface ItemFilter  {
//     any?: (typeof Item)[], all?: (typeof Item)[], none?: (typeof Item)[],
// }

// 下面这两个是一样的
//type Type<T> = new(...args: any[]) => T;
// interface Type<T> {
//     new(...args: any[]): T;
// }



$([ speed, health ], (c,e) => {
    e.speed(v => v.value += 1)
})

let test = $([], el.text).text;
$([], el.button).onClick.AddListener(() => {
    Debug.Log('test');
})

$(el.text).text = "test";

$(el.button).onClick.AddListener(() => {
    Debug.Log('test');
})