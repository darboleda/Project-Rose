using UnityEngine;
using System.Collections;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class CameraControllerTarget : Behavior
    {
        private CameraController CurrentController;
        public void SetController(CameraController controller)
        {
            if (CurrentController != null) CurrentController.Camera = null;
            CurrentController = controller;
        }
    }
}
