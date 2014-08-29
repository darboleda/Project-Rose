using UnityEngine;
using System.Collections;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class TriggerActivate : Behavior
    {
        public GameObject target;
        public bool invert = false;
        public bool setOnEnter = true;
        public bool setOnExit = true;

        public void Awake()
        {
            target.SetActive(invert);
        }
        
        public void OnCanalTriggerEnter(Trigger trigger)
        {
            if(setOnEnter)
            {
                target.SetActive(!invert);
            }
        }

        public void OnCanalTriggerExit(Trigger trigger)
        {
            if(setOnExit)
            {
                target.SetActive(invert);
            }
        }
    }
}
