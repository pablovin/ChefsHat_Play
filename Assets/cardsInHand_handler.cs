using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class cardsInHand_handler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        Color tmp = transform.GetComponent<UnityEngine.UI.Image>().color;
        tmp.a = 1f;
        transform.GetComponent<UnityEngine.UI.Image>().color = tmp;
    }

    public void OnPointerExit(PointerEventData eventData)
    {

        if (!transform.GetComponent<element_selected>().CardSelected)
        { 
            Color tmp = transform.GetComponent<UnityEngine.UI.Image>().color;
            tmp.a = 0.5f;
            transform.GetComponent<UnityEngine.UI.Image>().color = tmp;
        }
    }

    public void OnPointerClick(PointerEventData ped)
    {

        Color tmp = transform.GetComponent<UnityEngine.UI.Image>().color;

        if (transform.GetComponent<element_selected>().CardSelected)
        {
            tmp.a = 0.5f;

            transform.GetComponent<element_selected>().CardSelected = false;

        }
        else {
            tmp.a = 1f;

            transform.GetComponent<element_selected>().CardSelected = true;
                
                }

        transform.GetComponent<UnityEngine.UI.Image>().color = tmp;
        
        //whatever happens on click
    }


}
