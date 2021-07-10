namespace GameEngine.Views.Contracts {

public abstract class TView<T> : View where T : TView<T> { }

}