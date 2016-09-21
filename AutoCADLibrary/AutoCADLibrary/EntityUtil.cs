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
    /// 객체에 대한 명령들을 가지고 있는 클래스 입니다.
    /// </summary>
    public class EntityUtil
    {
        /// <summary>
        /// 객체들을 지웁니다.
        /// </summary>
        /// <param name="entityIds">지울 객체의 ObjectId입니다.</param>
        public static void EraseEntities(ObjectIdCollection entityIds)
        {
            Document doc = DatabaseUtil.GetActiveDocument();
            Database db = doc.Database;
            
            try
            {
                using (doc.LockDocument())
                {
                    using (Transaction tr = DatabaseUtil.GetTransaction(db))
                    {
                        for (int i = 0 ; i < entityIds.Count ; i ++)
                        {
                            try
                            {
                                Entity oEnt = tr.GetObject(entityIds[i], OpenMode.ForWrite) as Entity;
                                oEnt.Erase();
                            }
                            catch
                            {

                            }
                        }

                        tr.Commit();
                    }
                }
            }
            catch (System.Exception ex)
            {

            }
        }
    }
}
