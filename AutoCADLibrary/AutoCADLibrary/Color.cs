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
    public class Color
    {
        /// <summary>
        /// 색깔을 선택하는 창을 띄워 사용자가 선택한 색을 가져옵니다.
        /// </summary>
        /// <param name="IncludeByLayerByBlock">ByLayer, ByBlock에 대한 선택이 유효한지를 결정합니다.</param>
        /// <returns>Autodesk.AutoCAD.Colors.Color가 리턴됩니다.</returns>
        public AcColor.Color GetColorByDialog(bool IncludeByLayerByBlock = true)
        {
            AcWindow.ColorDialog dlgColor = new AcWindow.ColorDialog();
            dlgColor.IncludeByBlockByLayer = IncludeByLayerByBlock;
            if (dlgColor.ShowDialog() != DialogResult.OK) return null;
            else return dlgColor.Color;
        }

        [Autodesk.AutoCAD.Runtime.CommandMethod("TESTCOLOR")]
        public void Test()
        {
            AcColor.Color colObject = GetColorByDialog(false);

            if (colObject == null || colObject.IsNone)
            {
                MsgBox.Show("색상 선택이 되지 않았습니다.");
            }
            else
            {
                MsgBox.Show(colObject.ToString());
            }
        }
    }
}
