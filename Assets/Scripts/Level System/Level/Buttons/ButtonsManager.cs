using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ButtonsManager : MonoBehaviour
{
    private List<ButtonController> buttonControllers = new();

    void Start()
    {
        buttonControllers = FindObjectsByType<ButtonController>(FindObjectsInactive.Include, FindObjectsSortMode.None).ToList();
    }

    public void DisableButtons()
    {
        foreach (ButtonController controller in buttonControllers)
        {
            if (controller.gameObject.activeInHierarchy)
                controller.Disable();
        }
    }

    public void EnableButtons()
    {
        foreach (ButtonController controller in buttonControllers)
        {
            if (controller.gameObject.activeInHierarchy)
                controller.Enable();
        }
    }
}