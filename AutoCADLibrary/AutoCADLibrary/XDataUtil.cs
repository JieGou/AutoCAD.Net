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
    /// XData에 대한 명령이 들어있는 클래스입니다.
    /// </summary>
    public class XDataUtil
    {
        public static void SetXRecord(ObjectId id, string Key, ResultBuffer resbuf)
        {
            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                DBObject ent = tr.GetObject(id, OpenMode.ForRead);
                if (ent != null)
                {
                    ent.UpgradeOpen();
                    ent.CreateExtensionDictionary();
                    DBDictionary xDict = (DBDictionary)tr.GetObject(ent.ExtensionDictionary, OpenMode.ForWrite);
                    Xrecord xRec = new Xrecord();
                    xRec.Data = resbuf;
                    xDict.SetAt(Key, xRec);
                    tr.AddNewlyCreatedDBObject(xRec, true);
                }
                tr.Commit();
            }
        }

        public static ResultBuffer GetXRecord(ObjectId id, string Key)
        {
            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            ResultBuffer result = new ResultBuffer();
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                Xrecord xRec = new Xrecord();
                DBObject ent = tr.GetObject(id, OpenMode.ForRead, false);
                if (ent != null)
                {
                    try
                    {
                        DBDictionary xDict = (DBDictionary)tr.GetObject(ent.ExtensionDictionary, OpenMode.ForRead, false);
                        xRec = (Xrecord)tr.GetObject(xDict.GetAt(Key), OpenMode.ForRead, false);
                        return xRec.Data;
                    }
                    catch
                    {
                        return null;
                    }
                }
                else
                    return null;
            }
        }

        private static void AddRegAppTableRecord(string applicationName)
        {
            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (doc.LockDocument())
            {
                using (Transaction tr = DatabaseUtil.GetTransaction(db))
                {
                    RegAppTable rat = tr.GetObject(db.RegAppTableId, OpenMode.ForRead, false) as RegAppTable;

                    if (!rat.Has(applicationName))
                    {
                        rat.UpgradeOpen();
                        RegAppTableRecord ratr = new RegAppTableRecord();
                        ratr.Name = applicationName;
                        rat.Add(ratr);
                        tr.AddNewlyCreatedDBObject(ratr, true);
                    }

                    tr.Commit();
                }
            }
        }

        public static void SetXData(ObjectId id, ResultBuffer data)
        {
            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (doc.LockDocument())
            {
                using (Transaction tr = DatabaseUtil.GetTransaction(db))
                {
                    DBObject ent = tr.GetObject(id, OpenMode.ForWrite);

                    #region Create RegAppTableRecord
                    TypedValue[] arrTypedValue = data.AsArray();
                    for (int i = 0; i < arrTypedValue.Length; i++)
                    {
                        TypedValue oTV = arrTypedValue[i];

                        if (oTV.TypeCode == (int)DxfCode.ExtendedDataRegAppName)
                        {
                            AddRegAppTableRecord(oTV.Value.ToString());
                        }
                    }
                    #endregion

                    ent.XData = data;

                    tr.Commit();
                }
            }
        }

        public static ResultBuffer GetXDataForApplication(ObjectId id, string applicationName)
        {
            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (doc.LockDocument())
            {
                using (Transaction tr = DatabaseUtil.GetTransaction(db))
                {
                    DBObject ent = tr.GetObject(id, OpenMode.ForWrite);

                    return ent.GetXDataForApplication(applicationName);
                }
            }
        }

        public static ResultBuffer GetXData(ObjectId id)
        {
            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            using (doc.LockDocument())
            {
                using (Transaction tr = DatabaseUtil.GetTransaction(db))
                {
                    DBObject ent = tr.GetObject(id, OpenMode.ForWrite);

                    return ent.XData;
                }
            }
        }

        //[CommandMethod("test1")]
        //public void test()
        //{
        //    Document doc = AcadApp.DocumentManager.MdiActiveDocument;
        //    Database db = doc.Database;
        //    Editor ed = doc.Editor;

        //    ObjectId oEntId = PromptUtil.GetEntity("soomin");

        //    using (Transaction tr = db.TransactionManager.StartTransaction())
        //    {
        //        Entity oEnt = tr.GetObject(oEntId, OpenMode.ForWrite) as Entity;

        //        //SetXData(oEnt, "TESTAPP[0]", "Kevin");

        //        //MessageBox.Show(GetXData(oEnt)[0].ToString());

        //        tr.Commit();
        //    }
        //}

        //[CommandMethod("test2")]
        //public void test2()
        //{
        //    Document doc = AcadApp.DocumentManager.MdiActiveDocument;
        //    Database db = doc.Database;
        //    Editor ed = doc.Editor;

        //    ObjectId oEntId = PromptUtil.GetEntity("soomin");

        //    using (Transaction tr = db.TransactionManager.StartTransaction())
        //    {
        //        Entity oEnt = tr.GetObject(oEntId, OpenMode.ForWrite) as Entity;

        //        //RemoveXData(oEnt, "TESTAPP[0]");

        //        //MessageBox.Show(GetXData(oEnt)[0].ToString());

        //        tr.Commit();
        //    }
        //}
    }
}
