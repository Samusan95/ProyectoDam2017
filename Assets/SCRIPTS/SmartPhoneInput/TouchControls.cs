using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
public class TouchControls : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{

   

    private Image bgImg;
    private Image joystickImg;
    private Vector3 inputVector;
    private float swipeX;

    public float minSwipeDistY;
    public float minSwipeDistX;
    private Vector2 startPos;


    private void Start()
    {
        bgImg = GetComponent<Image>();
        joystickImg = transform.GetChild(0).GetComponent<Image>();
        
    }

    

    void Update()
    {
        swipeX = 0;
        //#if UNITY_ANDROID
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;

                case TouchPhase.Ended:
                    /*float swipeDistVertical = (new Vector3(0, touch.position.y, 0) - new Vector3(0, startPos.y, 0)).magnitude;
                    if (swipeDistVertical > minSwipeDistY)
                    {
                         float swipeValue = Mathf.Sign(touch.position.y - startPos.y);

                         if (swipeValue > 0)
                             Debug.Log("UpSwipe");
                         else if (swipeValue < 0)
                             Debug.Log("DownSwipe");
                     }*/
                    if (touch.position.x > Screen.width / 2)
                    {
                        float swipeDistHorizontal = (new Vector3(touch.position.x, 0, 0) - new Vector3(startPos.x, 0, 0)).magnitude;
                        if (swipeDistHorizontal > minSwipeDistX)
                        {
                            float swipeValue = Mathf.Sign(touch.position.x - startPos.x);
                            if (swipeValue > 0)//right swipe
                                swipeX = 1;
                            else if (swipeValue < 0)//left swipe
                                swipeX = -1;
                        }
                    }
                    break;
            }
        }
    }




    public virtual void OnDrag(PointerEventData ped)
    {
        Vector2 pos;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(bgImg.rectTransform,
                                                                    ped.position,
                                                                    ped.pressEventCamera,
                                                                    out pos))
        {
            pos.x = (pos.x / bgImg.rectTransform.sizeDelta.x);
            pos.y = (pos.y / bgImg.rectTransform.sizeDelta.y);

            // inputVector = new Vector3(pos.x * 2 + 1, 0, pos.y * 2 - 1);
            inputVector = new Vector3(pos.x, 0, pos.y);
            inputVector = (inputVector.magnitude > 1.0f) ? inputVector.normalized : inputVector;



            joystickImg.rectTransform.anchoredPosition = new Vector3(inputVector.x * (bgImg.rectTransform.sizeDelta.x / 3),
                                                                     inputVector.z * (bgImg.rectTransform.sizeDelta.y / 3));
        }
    }
    public virtual void OnPointerDown(PointerEventData ped)
    {
        OnDrag(ped);
    }
    public virtual void OnPointerUp(PointerEventData ped)
    {
        inputVector = Vector3.zero;
        joystickImg.rectTransform.anchoredPosition = Vector3.zero;
    }



    public float SwipeX()
    {
        if (swipeX != 0)
            return swipeX;
        else
            return 0;
    }

    public float Horizontal()
    {
        if (inputVector.x != 0)
            return inputVector.x;
        else
            return Input.GetAxis("Horizontal");
    }
    public float Vertical()
    {
        if (inputVector.z != 0)
            return inputVector.z;
        else
            return Input.GetAxis("Vertical");
    }

}