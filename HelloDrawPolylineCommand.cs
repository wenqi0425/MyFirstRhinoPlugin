using System;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input.Custom;
using Rhino.Input;

namespace HelloRhinoCommon
{
    public class HelloDrawPolylineCommand : Command
    {
        public HelloDrawPolylineCommand()
        {
            Instance = this;
        }

        ///<summary>The only instance of the MyCommand command.</summary>
        public static HelloDrawPolylineCommand Instance { get; private set; }

        public override string EnglishName => "DrawPolyline";

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: complete command.

            Point3d pt0;
            using (GetPoint getPointAction = new GetPoint())
            {
                getPointAction.SetCommandPrompt("Please select the start point");
                if (getPointAction.Get() != GetResult.Point)
                {
                    RhinoApp.WriteLine("No start point was selected.");
                    return getPointAction.CommandResult();
                }
                pt0 = getPointAction.Point();
            }

            Point3d pt1;
            using (GetPoint getPointAction = new GetPoint())
            {
                getPointAction.SetCommandPrompt("Please select the 2nd point");
                getPointAction.SetBasePoint(pt0, true);
                getPointAction.DynamicDraw +=
                  (sender, e) => e.Display.DrawLine(pt0, e.CurrentPoint, System.Drawing.Color.Aqua);
                if (getPointAction.Get() != GetResult.Point)
                {
                    RhinoApp.WriteLine("No 2nd point was selected.");
                    return getPointAction.CommandResult();
                }
                pt1 = getPointAction.Point();
            }

            Point3d pt2;
            using (GetPoint getPointAction = new GetPoint())
            {
                getPointAction.SetCommandPrompt("Please select the end point");
                getPointAction.SetBasePoint(pt1, true);
                getPointAction.DynamicDraw +=
                  (sender, e) => e.Display.DrawLine(pt1, e.CurrentPoint, System.Drawing.Color.Coral);
                if (getPointAction.Get() != GetResult.Point)
                {
                    RhinoApp.WriteLine("No end point was selected.");
                    return getPointAction.CommandResult();
                }
                pt2 = getPointAction.Point();
            }

            Rhino.Geometry.Line line1 = new Line(pt0, pt1);
            Line line2 = new Line(pt1, pt2);
            doc.Objects.AddLine(line1);
            doc.Objects.AddLine(line2);
            doc.Views.Redraw();
            RhinoApp.WriteLine("The {0} command Draw a Polyline to the document.", EnglishName);
            


            return Result.Success;
        }
    }
}