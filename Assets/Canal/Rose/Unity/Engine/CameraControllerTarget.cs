using UnityEngine;
using System.Collections;

using Canal.Unity;

namespace Canal.Rose.Unity.Engine
{
    public class CameraControllerTarget : Behavior
    {
        public float FollowSpeed;
        public float RotationSpeed;

        public float InterpolationRatio = 0.3f;

        [SerializeField]
        private CameraController CurrentController;
        private IEnumerator currentTransition;

        public void Start()
        {
            if (CurrentController != null) this.StartTransition(CurrentController, 1.0f);
        }

        public void SetController(CameraController controller)
        {
            CurrentController = controller;
            if (controller != null) this.transform.parent = controller.CameraContainer;
            else this.transform.parent = null;
        }

        public void StartTransition(CameraController controller)
        {
            this.StartTransition(controller, InterpolationRatio);
        }

        public void StartTransition(CameraController controller, float interpolationRatio)
        {
            if(this.currentTransition != null) this.StopCoroutine(this.currentTransition);
            this.currentTransition = Transition(controller, interpolationRatio);
            this.StartCoroutine(this.currentTransition);
        }

        private IEnumerator Transition(CameraController controller, float interpolationRatio)
        {
            controller.SetCamera(this);
            float coveredDistance = 1;
            do
            {
                this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, Vector3.zero, interpolationRatio);
                this.transform.localRotation = Quaternion.Lerp(this.transform.localRotation, Quaternion.identity, interpolationRatio);
                this.transform.localScale = Vector3.Lerp(this.transform.localScale, Vector3.one, interpolationRatio);
                coveredDistance *= (1 - interpolationRatio);
                yield return null;

            } while (coveredDistance > 0.00001f);
            this.transform.localPosition = Vector3.zero;
            this.transform.localRotation = Quaternion.identity;
            this.transform.localScale = Vector3.one;
            this.currentTransition = null;
        }
    }
}
