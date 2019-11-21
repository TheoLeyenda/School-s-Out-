﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckSelectedEventSystem : MonoBehaviour
{
    // Update is called once per frame
    public GameObject newCurrentGameObject;
   
    
    void Update()
    {
        if (EventSystem.current != null)
        {
            if (!EventSystem.current.currentSelectedGameObject)
            {
                EventSystem.current.SetSelectedGameObject(EventSystem.current.firstSelectedGameObject);
            }
        }
    }
}
