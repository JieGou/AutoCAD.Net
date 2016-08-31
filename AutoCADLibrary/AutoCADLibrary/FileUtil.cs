using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoCADLibrary
{
    /// <summary>
    /// 파일 관리에 대한 명령어를 가지고 있는 클래스입니다.
    /// </summary>
    public class FileUtil
    {
        /// <summary>
        /// 파일 저장 대화상자를 띄워 사용자가 지정한 저장경로를 가져옵니다.
        /// </summary>
        /// <param name="title">대화상자의 제목</param>
        /// <param name="initialDirectory">대화상장의 초기 폴더 위치</param>
        /// <param name="filter">대화상자에 표시할 저장 형식</param>
        /// <returns>사용자가 지정한 저장경로입니다.</returns>
        public static string GetSaveFilePath(string title = "", string initialDirectory = "", string filter = "모든 파일|*.*")
        {
            SaveFileDialog oDlg = new SaveFileDialog();

            if (title != "") oDlg.Title = title;
            if (initialDirectory != "") oDlg.InitialDirectory = initialDirectory;
            oDlg.Filter = filter;

            if (oDlg.ShowDialog() == DialogResult.OK) return oDlg.FileName;

            return "";
        }

        /// <summary>
        /// 파일 저장 대화상자를 띄워 사용자가 지정한 파일경로를 가져옵니다.
        /// </summary>
        /// <param name="title">대화상자의 제목</param>
        /// <param name="initialDirectory">대화상장의 초기 폴더 위치</param>
        /// <param name="filter">대화상자에 표시할 파일 형식</param>
        /// <returns>사용자가 지정한 파일경로입니다.</returns>
        public static string GetOpenFilePath(string title = "", string initialDirectory = "", string filter = "모든 파일|*.*")
        {
            OpenFileDialog oDlg = new OpenFileDialog();

            if (title != "") oDlg.Title = title;
            if (initialDirectory != "") oDlg.InitialDirectory = initialDirectory;
            oDlg.Filter = filter;

            if (oDlg.ShowDialog() == DialogResult.OK) return oDlg.FileName;

            return "";
        }
    }
}
