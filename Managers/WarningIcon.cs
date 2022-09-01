using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningIcon : MonoBehaviour
{
    public Transform target;

    RectTransform pointerRectTransform;

    Image icon;

    private void Awake()
    {
        pointerRectTransform = GetComponent<RectTransform>();
        icon = GetComponent<Image>();
        icon.enabled = false;
    }

    void Update()
    {
        if(target != null)
        {
            icon.enabled = true;

            Vector3 targetScreenPosition = Camera.main.WorldToScreenPoint(target.position);
            bool offScreen = targetScreenPosition.x <= -20 || targetScreenPosition.x >= Screen.width + 20 || targetScreenPosition.y <= -20 || targetScreenPosition.y >= Screen.height + 20;

            if (offScreen)
            {
                Vector3 cappedTargetScreenPosition = new Vector3(Mathf.Clamp(targetScreenPosition.x, 30, Screen.width - 30), Mathf.Clamp(targetScreenPosition.y, 30, Screen.height - 30));

                pointerRectTransform.position = cappedTargetScreenPosition;
            }
            else
            {
                GameplayManager.instance.AddScore(5);
                UIManager.instance.AddScore(5, target.position);

                Destroy(gameObject);
            }
        }
        else
        {
            icon.enabled = false;
        }
    }
}
