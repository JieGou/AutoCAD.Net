using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
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
        /// <param name="Point">변환할 좌표</param>
        /// <returns>변환된 좌표 Point2d를 리턴합니다.</returns>
        public static Point2d ToPoint2d(Point3d Point)
        {
            return new Point2d(Point.X, Point.Y);
        }

        /// <summary>
        /// Point2d 좌표를 Point3d로 변환합니다.
        /// </summary>
        /// <param name="Point">변환할 좌표</param>
        /// <returns>변환된 좌표 Point3d를 리턴합니다.</returns>
        public static Point3d ToPoint3d(Point2d Point)
        {
            return new Point3d(Point.X, Point.Y, 0);
        }
    }
}
