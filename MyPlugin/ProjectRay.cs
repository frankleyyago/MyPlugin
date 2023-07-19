using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.Attributes;

namespace MyPlugin
{
    [TransactionAttribute(TransactionMode.Manual)]
    public class ProjectRay : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Get Document
            Document doc = uidoc.Document;
            
            try
            {
                //Pick Object
                Reference pickedObj = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                if (pickedObj != null)
                {
                    //Retrieve element
                    ElementId eleId = pickedObj.ElementId;
                    Element ele = doc.GetElement(eleId);

                    //project ray
                    LocationPoint locP = ele.Location as LocationPoint;
                    XYZ p1 = locP.Point;

                    //ray
                    XYZ rayd = new XYZ(0, 0, 1);

                    ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Roofs);
                    ReferenceIntersector refI = new ReferenceIntersector(filter, FindReferenceTarget.Face, (View3D)doc.ActiveView);
                    ReferenceWithContext refC = refI.FindNearest(p1, rayd);
                    Reference reference = refC.GetReference();
                    XYZ intPoint = reference.GlobalPoint;
                    double dist = p1.DistanceTo(intPoint);

                    TaskDialog.Show("Ray", string.Format("Distance to roof {0}", dist));

                    //filter for intersection

                }

                return Result.Succeeded;
            }
            catch (Exception e)
            {
                message = e.Message;
                return Result.Failed;
            }

        }
    }
}
