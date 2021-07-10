namespace GameEngine.Updater.Tests {

class DamageOverTime {

    IPlayer player;
    IUpdater updater;

    public DamageOverTime(IUpdater updater, IPlayer player)
    {
        this.player = player;
        this.updater = updater;

        // Subscribe to be called every frame
        updater.OnUpdate += HandleUpdate;
    }

    // Called every frame
    void HandleUpdate()
    {
        player.Health -= 5;

        if (player.Health <= 0) {
            // Unsubscribe to stop being called every frame
            updater.OnUpdate -= HandleUpdate;
        }
    }

}

}