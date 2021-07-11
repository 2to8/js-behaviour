using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Playables;

namespace UnityRoyale
{
    public class CinematicsManager : MonoBehaviour
    {
        public PlayableDirector redCastleCollapse, blueCastleCollapse;
        void OnEnable() { }

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