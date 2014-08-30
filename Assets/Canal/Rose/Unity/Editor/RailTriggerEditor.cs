using UnityEngine;
using UnityEditor;

using System.Collections.Generic;

using Canal.Unity;
using Canal.Rose.Unity.Engine;

namespace Canal.Rose.Unity.Editor
{
    [CustomEditor(typeof(RailTrigger))]
    public class RailTriggerEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            RailTrigger trigger = target as RailTrigger;
            if (trigger == null) return;
            if (trigger.Parent == null) return;

            float min, max, startMin, startMax;
            min = startMin = trigger.Minimum;
            max = startMax = trigger.Maximum;
            EditorGUILayout.MinMaxSlider(new GUIContent("Area"), ref min, ref max, 0, trigger.Parent.GetWorldLength());
            if (min != startMin
             || max != startMax)
            {
                trigger.Minimum = min;
                trigger.Maximum = max;
                trigger.Minimum = min;
                SceneView.RepaintAll();
            }
        }

        public void OnSceneGUI()
        {
            RailTrigger trigger = target as RailTrigger;
            if (trigger == null) return;
            if (trigger.Parent == null) return;
            
            trigger.Parent.Draw();

            Color color = Handles.color;
            Handles.color = Color.red;
            
            float min = trigger.Minimum;
            float max = trigger.Maximum;
            float size = HandleUtility.GetHandleSize(trigger.Parent.SampleWorld(min));
            float delta = size * 0.1f;
            Vector3 position;
            do
            {
                position = trigger.Parent.SampleWorld(min);
                Handles.CubeCap(0, position, Quaternion.identity, size * 0.05f);
                min += delta;
                
            } while (min < max);
            position = trigger.Parent.SampleWorld(max);
            Handles.CubeCap(0, position, Quaternion.identity, size * 0.05f);

            Handles.color = color;
        }
    }
}
