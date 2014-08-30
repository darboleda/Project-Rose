using UnityEngine;
using UnityEditor;

using System.Collections.Generic;

using Canal.Unity;
using Canal.Rose.Unity.Engine;

namespace Canal.Rose.Unity.Editor
{
    public static class RailDrawer
    {
        public static void Draw(this Rail rail)
        {
            if (rail is BezierRail)
            {
                BezierRailEditor.DrawSceneRail(rail as BezierRail);
            }
            if (rail is FusedRail)
            {
                FusedRailEditor.DrawSceneRail(rail as FusedRail);
            }
        }
    }
}
