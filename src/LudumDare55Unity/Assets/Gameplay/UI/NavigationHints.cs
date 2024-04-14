using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class NavigationHints : MonoBehaviour
{
	[SerializeField] private bool disableWhenOutOfScope;

	[Space]
	[SerializeField] private Animator upArrow;
	[SerializeField] private Animator downArrow;
	[SerializeField] private Animator leftArrow;
	[SerializeField] private Animator rightArrow;

	private GameObject lastSelectedGameObject;

	void Update()
	{
		var selectedGameObject = EventSystem.current.currentSelectedGameObject;

		if (lastSelectedGameObject != selectedGameObject)
		{
			lastSelectedGameObject = selectedGameObject;

			bool allowEnabled = !disableWhenOutOfScope || IsContainedInScope(lastSelectedGameObject);

			var selectable = lastSelectedGameObject.GetComponent<Selectable>();

			if (upArrow != null)
			{
				var upSelectable = selectable.FindSelectableOnUp();
				upArrow.SetBool("ShowArrow", allowEnabled && upSelectable != null);
			}

			if (downArrow != null)
			{
				var downSelectable = selectable.FindSelectableOnDown();
				downArrow.SetBool("ShowArrow", allowEnabled && downSelectable != null);
			}

			if (leftArrow != null)
			{
				var leftSelectable = selectable.FindSelectableOnLeft();
				leftArrow.SetBool("ShowArrow", allowEnabled && leftSelectable != null);
			}

			if (rightArrow != null)
			{
				var rightSelectable = selectable.FindSelectableOnRight();
				rightArrow.SetBool("ShowArrow", allowEnabled && rightSelectable != null);
			}
		}
	}

	private bool IsContainedInScope(GameObject childObject)
	{
		var current = childObject.transform;
		while (current != null)
		{
			if (current.gameObject == gameObject)
			{
				return true;
			}

			current = current.parent;
		}
		return false;
	}
}
