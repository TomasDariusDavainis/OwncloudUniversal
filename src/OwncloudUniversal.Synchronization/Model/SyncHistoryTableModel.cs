﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLitePCL;

namespace OwncloudUniversal.Synchronization.Model
{
    public class SyncHistoryTableModel : AbstractTableModelBase<SyncHistoryEntry, long>
    {
        private SyncHistoryTableModel() { }

        private static SyncHistoryTableModel instance;

        public static SyncHistoryTableModel GetDefault()
        {
            lock (typeof(SyncHistoryTableModel))
            {
                if (instance == null)
                    instance = new SyncHistoryTableModel();
                return instance;
            }
        }

        protected override SyncHistoryEntry CreateInstance(ISQLiteStatement query)
        {
            var entry = new SyncHistoryEntry();
            entry.Id = (long) query["Id"];
            DateTime date;
            if (!DateTime.TryParse((string)query["CreateDate"], out date))
                date = DateTime.MinValue;
            entry.CreateDate = date;
            entry.Message = (string) query["Message"];
            SyncResult result;
            Enum.TryParse((string) query["Result"], out result);
            entry.Result = result;
            entry.EntityId = (string) query["EntityId"];
            entry.ContentType = (string) query["ContentType"];
            return entry;
        }

        protected override string GetSelectItemQuery()
        {
            return
                "SELECT Id, CreateDate, Message, Result, EntityId, ContentType From SyncHistory where Id = @Id";
        }

        protected override void BindSelectItemQuery(ISQLiteStatement query, long key)
        {
            query.Bind(1, key);
        }

        protected override void BindSelectItemQuery(ISQLiteStatement query, string itemId)
        {
            throw new NotImplementedException();
        }

        protected override string GetInsertItemQuery()
        {
            return
                "INSERT INTO SyncHistory (CreateDate, Message, Result, EntityId, ContentType) VALUES (@CreateDate, @Message, @Result, @EntityId, @ContentType)";
        }

        protected override void BindInsertItemQuery(ISQLiteStatement query, SyncHistoryEntry item)
        {
            query.Bind(1, SQLite.DateTimeHelper.DateTimeSQLite(item.CreateDate));
            query.Bind(2, item.Message);
            query.Bind(3, item.Result.ToString());
            query.Bind(4, item.EntityId);
            query.Bind(5, item.ContentType);
        }

        protected override string GetUpdateItemQuery()
        {
            return
                "UPDATE SyncHistory SET CreateDate = @CreateDate, Message = @Message, Result = @Result, EntityId = @EntityId, ContentType = @ContentType where Id = @Id";
        }

        protected override void BindUpdateItemQuery(ISQLiteStatement query, SyncHistoryEntry item, long key)
        {
            query.Bind(1, key);
            query.Bind(2, SQLite.DateTimeHelper.DateTimeSQLite(item.CreateDate));
            query.Bind(3, item.Message);
            query.Bind(4, item.Result.ToString());
            query.Bind(5, item.EntityId);
            query.Bind(6, item.ContentType);
        }

        protected override string GetDeleteItemQuery()
        {
            return "DELETE FROM SyncHistory Where Id = @Id";
        }

        protected override void BindDeleteItemQuery(ISQLiteStatement query, long key)
        {
            query.Bind(1, key);
        }

        protected override string GetSelectAllQuery()
        {
            return
                "SELECT Id, CreateDate, Message, Result, EntityId, ContentType From SyncHistory ORDER BY Id DESC";
        }

        protected override void BindSelectAllQuery(ISQLiteStatement query)
        {
            //nothing to do
        }

        protected override string GetLastInsertRowIdQuery()
        {
            return "SELECT last_insert_rowid() FROM SyncHistory";
        }

        protected override string GetSelectByEntityIdQuery()
        {
            throw new NotImplementedException();
        }

        protected override void BindSelectByPathQuery(ISQLiteStatement query, string path, long folderId)
        {
            throw new NotImplementedException();
        }
    }
}
