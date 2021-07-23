namespace GameEngine.Views.Contracts {

public abstract class BaseView<T> : BaseView where T : BaseView<T> { }

}