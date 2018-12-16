using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using FinanceWebScraperUsingAsp.NetAndSQL.Models;
using FinanceWebScraperUsingAsp.NetAndSQL.Services;

namespace FinanceWebScraperUsingAsp.NetAndSQL.Controllers
{
    public class StocksController : Controller
    {
        private FinanceWebScraperUsingAspNetAndSqlEntities db = new FinanceWebScraperUsingAspNetAndSqlEntities();

        // GET: Stocks
        public ActionResult Index()
        {
            return View(db.Stocks.ToList());
        }

        // GET: Stocks/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // GET: Stocks/Create
        public ActionResult Create()
        {
            return View();
        }
        /*
        // POST: Stocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Symbol,Change,PercentChange,Currency,AverageVolume,MarketCap,Price,SnapShotTime")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Stocks.Add(stock);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(stock);
        }
        */

       //custom method to run scrape and load into database
       public ActionResult ScrapeYahoo()
       {
            if (ModelState.IsValid)
            {
                Scraper s = new Scraper("asangeethu@yahoo.com", "@nuk1978");
                DateTime myDateTime = DateTime.Now;
                
                //DateTime myDateTime = DateTime.Today;

                var connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FinanceWebScraperUsingAspNetAndSql;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

                using (SqlConnection connection = new SqlConnection(connString))
                {
                    connection.Open();
                    
                    var snapShot = s.Scrape();
                    foreach (var item in snapShot)
                    {
                        
                        SqlCommand insCommand = new SqlCommand("INSERT INTO [Stock] (SnapShotTime, Symbol, Change, PercentChange, Currency, AverageVolume, MarketCap, Price) VALUES (@SnapShotTime, @Symbol, @Change, @PercentChange, @Currency, @AverageVolume, @MarketCap, @Price)", connection);
                        insCommand.Parameters.AddWithValue("@Symbol", item.Symbol.ToString());
                        insCommand.Parameters.AddWithValue("@Change", item.Change.ToString());
                        insCommand.Parameters.AddWithValue("@PercentChange", item.PercentChange.ToString());
                        insCommand.Parameters.AddWithValue("@Currency", item.Currency.ToString());
                        insCommand.Parameters.AddWithValue("@AverageVolume", item.AverageVolume.ToString());
                        insCommand.Parameters.AddWithValue("@MarketCap", item.MarketCap.ToString());
                        insCommand.Parameters.AddWithValue("@Price", item.Price.ToString());
                        insCommand.Parameters.AddWithValue("@SnapShotTime", myDateTime);

                        insCommand.ExecuteNonQuery();
                    }
                    Console.WriteLine("DB updated");
                    connection.Close();
                    
                    //db.Stocks.Add(stock);
                    db.SaveChanges();
                    //return RedirectToAction("Index");
                }
               

            }
            return RedirectToAction("Index");

        }

        // GET: Stocks/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Stock stock = db.Stocks.Find(id);
            if (stock == null)
            {
                return HttpNotFound();
            }
            return View(stock);
        }

        // POST: Stocks/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Symbol,Change,PercentChange,Currency,AverageVolume,MarketCap,Price,SnapShotTime")] Stock stock)
        {
            if (ModelState.IsValid)
            {
                db.Entry(stock).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(stock);
        }

        // GET: Stocks/Delete/5
        public ActionResult Delete(DateTime SnapShotTime)
        //public DateTime Delete(DateTime SnapShotTime)
        {
            /*
            //if (SnapShotTime == null)
            //{
            //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //}
            //Stock stock = db.Stocks.Find(SnapShotTime);
            //if (stock == null)
            //{
            //   return HttpNotFound();
            //}
            */

            //return the delete view which accepts the DateTime as model
                return View(SnapShotTime);
        }

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(DateTime SnapShotTime)
//        public ActionResult DeleteConfirmed(DateTime SnapShotTime)
        public DateTime DeleteConfirmed(DateTime SnapShotTime)
        {
            //Stock stock = db.Stocks.Find(id);
            //db.Stocks.Remove(stock);
            //db.SaveChanges();
            /*
            var connString = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=FinanceWebScraperUsingAspNetAndSql;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            "2018-12-16 13:18:14.3933333"
            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                SqlCommand deleteCommand = new SqlCommand("DELETE FROM [Stock] WHERE SnapShotTime = @SnapShotTime", connection);
                deleteCommand.Parameters.AddWithValue("@SnapShotTime", SnapShotTime);
                deleteCommand.ExecuteNonQuery();
                connection.Close();
            }
            Console.WriteLine("Deleted Records for the Snapshot");
            */
            return SnapShotTime;
            //return RedirectToAction("Index");
        }
                

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
