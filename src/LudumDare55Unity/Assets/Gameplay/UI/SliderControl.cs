using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderControl : MonoBehaviour, ISelectHandler, IDeselectHandler
{
	[SerializeField] private int steps = 10;
	[SerializeField] private int current = 5;

	[Space]
	[SerializeField] private Image sliderGraphic;
	[SerializeField] private Animator leftArrow;
	[SerializeField] private Animator rightArrow;

	private void Start()
	{
		bool isActiveControl = EventSystem.current.currentSelectedGameObject == gameObject;
		SetValue(current, isActiveControl);
	}

	public void SetValue(int value, bool selected)
	{
		current = Mathf.Clamp(value, 0, steps - 1);

		leftArrow.SetBool("ShowArrow", selected && current > 0);
		rightArrow.SetBool("ShowArrow", selected && current < (steps - 1));

		sliderGraphic.fillAmount = ((float)current) / (steps - 1);
	}

	public void UiIncrease()
	{
		bool isActiveControl = EventSystem.current.currentSelectedGameObject == gameObject;

		SetValue(current + 1, isActiveControl);
	}

	public void UiDecrease()
	{
		bool isActiveControl = EventSystem.current.currentSelectedGameObject == gameObject;

		SetValue(current - 1, isActiveControl);
	}

	public void OnSelect(BaseEventData eventData)
	{
		SetValue(current, true);
	}

	public void OnDeselect(BaseEventData eventData)
	{
		SetValue(current, false);
	}
}
