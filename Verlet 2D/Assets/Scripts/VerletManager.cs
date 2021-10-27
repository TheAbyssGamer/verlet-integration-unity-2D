using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerletManager : MonoBehaviour
{
    public List<GameObject> points;
    public List<GameObject> sticks;
    RaycastHit hit;
    public GameObject stickPrefab;
    public int stickIteration = 100;
    //public GameObject[] points;

    void Start()
    {
        AddPoints();
        AddSticks();
        foreach(GameObject point in points){
            Point pointScript = point.GetComponent<Point>();
            Rigidbody2D rb = point.GetComponent<Rigidbody2D>();
            
            if(pointScript.isLocked)
            {
                rb.bodyType = RigidbodyType2D.Static;

            }
            else
            {
                rb.bodyType = RigidbodyType2D.Dynamic;
            }
        }
    }

    void Update()
    {
        UpdateSticks();

        //Locking/Unlocking points
        if (Input.GetButtonDown("Fire2")) {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
            
            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);
            if (hit.collider != null) {
                Debug.Log(hit.collider.gameObject.name);
                Point pointScript = hit.collider.gameObject.GetComponent<Point>();
                Rigidbody2D rb = hit.collider.gameObject.GetComponent<Rigidbody2D>();

                if(pointScript.isLocked)
                {
                    pointScript.isLocked = false;
                    rb.bodyType = RigidbodyType2D.Dynamic;

                }
                else
                {
                    pointScript.isLocked = true;
                    rb.bodyType = RigidbodyType2D.Static;
                }
            }
        }

    }

    void AddSticks()
    {   
        for(int i=0;i<points.Count-1;i++){
            GameObject objectStick = Instantiate(stickPrefab,new Vector2(0f,0f),Quaternion.identity);
            sticks.Add(objectStick);
            Stick stick = objectStick.GetComponent<Stick>();
            stick.lenght = 2f;
            //Stick stick = sticks[stickCount].gameObject.GetComponent<Stick>();
            Point currentPoint = points[i].gameObject.GetComponent<Point>();
            Point nextPoint = points[i+1].gameObject.GetComponent<Point>();
            stick.pointA = currentPoint;
            stick.pointB =  nextPoint;
        }
    }

    void AddPoints(){
        foreach(GameObject pointsObject in GameObject.FindObjectsOfType(typeof(GameObject))){
            if(pointsObject.GetComponent<Point>()){
                points.Add(pointsObject);
            }
        }
    }

    void UpdateSticks(){
        for(int i=0;i<=stickIteration;i++){
            foreach(GameObject stick in sticks){
                Stick currentStick = stick.gameObject.GetComponent<Stick>();
                Debug.DrawLine(currentStick.pointA.transform.position,currentStick.pointB.transform.position,Color.cyan);

                Vector2 stickCenter = (currentStick.pointA.transform.position+currentStick.pointB.transform.position)/2;
                Vector2 stickDir = (currentStick.pointA.transform.position - currentStick.pointB.transform.position).normalized;

                if(!currentStick.pointA.isLocked){
                    currentStick.pointA.transform.position = stickCenter + stickDir * currentStick.lenght/2;
                }
                if(!currentStick.pointB.isLocked){
                    currentStick.pointB.transform.position = stickCenter - stickDir * currentStick.lenght/2;
                }
            }
        }
    }
}
