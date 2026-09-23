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
                foreach (Edge edge in edgeArrays)
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

    }
}
