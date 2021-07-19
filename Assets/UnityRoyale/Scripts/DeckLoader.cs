using System;
using System.Collections;
using System.Collections.Generic;
using GameEngine.Extensions;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.AddressableAssets;
using UnityEngine.AddressableAssets.ResourceLocators;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace UnityRoyale
{
    public class DeckLoader : MonoBehaviour
    {
        public int count = 0;
        private DeckData targetDeck;
        public UnityAction<DeckLoader> OnDeckLoaded;

        public void LoadDeck(DeckData deckToLoad)
        {
            targetDeck = deckToLoad;
            Addressables.LoadAssetsAsync<CardData>(targetDeck.labelsToInclude[0].labelString, null).Completed += obj => {
                targetDeck.CardsRetrieved((List<CardData>)obj.Result);
                OnDeckLoaded?.Invoke(this);
                this.DestroySelf();

                //Destroy(this);                
            };
        }

        //...

		// private void OnResourcesRetrieved(IAsyncOperation<IList<CardData>> obj)
		// {

		// }
	}
}