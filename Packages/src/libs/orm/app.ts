import { isDefined } from 'libs/cs_helpers';
import { UnityEngine } from 'csharp';
import { $typeof } from 'puerts';
import knexConfig from './knexfile';
import Knex from 'knex';
import { Model } from 'objection';
import { Person } from './models/Person';

// Initialize knex.
console.log('Initialize knex.')
const knex = Knex(knexConfig.development);

// Bind all Models to the knex instance. You only
// need to do this once before you use any of
// your model classes.
console.log('Initialize model')
Model.knex(knex);

 async function main() {
    // Delete all persons from the knex_db.
    console.log('delete')
    await Person.query().delete();
    
    // Insert one row to the database.
    console.log('insert')
    await Person.query().insert({
        firstName: 'Jennifer', lastName: 'Aniston',
    } as any);
    
    // Read all rows from the knex_db.
    console.log('query')
    const people = await Person.query();
    
    console.log('query result', people);
}

export default () => {
    main().then(() => knex.destroy()).catch(err => {
        console.error('Error', err);
        return knex.destroy();
    })
};
