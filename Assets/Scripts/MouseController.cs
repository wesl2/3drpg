using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
//[System.Serializable]
//public class EventVector3 : UnityEvent<Vector3>{ }
 

   public class MouseController  : MonoBehaviour
{
    static public MouseController instance;


    public event Action<Vector3> OnMouseClicked;

    public event Action<GameObject> OnEnemyClicked; 

    public Texture2D point, attack, arrow, target, doorway;
    
   public RaycastHit Rhit;
   
    Ray ray;

    void Awake()
    {
        
        if (instance == this)
        {
            Debug.Log("destroy something...");
            Debug.Log(this);
            Destroy(gameObject);
            return;
        }
        else
        {
            Debug.Log(" something...");
            Debug.Log(this);
            instance = this;
        }

    }
    private void Update()
    {
        SetCursorTexture();
       // Physics.Raycast(ray, out Rhit);
        MouseControll();
    }

    void SetCursorTexture()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out Rhit))
        {
            //切换鼠标贴图
            switch (Rhit.collider.gameObject.tag)
            {
                case "Ground":
                    Cursor.SetCursor(target, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Enemy":
                    Cursor.SetCursor(attack, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Portal":
                    Cursor.SetCursor(doorway, new Vector2(16, 16), CursorMode.Auto);
                    break;
                case "Item":
                    Cursor.SetCursor(point, new Vector2(16, 16), CursorMode.Auto);
                    break;

                default:
                    Cursor.SetCursor(arrow, new Vector2(16, 16), CursorMode.Auto);
                    break;
            }
        }
}
    void MouseControll()
    {
        if(Input.GetMouseButtonDown(0) && Rhit.collider != null)
        {
            if(Rhit.collider.gameObject.tag == "Ground")//collider是碰撞物的基类
            {
                OnMouseClicked?.Invoke(Rhit.point);
                //the same as:
                //if(OnMouseClicked != null)
                //{ OnMouseClicked.Invoke(Rhit.point); }
            }
            if(Rhit.collider.gameObject.tag == "Enemy")
            {
                OnEnemyClicked?.Invoke(Rhit.collider.gameObject);
            }
        }
    }
}
