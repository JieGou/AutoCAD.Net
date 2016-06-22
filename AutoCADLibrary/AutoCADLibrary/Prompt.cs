using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MsgBox = System.Windows.Forms.MessageBox;

using Autodesk.AutoCAD.ApplicationServices;
using AcadApp = Autodesk.AutoCAD.ApplicationServices.Application;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.Runtime;
using AcColor = Autodesk.AutoCAD.Colors;
using AcWindow = Autodesk.AutoCAD.Windows;

namespace AutoCADLibrary
{
    /// <summary>
    /// 객체 선택, 프롬프트 창에 대한 명령들을 가지고 있는 클래스입니다.
    /// Coded By KevinSung
    /// </summary>
    public class Prompt
    {
        public static readonly Point3d NullPoint3d = new Point3d(-65536.65536, -65536.65536, -65536.65536);

        private const string sRejectMsg = "잘못된 선택입니다.";

        /// <summary>
        /// 사용자가 선택한 객체를 가져오는 메소드 입니다.
        /// </summary>
        /// <param name="message">선택 전, 프롬프트에 표시할 메시지</param>
        /// <param name="Filter">선택이 가능하게 할 객체의 종류(클래스)</param>
        /// <returns>선택한 객체의 ObjectId를 리턴하며, ESC 등의 취소를 받았을 경우, ObjectId.Null 값을 리턴합니다.</returns>
        public static ObjectId GetEntity(string message = "객체 선택", params Type[] Filter)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptEntityOptions peo = new PromptEntityOptions("\n" + message);
            peo.SetRejectMessage("\n" + sRejectMsg);
            foreach (Type tType in Filter) peo.AddAllowedClass(tType, true);

            PromptEntityResult per = ed.GetEntity(peo);

            if (per.Status != PromptStatus.OK) return ObjectId.Null;

            return per.ObjectId;
        }

        /// <summary>
        /// 사용자가 선택한 점을 가져오는 메소드입니다.
        /// </summary>
        /// <param name="message">선택 전, 프롬프트에 표시할 메시지</param>
        /// <returns>지정한 점의 Point3d 좌표</returns>
        public static Point3d GetPoint(string message = "점 선택")
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptPointOptions PrmptPntOpt = new PromptPointOptions("\n" + message);
            PromptPointResult PrmptPntRst = ed.GetPoint(PrmptPntOpt);

            if (PrmptPntRst.Status != PromptStatus.OK) return NullPoint3d;

            return PrmptPntRst.Value;
        }

        /// <summary>
        /// 사용자가 선택한 점을 가져오는 메소드입니다. 점을 선택할 때, BasePoint를 기준으로 대쉬라인이 함께 나타납니다.
        /// </summary>
        /// <param name="message">선택 전, 프롬프트에 표시할 메시지</param>
        /// <param name="BasePoint">기준점의 좌표</param>
        /// <returns>지정한 점의 Point3d 좌표</returns>
        public static Point3d GetPoint(Point3d BasePoint, string message = "점 선택")
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptPointOptions PrmptPntOpt = new PromptPointOptions("\n" + message);

            PrmptPntOpt.UseBasePoint = true;
            PrmptPntOpt.UseDashedLine = true;
            PrmptPntOpt.BasePoint = BasePoint;

            PromptPointResult PrmptPntRst = ed.GetPoint(PrmptPntOpt);

            if (PrmptPntRst.Status != PromptStatus.OK) return NullPoint3d;

            return PrmptPntRst.Value;
        }
    }
}
