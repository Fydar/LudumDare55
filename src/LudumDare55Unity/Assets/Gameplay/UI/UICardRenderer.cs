using UnityEngine;
using UnityEngine.UI;

public class UICardRenderer : MonoBehaviour
{
    public Text CardNameText;
    public RawImage GraphicRawImage;

    public void Render(CardTemplate cardTemplate)
    {
        CardNameText.text = cardTemplate.cardName;

        var worldCard = WorldCardPool.Instance.RenderCard(cardTemplate);
        GraphicRawImage.texture = worldCard.Rendered;
    }
}
