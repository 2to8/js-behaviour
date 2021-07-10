using UnityEngine;
using System.Collections;
using PowerInject;

namespace Uhyre1
{
    [Power]
    public class HUD : MonoBehaviour
    {
        [Inject]
        private PointsKeeper pointsKeeper;

        [Inject]
        private GameState gameState;

        [Inject]
        private Labyrinth labyrinth;

        private void OnGUI()
        {
            GUI.contentColor = Color.white;
            GUI.skin.label.fontSize = 20;
            GUILayout.Label(" POINTS : " + pointsKeeper.getTotal());
            if(gameState.PlayerIsDead) {
                GUI.contentColor = Color.red;
                GUI.skin.label.fontSize = 100;
                GUILayout.Label("                  You lost !");
            }
            if(gameState.PlayerWon) {
                GUI.contentColor = Color.white;
                GUI.skin.label.fontSize = 100;
                GUILayout.Label("                  You WON !");
            }
        }
    }
}