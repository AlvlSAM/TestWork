using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField]
    private GameLogix gl;


    public RectTransform BitchParent;
    public RectTransform SeaParent;
    public RectTransform SkyParent;

    private float deltaX;
    private static float deltaY = 5f;

    private float coefX;
    private float coefY;

    private void Start()
    {
        deltaX = (1f*Screen.currentResolution.width / Screen.currentResolution.height) * 5;

        coefX = (1f * Screen.currentResolution.width / (2 * deltaX));   //  Определяем отношение координат клика по оси x к ширине экрана
        coefY = (1f * Screen.currentResolution.height / (2 * deltaY));

        Debug.Log(coefX);
    }
    public void Click(BaseEventData e)
    {
        string ParentName = null;

        float floatX = deltaX + Camera.main.ScreenToWorldPoint(Input.mousePosition).x;
        float floatY = Camera.main.ScreenToWorldPoint(Input.mousePosition).y - deltaY;
        GameObject dl = gl.CreatedObject(Camera.main.ScreenToWorldPoint(Input.mousePosition).y, out ParentName);
        if (dl != null)
        {
            RectTransform Parent = null;
            switch(ParentName)
            {
                case "Bich":
                    {
                        Parent = BitchParent;
                        break;
                    }
                case "Sea":
                    {
                        Parent = SeaParent;
                        break;
                    }
                case "Sky":
                    {
                        Parent = SkyParent;
                        break;
                    }
            }
            GameObject g = Instantiate(dl, Parent);
            g.GetComponent<RectTransform>().anchoredPosition = new Vector2(floatX * coefX, floatY * coefY);
        }
        
        Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }
}
