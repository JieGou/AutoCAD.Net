using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
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
    public class PromptUtil
    {
        private const string sRejectMsg = "잘못된 선택입니다.";

        public static Point3d NullPoint3d
        {
            get
            {
                return new Point3d(-65536.65536, -65536.65536, -65536.65536);
            }
        }

        /// <summary>
        /// 사용자가 선택한 객체를 가져오는 메소드 입니다.
        /// </summary>
        /// <param name="message">선택 전, 프롬프트에 표시할 메시지</param>
        /// <param name="Filter">선택이 가능하게 할 객체의 종류(클래스)</param>
        /// <returns>선택한 객체의 ObjectId를 리턴하며, ESC 등의 취소를 받았을 경우, ObjectId.Null 값을 리턴합니다.</returns>
        public static ObjectId GetEntity(string message = "객체 선택", params Type[] Filter)
        {
            ActivateApplication();

            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
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
        /// pt1과 pt2를 기준으로 사각형 내의 객체를 가져오는 메소드입니다.
        /// </summary>
        /// <param name="pt1">첫번째점</param>
        /// <param name="pt2">두번째점</param>
        /// <param name="Filter">선택이 가능하게 할 조건</param>
        /// <returns></returns>
        public static ObjectId[] GetEntitiesByWindow(Point3d pt1, Point3d pt2, params TypedValue[] Filter)
        {
            ActivateApplication();

            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            SelectionFilter oSF = new SelectionFilter(Filter);

            PromptSelectionResult psr = ed.SelectCrossingWindow(pt1, pt2, oSF);

            if (psr.Status != PromptStatus.OK) return new ObjectId[] { };

            return psr.Value.GetObjectIds();
        }

        /// <summary>
        /// 사용자가 선택한 여러 객체들을 가져오는 메소드입니다.
        /// </summary>
        /// <param name="message">선택 전, 프롬프트에 표시할 메시지</param>
        /// <param name="Filter">선택이 가능하게 할 조건</param>
        /// <returns></returns>
        public static ObjectId[] GetEntities(string message = "객체 선택", params TypedValue[] Filter)
        {
            ActivateApplication();

            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptSelectionOptions pso = new PromptSelectionOptions();
            pso.MessageForAdding = "\n" + message;
            pso.MessageForRemoval = "\n" + message;

            SelectionFilter oSF = new SelectionFilter(Filter);

            PromptSelectionResult psr = ed.GetSelection(pso, oSF);

            if (psr.Status != PromptStatus.OK) return new ObjectId[] { };

            return psr.Value.GetObjectIds();
        }

        /// <summary>
        /// 사용자가 선택한 점을 가져오는 메소드입니다.
        /// </summary>
        /// <param name="message">선택 전, 프롬프트에 표시할 메시지</param>
        /// <returns>지정한 점의 Point3d 좌표</returns>
        public static Point3d GetPoint(string message = "점 선택")
        {
            ActivateApplication();

            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
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
            ActivateApplication();

            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
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

        /// <summary>
        /// 사용자가 선택한 점을 가져오는 메소드입니다.
        /// </summary>
        /// <param name="message">선택 전, 프롬프트에 표시할 메시지</param>
        /// <returns>사용자의 입력에 대한 결과</returns>
        public static PromptPointResult GetPointResult(string message = "점 선택", bool useEnterKey = false)
        {
            ActivateApplication();

            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptPointOptions PrmptPntOpt = new PromptPointOptions("\n" + message);
            PrmptPntOpt.AllowNone = useEnterKey;

            PromptPointResult PrmptPntRst = ed.GetPoint(PrmptPntOpt);

            return PrmptPntRst;
        }

        /// <summary>
        /// 사용자가 선택한 점을 가져오는 메소드입니다. 점을 선택할 때, BasePoint를 기준으로 대쉬라인이 함께 나타납니다.
        /// </summary>
        /// <param name="message">선택 전, 프롬프트에 표시할 메시지</param>
        /// <param name="BasePoint">기준점의 좌표</param>
        /// <returns>사용자의 입력에 대한 결과</returns>
        public static PromptPointResult GetPointResult(Point3d BasePoint, string message = "점 선택", bool useEnterKey = false)
        {
            ActivateApplication();

            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptPointOptions PrmptPntOpt = new PromptPointOptions("\n" + message);

            PrmptPntOpt.AllowNone = useEnterKey;
            PrmptPntOpt.UseBasePoint = true;
            PrmptPntOpt.UseDashedLine = true;
            PrmptPntOpt.BasePoint = BasePoint;

            PromptPointResult PrmptPntRst = ed.GetPoint(PrmptPntOpt);

            return PrmptPntRst;
        }

        /// <summary>
        /// 사용자로부터 문자열을 입력받습니다.
        /// </summary>
        /// <param name="message">입력 전, 프롬프트에 표시할 메시지</param>
        /// <returns></returns>
        public static string GetString(string message = "입력", bool allowSpace = false, string defaultString = "")
        {
            ActivateApplication();

            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            PromptStringOptions PrmptStrOpt = new PromptStringOptions("\n" + message);
            PrmptStrOpt.DefaultValue = defaultString;
            PrmptStrOpt.AllowSpaces = allowSpace;

            PromptResult PrmptRst = ed.GetString(PrmptStrOpt);

            if (PrmptRst.Status != PromptStatus.OK) return "";

            return PrmptRst.StringResult;
        }

        [DllImport("user32.dll")]
        private static extern int SetActiveWindow(int hwnd);

        /// <summary>
        /// AutoCAD 창을 활성화시킵니다.
        /// </summary>
        public static void ActivateApplication()
        {
            SetActiveWindow(AcadApp.MainWindow.Handle.ToInt32());
        }

        /// <summary>
        /// AutoCAD의 진행바를 정의하여 리턴합니다.
        /// </summary>
        /// <param name="message">진행바 진행 시, 어떤 메시지를 출력할지 나타냅니다.</param>
        /// <param name="limit">진행바의 최대값을 설정합니다.</param>
        /// <returns></returns>
        public static ProgressMeter GetProgressBar(string message, int limit)
        {
            ProgressMeter oProgressMeter = new ProgressMeter();
            oProgressMeter.Start(message);
            oProgressMeter.SetLimit(limit);

            return oProgressMeter;
        }

        /// <summary>
        /// AutoCAD에 메세지를 출력합니다.
        /// </summary>
        /// <param name="message">출력할 메세지입니다.</param>
        public static void PromptMessage(string message)
        {
            DatabaseUtil.GetEditor(DatabaseUtil.GetActiveDocument()).WriteMessage(message);
        }
    }
}
