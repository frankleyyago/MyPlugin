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
    public class ViewFilter : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Get Document
            Document doc = uidoc.Document;

            //create a filter
            List<ElementId> cats = new List<ElementId>();
            cats.Add(new ElementId(BuiltInCategory.OST_Sections));

            ElementParameterFilter filter = new ElementParameterFilter(ParameterFilterRuleFactory.CreateContainsRule(new ElementId(BuiltInParameter.VIEW_NAME), "WIP", false));
                        
            
            try
            {
                using (Transaction trans = new Transaction(doc, "Apply filter"))
                {
                    trans.Start();

                    //apply filter
                    ParameterFilterElement filterElement = ParameterFilterElement.Create(doc, "My first filter", cats, filter);
                    doc.ActiveView.AddFilter(filterElement.Id);
                    doc.ActiveView.SetFilterVisibility(filterElement.Id, false);
                    

                    trans.Commit();
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
