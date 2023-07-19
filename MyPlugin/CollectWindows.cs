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
    public class CollectWindows : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            try
            {
                //get UIDocument
                UIDocument uidoc = commandData.Application.ActiveUIDocument;

                //get document
                Document doc = uidoc.Document;

                //create filtered element collector
                FilteredElementCollector collector = new FilteredElementCollector(doc);

                //create a filter
                ElementCategoryFilter filter = new ElementCategoryFilter(BuiltInCategory.OST_Windows);

                //apply filter
                IList<Element> windows = collector.WherePasses(filter).WhereElementIsNotElementType().ToElements();

                TaskDialog.Show("Windows", string.Format("{0} windows counted!", windows.Count));

                return Result.Succeeded;
            }
            catch (Exception ex)
            {
                message = ex.Message;
                return Result.Failed;
            }
        }
    }
}
