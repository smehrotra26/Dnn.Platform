﻿// 
// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the MIT License. See LICENSE file in the project root for full license information.
// 
using System;
using System.Collections.Generic;
using System.Linq;
using DotNetNuke.Common;
using DotNetNuke.Common.Utilities;
using DotNetNuke.Data;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework;
using DotNetNuke.Services.Localization;

namespace DotNetNuke.Entities.Tabs.TabVersions
{
    public class TabVersionController: ServiceLocator<ITabVersionController, TabVersionController>, ITabVersionController
    {
        private static readonly DataProvider Provider = DataProvider.Instance();

        #region Public Methods
        public TabVersion GetTabVersion(int tabVersionId, int tabId, bool ignoreCache = false)
        {
            return GetTabVersions(tabId, ignoreCache).SingleOrDefault(tv => tv.TabVersionId == tabVersionId);
        }
        
        public IEnumerable<TabVersion> GetTabVersions(int tabId, bool ignoreCache = false)
        {
            //if we are not using the cache, then remove from cache and re-add loaded items when needed later
            var tabCacheKey = GetTabVersionsCacheKey(tabId);
            if (ignoreCache || Host.Host.PerformanceSetting == Globals.PerformanceSettings.NoCaching)
            {
                DataCache.RemoveCache(tabCacheKey);
            }
            var tabVersions = CBO.Instance.GetCachedObject<List<TabVersion>>(new CacheItemArgs(tabCacheKey,
                                                                    DataCache.TabVersionsCacheTimeOut,
                                                                    DataCache.TabVersionsCachePriority),
                                                            c => CBO.FillCollection<TabVersion>(Provider.GetTabVersions(tabId)),
                                                            false
                                                            );

            return tabVersions;
        }
        public void SaveTabVersion(TabVersion tabVersion)
        {
            SaveTabVersion(tabVersion, tabVersion.CreatedByUserID, tabVersion.LastModifiedByUserID);
        }

        public void SaveTabVersion(TabVersion tabVersion, int createdByUserID)
        {
            SaveTabVersion(tabVersion, createdByUserID, createdByUserID);
        }

        public void SaveTabVersion(TabVersion tabVersion, int createdByUserID, int modifiedByUserID)
        {
            tabVersion.TabVersionId = Provider.SaveTabVersion(tabVersion.TabVersionId, tabVersion.TabId, tabVersion.TimeStamp, tabVersion.Version, tabVersion.IsPublished, createdByUserID, modifiedByUserID);
            ClearCache(tabVersion.TabId);
        }

        public TabVersion CreateTabVersion(int tabId, int createdByUserID, bool isPublished = false)
        {
            var lastTabVersion = GetTabVersions(tabId).OrderByDescending(tv => tv.Version).FirstOrDefault();
            var newVersion = 1;

            if (lastTabVersion != null)
            {
                if (!lastTabVersion.IsPublished && !isPublished)
                {
                    throw new InvalidOperationException(String.Format(Localization.GetString("TabVersionCannotBeCreated_UnpublishedVersionAlreadyExists", Localization.ExceptionsResourceFile)));
                }
                newVersion = lastTabVersion.Version + 1;
            }
            
            var tabVersionId = Provider.SaveTabVersion(0, tabId, DateTime.UtcNow, newVersion, isPublished, createdByUserID, createdByUserID);
            ClearCache(tabId);

            return GetTabVersion(tabVersionId, tabId);
        }

        public void DeleteTabVersion(int tabId, int tabVersionId)
        {
            Provider.DeleteTabVersion(tabVersionId);
            ClearCache(tabId);
        }

        public void DeleteTabVersionDetailByModule(int moduleId)
        {
            Provider.DeleteTabVersionDetailByModule(moduleId);
        }
        #endregion

        #region Private Methods
        private void ClearCache(int tabId)
        {
            DataCache.RemoveCache(GetTabVersionsCacheKey(tabId));
        }

        private static string GetTabVersionsCacheKey(int tabId)
        {
            return string.Format(DataCache.TabVersionsCacheKey, tabId);
        }

        #endregion

        protected override Func<ITabVersionController> GetFactory()
        {
            return () => new TabVersionController();
        }
    }
}
