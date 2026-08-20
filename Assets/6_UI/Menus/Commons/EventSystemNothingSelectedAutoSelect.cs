using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemNothingSelectedAutoSelect : MonoBehaviour
{
    [Tooltip("If nothing is selected, EventSystem firstSelectedObject will be selected.")]
    [SerializeField] private GameObject autoSelectObject;

    private void Update()
    {
        if(EventSystem.current.currentSelectedGameObject == null)
        {
            if(autoSelectObject == null)
            {
                EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(autoSelectObject);
            }
        }
    }
}
