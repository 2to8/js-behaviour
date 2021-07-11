import client from 'libs/client'

export default {
    development: {
        client: client, //'sqlite3',
        useNullAsDefault: true,
        connection: {
            filename: './example.knex_db'
        },
        pool: {
            afterCreate: (conn, cb) => {
                conn.run('PRAGMA foreign_keys = ON', cb);
            }
        }
    },
    
    production: {
        client: client, //'postgresql',
        connection: {
            database: 'example'
        },
        pool: {
            min: 2,
            max: 10
        }
    }
} as any;

// export default config