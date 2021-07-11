import Client_SQLite3 from 'knex/lib/dialects/sqlite3'
import { default as sqlite } from 'libs/sqlite';

export default class client extends Client_SQLite3 {
    _driver() {
        return sqlite;
    }
}
