using Coldairarrow.Business.Base_Manage;
using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Coldairarrow.Api.Controllers.Base_Manage
{
    /// <summary>
    /// ϵͳȨ��
    /// </summary>
    /// <seealso cref="Coldairarrow.Api.BaseApiController" />
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_ActionController : BaseApiController
    {
        #region DI

        public Base_ActionController(IBase_ActionBusiness actionBus)
        {
            _actionBus = actionBus;
        }

        IBase_ActionBusiness _actionBus { get; }

        #endregion

        #region ��ȡ

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="id">id����</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<AjaxResult<Base_Action>> GetTheData(string id)
        {
            var theData = _actionBus.GetTheData(id) ?? new Base_Action();

            return Success(theData);
        }

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <param name="parentId">����Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<AjaxResult<List<Base_Action>>> GetPermissionList(string parentId)
        {
            var dataList = _actionBus.GetDataList(new Pagination(), null, parentId, new List<int> { 2 });

            return Success(dataList);
        }

        /// <summary>
        /// ��ȡ�˵����б�
        /// </summary>
        /// <param name="keyword">�ؼ���</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<AjaxResult<List<Base_ActionDTO>>> GetMenuTreeList(string keyword)
        {
            var dataList = _actionBus.GetTreeDataList(keyword, new List<int> { 0, 1 });

            return Success(dataList);
        }

        #endregion

        #region �ύ

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="theData">���������</param>
        [HttpPost]
        public ActionResult<AjaxResult> SaveData(Base_Action theData)
        {
            AjaxResult res;
            if (theData.Id.IsNullOrEmpty())
            {
                theData.Id = IdHelper.GetId();
                theData.CreateTime = DateTime.Now;
                theData.CreatorId = Operator.UserId;
                //theData.CreatorRealName = Operator.Property.RealName;

                res = _actionBus.AddData(theData);
            }
            else
            {
                res = _actionBus.UpdateData(theData);
            }

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="ids">id����,JSON����</param>
        [HttpPost]
        public ActionResult<AjaxResult> DeleteData(string ids)
        {
            var res = _actionBus.DeleteData(ids.ToList<string>());

            return JsonContent(res.ToJson());
        }

        #endregion
    }
}