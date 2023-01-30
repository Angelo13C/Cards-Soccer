using System;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card")]
public class Card : ScriptableObject
{
    [SerializeField] [Range(1, 10)]
    private int _cost = 1;
    public int Cost => _cost;
    
    [SerializeField]
    private Sprite _sprite;
    public Sprite Sprite => _sprite;

    [SerializeReference]
    public Effect Effect;

    public void Use()
    {
        Effect.Use();
    }
    
#if UNITY_EDITOR
    private void OnEnable() {
        var cardDirectoryPath = Path.GetDirectoryName(AssetDatabase.GetAssetPath(this));

        var cards = AssetDatabase.FindAssets("t: Card", new [] { cardDirectoryPath })
            .Select(cardUID => AssetDatabase.LoadAssetAtPath<Card>(AssetDatabase.GUIDToAssetPath(cardUID)))
            .ToArray();
            
        var effectTypes = System.Reflection.Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => type.IsSubclassOf(typeof(Effect))).ToArray();
            
        // If for some reasons a card lost its Effect reference, reassign it to the default one
        foreach(var card in cards)
        {
            if(card.Effect == null)
            {
                var cardEffectType = effectTypes.First(type => type.Name.Replace("Effect", "") == card.name);
                if(cardEffectType != null)
                    card.Effect = (Effect) System.Activator.CreateInstance(cardEffectType);
            }
        }

        foreach(var effectType in effectTypes)
        {
            // If there isn't a card with this effect
            if(!cards.Any(card => card.Effect != null && card.Effect.GetType() == effectType))
            {
                var effect = (Effect) System.Activator.CreateInstance(effectType);
                
                var newCard = ScriptableObject.CreateInstance<Card>();
                var assetName = effect.ToString().Replace("Effect", "") + ".asset";
                AssetDatabase.CreateAsset(newCard, cardDirectoryPath + "/" + assetName);
                AssetDatabase.SaveAssets();
                newCard.Effect = effect;
                EditorUtility.SetDirty(newCard);                
            }
        }
    }
#endif
}