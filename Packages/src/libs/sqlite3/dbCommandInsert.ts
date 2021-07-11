import { SqlCipher4Unity3D, System } from 'csharp';
import { BindParameter } from './dbCommand';
import { DBConnection } from './dbConnection';
import SQLiteCommand = SqlCipher4Unity3D.SQLiteCommand;

// type SqliteCommand = CS.Mono.Data.Sqlite.SqliteCommand;
// const ConnectionState = CS.System.Data.ConnectionState;

class DBCommandInsert {
    commandText: string;
    private _conn: DBConnection;
    private _command: SQLiteCommand;
    
    constructor(conn: DBConnection) {
        this._conn = conn;
        this.commandText = '';
    }
    
    isConnect(conn: DBConnection) {
        return this._conn === conn;
    }
    
    isValid() {
        return this._conn && this._conn.opened;//&& this._conn.handle.State !== ConnectionState.Closed;
    }
    
    executeUpdate(objs: any[]) {
        if (this._conn.trace) {
            console.log(this.commandText + '\nBindings:' + objs);
        }
        
        if (!this._command) this._command = this.prepare();
        //bind the values.
        this._command.ClearBindings();
        if (objs) objs.forEach(val => BindParameter(this._command, val));
        
        return this._command.ExecuteNonQuery();
    }
    
    dispose() {
        let command = this._command;
        this._command = null;
        if (command) {
            command = null;
        }
    }
    
    private prepare(): SQLiteCommand {
        var command = this._conn.handle.CreateCommand(this.commandText);
        // command.Prepare();
        return command;
    }
}

export {
    DBCommandInsert,
}