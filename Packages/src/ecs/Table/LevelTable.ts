import {GameEngine, MoreTags, UnityEngine} from 'csharp';
import {AutoInc, Column, PrimaryKey} from 'libs/sqlite3/utils/attribute';
import {$typeof} from 'puerts';
import {BaseTable} from 'Table/BaseTable';
import {$} from 'types';
import {id} from 'Widget/id';
import Debug = UnityEngine.Debug;
import Strings = GameEngine.Extensions.Strings;
import Tags = MoreTags.Tags;

export enum CellType {
    normal = 0,
    hard,
    rest,
    unknown,
    boss,
    shop,
}

export class LevelCell {
    stars: number;
    id: number;
    row: number;
    col: number;
    type: CellType = CellType.normal;
}

export class LevelTable extends BaseTable<LevelTable> {
    @Column('number')
    width: number = 7;

    @Column('number')
    height: number = 10;

    @Column('object')
    data: LevelCell[][] = []

    init() {
        Debug.Log(Strings.ToRed('init level'));
        if (this.id == 0) {
            let count = this.db.updateOrInsert(this);
            console.log(`insert first level: ${count} => ${this.id}`)
        }
    }
}
