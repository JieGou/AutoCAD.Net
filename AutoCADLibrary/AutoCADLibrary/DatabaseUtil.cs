using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    /// AutoCAD Database에 대한 명령을 가지고 있는 클래스입니다.
    /// </summary>
    public class DatabaseUtil
    {
        /// <summary>
        /// database의 새로운 트랜잭션을 만들고 반환합니다.
        /// </summary>
        /// <param name="database">새로운 트랜잭션을 만들 database</param>
        /// <returns></returns>
        public static Transaction GetTransaction(Database database)
        {
            return database.TransactionManager.StartTransaction();
        }
    }
}
