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
    public class Sheets : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            //Get UIDocument
            UIDocument uidoc = commandData.Application.ActiveUIDocument;

            //Get Document
            Document doc = uidoc.Document;

            //get family symbol
            FamilySymbol tBlock = new FilteredElementCollector(doc)
                .OfCategory(BuiltInCategory.OST_TitleBlocks)
                .WhereElementIsElementType()
                .Cast<FamilySymbol>()
                .First();

            try
            {
                using (Transaction trans = new Transaction(doc, "Create Sheet"))
                {
                    trans.Start();

                    //create 10 sheet
                    for (int i = 0; i < 10; i++)
                    {
                        ViewSheet vSheet = ViewSheet.Create(doc, tBlock.Id);
                        vSheet.Name = "My first sheet";
                        vSheet.SheetNumber = "J" + i.ToString();
                    }
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
