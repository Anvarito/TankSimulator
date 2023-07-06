using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class UISelectable : MonoBehaviour, ISelectHandler,  IDeselectHandler
{


    [SerializeField] private bool isForPanelsSwitching;
    [SerializeField] private GameObject panel;
    public MoveDirection[] allowedDirections = new MoveDirection[] { MoveDirection.Right };
    public UnityEvent onSelect, onDeselect;
   
    public void OnDeselect(BaseEventData eventData)
    {
        onDeselect.Invoke();

        if (isForPanelsSwitching) 
        {
            AxisEventData axisEventData = eventData as AxisEventData;

            if (!allowedDirections.Contains(axisEventData.moveDir))
            {
                panel.SetActive(false);
            }
        }
           
          
      
    }

    public void OnSelect(BaseEventData eventData)
    {
        onSelect.Invoke();
        if (isForPanelsSwitching)
        {
            panel.SetActive(true);
        }
    }

 
}
