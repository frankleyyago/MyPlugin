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
    public class DeleteElement : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {            
                //get UIDocument
                UIDocument uidoc = commandData.Application.ActiveUIDocument;

                //get document
                Document doc = uidoc.Document;
            try
            {
                //pick object
                Reference pickedObj = uidoc.Selection.PickObject(Autodesk.Revit.UI.Selection.ObjectType.Element);

                //delete element
                if (pickedObj != null)
                {
                    using(Transaction trans = new Transaction(doc, "Delete element"))
                    {
                        trans.Start();
                        doc.Delete(pickedObj.ElementId);

                        TaskDialog tDialog = new TaskDialog("Delete Element");
                        tDialog.MainContent = "Are you sure you want to delete this element?";
                        tDialog.CommonButtons = TaskDialogCommonButtons.Ok | TaskDialogCommonButtons.Cancel;

                        if(tDialog.Show() == TaskDialogResult.Ok)
                        {
                            trans.Commit();
                            TaskDialog.Show("Delete", pickedObj.ElementId.ToString() + " deleted");
                        }
                        else
                        {
                            trans.RollBack();
                            TaskDialog.Show("Delete", pickedObj.ElementId.ToString() + " not deleted");
                        }
                    }
                }

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
