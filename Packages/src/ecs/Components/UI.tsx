// import React from 'react'
// import { UnityEngine } from 'csharp';
//
// export namespace Raw {
//     import CommandProps = Raw.Props.CommandProps;
//
//     export enum Id {
//         EmptyId = 0x1000, BlueTank, YellowTank,
//     }
//
//     export enum Tags {
//         EmptyTag = 0x100, Tank, Enemy
//     }
//
//     export namespace Props {
//         export type TextProps = { text?: string, font?: any }
//         export type ButtonProps = { onClick?: () => void }
//         export type GridLayoutGroupProps = { cellSize?: UnityEngine.Vector2 }
//         export type HorizontalLayoutGroupProps = { spacing: number }
//         export type CommandProps = {}
//     }
//
//     export class Command extends React.Component<CommandProps> {
//         Any: []
//         All: []
//         None: []
//         // render 执行完毕自动添加tags
//         toAdd: []
//         // render 执行完毕自动移除tags
//         toRemove: []
//
//         constructor(props) {
//             super(props);
//             this.state = {
//                 ...this,
//                 entites: null,
//             };
//         }
//
//         public render(): React.ReactNode {
//
//             return '';
//         }
//
//     }
//
//     export class Text extends React.Component<Props.TextProps> {
//         render() {
//             return React.createElement('UnityEngine.UI.Text', this.props, undefined);
//         }
//     }
//
//     export class Button extends React.Component<Props.ButtonProps> {
//         render() {
//             return React.createElement('UnityEngine.UI.Button', this.props, undefined);
//         }
//     }
//
//     export class GridLayoutGroup extends React.Component<Raw.Props.GridLayoutGroupProps> {
//         render() {
//             return React.createElement('UnityEngine.UI.GridLayoutGroup', this.props, undefined);
//         }
//     }
//
//     export class GameObject extends React.Component {
//         render() {
//             return React.createElement('GameObject', this.props);
//         }
//     }
//
// }
//
// export class TextButton extends React.Component<Raw.Props.ButtonProps & Raw.Props.TextProps> {
//     render() {
//         return (<Raw.GameObject>
//             { React.createElement(Raw.Text, { font: UnityEngine.Font.CreateDynamicFontFromOSFont('Arial', 12), ...this.props }, undefined) }
//             { React.createElement(Raw.Button, this.props, undefined) }
//             { this.props.children }
//         </Raw.GameObject>);
//     }
// }
//
// export class GridLayout extends React.Component<Raw.Props.GridLayoutGroupProps> {
//     render() {
//         return (<Raw.GameObject>
//             { React.createElement(Raw.GridLayoutGroup, this.props, undefined) }
//             { this.props.children }
//         </Raw.GameObject>);
//     }
// }
//
// export class Text extends React.Component<{ text: string, font?: any }> {
//     render() {
//         return (<Raw.GameObject>
//             { React.createElement(Raw.Text, { font: UnityEngine.Font.CreateDynamicFontFromOSFont('Arial', 12), ...this.props }, undefined) }
//             { this.props.children }
//         </Raw.GameObject>);
//     }
// }
