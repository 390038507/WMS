﻿using Coldairarrow.Business.PB;
using Coldairarrow.Entity.PB;
using Coldairarrow.IBusiness.DTO;
using Coldairarrow.Util;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Coldairarrow.Api.Controllers.PB
{
    [Route("/PB/[controller]/[action]")]
    public class PB_LocalTrayController : BaseApiController
    {
        #region DI

        public PB_LocalTrayController(IPB_LocalTrayBusiness pB_LocalTrayBus)
        {
            _pB_LocalTrayBus = pB_LocalTrayBus;
        }

        IPB_LocalTrayBusiness _pB_LocalTrayBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<PB_LocalTray>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _pB_LocalTrayBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<PB_LocalTray> GetTheData(IdInputDTO input)
        {
            return await _pB_LocalTrayBus.GetTheDataAsync(input.id);
        }

        [HttpPost]
        public async Task<List<PB_LocalTray>> GetDataListByTypeId(string typeId)
        {
            return await _pB_LocalTrayBus.GetDataListAsync(typeId);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(PB_LocalTray data)
        {
            await _pB_LocalTrayBus.AddDataAsync(data);
        }

        [HttpPost]
        public async Task SaveDatas(PBLocalTrayConditionDTO data)
        {
            var typeId = data.id;
            var targetKeys = data.keys;

            var list = await _pB_LocalTrayBus.GetDataListAsync(typeId);
            var addList = new List<PB_LocalTray>();

            foreach (var i in targetKeys)
            {
                foreach (var s in list)
                {
                    if (i == s.LocalId)
                    {
                        list.Remove(s);
                    }
                }
                addList.Add(new PB_LocalTray()
                {
                    TrayTypeId = typeId,
                    LocalId = i
                });
            }
            var deleteList = list.Select(s => s.LocalId).ToList();

            await _pB_LocalTrayBus.AddDataAsync(addList);
            await _pB_LocalTrayBus.DeleteDataAsync(typeId, deleteList);
        }

        [HttpPost]
        public async Task DeleteData(string typeId, List<string> LocalIds)
        {
            await _pB_LocalTrayBus.DeleteDataAsync(typeId, LocalIds);
        }


        #endregion
    }
}