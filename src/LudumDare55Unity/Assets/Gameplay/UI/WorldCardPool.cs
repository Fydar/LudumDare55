using UnityEngine;

public class WorldCardPool : MonoBehaviour
{
    public static WorldCardPool Instance;

    public WorldCardZone zoneTemplate;

    private int counter = 0;

    private void Awake()
    {
        Instance = this;
    }

    public WorldCardZone RenderCard(CardTemplate cardTemplate)
    {
        var clone = Instantiate(
            zoneTemplate,
            new Vector3(counter * 2.5f, 0.0f, 0.0f),
            Quaternion.identity,
            transform);
        counter++;

        clone.Render(cardTemplate);

        return clone;
    }
}
