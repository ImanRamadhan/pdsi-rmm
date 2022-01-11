using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
namespace Kairos.Library.Caching

{
    public class ViewStateCacheManager
    {
        public const string VIEWSTATE_PAGEID = "UniquePageID";

        private System.Web.UI.StateBag stateBag;

        public ViewStateCacheManager(System.Web.UI.StateBag _stateBag)
        {
            this.stateBag = _stateBag;
        }

        #region " Generate Key "
        //-----------------------------------
        protected string SessionID
        {
            get
            {
                return HttpContext.Current.Session.SessionID;
            }
        }
        protected string GetNewUniquePageID()
        {
            return Guid.NewGuid().ToString();
        }

        protected string GetPageID()
        {
            return (string)stateBag[VIEWSTATE_PAGEID];
        }

        public void SetPageID()
        {
            if (stateBag[VIEWSTATE_PAGEID] != null)
                throw new Exception("This page has already been assigned Page ID.");
            stateBag[VIEWSTATE_PAGEID] = GetNewUniquePageID();
        }

        private string GenerateKey(string VariableName)
        {
            string PageID = GetPageID();
            if (string.IsNullOrEmpty(PageID))
                throw new Exception("Page ID has not been set.");

            return string.Format("{0};{1};{2}", SessionID, PageID, VariableName);
        }
        #endregion
        //-----------------------------------

        #region " Cache Method "
        //-----------------------------------
        public object Get(string VariableName)
        {
            object result = null;   
            string Key = GenerateKey(VariableName);
            result = HttpContext.Current.Cache[Key];
            return result;
        }

        public void Set(string VariableName, object Data, int MinuteCacheTime)
        {
            string Key = GenerateKey(VariableName);
            if (Data == null)
                throw new Exception("For validation purpose, no null data is allowed to be inserted to our cache.");
            if (HttpContext.Current.Cache[Key] == null)
                HttpContext.Current.Cache.Insert(Key, Data, null, DateTime.Now.AddMinutes(MinuteCacheTime), System.Web.Caching.Cache.NoSlidingExpiration);
            else
                HttpContext.Current.Cache[Key] = Data;
        }

        public void Remove(string VariableName)
        {
            if (HttpContext.Current.Cache.Count == 0)
                return;
            string Key = GenerateKey(VariableName);
            HttpContext.Current.Cache.Remove(Key);
        }

        public void Clear()
        {
            if (HttpContext.Current.Cache.Count == 0)
                return;

            var enumerator = HttpContext.Current.Cache.GetEnumerator();
            var keysToRemove = new List<String>();
            while (enumerator.MoveNext())
            {
                keysToRemove.Add(enumerator.Key.ToString());
            }

            foreach (string key in keysToRemove)
            {
                HttpContext.Current.Cache.Remove(key);
            }
        }
        #endregion
        //-----------------------------------

    }
}
