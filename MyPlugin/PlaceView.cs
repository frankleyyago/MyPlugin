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
    public class PlaceView : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Get Document
            Document doc = uidoc.Document;

            //find sheet
            ViewSheet vSheet = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Sheets)
                .Cast<ViewSheet>()
                .First(x => x.Name == "My first sheet");

            //find view
            Element vPlan = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_Views)
                .First(x => x.Name == "Our first plan!");

            //get midpoint
            BoundingBoxUV outline = vSheet.Outline;
            double xu = (outline.Max.U + outline.Min.U) / 2;
            double yu = (outline.Max.V + outline.Min.V) / 2;
            XYZ midPoint = new XYZ(xu, yu, 0);

            try
            {
                using (Transaction trans = new Transaction(doc, "Place view"))
                {
                    trans.Start();

                    //place view
                    Viewport vPort = Viewport.Create(doc, vSheet.Id, vPlan.Id, midPoint);

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
