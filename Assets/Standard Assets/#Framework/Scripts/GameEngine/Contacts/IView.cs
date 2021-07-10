using System.Collections.Generic;
using UnityEngine;

namespace GameEngine.Contacts {

public interface IView : IApplication {

    // 视图标识
    [HideInInspector]
    string Name { get; set; }

    List<string> attentionEvents { get; set; }
    List<IController> controllers { get; set; }

    void RegisterViewEvents();

    void Handle(string eventName, object sender, object data);

}

}