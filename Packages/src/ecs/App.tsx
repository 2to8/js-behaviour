// import { Autoload } from '@/Codegen/Autoload';
// import { Dump } from '@/Components/Dump';
// import { Items } from '@/Widget/Items';
// import { UnityEngine } from 'csharp';
// import React from 'react';
// import RouteProxy = ReactECS.RouteProxy;
// import JSTools = ReactECS.JSTools;
// import SceneManagement = UnityEngine.SceneManagement;
// import SceneManager = UnityEngine.SceneManagement.SceneManager;
//
// type _props = {
//     router: RouteProxy, //reconciler: typeof Reconciler
// }
//
// type _state = {
//     booted: boolean
//     helloWorld: string
// }
//
// export class App extends React.Component<_props, _state> {
//     router: RouteProxy
//    
//     //reconciler: typeof Reconciler
//    
//     constructor(props) {
//         super(props);
//         this.router = props.router;
//         //this.reconciler = props.reconciler;
//        
//         //console.log(JSTools.getDecimal(3.1415926));
//        
//         this.state = {
//             booted: true, helloWorld: 'Components Started',
//         };
//     }
//    
//     public render(): React.ReactNode {
//         return (<>
//             <Dump log={ this.state.helloWorld }/>
//             <Autoload/>
//             <Items scene={ SceneManager.GetActiveScene() }/>
//         </>);
//     }
// }