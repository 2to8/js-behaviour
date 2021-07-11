import { AutoInc, Column, PrimaryKey } from 'libs/sqlite3/utils/attribute';
import { BaseTable } from 'Table/BaseTable';

class GlobalConfig extends BaseTable<GlobalConfig>
{
    protected isUser = false;
    
    @Column('string')
    public name: string;
    
    @Column('string')
    public lastUID: string;
    
    @Column('string')
    public lastUsername: string;
    
    @Column('number')
    public age: number;
    
    @Column('number')
    public sex: number;
}

export { GlobalConfig }