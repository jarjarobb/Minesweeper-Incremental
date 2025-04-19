using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class RightClick : MonoBehaviour, IPointerClickHandler
{
    //This script deals with the right click for the button

    //event
    public UnityEvent rightClick;

    //when the mouse clicks
    public void OnPointerClick(PointerEventData eventData)
    {
        //if right
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            rightClick.Invoke();

        }
    }
}