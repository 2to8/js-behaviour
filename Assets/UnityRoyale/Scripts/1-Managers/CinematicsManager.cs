using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

namespace UnityRoyale
{
    public class CinematicsManager : Manager<CinematicsManager>
    {
        public PlayableDirector redCastleCollapse, blueCastleCollapse;
        

        public void PlayCollapseCutscene(Placeable.Faction f)
        {
            if (f == Placeable.Faction.Player) {
                redCastleCollapse.Play();
            }
            else {
                blueCastleCollapse.Play();
            }
        }
    }
}