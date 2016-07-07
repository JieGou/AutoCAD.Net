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
    /// 점, 벡터, 각도 등에 관련된 명령을 가지고 있는 클래스입니다.
    /// </summary>
    public class GeometryUtil
    {
        /// <summary>
        /// Point3d 좌표를 Point2d로 변환합니다.
        /// </summary>
        /// <param name="Point">변환할 좌표입니다.</param>
        /// <returns>변환된 좌표 Point2d를 리턴합니다.</returns>
        public static Point2d ToPoint2d(Point3d Point)
        {
            return new Point2d(Point.X, Point.Y);
        }

        /// <summary>
        /// Point2d 좌표를 Point3d로 변환합니다.
        /// </summary>
        /// <param name="Point">변환할 좌표입니다.</param>
        /// <returns>변환된 좌표 Point3d를 리턴합니다.</returns>
        public static Point3d ToPoint3d(Point2d Point)
        {
            return new Point3d(Point.X, Point.Y, 0);
        }

        /// <summary>
        /// 객체의 최대(오른쪽위), 최소(왼쪽아래) 점을 가져옵니다.
        /// </summary>
        /// <param name="EntityId">점을 가져올 객체의 ObjectId 입니다.</param>
        /// <returns></returns>
        public static Point3dCollection GetMinMaxPoint(ObjectId EntityId)
        {
            Document doc = Autodesk.AutoCAD.ApplicationServices.Application.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;
            Point3dCollection vPnts = new Point3dCollection();

            try
            {
                using (doc.LockDocument())
                {
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        Entity oEnt = tr.GetObject(EntityId, OpenMode.ForRead) as Entity;

                        vPnts.Add(oEnt.GeometricExtents.MinPoint);
                        vPnts.Add(oEnt.GeometricExtents.MaxPoint);

                        return vPnts;
                    }
                }
            }
            catch(System.Exception ex)
            {
                System.Diagnostics.Debug.Print(string.Format("************에러발생************\n위치 : CreateRegApp\n메시지 : {0}", ex.Message));
                return null;
            }
        }
    }
}
