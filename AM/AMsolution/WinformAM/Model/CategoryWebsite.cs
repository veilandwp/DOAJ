using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinformAM.Model
{
    public class CategoryWebsite
    {
        public CategoryWebsite(int categoryWebsiteID, string CategoryLevelOne, string categoryLevelTwo, string categoryLevelThree, string website, int websiteCount)
        {
            this.categoryWebsiteID = categoryWebsiteID;
            this.categoryLevelOne = categoryLevelOne;
            this.categoryLevelTwo = categoryLevelTwo;
            this.categoryLevelThree = categoryLevelThree;
            this.websiteCount = websiteCount;
            this.website = website;
        }
        private int categoryWebsiteID;

        public int CategoryWebsiteID
        {
            get { return categoryWebsiteID; }
            set { categoryWebsiteID = value; }
        }
        private string categoryLevelOne;

        public string CategoryLevelOne
        {
            get { return categoryLevelOne; }
            set { categoryLevelOne = value; }
        }
        private string categoryLevelTwo;

        public string CategoryLevelTwo
        {
            get { return categoryLevelTwo; }
            set { categoryLevelTwo = value; }
        }
        private string categoryLevelThree;

        public string CategoryLevelThree
        {
            get { return categoryLevelThree; }
            set { categoryLevelThree = value; }
        }
        private string website;

        public string Website
        {
            get { return website; }
            set { website = value; }
        }
        private int websiteCount;

        public int WebsiteCount
        {
            get { return websiteCount; }
            set { websiteCount = value; }
        }
    }
}
