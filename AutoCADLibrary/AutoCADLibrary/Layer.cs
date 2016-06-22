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
    public class Layer
    {
        /// <summary>
        /// 레이어를 추가하는 메소드입니다. 레이어가 존재한다면, 그 레이어의 색깔만 바꿔줍니다.
        /// </summary>
        /// <param name="LayerName">추가할 레이어의 이름입니다.</param>
        /// <param name="AcadColor">추가할 레이어의 색깔입니다.</param>
        /// <returns>추가한 레이어의 ObjectId를 리턴합니다.</returns>
        public static ObjectId AddLayer(string LayerName, AcColor.Color AcadColor = null)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
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
    }
}
