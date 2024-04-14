using UnityEngine;

[CreateAssetMenu(menuName = "CardTemplate")]
public class CardTemplate : ScriptableObject
{
    public string cardName = "Card Name";

    [TextArea(2, 3)]
    public string cardFlavourText = "A card.";

    public GameObject graphics;

    [Min(0)]
    public int energyCost = 1;
}
