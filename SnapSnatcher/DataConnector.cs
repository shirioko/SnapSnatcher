using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnapSnatcher
{
    internal class DataConnector
    {
        protected SQLiteConnection connection;

        public DataConnector()
        {
            this.connection = new SQLiteConnection("snaps.db");
            this.connection.CreateTable<JsonClasses.Snap>();
            this.connection.CreateTable<AppSettings>();
            this.connection.CreateTable<JsonClasses.Story>();
        }

        public bool AddSnap(JsonClasses.Snap snap)
        {
            JsonClasses.Snap existing = this.connection.Table<JsonClasses.Snap>().Where<JsonClasses.Snap>(u => u.id.Equals(snap.id)).FirstOrDefault<JsonClasses.Snap>();
            if (existing != null)
            {
                return false;
            }
            else
            {
                this.connection.Insert(snap);
                return true;
            }
        }

        public string GetAppSetting(string key)
        {
            AppSettings setting = this.connection.Table<AppSettings>().Where<AppSettings>(u => u.key.Equals(key)).FirstOrDefault<AppSettings>();
            if (setting != null)
            {
                return setting.value;
            }
            return string.Empty;
        }

        public void SetAppSetting(string key, string value)
        {
            AppSettings setting = this.connection.Table<AppSettings>().Where<AppSettings>(u => u.key.Equals(key)).FirstOrDefault<AppSettings>();
            if (setting != null)
            {
                setting.value = value;
                this.connection.Update(setting);
            }
            else
            {
                setting = new AppSettings();
                setting.key = key;
                setting.value = value;
                this.connection.Insert(setting);
            }
        }

        public bool AddStory(JsonClasses.Story story)
        {
            JsonClasses.Story existing = this.connection.Table<JsonClasses.Story>().Where<JsonClasses.Story>(u => u.id.Equals(story.id)).FirstOrDefault<JsonClasses.Story>();
            if (existing != null)
            {
                return false;
            }
            else
            {
                this.connection.Insert(story);
                return true;
            }
        }
    }
}
