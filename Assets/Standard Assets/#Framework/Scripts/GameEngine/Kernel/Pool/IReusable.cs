namespace GameEngine.Kernel.Pool {

public interface IReusable {

    void OnSpawn(); //当取出时调用

    void OnUnspawn(); //当回收时调用

}

}