using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class RightClick : MonoBehaviour, IPointerClickHandler
{
    //events
    public UnityEvent leftClick;
    public UnityEvent middleClick;
    public UnityEvent rightClick;

    //when the mouse clicks
    public void OnPointerClick(PointerEventData eventData)
    {
        //if the mouse click is a left click
        if (eventData.button == PointerEventData.InputButton.Left)
        { /*trigger event */ leftClick.Invoke(); }
        //if middle
        else if (eventData.button == PointerEventData.InputButton.Middle)
        { middleClick.Invoke(); }
        //if right
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            rightClick.Invoke();

        }
    }
}