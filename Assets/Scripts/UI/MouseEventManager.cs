using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class MouseEventManager : Singletons.Singleton<MouseEventManager>
    { 
        public delegate void OnCardClickDelegate();
        public event OnCardClickDelegate OnCardClickEventHandler;

        private void Update()
        {
            if(Input.GetMouseButtonDown(0))
            {
                RaycastHit hit;
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if(Physics.Raycast(ray, out hit, 100.0f))
                {
                    if(hit.transform != null)
                    {
                        // TODO
                    }
                }
            }
        }
    }

}
