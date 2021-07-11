import client from 'libs/client';
import { UnityEngine } from 'csharp';
import knex from 'knex';
import Debug = UnityEngine.Debug;


Debug.Log("test");

let knex_db = knex({
    
    client: client ,   //
    connection: ':memory:', //
    useNullAsDefault: true,    //
    
    // Optional properties with default values
    //@ts-ignore
    name: 'knex_database',//
    version: '1.0', //
    displayName: 'knex_database', // inherited from 'name'
    estimatedSize: 5 * 1024 * 1024, // 5MB
    
    // AlaSQL specific options https://github.com/agershun/alasql/wiki/AlaSQL%20Options
    // options: {
    //     mysql: true
    // }
} as any);

export default knex_db;
