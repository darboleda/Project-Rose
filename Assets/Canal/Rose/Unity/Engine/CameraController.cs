using UnityEngine;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class CameraController : Behavior
    {
        public Transform CameraContainer;

        protected bool ready = false;
        public void SetCamera(CameraControllerTarget target)
        {
            target.SetController(this);
        }
    }
}
