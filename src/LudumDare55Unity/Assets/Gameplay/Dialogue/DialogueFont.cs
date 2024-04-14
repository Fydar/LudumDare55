using UnityEngine;

[CreateAssetMenu]
public class DialogueFont : ScriptableObject
{
    [Header("Font")]
    public Font Font;
    public int FontSize = 12;
    public FontStyle FontStyle = FontStyle.Normal;

    [Header("Timing")]
    public float CharactersPerMinute = 360.0f;
    [Space]
    public float DelayBetweenWords = 0.0f;
    public float DelayBetweenSentences = 0.0f;

    [Header("Animation")]
    public RuntimeAnimatorController Controller;

    [Header("Positioning")]
    public TextAnchor TextAnchor = TextAnchor.UpperLeft;
}
