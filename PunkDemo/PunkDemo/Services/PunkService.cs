using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using PunkDemo.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace PunkDemo.Services
{
    public static class PunkService
    {

        static string databaseFile = "";

        static PunkService()
        {
            string root = AppDomain.CurrentDomain.BaseDirectory;
            string saveDirectory = Path.Combine(root, "PunkDb");
            if(!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            databaseFile = Path.Combine(saveDirectory, "database.json");
        }

        public static void SaveRating(int id, UserRating rating)
        {
            string fileData = "";
            List<UserRating> savedRatings;

            if(rating == null)
            {
                return;
            }

            rating.Id = id;

            if (File.Exists(databaseFile))
            {
                fileData = System.IO.File.ReadAllText(databaseFile);
                savedRatings = JsonConvert.DeserializeObject<List<UserRating>>(fileData);
                if (savedRatings == null)
                {
                    savedRatings = new List<UserRating>();
                }
            }
            else
            {
                savedRatings = new List<UserRating>();
            }

            //check to update
            if (savedRatings.Any(x => x.Id == rating.Id && x.Username == rating.Username))
            {
                UserRating toUpdate = savedRatings.First(x => x.Id == rating.Id && x.Username == rating.Username);
                toUpdate.Rating = rating.Rating;
                toUpdate.Comments = rating.Comments;
            }
            else
            {
                savedRatings.Add(rating);
            }

            fileData = JsonConvert.SerializeObject(savedRatings);
            File.WriteAllText(databaseFile, fileData);
        }
    
        public static ICollection<BeerItem> GetRating(string beerData)
        {
            JArray list = JArray.Parse(beerData);
            if(list == null || list.Count < 1)
            {
                return new List<BeerItem>();
            }

            List<UserRating> userRatings;
            if (File.Exists(databaseFile))
            {
                string fileData = System.IO.File.ReadAllText(databaseFile);
                userRatings = JsonConvert.DeserializeObject<List<UserRating>>(fileData);
                if (userRatings == null)
                {
                    userRatings = new List<UserRating>();
                }
            }
            else
            {
                userRatings = new List<UserRating>();
            }

            List<BeerItem> beers = new List<BeerItem>();

            list.ForEach(obj =>
            {
                int beerId = obj.Value<int>("id");
                string name = obj.Value<string>("name");
                string description = obj.Value<string>("description");

                var ratings = userRatings.Where(x => x.Id == beerId).ToList();
                beers.Add(new BeerItem
                {
                    Id = beerId,
                    Name = name,
                    Description = description,
                    UserRatings = ratings.Select(x => new UserRatingItem
                    {
                        Username = x.Username,
                        Comments = x.Comments,
                        Rating = x.Rating
                    }).ToList()
                });
            });

            return beers;
        }
    }
}