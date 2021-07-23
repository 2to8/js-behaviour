using Sirenix.OdinInspector;

namespace Common
{
    public partial class View : SerializedMonoBehaviour { }

    public partial class View<T> : View where T : View<T> { }
}