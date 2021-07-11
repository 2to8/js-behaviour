import { System } from 'csharp';

export default class MyUtil {
    static toArray<T>(a: System.Array$1<T>): T[] {
        let result = [];
        for (let i = 0; i < a.Length; i++) {
            result.push(a.get_Item(i))
        }
        return result;
    }
}