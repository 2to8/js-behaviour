import typescript from 'rollup-plugin-typescript2';
//import babel from '@rollup/plugin-babel';
import commonjs from '@rollup/plugin-commonjs';
import json from "@rollup/plugin-json";
import resolve from '@rollup/plugin-node-resolve';
import replace from "@rollup/plugin-replace";
//import typescript from "@rollup/plugin-typescript";
//import typescript from "rollup-plugin-typescript";
//import cleaner from 'rollup-plugin-cleaner';
import sourceMaps from "rollup-plugin-sourcemaps";
//import builtins from 'rollup-plugin-node-builtins';
import globals from 'rollup-plugin-node-globals';
//import node_builtins from 'builtin-modules'
//import image from 'rollup-plugin-img';

const path = require("path")

const extensions = [
    '.js', '.jsx', '.ts', '.tsx', '.mjs', '.json', ".cjs"
];


export default [{
    input: "src/index.ts",
    output: {
        file: "../Assets/Res/JavaScript/index.js.txt",
        format: "umd",
        sourcemap: true,
        name: "index",
        globals: {
            csharp: "csharp",
            puerts: "puerts"
        }
    },
    plugins: [
        // 这个要是第一个
        replace({
            'preventAssignment': true,
            'process.env.NODE_ENV': JSON.stringify(process.env.NODE_ENV || 'development')
        }),
        json(),
    
        // Allows node_modules resolution
        globals(),
        //builtins(),
    
        // Allow bundling cjs modules. Rollup doesn't understand cjs
        commonjs({
            include: /node_modules/,
            transformMixedEsModules: true,
            esmExternals: true,
            requireReturnsDefault: "auto",
        }),
        resolve({
            jsnext: true,
            main:true,
            preferBuiltins: false,
            // browser: true,
            mainFields: [
                "jsnext",
                "preferBuiltins",
                // "browser"
            ],
            extensions
        }),
        sourceMaps(),
        typescript({
            tsconfig: 'tsconfig.json',
        })
    ],
    external: [
        "puerts",
        "csharp"
    ],
}
]
