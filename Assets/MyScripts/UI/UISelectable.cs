using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UISelectable : MonoBehaviour, ISelectHandler,  IDeselectHandler, IPointerClickHandler, ISubmitHandler
{


    [SerializeField] private bool isForPanelsSwitching;
    [SerializeField] private GameObject panel;
    [SerializeField] private List<GameObject> disablePanel;
    public MoveDirection[] allowedDirections = new MoveDirection[] { MoveDirection.Right };
    public UnityEvent onSelect, onDeselect;
    
    public void OnDeselect(BaseEventData eventData)
    {
        if (Mouse.current.IsActuated()) return;
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
        if (Mouse.current.IsActuated()) return;
        onSelect.Invoke();
        if (isForPanelsSwitching)
        {
            panel.SetActive(true);
        }
    }


    public void OnPointerClick(PointerEventData eventData) => 
        ActivatePanel();

    public void OnSubmit(BaseEventData eventData) => 
        ActivatePanel();

    private void ActivatePanel()
    {
        panel.SetActive(!panel.activeSelf);

        foreach (var panel in disablePanel)
            panel.SetActive(false);
    }
}
