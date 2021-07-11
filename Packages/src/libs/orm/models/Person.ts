import { Model } from 'objection';

class Person extends Model {
    // Table name is the only required property.
    static get tableName() {
        return 'persons';
    }
}

export {
    Person
}
