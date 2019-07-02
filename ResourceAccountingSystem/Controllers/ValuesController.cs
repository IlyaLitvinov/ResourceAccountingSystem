using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RAS.Models.Models;
using System.Threading.Tasks;
using RAS.DB;
using System.Data.Entity;
using System.Text.RegularExpressions;

namespace ResourceAccountingSystem.Controllers
{
    public class ValuesController : ApiController
    {

        #region                               GENERAL DATA

        //*------------------- Регулярное выражение для проверки адреса ----------------------*
        Regex pattern = new Regex(@"^[a-zа-яA-ZА-Я]{1}[#.0-9a-zа-яA-ZА-Я\s,-]+$");
        //*-------------------------------------- End ----------------------------------------*

        #endregion

        #region                           SYNCHRONOUS METHODS 



        #region                        GET HOUSE BY ID

        //*-------------------------- Получение дома по его ID -------------------------------*
        [HttpGet]
        [ActionName("GetHouse")]
        public HttpResponseMessage GetHouse(int id)
        {
            using (var db = new Context())
            {
                var house = db.Houses.Find(id);
                return Request.CreateResponse(HttpStatusCode.OK, house);
            }
        }
        //*-------------------------------------- End ----------------------------------------*

        #endregion

        #region                      GET WATER METER BY ID

        //*-------------------------- Получение счетчикa по его ID ----------------------------*
        [HttpGet]
        [ActionName("GetCount")]
        public HttpResponseMessage GetCount(int id)
        {
            using (var db = new Context())
            {
                var count = db.CountersOfWater.Find(id);
                return Request.CreateResponse(HttpStatusCode.OK, count);
            }
        }
        //*-------------------------------------- End ----------------------------------------*

        #endregion

        #region                  GET A LIST OF NOT CONNECTED METERS

        //*------------------- Получение списка не подключенных счетчиков --------------------*
        [HttpGet]
        [ActionName("GetCountersNull")]
        public HttpResponseMessage GetCounters()
        {
            using (var db = new Context())
            {
                var array = db.Houses.Select(m=> m.CounterOfWaterId).ToArray();
                var counters = db.CountersOfWater.Where(p => !array.Contains(p.Id)).ToList();
                return Request.CreateResponse(HttpStatusCode.OK, counters);
            }
        }
        //*-------------------------------------- End ----------------------------------------*

        #endregion

        #region                          CREATE A NEW METER

        //*-------------------- Создание нового счетчика по ID счетчика ----------------------*
        [HttpPost]
        [ActionName("AddCounter")]
        public void CreateCounter([FromBody]CounterOfWater counter)
        {
            using (var db = new Context())
            {
                var arrayCounts = db.CountersOfWater.Select(m => m.SerialNumber).ToArray();

                if (counter.SerialNumber != null && (counter.Readings >= 0 || counter.Readings == null)  && !arrayCounts.Contains(counter.SerialNumber))
                {
                    db.CountersOfWater.Add(counter);
                    db.SaveChanges();
                }
            }
        }
        //*-------------------------------------- End ----------------------------------------*

        #endregion

        #region                       CREATE A NEW HOUSE

        //*----------------------------- Создание нового дома ---------------------------------* 
        [HttpPost]
        [ActionName("AddHouse")]
        public void CreateHouse([FromBody]House house)
        {
            using (var db = new Context())
            {
                var arrayAdress = db.Houses.Select(m => m.Address).ToArray();
                int[] arrayCounts = db.CountersOfWater.Select(m => m.Id).ToArray();

                if ((house.CounterOfWaterId == null || arrayCounts.Contains(house.CounterOfWaterId.Value)) && house.Address != null && !arrayAdress.Contains(house.Address) && pattern.IsMatch(house.Address))
                {
                    db.Houses.Add(house);
                    db.SaveChanges();
                }
            }
        }
        //*-------------------------------------- End ----------------------------------------*

        #endregion

        #region                      DELETE A HOUSE BY ID

        //*---------------------------- Удаление дома по ID ----------------------------------*
        [HttpDelete]
        [ActionName("DeleteHouse")]
        public void DeleteHouse(int id)
        {
            using (var db = new Context())
            {
                House house = db.Houses.Find(id);
                if (house != null)
                {
                    db.Houses.Remove(house);
                    db.SaveChanges();
                }
            }
        }
        //*-------------------------------------- End ----------------------------------------*

        #endregion

        #region                        DELETE A METER BY ID

        //*---------------------------- Удаление счетчика по Id ------------------------------*
        [HttpDelete]
        [ActionName("DeleteCount")]
        public void DeleteCount(int id)
        {
            using (var db = new Context())
            {
                CounterOfWater count = db.CountersOfWater.Find(id);
                if (count != null)
                {
                    db.CountersOfWater.Remove(count);
                    db.SaveChanges();
                }
            }
        }
        //*-------------------------------------- End ----------------------------------------*

