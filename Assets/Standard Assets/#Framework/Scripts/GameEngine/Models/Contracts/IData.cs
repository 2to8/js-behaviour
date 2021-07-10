#if ECS
using Unity.Entities;

namespace Engine.Models.Contracts {

public interface IData : IComponentData {

    int id { get; set; }

}

public interface ISharedData : ISharedComponentData {

    int id { get; set; }

}

}
#endif