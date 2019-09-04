using AutoMapper;
using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Util;
using System.Collections.Generic;
using System.Linq;

namespace Coldairarrow.Business.Base_Manage
{
    public class Base_SysRoleBusiness : BaseBusiness<Base_Role>, IBase_SysRoleBusiness, IDependency
    {
        #region DI

        #endregion

        #region �ⲿ�ӿ�

        public List<Base_RoleDTO> GetDataList(Pagination pagination, string roldId = null, string roleName = null)
        {
            var where = LinqHelper.True<Base_Role>();
            if (!roldId.IsNullOrEmpty())
                where = where.And(x => x.Id == roldId);
            if (!roleName.IsNullOrEmpty())
                where = where.And(x => x.RoleName.Contains(roleName));

            var list = GetIQueryable()
                .Where(where)
                .GetPagination(pagination)
                .ToList()
                .Select(x => Mapper.Map<Base_RoleDTO>(x))
                .ToList();

            return list;
        }

        /// <summary>
        /// ��ȡָ���ĵ�������
        /// </summary>
        /// <param name="id">����</param>
        /// <returns></returns>
        public Base_Role GetTheData(string id)
        {
            return GetEntity(id);
        }

        public Base_RoleDTO GetTheInfo(string id)
        {
            return GetDataList(new Pagination(), id).FirstOrDefault();
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="newData">����</param>
        [DataAddLog(LogType.ϵͳ��ɫ����, "RoleName", "��ɫ")]
        [DataRepeatValidate(new string[] { "RoleName" }, new string[] { "��ɫ��" })]
        public AjaxResult AddData(Base_Role newData)
        {
            Insert(newData);

            return Success();
        }

        /// <summary>
        /// ��������
        /// </summary>
        [DataEditLog(LogType.ϵͳ��ɫ����, "RoleName", "��ɫ")]
        [DataRepeatValidate(new string[] { "RoleName" }, new string[] { "��ɫ��" })]
        public AjaxResult UpdateData(Base_Role theData)
        {
            Update(theData);

            return Success();
        }

        [DataDeleteLog(LogType.ϵͳ��ɫ����, "RoleName", "��ɫ")]
        public AjaxResult DeleteData(List<string> ids)
        {
            Delete(ids);

            return Success();
        }

        #endregion

        #region ˽�г�Ա

        #endregion

        #region ����ģ��

        #endregion
    }
}