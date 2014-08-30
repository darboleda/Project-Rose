using UnityEngine;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class CameraPosition : CameraController
    {
        public Transform LookAtTarget;
        public Transform UpTarget;

        public bool LockXRotation = true;
        public bool LockYRotation = false;
        public bool LockZRotation = true;

        public void Update()
        {
            Vector3 startEuler = CameraContainer.eulerAngles;
            CameraContainer.LookAt(this.LookAtTarget.position, this.UpTarget.position - this.CameraContainer.position);
            Vector3 euler = CameraContainer.eulerAngles;
            euler.x = (LockXRotation ? startEuler.x : euler.x);
            euler.y = (LockYRotation ? startEuler.y : euler.y);
            euler.z = (LockZRotation ? startEuler.z : euler.z);
            CameraContainer.eulerAngles = euler;
        }
    }
}
