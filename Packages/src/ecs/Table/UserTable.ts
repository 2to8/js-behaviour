import { GameEngine, UnityEngine } from 'csharp';
import { AutoInc, Column, PrimaryKey } from 'libs/sqlite3/utils/attribute';
import { BaseTable } from 'Table/BaseTable';
import Debug = UnityEngine.Debug;
import Strings = GameEngine.Extensions.Strings;

class UserTable extends BaseTable<UserTable> {
    money: number = 123;
    coin: number = 456;
    diamond: number = 789;
    userLevel: number = 1;
    health: number = 100;
    healthTotal: number = 200;
    levelPoint: number = 100;
    levelPointTotal: number = 300;
    
    init() {
        Debug.Log(Strings.ToYellow('UserTable Init'))
        // this.coin = this.coin || 500;
        // this.diamond = this.diamond || 500;
        // this.money = this.money || 500;
    }
    
}

export { UserTable }