        #endregion

        #region                        EDIT HOUSE BY ID

        //*------------------------- Редактирование дома по Id -------------------------------*
        [HttpPut]
        [ActionName("EditHouse")]
        public void EditHouse(int id, [FromBody]House house)
        {
            using (var db = new Context())
            {            
                var arrayAdress = db.Houses.Where(h => h.Id != house.Id).Select(m => m.Address).ToArray();
                int[] arrayCounts = db.CountersOfWater.Select(m => m.Id).ToArray();

                if ((house.CounterOfWaterId == null || arrayCounts.Contains(house.CounterOfWaterId.Value)) && house.Address != null && !arrayAdress.Contains(house.Address) && pattern.IsMatch(house.Address))
                {
                    db.Entry(house).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
        //*-------------------------------------- End ----------------------------------------*

        #endregion

        #region                          EDIT A METER BY HOUSE ID

        //*------------------------- Редактирование счетчика по Id дома -------------------------------*
        [HttpPut]
        [ActionName("EditCountByHouse")]
        public void EditCountByHouse(int id, [FromBody]CounterOfWater counter)
        {
            using (var db = new Context())
            {
                var house = db.Houses.Where(p => p.Id == id).Include(p => p.CounterOfWater);
                var arrayCounts = db.CountersOfWater.Where(c => c.Id != counter.Id).Select(m => m.SerialNumber).ToArray();

                if (house != null)
                {
                    if (counter.SerialNumber != null && (counter.Readings >= 0 || counter.Readings == null) && !arrayCounts.Contains(counter.SerialNumber))
                    {
                        db.Entry(counter).State = EntityState.Modified;
                        db.SaveChanges();
                    }
                }
            }
        }
        //*-------------------------------------- End ----------------------------------------*

        #endregion

        #region                         EDIT A METER BY ID

        //*------------------------- Редактирование счетчика по Id ---------------------------*
        [HttpPut]
        [ActionName("EditCount")]
        public void EditCount(int id, [FromBody]CounterOfWater counter)
        {
            using (var db = new Context())
            {
                var arrayCounts = db.CountersOfWater.Where(c=> c.Id != counter.Id).Select(m => m.SerialNumber).ToArray();

                if (id != null && id == counter.Id && counter.SerialNumber != null && (counter.Readings >= 0 || counter.Readings == null) && !arrayCounts.Contains(counter.SerialNumber))
                {
                    db.Entry(counter).State = EntityState.Modified;
                    db.SaveChanges();
                }
            }
        }
        //*-------------------------------------- End ----------------------------------------*

        #endregion

        #endregion

        #region                          ASYNCHRONOUS METHODS



        #region                      GET WHOLE DATA ABOUT HOUSES

        //*------------------------ Получение всех данных по домам ----------------------------*
        [HttpGet]
        [ActionName("GetHouses")]
        public async Task<HttpResponseMessage> GetHouses()
        {
            using (var db = new Context())
            {
                var list = await db.Houses.Include(p => p.CounterOfWater)./*Where(house => house.Id == 0).*/ToListAsync();
                return Request.CreateResponse(HttpStatusCode.OK, list);
            }
        }
        //*-------------------------------------- End ----------------------------------------*

        #endregion

        #region                    GET THE MOST CONSUMED HOUSE

        //*-------------------- Получение максимально потребившего дома -----------------------*
        [HttpGet]
        [ActionName("GetMaxHouse")]
        public async Task<HttpResponseMessage> GetMaxHouses()
        {
            using (var db = new Context())
            {
                var MaxHouse = await db.Houses.Include(p => p.CounterOfWater).OrderByDescending(p => p.CounterOfWater.Readings).FirstOrDefaultAsync()/*Max(o => o.CounterOfWater.Readings)*/;
                return Request.CreateResponse(HttpStatusCode.OK, MaxHouse);
            }
        }
        //*-------------------------------------- End ----------------------------------------*

        #endregion

        #region                  GET THE MINIMAL CONSUMED HOUSE

        //*-------------------- Получение минимально потребившего дома ------------------------*
        [HttpGet]
        [ActionName("GetMinHouse")]
        public async Task<HttpResponseMessage> GetMinHouses()
        {
            using (var db = new Context())
            {
                var MinHouse = await db.Houses.Include(p => p.CounterOfWater).OrderBy(p => p.CounterOfWater.Readings).Where(p => p.CounterOfWater != null). FirstOrDefaultAsync()/*Max(o => o.CounterOfWater.Readings)*/;
                return Request.CreateResponse(HttpStatusCode.OK, MinHouse);
            }
        }
        //*-------------------------------------- End ----------------------------------------*

        #endregion

        #endregion


    }
}
