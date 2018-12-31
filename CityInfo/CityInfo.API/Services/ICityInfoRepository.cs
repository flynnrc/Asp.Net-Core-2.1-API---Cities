using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Services
{
    public interface ICityInfoRepository
    {
        IEnumerable<City> GetCities();
        bool CityExists(int cityId);
        City GetCity(int cityId, bool includePointsOfInterest);
        PointOfInterest GetPointOfInterestsForCity(int cityId, int pointOfInterestId);
        IEnumerable<PointOfInterest> GetPointsOfInterestsForCity(int cityId);
        void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest);
        void DeletePointOfInterest(PointOfInterest pointOfInterest);
        bool Save();


        /*
         * IQueryable vs IEnumerable
         * IQueryable<City> GetCities();
         * IQueryable allows the query to be added on to, which is nice in some situations, however this can leak repo/persistance concerns to the application (which is suppose to be independent) thereby violating the repo pattern.
         * As a guideline, use IEnumerable for straight forward cases, use IQueryable when high variability of queries is unavoidable  
        */
    }
}
