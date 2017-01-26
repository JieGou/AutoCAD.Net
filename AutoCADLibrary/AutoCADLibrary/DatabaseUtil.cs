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

        public static Document GetActiveDocument()
        {
            return Application.DocumentManager.MdiActiveDocument;
        }

        /// <summary>
        /// 사용하지 않습니다.
        /// </summary>
        /// <param name="document">Datbase를 가져올 Document 입니다.</param>
        /// <returns></returns>
        public static Database GetDatabase(Document document)
        {
            return document.Database;
        }

        /// <summary>
        /// 사용하지 않습니다.
        /// </summary>
        /// <param name="document">Editor를 가져올 Document 입니다.</param>
        /// <returns></returns>
        public static Editor GetEditor(Document document)
        {
            return document.Editor;
        }
    }
}
