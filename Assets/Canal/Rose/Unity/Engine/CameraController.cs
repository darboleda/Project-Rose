using UnityEngine;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class CameraController : Behavior
    {
        public CameraControllerTarget Camera;
        public void SetCamera(Camera camera)
        {
            CameraControllerTarget target = camera.GetComponent<CameraControllerTarget>();
            if (target != null) target.SetController(this);
            Camera = target;
        }
    }
}
