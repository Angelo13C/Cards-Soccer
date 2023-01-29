using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "Cards", menuName = "ScriptableObjects/Cards Collection")]
public class CardsCollection : ScriptableObject
{
    [SerializeField]
    private Card[] _cards;

    public Card[] Cards => _cards;

    public int MaxCardsCount => _cards.Length;
    public int CardsCount => _cards.Count(card => card != null);

    public void SetCard(Card card, int position)
    {
        _cards[position] = card;
    }

    public void RemoveCard(int position) => SetCard(null, position);
}