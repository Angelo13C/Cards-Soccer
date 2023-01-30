using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HandVisualizer : MonoBehaviour
{
    [SerializeField] private GameObject _cardTemplatePrefab;
    
    public Hand Hand;

    private void Update() {
        var cardsInHand = Hand.CardsInHand();
        
        if(cardsInHand != null && transform.childCount < cardsInHand.Length)
        {
            for(var i = transform.childCount; i < cardsInHand.Length; i++)
            {
                var card = Instantiate(_cardTemplatePrefab, transform);
                card.GetComponent<CardVisualizer>().Visualize(cardsInHand[i]);
            }
        }
    }
}
