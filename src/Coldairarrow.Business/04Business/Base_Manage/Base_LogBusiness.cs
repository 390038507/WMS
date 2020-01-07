﻿using Coldairarrow.Entity.Base_Manage;
using Coldairarrow.Util;
using System;
using System.Collections.Generic;

namespace Coldairarrow.Business.Base_Manage
{
    public class Base_LogBusiness : BaseBusiness<Base_Log>, IBase_LogBusiness, IDependency
    {
        #region 外部接口

        /// <summary>
        /// 获取日志列表
        /// </summary>
        /// <param name="logContent">日志内容</param>
        /// <param name="logType">日志类型</param>
        /// <param name="level">日志级别</param>
        /// <param name="opUserName">操作人用户名</param>
        /// <param name="startTime">开始时间</param>
        /// <param name="endTime">结束时间</param>
        /// <param name="pagination">分页参数</param>
        /// <returns></returns>
        public List<Base_Log> GetLogList(
            Pagination pagination,
            string logContent,
            string logType,
            string level,
            string opUserName,
            DateTime? startTime,
            DateTime? endTime)
        {
            ILogSearcher logSearcher = null;

            if (GlobalSwitch.LoggerType.HasFlag(LoggerType.RDBMS))
                logSearcher = new RDBMSTarget();
            else if (GlobalSwitch.LoggerType.HasFlag(LoggerType.ElasticSearch))
                logSearcher = new ElasticSearchTarget();
            else
                throw new Exception("请指定日志类型为RDBMS或ElasticSearch!");

            return logSearcher.GetLogList(pagination, logContent, logType, level, opUserName, startTime, endTime);
        }

        public void DeleteLog(string logContent, string logType, string level, string opUserName, DateTime? startTime, DateTime? endTime)
        {
            ILogDeleter logDeleter;
            if (GlobalSwitch.LoggerType.HasFlag(LoggerType.RDBMS))
            {
                logDeleter = new RDBMSTarget();
                logDeleter.DeleteLog(logContent, logType, level, opUserName, startTime, endTime);
            }
            if (GlobalSwitch.LoggerType.HasFlag(LoggerType.ElasticSearch))
            {
                logDeleter = new ElasticSearchTarget();
                logDeleter.DeleteLog(logContent, logType, level, opUserName, startTime, endTime);
            }
        }

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }
}