using System;

namespace GameEngine.Updater {

/// <summary>
/// Periodically dispatches an event
/// </summary>
/// <author>Jackson Dunstan, http://JacksonDunstan.com/articles/3382
/// <license>MIT</license>
public interface IUpdater {

    float deltaTime { get; set; }

    /// <summary>
    /// Dispatched periodically
    /// </summary>
    event Action OnUpdate;

    /// <summary>
    /// Start dispatching OnUpdate events
    /// </summary>
    void Start();

    /// <summary>
    /// Stop dispatching OnUpdate events
    /// </summary>
    void Stop();

}

}