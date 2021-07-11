import { AutoInc, Column, PrimaryKey } from 'libs/sqlite3/utils/attribute';
import { BaseTable } from 'Table/BaseTable';

class DemoData extends BaseTable<DemoData>
{
    @Column('string')
    public name: string;
    
    @Column('number')
    public age: number;
    
    @Column('number')
    public sex: number;
}

export { DemoData }