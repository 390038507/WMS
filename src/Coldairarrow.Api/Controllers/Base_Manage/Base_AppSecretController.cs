using Coldairarrow.Business.Base_Manage;
using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace Coldairarrow.Api.Controllers.Base_Manage
{
    /// <summary>
    /// Ӧ����Կ
    /// </summary>
    /// <seealso cref="Coldairarrow.Api.BaseApiController" />
    [Route("Api/Base_Manage/[controller]/[action]")]
    public class Base_AppSecretController : BaseApiController
    {
        #region DI

        public Base_AppSecretController(IBase_AppSecretBusiness appSecretBus)
        {
            _appSecretBus = appSecretBus;
        }

        IBase_AppSecretBusiness _appSecretBus { get; }

        #endregion

        #region ��ȡ

        /// <summary>
        /// ��ȡ�����б�
        /// </summary>
        /// <param name="pagination">��ҳ����</param>
        /// <param name="keyword">�ؼ���</param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult<AjaxResult<List<Base_AppSecret>>> GetDataList(Pagination pagination, string keyword)
        {
            var dataList = _appSecretBus.GetDataList(pagination, keyword);

            return Content(pagination.BuildTableResult_DataGrid(dataList).ToJson());
        }

        #endregion

        #region �ύ

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="theData">���������</param>
        [HttpPost]
        public ActionResult SaveData(Base_AppSecret theData)
        {
            AjaxResult res;
            if (theData.Id.IsNullOrEmpty())
            {
                theData.Id = IdHelper.GetId();

                res = _appSecretBus.AddData(theData);
            }
            else
            {
                res = _appSecretBus.UpdateData(theData);
            }

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// ɾ������
        /// </summary>
        /// <param name="ids">id����,JSON����</param>
        [HttpPost]
        public ActionResult DeleteData(string ids)
        {
            var res = _appSecretBus.DeleteData(ids.ToList<string>());

            return JsonContent(res.ToJson());
        }

        #endregion
    }
}