// import { Item } from "@/Widget/Item";
//
// (function(window:any) {
//     let jQuery
//     let $ = jQuery = function(selector: new (_: typeof Item) => any | string | typeof Item[] | void, context: new (_: typeof Item) => any | void | typeof Item, callback: new (_: new() => Item) => any | void) {  //jQuery构造函数
//         return new jQuery.fn.init(selector, context, callback);  //jQuery实例对象
//     };
//     jQuery.fn = jQuery.prototype = {  //jQuery原型对象
//         init: function(selector, context, callback) {  //初始化构造函数
//             selector = selector || document;  //初始化选择器，默认值为document
//             context = context || document;  //初始化上下文对象，默认值为document
//             if (selector.nodeType) {  //如果是DOM元素
//                 this[0] = selector;  //直接把该DOM元素传递给实例对象的伪数组
//                 this.length = 1;  //设置实例对象的length属性，表示包含1个元素
//                 this.context = selector;  //重新设置上下文为DOM元素
//                 return this;  //返回当前实例
//             }
//             if (typeof selector === 'string') {  //如果是选择符字符串
//                 let e = context.getElementsByTagName(selector);  //获取指定名称的元素
//                 for (let i = 0; i < e.length; i++) {  //使用for把所有元素传入当前实例数组中
//                     this[i] = e[i];
//                 }
//                 this.length = e.length;  //设置实例的length属性，定义包含元素的个数
//                 this.context = context;  //保存上下文对象
//                 return this;  //返回当前实例
//             } else {
//                 this.length = 0;  //设置实例的length属性值为0，表示不包含元素
//                 this.context = context;  //保存上下文对象
//                 return this;  //返回当前实例
//             }
//         },
//     }
//     jQuery.fn.init.prototype = jQuery.fn;
//     //扩展方法：jQuery迭代函数
//     jQuery.each = function(object, callback, args) {
//         for (let i = 0; i < object.length; i++) {  //使用for迭代jQuery对象中每个DOM元素
//             callback.call(object[i], args);  //在每个DOM元素上调用函数
//         }
//         return object;  //返回jQuery对象
//     }
//     //jQuery扩展函数
//     jQuery.extend = jQuery.fn.extend = function() {
//         const destination = arguments[0], source = arguments[1];  //获取第1个和第2个参数
//         //如果两个参数都存在，且都为对象
//         if (typeof destination == 'object' && typeof source == 'object') {
//             //把第2个参数对合并到第1个参数对象中，并返回合并后的对象
//             for (const property in source) {
//                 destination[property] = source[property];
//             }
//             return destination;
//         } else {  //如果包含一个参数，则把插件复制到jQuery原型对象上
//             for (const prop in destination) {
//                 this[prop] = destination[prop];
//             }
//             return this;
//         }
//     }
//     //开放jQuery接口
//     window.jQuery = window.types = jQuery;
// })(globalThis)