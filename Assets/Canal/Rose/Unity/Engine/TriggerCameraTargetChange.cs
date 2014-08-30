using UnityEngine;
using System.Collections;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class TriggerCameraTargetChange : Behavior
    {
        public CameraController Controller;

        public void OnCanalTriggerEnter(Trigger trigger)
        {
            Debug.Log(Controller);
            CameraControllerTarget target = Camera.main.GetComponent<CameraControllerTarget>();
            if (target != null) target.StartTransition(Controller);
        }
    }
}
