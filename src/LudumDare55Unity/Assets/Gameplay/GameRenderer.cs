using UnityEngine;
using UnityEngine.InputSystem;

public class GameRenderer : MonoBehaviour
{
    public CardTemplate[] StartingHand;

    public RectTransform handLayout;
    public GameObjectPool<RectTransform> handPlaceholders;

    [Header("Cards")]
    public RectTransform floatingCardHolder;
    public RectTransform deckDrawPoint;
    public FloatingCard floatingCard;

    private void Start()
    {
        handPlaceholders.Flush();

        for (int i = 0; i < StartingHand.Length; i++)
        {
            var card = StartingHand[i];

            var floatingCardClone = Instantiate(floatingCard, deckDrawPoint.position, deckDrawPoint.rotation, floatingCardHolder);

            var newSlot = handPlaceholders.Grab(handLayout.transform);
            floatingCardClone.FollowTarget = newSlot;

            floatingCardClone.UICardRenderer.Render(card);
        }
    }

    private void Update()
    {
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            var randomCard = StartingHand[Random.Range(0, StartingHand.Length)];

            var floatingCardClone = Instantiate(floatingCard, deckDrawPoint.position, deckDrawPoint.rotation, floatingCardHolder);
            var newSlot = handPlaceholders.Grab(handLayout.transform);
            floatingCardClone.FollowTarget = newSlot;
            floatingCardClone.UICardRenderer.Render(randomCard);
        }
    }
}
