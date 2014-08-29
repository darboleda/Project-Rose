using UnityEngine;
using System.Collections;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class TriggerCameraTargetChange : Behavior
    {
        public CameraPosition target;

        public void OnCanalTriggerEnter(Trigger trigger)
        {
            Camera.main.GetComponent<MovingCamera>().Target = target;
        }
    }
}
