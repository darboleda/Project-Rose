using UnityEngine;
using System.Collections;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class TriggerCameraTargetChange : Behavior
    {
        public CameraController Controller;

        public bool UseDefaultRatio = true;
        public float InterpolationRatio = 0.03f;

        public void OnCanalTriggerEnter(Trigger trigger)
        {
            CameraControllerTarget target = Camera.main.GetComponent<CameraControllerTarget>();
            if (target != null)
            {
                if (UseDefaultRatio)
                {
                    target.StartTransition(Controller);
                }
                else
                {
                    target.StartTransition(Controller, InterpolationRatio);
                }
            }
        }
    }
}
