// using GameEngine.Kernel;
// using SqlCipher4Unity3D;
//
// #if ECS
// using Unity.Entities;
// #endif
//
// namespace GameEngine.Extensions {
//
// public static class ComponentDataExtesion {
//
//     static SQLiteConnection knex_db => Core.Connection;
// #if ECS
//     public static string ToString(this IComponentData data) => JsonConvert.SerializeObject(data);
//
//     public static void Insert(this IComponentData data)
//     {
//         knex_db.CreateTable(data.GetType());
//         knex_db.InsertWithChildren(data);
//     }
//
//     public static void Save(this IComponentData data)
//     {
//         knex_db.CreateTable(data.GetType());
//         knex_db.InsertOrReplaceWithChildren(data);
//
//         // knex_db.UpdateWithChildren(data);
//     }
// #endif
//
//     // public static TableQuery<T> Table()
//     // {
//     //     // 同步数据库结构
//     //     Connection.CreateTable<T>();
//     //
//     //     return Connection.Table<T>();
//     // }
//     //
//     // public static TableQuery<T> Where(Expression<Func<T, bool>> predExp)
//     // {
//     //     return Connection.Table<T>().Where(predExp);
//     // }
//    #if ECS
//     public static TableQuery<T> Query<T>(this IComponentData data, Expression<Func<T, bool>> predExp = null)
//         where T : IComponentData, new()
//     {
//         knex_db.CreateTable<T>();
//
//         if(predExp != null) {
//             return knex_db.Table<T>().Where(predExp);
//         }
//
//         return knex_db.Table<T>();
//     }
//
//     // public static IComponentData Load(int pk)
//     // {
//     //     return knex_db.GetWithChildren(pk);
//     // }
//     public static int Delete(this IComponentData data) => knex_db.Delete(data.GetType());
//     public static int DropTable(this IComponentData data) => knex_db.DropTable(data.GetType());
// #endif
//
// }
//
// }

