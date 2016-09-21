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
    /// 색깔에 대한 명령을 가지고 있는 클래스입니다.
    /// </summary>
    public class ColorUtil
    {
        /// <summary>
        /// ByLayer에 대한 색깔 값을 리턴합니다.
        /// </summary>
        public static AcColor.Color ByLayer
        {
            get
            {
                return AcColor.Color.FromColorIndex(AcColor.ColorMethod.ByLayer, 256);
            }
        }

        /// <summary>
        /// ByBlock에 대한 색깔 값을 리턴합니다.
        /// </summary>
        public static AcColor.Color ByBlock
        {
            get
            {
                return AcColor.Color.FromColorIndex(AcColor.ColorMethod.ByBlock, 0);
            }
        }

        /// <summary>
        /// 색깔을 선택하는 창을 띄워 사용자가 선택한 색을 가져옵니다.
        /// </summary>
        /// <param name="ShowByLayerByBlock">ByLayer, ByBlock에 대한 선택이 유효한지를 결정합니다.</param>
        /// <returns>Autodesk.AutoCAD.Colors.Color가 리턴됩니다.</returns>
        public static AcColor.Color GetColorByDialog(bool ShowByLayerByBlock = true)
        {
            AcWindow.ColorDialog dlgColor = new AcWindow.ColorDialog();
            dlgColor.IncludeByBlockByLayer = ShowByLayerByBlock;

            if (dlgColor.ShowDialog() == DialogResult.OK) return dlgColor.Color;

            return null;
        }

        /// <summary>
        /// 인덱스에 따른 색깔을 나타냅니다.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static AcColor.Color GetColorByIndex(short index)
        {
            return AcColor.Color.FromColorIndex(AcColor.ColorMethod.ByAci, index);
        }
    }
}
