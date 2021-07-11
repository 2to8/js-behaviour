//import 'libs/cs_presets'
import { GameEngine, System, UnityEngine } from 'csharp';
import { DemoData } from 'Table/DemoData';
import { GlobalConfig } from 'Table/GlobalConfig';
import { LevelTable } from 'Table/LevelTable';
import { StageTable } from 'Table/StageTable';
import { UserTable } from 'Table/UserTable';

import { DBConnection } from 'libs/sqlite3/dbConnection';
import { Column } from 'libs/sqlite3/utils/attribute';
import Application = UnityEngine.Application;
import Debug = UnityEngine.Debug;
import Directory = System.IO.Directory;
import Strings = GameEngine.Extensions.Strings;
import AppData = GameEngine.Kernel._Appliation.Models.AppData;


global.app = global.app || {} as DBConnection
global.db = global.db || {} as DBConnection
global.config = global.config || {}
global.user = global.user || {} as UserTable
global.level = global.level || {} as LevelTable
global.stage = global.stage || {}


let dir = Application.persistentDataPath + '/user_data/';

if (!Directory.Exists(dir)) {
    Directory.CreateDirectory(dir)
}

// 系统数据库
app = new DBConnection(dir + Strings.md5('appdata')).open().IsUser(false);
// 当前用户数据库
db = new DBConnection(dir + Strings.md5(config.lastUsername || 'guest')).open()

config = app.instance(GlobalConfig);
user = db.instance(UserTable);
level = db.instance(LevelTable);
stage = db.instance(StageTable);

Debug.Log(Strings.ToRed(`user: ${ JSON.stringify(user) }`))


function* countAppleSales() {
    let saleList = [ 3, 7, 5 ]
    for (let i = 0; i < saleList.length; i++) {
        yield saleList[i]
    }
}

export function DatabaseInit() {
    debug_sqlite();
}

export function debug_sqlite() {

    let data = new DemoData();
    data.id = 1;
    data.age = 10;
    data.sex = 1;
    data.name = 'test'
    let state

    // todo:data.id 报错, 需要用下面那个方式
    // state = conn.table(Data)
    //         .where(o => o.id == data.id && data.id != 0)
    //         .updateOrInsert(data);
    // console.log(state);

    let id = data.id;
    state = db.table(DemoData)
    .where(o => o.id == id && id != 0, { id })
    .updateOrInsert(data);
    console.log(JSON.stringify(state));

    let queryAll: DemoData[] = db.table(DemoData).query();
    console.log(JSON.stringify(queryAll));

    let queryBetween: DemoData[] = db.table(DemoData)
    .between(o => o.age, '20', '30')
    .query();
    console.log(JSON.stringify(queryBetween));

    let appleStore = countAppleSales()  // Generator { }
    console.log(appleStore.next())      // { value: 3, done: false }
    console.log(appleStore.next())      // { value: 7, done: false }
    console.log(appleStore.next())      // { value: 5, done: false }
    console.log(appleStore.next())      // { value: undefined, done: true }
    let current;
    while (!(current = appleStore.next()).done) {

    }

}



