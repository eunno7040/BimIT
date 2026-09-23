using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB.Structure;



namespace Enl
{
    internal class util
    {
        /// <summary>
        /// 선택한 face의 edgd를 curve로 변환하는 함수
        /// </summary>
        /// <param name="face"></param>
        /// <returns></returns>
        public static List<Curve> GetCurves(Face face)
        {
            List<Curve> result = new List<Curve>();

            EdgeArrayArray edgeArrays = face.EdgeLoops;
            List<Curve> curves = new List<Curve>();
            foreach (EdgeArray edgeArray in edgeArrays)
            {
                foreach (Edge edge in edgeArray)
                {
                    Curve c = edge.AsCurve();
                    curves.Add(c);
                }
            }

            return curves;
        }

        public static FamilySymbol GetFamilySymbolBYName(string name, Document doc)
        {
            FilteredElementCollector collector = new FilteredElementCollector(doc);
            collector.OfCategory(BuiltInCategory.OST_StructuralFraming);
            collector.OfClass(typeof(FamilySymbol));
            FamilySymbol fs = null;

            foreach (FamilySymbol item in collector)
            {
                if (name == item.Name)
                {
                    fs = item;
                    break;
                }
            }
            return fs;

        }


        /// <summary>
        /// xyz 좌표 리스트를 받아서 curve 리스트로 변환하는 함수
        /// </summary>
        /// <param name="points"></param>
        /// <returns></returns>
        public static List<Curve> GetCurvesListFromPts(List<XYZ> points)
        {
            List<Curve> curves = new List<Curve>();

            for (int i = 0; i < points.Count -1; i++)
            {
                Line line = Line.CreateBound(points[i], points[i + 1]);
                curves.Add(line);
            }

            return curves;
        }



        public static void CreateFamilyInstanceFromCuvve(List<Curve> c, FamilySymbol fs, Level level, Document doc)
        {
            foreach (Curve item in c)
            {
                using (Transaction trans = new Transaction(doc, "Create Beam"))
                {
                    trans.Start();
                    fs.Activate();
                    FamilyInstance fi = doc.Create.NewFamilyInstance(item, fs, level, StructuralType.Beam);
                    trans.Commit();
                }
            }
        }
    }
}
