using GameEngine.Kernel;

//using Unity.Entities;

namespace GameEngine.Views.Contracts {

//[ RequiresEntityConversion ]
public abstract class TManager<T> : ApplicationBase<T> where T : TManager<T> { }

}