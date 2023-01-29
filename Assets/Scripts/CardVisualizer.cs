using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardVisualizer : MonoBehaviour, IDragHandler
{
    [SerializeField] private TextMeshProUGUI _cost;
    [SerializeField] private Image _effectIcon;

    private Card _card;

    public void Visualize(Card card)
    {
        _card = card;
        
        _cost.text = card.Cost.ToString();
        _effectIcon.sprite = card.Sprite;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position += (Vector3) eventData.delta;
    }
}
