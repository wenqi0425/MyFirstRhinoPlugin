using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using System;
using System.Collections.Generic;

namespace HelloRhinoCommon
{
    public class HelloDrawLineCommand : Command
    {
        public HelloDrawLineCommand()
        {
            Instance = this;
        }

        ///<summary>The only instance of this command.</summary>
        // Rhino only creates one instance of each command class defined in a plug-in,
        // so it is safe to store a refence in a static property.
        public static HelloDrawLineCommand Instance { get; private set; }

        ///<returns>The command name as it appears on the Rhino command line.</returns>
        // All Rhino commands must have a EnglishName property.
        // This command prompts the user to draw a line.
        public override string EnglishName => "DrawLine";

        // All Rhino commands must have a RunCommand method.
        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            // TODO: start here modifying the behaviour of your command.
            RhinoApp.WriteLine("The {0} command will add a line right now.", EnglishName);

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
                getPointAction.SetCommandPrompt("Please select the end point");
                getPointAction.SetBasePoint(pt0, true);
                getPointAction.DynamicDraw +=
                  (sender, e) => e.Display.DrawLine(pt0, e.CurrentPoint, System.Drawing.Color.DarkRed);
                if (getPointAction.Get() != GetResult.Point)
                {
                    RhinoApp.WriteLine("No end point was selected.");
                    return getPointAction.CommandResult();
                }
                pt1 = getPointAction.Point();
            }

            Line line = new Line(pt0, pt1);
            doc.Objects.AddLine(line);
            doc.Views.Redraw();
            RhinoApp.WriteLine("The {0} command added one line to the document.", EnglishName);
            RhinoApp.WriteLine("The distance between the two points is {0}.", line.Length);
            RhinoApp.WriteLine("The distance between the two points is {0} {1}.", 
                               line.Length, doc.ModelUnitSystem.ToString().ToLower());  // millimeters

            return Result.Success;
        }
    }
}
