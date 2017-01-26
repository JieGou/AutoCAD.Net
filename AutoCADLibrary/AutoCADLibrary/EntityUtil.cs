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

        /// <summary>
        /// 객체를 삽입합니다.
        /// </summary>
        /// <param name="entity">삽입할 객체입니다.</param>
        public static ObjectId InsertEntity(Entity entity)
        {
            Document doc = DatabaseUtil.GetActiveDocument();
            Database db = doc.Database;
            ObjectId idResult = ObjectId.Null;

            try
            {
                using (doc.LockDocument())
                {
                    using (Transaction tr = DatabaseUtil.GetTransaction(db))
                    {
                        BlockTableRecord oModelSpace = tr.GetObject(SymbolUtilityServices.GetBlockModelSpaceId(db), OpenMode.ForWrite) as BlockTableRecord;
                        idResult = oModelSpace.AppendEntity(entity);
                        tr.AddNewlyCreatedDBObject(entity, true);

                        tr.Commit();
                    }
                }

                return idResult;
            }
            catch (System.Exception ex)
            {
                return ObjectId.Null;
            }
        }

        /// <summary>
        /// 객체들을 삽입합니다.
        /// </summary>
        /// <param name="entities">삽입할 객체들 입니다.</param>
        public static ObjectIdCollection InsertEntities(IEnumerable<Entity> entities)
        {
            ObjectIdCollection idColl = new ObjectIdCollection();
            foreach (Entity oEnt in entities)
            {
                idColl.Add(InsertEntity(oEnt));
            }

            return idColl;
        }

        /// <summary>
        /// objectIds에 들어있는 객체들로 group 객체를 생성합니다.
        /// </summary>
        /// <param name="groupName">Group의 이름입니다.</param>
        /// <param name="objectIds">객체의 ID들 입니다.</param>
        /// <returns></returns>
        public static ObjectId CreateGroup(string groupName, ObjectIdCollection objectIds)
        {
            Document doc = DatabaseUtil.GetActiveDocument();
            Database db = doc.Database;
            ObjectId idResult = ObjectId.Null;

            try
            {
                using (doc.LockDocument())
                {
                    using (Transaction tr = DatabaseUtil.GetTransaction(db))
                    {
                        DBDictionary oGroupDict = tr.GetObject(db.GroupDictionaryId, OpenMode.ForWrite) as DBDictionary;
                        Group oGrp = null;

                        if (oGroupDict.Contains(groupName))
                        {
                            oGrp = oGroupDict[groupName] as Group;
                            oGrp.Clear();
                        }

                        oGrp = new Group(groupName, false); // 새로운 그룹생성
                        idResult = oGroupDict.SetAt(groupName, oGrp); // 그룹을 그룹DB에 등록하고, ObjectId를 저장한다.
                        tr.AddNewlyCreatedDBObject(oGrp, true); // 트랜잭션에 새로 추가

                        oGrp.InsertAt(0, objectIds); // 그룹에 객체를 추가합니다.

                        oGrp.Selectable = true; // 선택가능하게.

                        tr.Commit();
                    }
                }

                return idResult;
            }
            catch (System.Exception ex)
            {
                return ObjectId.Null;
            }
        }
    }
}
