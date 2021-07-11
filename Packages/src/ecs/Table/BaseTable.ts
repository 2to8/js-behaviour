import { DBConnection } from 'libs/sqlite3/dbConnection';
import { DBQuery } from 'libs/sqlite3/dbQuery';
import { AutoInc, Column, PrimaryKey } from 'libs/sqlite3/utils/attribute';

type Type<T> = { new(...args: any[]): T };

class BaseTable<T extends BaseTable<T>> {
    @Column('number') @PrimaryKey() @AutoInc()
    public id: number;
    db: DBQuery<T>;
    
    c: { new(...args: any[]): T };
    
    init() { }
    
}

export { BaseTable }