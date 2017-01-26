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
    /// MText, DBText에 대한 명령이 들어있는 클래스입니다.
    /// </summary>
    public class TextUtil
    {
        /// <summary>
        /// Text를 생성합니다.
        /// </summary>
        /// <param name="textString">객체에 표현될 문자열입니다.</param>
        /// <param name="textHeight">객체의 문자높이 입니다.</param>
        /// <param name="textAttachPoint">객체의 삽입점을 조절합니다.</param>
        /// <param name="layerName">객체의 레이어를 지정합니다.</param>
        /// <param name="color">객체의 색깔을 지정합니다.</param>
        /// <param name="textStyleId">객체에 적용할 TextStyle의 ID입니다.</param>
        /// <returns></returns>
        public static DBText CreateDBText(string textString, double textHeight, AttachmentPoint textAttachPoint, string layerName, AcColor.Color color, ObjectId textStyleId)
        {
            try
            {
                DBText oText = new DBText();
                oText.TextStyleId = textStyleId;
                oText.Height = textHeight;
                oText.TextString = textString;
                oText.Layer = layerName;
                oText.Justify = textAttachPoint;
                if (color != null) oText.Color = color;

                return oText;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.Print(string.Format("************에러발생************\n위치 : CreateDBText\n메시지 : {0}", ex.Message));
                return null;
            }
        }

        /// <summary>
        /// TextStyle을 name으로 찾아줍니다.
        /// </summary>
        /// <param name="name">TextStyle의 이름입니다.</param>
        /// <returns>TextStyle의 ObjectId를 리턴합니다.</returns>
        public static ObjectId GetTextStyleId(string name)
        {
            Document doc = DatabaseUtil.GetActiveDocument();
            Database db = doc.Database;
            try
            {
                using (doc.LockDocument())
                {
                    using (Transaction tr = DatabaseUtil.GetTransaction(db))
                    {
                        TextStyleTable oTextStyleTbl = tr.GetObject(db.TextStyleTableId, OpenMode.ForWrite) as TextStyleTable;

                        if (oTextStyleTbl.Has(name)) return oTextStyleTbl[name];

                        return ObjectId.Null;
                    }
                }
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.Print(string.Format("************에러발생************\n위치 : GetTextStyleId\n메시지 : {0}", ex.Message));
                return ObjectId.Null;
            }
        }
    }
}
