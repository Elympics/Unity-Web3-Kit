using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerEventEmitter : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler, IPointerClickHandler, IPointerEnterHandler
{
	public event Action PointerDown;
	public event Action PointerUp;
	public event Action PointerExit;
	public event Action PointerClicked;
	public event Action PointerEnter;

	public void OnPointerDown(PointerEventData eventData) => PointerDown?.Invoke();
	public void OnPointerUp(PointerEventData eventData) => PointerUp?.Invoke();
	public void OnPointerExit(PointerEventData eventData) => PointerExit?.Invoke();
	public void OnPointerClick(PointerEventData eventData) => PointerClicked?.Invoke();
	public void OnPointerEnter(PointerEventData eventData) => PointerEnter?.Invoke();
}