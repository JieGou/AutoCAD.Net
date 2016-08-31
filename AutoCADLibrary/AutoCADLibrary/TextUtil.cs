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
        public static DBText CreateDBText(string textString, double textHeight, AttachmentPoint textAttachPoint, string layerName, ObjectId textStyleId)
        {
            try
            {
                DBText oText = new DBText();
                oText.TextStyleId = textStyleId;
                oText.Height = textHeight;
                oText.TextString = textString;
                oText.Layer = layerName;
                oText.Justify = textAttachPoint;

                return oText;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.Print(string.Format("************에러발생************\n위치 : CreateDBText\n메시지 : {0}", ex.Message));
                return null;
            }
        }
    }
}
