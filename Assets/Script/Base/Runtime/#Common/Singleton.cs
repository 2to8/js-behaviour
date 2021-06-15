namespace Base.Runtime
{
    [System.Serializable]
    public class Singleton<T> where T : Singleton<T>, new()
    {
        protected Singleton() { }
        public static T Inst { get; } = new T();
    }
}