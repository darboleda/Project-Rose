using UnityEngine;
using UnityEditor;

using System.Collections.Generic;

using Canal.Unity;
using Canal.Rose.Unity.Engine;

namespace Canal.Rose.Unity.Editor
{
    [CustomEditor(typeof(BezierRail))]
    public class BezierRailEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            BezierRail rail = target as BezierRail;
            if (rail == null) return;

            BezierPath path = rail.PathToBake;
            path = EditorGUILayout.ObjectField(path, typeof(BezierPath), true) as BezierPath;
            if (path != null)
            {
                float newSample = 0;
                if (rail.SegmentPoints == null)
                {
                    //newSample = EditorGUILayout.Slider(rail.CurrentSample, 0, path.CurveCount);
                }
                else
                {
                    newSample = EditorGUILayout.Slider(rail.CurrentSample, 0.0f, rail.GetWorldLength());
                }

                if (newSample != rail.CurrentSample)
                {
                    rail.CurrentSample = newSample;
                    SceneView.RepaintAll();
                }

                if (path.CurveCount != rail.CurveSegmentCounts.Count)
                {
                    List<int> lengths = new List<int>();
                    for (int i = 0, length = path.CurveCount; i < length; ++i)
                    {
                        lengths.Add(rail.DefaultCurveSegmentCount);
                    }

                    for (int i = 0, length = Mathf.Min(rail.CurveSegmentCounts.Count, path.CurveCount); i < length; ++i)
                    {
                        lengths[i] = rail.CurveSegmentCounts[i];
                    }
                    rail.CurveSegmentCounts = lengths;

                }
                if(GUILayout.Button("Bake segments"))
                {
                    rail.BakePath(rail.PathToBake);
                    SceneView.RepaintAll();
                }
            }
            rail.PathToBake = path;
        }

        public static void DrawSceneRail(BezierRail rail)
        {
            if (rail == null) return;
            
            if (rail.SegmentPoints != null)
            {
                Color color = Handles.color;
                Handles.color = Color.green;
                
                if (rail.SegmentPoints.Count > 1)
                {
                    for(int i = 0, length = rail.SegmentPoints.Count - 1; i < length; ++i)
                    {
                        Handles.DrawLine(rail.transform.TransformPoint(rail.SegmentPoints[i]),
                                         rail.transform.TransformPoint(rail.SegmentPoints[i + 1]));
                    }
                    
                }
                Handles.color = color;
            }
        }

        public void OnSceneGUI()
        {
            BezierRail rail = target as BezierRail;
            BezierRailEditor.DrawSceneRail(rail);

            if(rail == null) return;
            Color color = Handles.color;
            Handles.color = Color.green;

            BezierPath path = rail.PathToBake;
            if ((rail.SegmentPoints == null || rail.SegmentPoints.Count == 0) && path != null)
            {
                Vector3 position;
                int curveIndex = Mathf.FloorToInt(rail.CurrentSample);
                if (curveIndex < path.CurveCount)
                {
                    position = path.GetCurve(curveIndex).Sample(rail.CurrentSample - curveIndex);
                }
                else
                {
                    position = path.GetCurve(curveIndex - 1).Sample(1.0f);
                }

                Handles.DrawSolidDisc(position, Vector3.up, 0.05f * HandleUtility.GetHandleSize(position));
            }
            else if (rail.SegmentPoints != null)
            {
                Vector3 position = rail.SampleWorld(rail.CurrentSample);
                Handles.DrawSolidDisc(position, Vector3.up, 0.05f * HandleUtility.GetHandleSize(position));
            }

            Handles.color = color;
        }
    }
}
