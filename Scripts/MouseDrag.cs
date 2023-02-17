using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseDrag : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    GameObject manager;
    CameraControls cc;
    [SerializeField] GameObject statsPanel;
    static GameObject panel;
    static GameObject canvas;

    void Start(){
        manager = GameObject.FindGameObjectWithTag("BoatManager");
        Camera cam = Camera.main;
        cc = cam.GetComponent<CameraControls>();
        canvas = GameObject.Find("Canvas");
    }

    void OnMouseDown()
    {
        
        if(!manager.GetComponent<BoatManager>().pause){
            if(panel != null){
                Destroy(panel); 
            }
            cc.focus = gameObject;
            panel = Instantiate(statsPanel, Vector3.zero, canvas.transform.rotation, canvas.transform);
            panel.GetComponent<BoatStatsPanel>().boat = gameObject;
            panel.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            return;
        }

        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;

        // Store offset = gameobject world pos - mouse world pos
        mOffset = gameObject.transform.position - GetMouseAsWorldPoint();

    }

    private Vector3 GetMouseAsWorldPoint()
    {
        // Pixel coordinates of mouse (x,y)
        Vector3 mousePoint = Input.mousePosition;

        // z coordinate of game object on screen
        mousePoint.z = mZCoord;

        // Convert it to world points
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        if(!manager.GetComponent<BoatManager>().pause){
            return;
        }
        transform.position = GetMouseAsWorldPoint() + mOffset;
        transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
}
