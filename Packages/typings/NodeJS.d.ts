
declare namespace NodeJS {
    // @ts-ignore
    import { DBConnection } from 'libs/sqlite3/dbConnection';
    // @ts-ignore
    import { NodeCanvas, MoreTags } from 'csharp';
    // @ts-ignore
    import { UserTable } from 'Table/UserTable';
    //  @ts-ignore
    import { LevelTable } from 'Table/LevelTable';
    
    
    // @ts-ignore
    import { GlobalConfig } from 'Table/DemoData';
    import BindScript = NodeCanvas.Tasks.Actions.BindScript;
    import Tags = MoreTags.Tags;
    
    interface Global {
        tables: Map<any, any>;
        app: DBConnection;
        db: DBConnection;
        user: UserTable;
        config: GlobalConfig;
        gKey: number;
        level: LevelTable;
        stage: StageData;
        
        Execute(task: BindScript);
        
        OnTagStart(context: Tags);
        
        OnTagAwake(context: Tags);
    }
}

