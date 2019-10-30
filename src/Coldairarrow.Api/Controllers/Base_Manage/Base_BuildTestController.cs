using Coldairarrow.Business.Base_Manage;
using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Coldairarrow.Api.Controllers.Base_Manage
{
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_BuildTestController : BaseApiController
    {
        #region DI

        public Base_BuildTestController(IBase_BuildTestBusiness base_BuildTestBus)
        {
            _base_BuildTestBus = base_BuildTestBus;
        }

        IBase_BuildTestBusiness _base_BuildTestBus { get; }

        #endregion

        #region ��ȡ

        /// <summary>
        /// ��ȡ�б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="condition">��ѯ�ֶ�</param>
        /// <param name="keyword">�ؼ���</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<AjaxResult<List<Base_BuildTest>>> GetDataList(Pagination pagination, string condition, string keyword)
        {
            var dataList = _base_BuildTestBus.GetDataList(pagination, condition, keyword);

            return DataTable(dataList, pagination);
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<AjaxResult<Base_BuildTest>> GetTheData(string id)
        {
            var theData = _base_BuildTestBus.GetTheData(id);

            return Success(theData);
        }

        #endregion

        #region �ύ

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="data">���������</param>
        [HttpPost]
        public ActionResult<AjaxResult> SaveData(Base_BuildTest data)
        {
            AjaxResult res;
            if (data.Id.IsNullOrEmpty())
            {
                data.InitEntity();

                res = _base_BuildTestBus.AddData(data);
            }
            else
            {
                res = _base_BuildTestBus.UpdateData(data);
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
            var res = _base_BuildTestBus.DeleteData(ids.ToList<string>());

            return JsonContent(res.ToJson());
        }

        #endregion
    }
}