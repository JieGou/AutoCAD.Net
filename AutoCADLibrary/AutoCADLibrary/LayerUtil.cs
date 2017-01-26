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
    /// 레이어에 대한 명령들을 가지고 있는 클래스 입니다.
    /// </summary>
    public class LayerUtil
    {
        /// <summary>
        /// 레이어를 추가하는 메소드입니다. 레이어가 존재한다면, 그 레이어의 색깔만 바꿔줍니다.
        /// </summary>
        /// <param name="LayerName">추가할 레이어의 이름입니다.</param>
        /// <param name="AcadColor">추가할 레이어의 색깔입니다.</param>
        /// <returns>추가한 레이어의 ObjectId를 리턴합니다.</returns>
        public static ObjectId AddLayer(string LayerName, AcColor.Color AcadColor = null)
        {
            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            ObjectId oLayerId = ObjectId.Null;

            try
            {
                using (doc.LockDocument())
                {
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        LayerTable oLayers = tr.GetObject(db.LayerTableId, OpenMode.ForWrite) as LayerTable;
                        LayerTableRecord oLayer = null;

                        if (oLayers.Has(LayerName)) // 가지고 있다면 속성을 변경해준다.
                        {
                            oLayer = tr.GetObject(oLayers[LayerName], OpenMode.ForWrite) as LayerTableRecord;
                            if (AcadColor != null) oLayer.Color = AcadColor;
                        }
                        else
                        {
                            oLayer = new LayerTableRecord();
                            oLayer.Name = LayerName;
                            if (AcadColor != null) oLayer.Color = AcadColor;

                            oLayers.Add(oLayer);
                            tr.AddNewlyCreatedDBObject(oLayer, true);
                        }

                        oLayerId = oLayer.Id;

                        tr.Commit();
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.Print(string.Format("************에러발생************\n위치 : AddLayer\n메시지 : {0}", ex.Message));
                return ObjectId.Null;
            }

            return oLayerId;
        }

        /// <summary>
        /// 레이어를 잠그거나, 풉니다.
        /// </summary>
        /// <param name="LayerName">작업할 레이어 이름입니다.</param>
        /// <param name="Lock">잠금여부</param>
        /// <returns>레이어의 ObjectId를 리턴합니다.</returns>
        public static ObjectId LockLayer(string LayerName, bool Lock)
        {
            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            ObjectId oLayerId = ObjectId.Null;

            try
            {
                using (doc.LockDocument())
                {
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        LayerTable oLayers = tr.GetObject(db.LayerTableId, OpenMode.ForWrite) as LayerTable;
                        LayerTableRecord oLayer = null;

                        if (oLayers.Has(LayerName)) // 가지고 있다면 속성을 변경해준다.
                        {
                            oLayer = tr.GetObject(oLayers[LayerName], OpenMode.ForWrite) as LayerTableRecord;
                            oLayer.IsLocked = Lock;
                        }
                        else
                        {
                            oLayer = new LayerTableRecord();
                            oLayer.Name = LayerName;
                            oLayer.IsLocked = Lock;

                            oLayers.Add(oLayer);
                            tr.AddNewlyCreatedDBObject(oLayer, true);
                        }

                        oLayerId = oLayer.Id;

                        tr.Commit();
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.Print(string.Format("************에러발생************\n위치 : AddLayer\n메시지 : {0}", ex.Message));
                return ObjectId.Null;
            }

            return oLayerId;
        }

        /// <summary>
        /// 레이어의 표시 여부를 바꿉니다.
        /// </summary>
        /// <param name="LayerName">작업할 레이어의 이름입니다.</param>
        /// <param name="onOff">레이어를 표시할지 표시하지 않을지 입니다.(true = 표시)</param>
        /// <returns>레이어의 ObjectId를 리턴합니다.</returns>
        public static ObjectId OnOffLayer(string LayerName, bool onOff)
        {
            Document doc = AcadApp.DocumentManager.MdiActiveDocument;
            Database db = doc.Database;
            ObjectId oLayerId = ObjectId.Null;

            try
            {
                using (doc.LockDocument())
                {
                    using (Transaction tr = db.TransactionManager.StartTransaction())
                    {
                        LayerTable oLayers = tr.GetObject(db.LayerTableId, OpenMode.ForWrite) as LayerTable;
                        LayerTableRecord oLayer = null;

                        if (oLayers.Has(LayerName)) // 가지고 있다면 속성을 변경해준다.
                        {
                            oLayer = tr.GetObject(oLayers[LayerName], OpenMode.ForWrite) as LayerTableRecord;
                            oLayer.IsOff = !onOff;
                        }
                        else
                        {
                            return ObjectId.Null;
                        }

                        oLayerId = oLayer.Id;

                        tr.Commit();
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.Print(string.Format("************에러발생************\n위치 : AddLayer\n메시지 : {0}", ex.Message));
                return ObjectId.Null;
            }

            return oLayerId;
        }

        /// <summary>
        /// database가 가지고있는 레이어를 가지고 옵니다.
        /// </summary>
        /// <param name="database">레이어 목록을 가져올 database입니다.</param>
        /// <returns></returns>
        public static List<string> GetLayerList(Database database)
        {
            List<string> lstLayerName = new List<string>();

            try
            {
                using (DatabaseUtil.GetActiveDocument().LockDocument())
                {
                    using (Transaction tr = DatabaseUtil.GetTransaction(database))
                    {
                        LayerTable oLayerIds = tr.GetObject(database.LayerTableId, OpenMode.ForWrite) as LayerTable;

                        foreach (ObjectId idLayer in oLayerIds)
                        {
                            LayerTableRecord oLayer = tr.GetObject(idLayer, OpenMode.ForWrite) as LayerTableRecord;
                            lstLayerName.Add(oLayer.Name);
                        }
                    }
                }

                return lstLayerName;
            }
            catch
            {
                return null;
            }
        }
    }
}
