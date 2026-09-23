using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.Structure;
using System.Diagnostics;

namespace Enl
{
    [Transaction(TransactionMode.Manual)]

    public class Command : IExternalCommand
    {
        public Result Execute(ExternalCommandData commandData, ref string message, ElementSet elements)
        {
            UIApplication uiApp = commandData.Application;
            UIDocument uiDoc = uiApp.ActiveUIDocument;
            Document doc = uiDoc.Document;

            #region
            //github 연습중
            //Reference r = uIDoc.Selection.PickObject(ObjectType.Element, "객체를 선택하세요");
            //Element e = doc.GetElement(r);
            //TaskDialog.Show("선택한 객체의 이름은 : ", e.Name);

            //IList<Reference> refs = uIDoc.Selection.PickObjects
            //    (ObjectType.Edge, "객체들를 선택하세요");


            //List<Curve> curves = new List<Curve>();    

            //foreach (Reference item in refs)
            //{
            //    Edge edge = doc.GetElement(refs[0]).GetGeometryObjectFromReference(item) as Edge;
            //    Curve c = edge.AsCurve();
            //    curves.Add(c);

            //}
            //IList<Reference> refs = uiDoc.Selection.PickObjects(ObjectType.Face, "객체를 선택하세요");

            //Face face = doc.GetElement(refs[0]).GetGeometryObjectFromReference(refs[0]) as Face;

            //List<Curve> dd = util.GetCurves(face);


            //FilteredElementCollector collector = new FilteredElementCollector(doc);
            //collector.OfCategory(BuiltInCategory.OST_StructuralFraming);
            //collector.OfClass(typeof(FamilySymbol));
            //FamilySymbol fs = collector.FirstElement() as FamilySymbol;

            //Level level = doc.ActiveView.GenLevel;

            //int count = 0;

            //FamilySymbol tt = util.GetFamilySymbolBYName("G1", doc);
            //if (tt == null)
            //{
            //    Autodesk.Revit.UI.TaskDialog.Show("오류", "해당이름의 패밀리심볼을 찾을 수 없습니다");

            //        return Result.Failed;
                             
            //}
            //foreach (Curve c in dd)
            //{
            //    using (Transaction trans = new Transaction(doc, "Create Beam"))
            //    {
            //        trans.Start();
            //        fs.Activate();
            //        FamilyInstance fi = doc.Create.NewFamilyInstance(c, fs, level, StructuralType.Beam);
            //        Parameter param = fi.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);
            //        param.Set(count);

            //        trans.Commit();
            //    }

            //}

            //foreach (Reference r in refs)
            //{
            //    Element e = doc.GetElement(r);
            //    Wall wall = e as Wall;
            //    Parameter param = wall.get_Parameter(BuiltInParameter.ALL_MODEL_INSTANCE_COMMENTS);
            //    Parameter param2 = wall.LookupParameter("mark");

            //    using (Transaction trans = new Transaction(doc, "Set Comment"))
            //    {
            //        trans.Start();
            //        param.Set("qwer");
            //        param2.Set("123");
            //        trans.Commit();
            //    }
            //    string a = param.AsString();
            //    Autodesk.Revit.UI.TaskDialog.Show("코맨트의 정보는 :", a);


            //            }
            #endregion

            OpenFileDialog ofd = new OpenFileDialog();
            string filePath = "";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                filePath = ofd.FileName;
            }

            List<XYZ> points = new List<XYZ>();
            using (StreamReader sr = new StreamReader(filePath))
            {
                string line = "";
                while ((line = sr.ReadLine()) !=null)
                {
                    string[] strs = line.Split(",");
                    double x = Convert.ToDouble(strs[0]);
                    double y = Convert.ToDouble(strs[1]);
                    double z = Convert.ToDouble(strs[2]);
                    XYZ p1 = new XYZ(x, y, z)/304.8;
                    points.Add(p1);
                }
            }

            //Debug.Print(points.Count.ToString());

            List<Curve> curves = util.GetCurvesListFromPts(points);
            FamilySymbol fs = util.GetFamilySymbolBYName("G1", doc);
            Level level = doc.ActiveView.GenLevel;

            util.CreateFamilyInstanceFromCuvve(curves, fs, level, doc);
            return Result.Succeeded;
            
        }
    }
}
