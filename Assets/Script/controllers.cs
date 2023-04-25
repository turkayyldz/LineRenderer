using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class controllers : MonoBehaviour
{
     
    [SerializeField] private List<Vector2> _fingerList;
    [SerializeField] private string tag;
   
    private int total;
    private bool selected;
    private RaycastHit2D hit;
    private int fingerPositionIndex;
    public Socket _socket;
    public LineRenderer _lineRenderer;

    void Update()
    {

        

        // iki boyutta raycast atýmý.
        hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -10)), Vector2.zero);

        // yukarýda tanýmladýgýmýz raycast isini  collider'a denk delmedigi sürece if bilogunun icine giremez.
        if (hit.collider!=null)
        {
            if (hit.collider.gameObject.CompareTag(tag)&&!selected && Input.GetMouseButtonDown(0))
            {
                CreateALine();
                selected = true;
            }
        } 
        if (Input.GetMouseButton(0) && selected)
        {
            Vector2 FingerPosition = GetMousePos();
            if (Vector2.Distance(FingerPosition, _fingerList[^1]) > .1f)
            {
                UpdateLine(FingerPosition);
            }
        }
        if (Input.GetMouseButtonUp(0) && selected)
        {
            ToStart();
            if (_socket.counter <= 3)
            {
                _lineRenderer.positionCount = 0;
                //counter = 1;
                
            }
          
        }
    }


    /// <summary>
    /// Line Renderer den Cizgi olusturmamizi saglayacak methoddur.
    /// SetPosition ile mouse pozisyonunu line renderere gönderme iþlemini yapar ve mouse pozisyonunu liseteye ekler 
    /// </summary> 
    void CreateALine()
    {
       
        _fingerList.Add(GetMousePos());
        _fingerList.Add(GetMousePos());

        _lineRenderer.SetPosition(0, _fingerList[0]);
        _lineRenderer.SetPosition(1, _fingerList[1]);
       


    }
   /// <summary>
   /// 
   /// Gelen parmak pozisyonunu vector2 olarak geriye döndürür.
   /// Line Renderer'in lisetesine parmak pozisyonunu ekler.
   /// Guncel parmak pozisyonunu bize verir. 
   /// 
   /// </summary>
   /// <param name="IncomingFingerPosition"></param>
    void UpdateLine(Vector2 IncomingFingerPosition)
    {
        _fingerList.Add(IncomingFingerPosition);
        _lineRenderer.positionCount++;
        _lineRenderer.SetPosition(_lineRenderer.positionCount - 1, IncomingFingerPosition);
       
    }
    
    /// <summary>
    /// 
    /// Bu method cameradan gelen isinin mouse posizyonunu verir.
    /// 
    /// </summary>
    /// <returns></returns>
    Vector2 GetMousePos()
    {
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

     public void ToStart()
    {
        _socket.Yerlesti = true;
    }
    
    public Vector2 LastPosition()
    {
        return _fingerList[fingerPositionIndex];
    }
    public Vector2 NextPosition()
    {
        if (fingerPositionIndex==_fingerList.Count-1)
        {
            _socket.Yerlesti = false;
            return _fingerList[fingerPositionIndex];
        }
        else
        {
            fingerPositionIndex++;
            return _fingerList[fingerPositionIndex];
        }
       
    }
}
