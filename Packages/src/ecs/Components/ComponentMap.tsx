import { id } from '@/Widget/id';
import { Item } from '@/Widget/Item';
import { System, UnityEngine } from 'csharp';
import { $typeof } from 'puerts';
import UI = UnityEngine.UI;

//
// function fillFullyQualifiedName(m: any, name: string) {
//     // if it's a module
//     if (typeof m === "object") {
//         for (const propName of Object.keys(m)) {
//             fillFullyQualifiedName(m[propName], name + "." + propName);
//         }
//     }
//     // else, if it's a class with the fullyQualifiedName property
//     else if (typeof m === "function" && m.hasOwnProperty("fullyQualifiedName")) {
//         m["fullyQualifiedName"] = name;
//     }
// }
//
// //   https://stackoverflow.com/questions/28173555/full-object-name
// fillFullyQualifiedName(id, "id");
// fillFullyQualifiedName(id.btn, "id.btn");

let types = new Map<typeof Item, System.Type>()
types.set(id.btn.pause, $typeof(UI.Button))


export default types;