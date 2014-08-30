using UnityEngine;
using UnityEditor;

using System.Collections.Generic;

using Canal.Unity;
using Canal.Rose.Unity.Engine;

namespace Canal.Rose.Unity.Editor
{
    [CustomEditor(typeof(FusedRail))]
    public class FusedRailEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            FusedRail rail = target as FusedRail;
            if (rail == null) return;

            float newSample = EditorGUILayout.Slider(rail.CurrentSample, 0.0f, rail.GetWorldLength());
            if (newSample != rail.CurrentSample)
            {
                rail.CurrentSample = newSample;
                SceneView.RepaintAll();
            }
        }

        public static void DrawSceneRail(FusedRail rail)
        {
            if (rail == null) return;
            foreach (Rail subRail in rail.Rails)
            {
                subRail.Draw();
            }
        }

        public void OnSceneGUI()
        {
            FusedRail rail = target as FusedRail;
            FusedRailEditor.DrawSceneRail(rail);
            if (rail == null) return;

            Color color = Handles.color;
            Handles.color = Color.green;
            Vector3 position = rail.SampleWorld(rail.CurrentSample);
            Handles.DrawSolidDisc(position, Vector3.up, 0.05f * HandleUtility.GetHandleSize(position));
            Handles.color = color;
        }
    }
}
