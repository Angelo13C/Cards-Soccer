using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "Hand", menuName = "ScriptableObjects/Hand")]
public class Hand : ScriptableObject
{
    [SerializeField] private CardsCollection _deck;

    [SerializeField] [Range(1, 80)] private int _cardsCount = 4;
    private Card[] _cards = null;
    
    public int MaxCardsCount => _cardsCount;
    public Span<Card> CardsInHand()
    {
        if(_cards == null)
            return null;
            
        return _cards.AsSpan().Slice(0, Mathf.Min(_cardsCount, _cards.Length));
    }

    [ContextMenu("Shuffle")]
    public void Shuffle()
    {
        if(_deck != null)
            _cards = _deck.Cards.OrderBy(_ => UnityEngine.Random.value).ToArray();
    }

    public void Use(int cardIndex)
    {
        if(cardIndex >= 0 && cardIndex < _cards.Length)
        {
            var usedCard = _cards[cardIndex];
            usedCard.Use();
            
            // Send the used card to the back of the hand
            for(var i = cardIndex + 1; i < _cards.Length; i++)
                _cards[i - 1] = _cards[i]; 

            _cards[_cards.Length - 1] = usedCard;
        }
    }
}