using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Util;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static Coldairarrow.Entity.Base_Manage.EnumType;

namespace Coldairarrow.Business.Base_Manage
{
    public class Base_ActionBusiness : BaseBusiness<Base_Action>, IBase_ActionBusiness, IDependency
    {
        #region �ⲿ�ӿ�

        public List<Base_Action> GetDataList(Pagination pagination, string keyword)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<Base_Action>();
            if (!keyword.IsNullOrEmpty())
            {
                where = where.And(x => EF.Functions.Like(x.Name, $"%{keyword}%"));
            }

            return q.Where(where).GetPagination(pagination).ToList();
        }

        public List<Base_ActionDTO> GetTreeDataList(string keyword, List<int> types)
        {
            var where = LinqHelper.True<Base_Action>();
            if (!types.IsNullOrEmpty())
                where = where.And(x => types.Contains(x.Type));
            var qList = GetIQueryable().Where(where).ToList();

            var treeList = qList.Select(x => new Base_ActionDTO
            {
                Id = x.Id,
                NeedAction = x.NeedAction,
                Text = x.Name,
                ParentId = x.ParentId,
                Type = x.Type,
                Url = x.Url,
                Value = x.Id
            }).ToList();

            return TreeHelper.BuildTree(treeList);
        }

        /// <summary>
        /// ��ȡָ���ĵ�������
        /// </summary>
        /// <param name="id">����</param>
        /// <returns></returns>
        public Base_Action GetTheData(string id)
        {
            return GetEntity(id);
        }

        /// <summary>
        /// �������
        /// </summary>
        /// <param name="newData">����</param>
        public AjaxResult AddData(Base_Action newData)
        {
            Insert(newData);

            return Success();
        }

        /// <summary>
        /// ��������
        /// </summary>
        public AjaxResult UpdateData(Base_Action theData)
        {
            Update(theData);

            return Success();
        }

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

    public class Base_ActionDTO : TreeModel
    {
        /// <summary>
        /// ����,�˵�=0,ҳ��=1,Ȩ��=2
        /// </summary>
        public Int32 Type { get; set; }

        /// <summary>
        /// �˵���ַ
        /// </summary>
        public String Url { get; set; }

        /// <summary>
        /// �Ƿ���ҪȨ��(��ҳ����Ч)
        /// </summary>
        public bool NeedAction { get; set; }

        public string TypeText { get => ((ActionTypeEnum)Type).ToString(); }
        public string NeedActionText { get => NeedAction ? "��" : "��"; }
        public object children { get => Children; }
    }
}