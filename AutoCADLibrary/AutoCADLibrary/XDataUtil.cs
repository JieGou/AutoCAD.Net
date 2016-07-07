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
        private static string ApplicationName = "KevinSungApplication";

        public static void SetXData(Entity entity, params object[] Datas)
        {

            try
            {
                CreateRegApp(ApplicationName);

                ResultBuffer oXData = new ResultBuffer();

                oXData.Add(new TypedValue((int)DxfCode.ExtendedDataRegAppName, ApplicationName));

                foreach (object Data in Datas)
                {
                    oXData.Add(new TypedValue((int)DxfCode.ExtendedDataAsciiString, Data.ToString()));
                }
                entity.XData = oXData;
            }
            catch(System.Exception ex)
            {

            }
        }

        public static object[] GetXData(Entity entity)
        {

            try
            {
                List<object> lstDatas = new List<object>();
                ResultBuffer oXData = entity.XData;

                foreach (TypedValue oData in oXData) lstDatas.Add(oData.Value);

                return lstDatas.ToArray();
            }
            catch
            {
                return null;
            }
        }

        private static void CreateRegApp(string AppName)
        {
            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;

            try
            {
                using (doc.LockDocument())
                {
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        RegAppTable oRegApps = tr.GetObject(db.RegAppTableId, OpenMode.ForWrite) as RegAppTable;

                        if (!oRegApps.Has(AppName))
                        {
                            RegAppTableRecord oRegApp = new RegAppTableRecord();
                            oRegApp.Name = AppName;
                            oRegApps.Add(oRegApp);
                            tr.AddNewlyCreatedDBObject(oRegApp, true);
                        }

                        tr.Commit();
                    }
                }
            }
            catch(System.Exception ex)
            {
                System.Diagnostics.Debug.Print(string.Format("************에러발생************\n위치 : CreateRegApp\n메시지 : {0}", ex.Message));
            }
        }

        public static void RemoveXData(Entity entity, string tag)
        {
            try
            {
                List<TypedValue> lstDatas = null;
                ResultBuffer oXData = entity.XData;
                lstDatas = new List<TypedValue>(oXData.AsArray());

                foreach (TypedValue oData in lstDatas)
                {
                    if (oData.Value.ToString() == tag)
                    {
                        lstDatas.Remove(oData);
                        break;
                    }
                }

                entity.XData = new ResultBuffer(lstDatas.ToArray());
            }
            catch
            {

            }
        }

        [CommandMethod("test1")]
        public void test()
        {
            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            ObjectId oEntId = PromptUtil.GetEntity("soomin");

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                Entity oEnt = tr.GetObject(oEntId, OpenMode.ForWrite) as Entity;

                SetXData(oEnt, "TESTAPP[0]", "Kevin");

                MessageBox.Show(GetXData(oEnt)[0].ToString());

                tr.Commit();
            }
        }

        [CommandMethod("test2")]
        public void test2()
        {
            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            Editor ed = doc.Editor;

            ObjectId oEntId = PromptUtil.GetEntity("soomin");

            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                Entity oEnt = tr.GetObject(oEntId, OpenMode.ForWrite) as Entity;

                RemoveXData(oEnt, "TESTAPP[0]");

                MessageBox.Show(GetXData(oEnt)[0].ToString());

                tr.Commit();
            }
        }
    }
}
