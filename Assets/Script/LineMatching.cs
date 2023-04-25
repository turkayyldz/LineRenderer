using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LineMatching : MonoBehaviour
{
    AnswerMatching answerMatching;
    LineMatching lineMatching;

    [SerializeField] private string tag;
    [SerializeField] private LineRenderer linered;
    private Vector2 mousePos;
    private Vector2 startMousePos;
    public Transform point;
    
 
    public string value;
    public List<Transform> points = new List<Transform>();


    private bool isvalue = true;
    private RaycastHit2D hit;
    private AnswerMatching[] childs;
    private float distance = .5f;
    private Transform go;
    private bool selected;





    private void Start()
    {
       
        linered.positionCount = 2;
       
       lineMatching = GetComponent<LineMatching>();

        childs = FindObjectsOfType<AnswerMatching>();
        foreach (var p in childs)
        {
            points.Add(p.point);
        }

    }


    private void Update()
    {
        LineInteractble();
    }
    /// <summary>
    /// burada line renderein cizgisinin 2 pozisyonlu ve suruklenerek gelmesini saglayan kod blogudur.
    /// </summary>
    /// <param name="enemies"></param>
    /// <returns></returns>
    public Transform GetClosestEnemy(Transform[] enemies)
    {
        Transform bestTarget = null;
        float closestDistanceSqr = Mathf.Infinity;
        Vector2 currentPosition = mousePos;
        foreach (Transform potentialTarget in enemies)
        {
            Vector2 directionToTarget = (Vector2)potentialTarget.position - currentPosition;
            float dSqrToTarget = directionToTarget.sqrMagnitude;
            if (dSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = dSqrToTarget;
                bestTarget = potentialTarget;
            }

        }

        return bestTarget;
    }

    /// <summary>
    /// pointler üzerinder cizgimizi olusturacak methoddur.
    /// </summary>
    public void StartButton()
    {
        Debug.Log("tüh");
        selected = true;
        lineMatching.enabled = true;
        linered.enabled = true;
        isvalue = false;
    }
    /// <summary>
    /// LineMatchAnswer den gelen verilerin eslesmesini yapar. Point3  ve textin ekranda gozukmesini saglayacak kod blogudur.
    /// </summary>
    /// <param name="a"></param>
    public void EndButton(string a)
    {
        if (value == a)
        {
            
            if (this.gameObject.name=="Point1")
            {
                GameManager.instance.isPoint1 = true;
            }

            if (this.gameObject.name == "Point2")
            {
                GameManager.instance.isPoint2 = true;
            }
            if (this.gameObject.name == "Point3")
            {
                GameManager.instance.isPoint3 = true;
            }
           
            GameManager.instance.points[0].SetActive(true);
           
            if (GameManager.instance.isPoint1 && GameManager.instance.isPoint2 && GameManager.instance.isPoint3)
            {
                GameManager.instance.ucgen.SetActive(true);
            }
        }
        else
        {
            Debug.Log(false, gameObject);

        }



    }
    /// <summary>
    /// buradaki method bizim cizgilerimizi mous yardimi ile cizmemizi saglar. 
    /// </summary>
    public void LineInteractble()
    {
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10)), Vector2.zero);
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag(tag) && !selected && Input.GetMouseButtonDown(0))
            {
                StartButton();
            }
        }
        if (Input.GetMouseButtonDown(0) && selected)
        {
            startMousePos = GetMousePos();
        }
        if (Input.GetMouseButton(0) && selected)
        {

            mousePos = GetMousePos();
            linered.SetPosition(0, new Vector2(point.position.x, point.position.y));

            isvalue = true;
            go = GetClosestEnemy(points.ToArray());

            /*
             * alttaki if blogu mause'nin posizyonunun mesafesini olcerek cizgi olusturmasini saglar.  
             * 
             * */
            
            if (Vector2.Distance(mousePos, go.position) < distance)
            {
                linered.SetPosition(1, (Vector2)go.position);

            }
            else
            {
                linered.SetPosition(1, new Vector2(mousePos.x, mousePos.y));
                go = null;
            }

        }
        if (Input.GetMouseButtonUp(0) && selected)
        {
            
            if (go)
            {
                EndButton(go.GetComponent<LineMatchAnswer>().answer);
                //lineMatching.enabled = false;
            }
            else
            {
                if (isvalue)
                {
                    linered.enabled = false;
                }

            }
            selected = false;
        }
    }
    Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

}
