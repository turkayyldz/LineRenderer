using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Socket : MonoBehaviour
{
   
    [SerializeField] private GameObject collider;
    [SerializeField] private Transform DOTweenTrasnform;
    [SerializeField] private Animator _anim;
    [SerializeField] private GameObject game;
    [SerializeField] private GameObject point1;
    [SerializeField] private GameObject point2;
   


    public bool Yerlesti;
    public controllers control;
    public int counter;


   
    void Update()
    {
        if (Yerlesti)
        {
            if (Vector2.Distance(transform.position, control.LastPosition()) > .1f)
                transform.position = Vector2.Lerp(transform.position, control.LastPosition(),.2f);
            else
                transform.position = Vector2.Lerp(transform.position, control.NextPosition(), .2f);
        }
    }


    /// <summary>
    /// Oyun sahnesindeki colliderlerin tetiklenmesini ve tetiklendikten sonra parmak gameObjenin DOTween methodlari sayesinde schale ve domove calismasini saglar.
    /// tutorialin bitmesi ve Pointlerin ekranda gozukmesini saglar.
    /// </summary>
    /// <param name="collision"></param>
    
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("aaaasss");

        counter++;
        if (counter == 2)
        {
            _anim.enabled = false;
            game.transform.DOScale(new Vector3(0.2f, 0.2f, 1), 1f).OnComplete(() =>
            {

                game.transform.DOMove(DOTweenTrasnform.position, 1f).OnComplete(() =>
                {

                    point1.SetActive(true);
                    point2.SetActive(true);
                    game.SetActive(false);
                    collider.SetActive(false);
                    control._lineRenderer.positionCount = 0;
                    control._lineRenderer.enabled = false;
                   

                });
            });
        }
    }

}
