using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SwipeObjectController : MonoBehaviour
{
    public GameObject swipingText;
    private Vector2 fingerDown;
    private Vector2 fingerUp;
    private float velocityX = 0f;
    public bool detectSwipeOnlyAfterRelease = false;

    public float SWIPE_THRESHOLD = 20f;

    // Update is called once per frame
    void Update()
    {
        if (EventSystem.current.currentSelectedGameObject.tag=="SwipingPanel")
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    fingerUp = touch.position;
                    fingerDown = touch.position;
                    velocityX = 0;
                }

                //Detects Swipe while finger is still moving
                if (touch.phase == TouchPhase.Moved)
                {
                    if (!detectSwipeOnlyAfterRelease)
                    {
                        fingerDown = touch.position;
                        checkSwipe();
                    }
                }

                //Detects swipe after finger is released
                if (touch.phase == TouchPhase.Ended)
                {
                    fingerDown = touch.position;
                    checkSwipe();
                }
            }
        }
        
    }
    void FixedUpdate()
    {
        if (swipingText.transform.position.x <= 5f && swipingText.transform.position.x >= -5f)
        {
            swipingText.transform.position += new Vector3(velocityX, 0f, 0f);
        }
        if (swipingText.transform.position.x > 5f)
        {
            swipingText.transform.position = new Vector3(5f, 0f, 0f);
        }
        if (swipingText.transform.position.x < -5f)
        {
            swipingText.transform.position = new Vector3(-5f, 0f, 0f);
        }

        if (swipingText.transform.position.x == -5f || swipingText.transform.position.x == 5f)
        {
            velocityX = 0;
        }
        Debug.Log(velocityX.ToString());
        //Debug.Log(swipingText.transform.position.x.ToString());
        if (velocityX != 0.0f)
        {
            if (velocityX > 0.0f)
            {
                velocityX -= 0.002f;
            }
            if (velocityX < 0.0f)
            {
                velocityX += 0.002f;
            }
        }
        else
        {
            velocityX = 0f;
        }

    }
    void checkSwipe()
    {
        //Check if Vertical swipe
        if (verticalMove() > SWIPE_THRESHOLD && verticalMove() > horizontalValMove())
        {
            //Debug.Log("Vertical");
            if (fingerDown.y - fingerUp.y > 0)//up swipe
            {
                OnSwipeUp();
            }
            else if (fingerDown.y - fingerUp.y < 0)//Down swipe
            {
                OnSwipeDown();
            }
            fingerUp = fingerDown;
        }

        //Check if Horizontal swipe
        else if (horizontalValMove() > SWIPE_THRESHOLD && horizontalValMove() > verticalMove())
        {
            //Debug.Log("Horizontal");
            if (fingerDown.x - fingerUp.x > 0)//Right swipe
            {
                OnSwipeRight();
            }
            else if (fingerDown.x - fingerUp.x < 0)//Left swipe
            {
                OnSwipeLeft();
            }
            fingerUp = fingerDown;
        }

        //No Movement at-all
        else
        {
            //Debug.Log("No Swipe!");
        }
    }

    float verticalMove()
    {
        return Mathf.Abs(fingerDown.y - fingerUp.y);
    }

    float horizontalValMove()
    {
        return Mathf.Abs(fingerDown.x - fingerUp.x);
    }

    //////////////////////////////////CALLBACK FUNCTIONS/////////////////////////////
    void OnSwipeUp()
    {
        Debug.Log("Swipe UP");
    }

    void OnSwipeDown()
    {
        Debug.Log("Swipe Down");
    }

    void OnSwipeLeft()
    {
        if (swipingText.transform.position.x > -5f)
        {
            velocityX -= 0.02f;
        }
    }

    void OnSwipeRight()
    {
        if (swipingText.transform.position.x < 5f)
        {
            velocityX += 0.02f;
        }
    }
}
