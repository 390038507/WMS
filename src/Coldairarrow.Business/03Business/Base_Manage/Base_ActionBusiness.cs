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

        public List<Base_Action> GetDataList(Pagination pagination, string keyword = null, string parentId = null, List<int> types = null)
        {
            var q = GetIQueryable();
            var where = LinqHelper.True<Base_Action>();
            if (!keyword.IsNullOrEmpty())
            {
                where = where.And(x => EF.Functions.Like(x.Name, $"%{keyword}%"));
            }
            if (!parentId.IsNullOrEmpty())
                where = where.And(x => x.ParentId == parentId);
            if (types?.Count > 0)
                where = where.And(x => types.Contains(x.Type));

            return q.Where(where).GetPagination(pagination).ToList();
        }

        public List<Base_ActionDTO> GetTreeDataList(string keyword, List<int> types)
        {
            var where = LinqHelper.True<Base_Action>();
            if (!types.IsNullOrEmpty())
                where = where.And(x => types.Contains(x.Type));
            var qList = GetIQueryable().Where(where).OrderBy(x => x.Sort).ToList();

            var treeList = qList.Select(x => new Base_ActionDTO
            {
                Id = x.Id,
                NeedAction = x.NeedAction,
                Text = x.Name,
                ParentId = x.ParentId,
                Type = x.Type,
                Url = x.Url,
                Value = x.Id,
                Icon = x.Icon,
                Sort = x.Sort
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

        public AjaxResult SavePermission(string parentId, List<Base_Action> permissionList)
        {
            var existsList = permissionList.Where(x => !x.Id.IsNullOrEmpty()).ToList();
            var insertList = permissionList.Where(x => x.Id.IsNullOrEmpty()).ToList();
            insertList.ForEach(aData =>
            {
                aData.Id = IdHelper.GetId();
                aData.CreateTime = DateTime.Now;
                aData.CreatorId = null;
                aData.CreatorRealName = null;
            });
            var existsIds = existsList.Select(x => x.Id).ToList();
            using (var transaction = BeginTransaction())
            {
                Insert(insertList);
                Update(existsList);
                Delete_Sql(x => x.ParentId == parentId && (!existsIds.Contains(x.Id)));
                var res = transaction.EndTransaction();
                if (res.Success)
                    return Success();
                else
                    throw new Exception("ϵͳ�쳣", res.ex);
            }
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

        public string title { get => Text; }
        public string value { get => Id; }
        public string key { get => Id; }

        /// <summary>
        /// ͼ��
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public int Sort { get; set; }
    }
}