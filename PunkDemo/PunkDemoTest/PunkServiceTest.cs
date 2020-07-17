using PunkDemo.Models;
using PunkDemo.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;

namespace PunkDemoTest
{
    public class PunkServiceTest
    {
        string db;

        public PunkServiceTest()
        {
            string root = AppDomain.CurrentDomain.BaseDirectory;
            string saveDirectory = Path.Combine(root, "PunkDb");
            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            db = Path.Combine(saveDirectory, "database.json");
        }

        [Fact]
        public void TestSaveRatingFailure()
        {
            //clear db files first
            if(File.Exists(db))
            {
                File.Delete(db);
            }

            //ensure rating is not created
            PunkDemo.Services.PunkService.SaveRating(1, null);
            Assert.False(File.Exists(db));
        }

        [Fact]
        public void TestSaveRatingSuccess()
        {
            //clear db files first
            if (File.Exists(db))
            {
                File.Delete(db);
            }

            //ensure rating is created
            PunkService.SaveRating(1, new UserRating
            {
                Comments = "Test Comment",
                Username = "test@user.com",
                Rating = 3m
            });

            Assert.True(File.Exists(db));
        }

        [Fact]
        public void TestSearchRating()
        {
            string searchResult = @"[{
                ""id"": 1,
                ""name"": ""Buzz"",
                ""tagline"": ""A Real Bitter Experience."",
                ""description"": ""A light, crisp and bitter IPA brewed with English and American hops. A small batch brewed only once.""
            }]";

            ICollection<BeerItem> result =  PunkService.GetRating(searchResult);
            Assert.True(result.Count > 0);

            bool beerExists = result.Any(x => x.Id == 1);
            Assert.True(beerExists);
        }
    }
}
