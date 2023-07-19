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
    public class EditElement : IExternalCommand
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

                    using(Transaction trans = new Transaction(doc, "Edit Elements"))
                    {

                        trans.Start();

                        //move element
                        XYZ moveVec = new XYZ(3, 3, 0);
                        ElementTransformUtils.MoveElement(doc, eleId, moveVec);

                        //rotate element
                        LocationPoint location = ele.Location as LocationPoint;
                        XYZ p1 = location.Point;
                        XYZ p2 = new XYZ(p1.X, p1.Y, p1.Z + 10);
                        Line axis = Line.CreateBound(p1,p2);
                        double angle = 30 * Math.PI / 180;
                        ElementTransformUtils.RotateElement(doc, eleId, axis, angle);

                        trans.Commit();

                    }
                    
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
