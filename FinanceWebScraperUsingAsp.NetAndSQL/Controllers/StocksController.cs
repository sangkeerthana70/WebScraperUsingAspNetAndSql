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
        private Stock1DbEntities db = new Stock1DbEntities();

        // GET: Stocks
        public ActionResult Index()
        {
            //return View(db.Stocks.ToList());
            return View();
        }

        // GET: Stocks/Details/5
        public ActionResult Details(string id)
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

        // POST: Stocks/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "Symbol,Change,PercentChange,Currency,AverageVolume,MarketCap,Price,SnapshotTime")] Stock stock)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Stocks.Add(stock);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(stock);
        //}

        // POST: Stocks/Create

        public ActionResult Scrape()
        {
            var connString = @"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = FinanceWebScraperUsingAsp.NetAndSQL; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False";

            using (SqlConnection connection = new SqlConnection(connString))
            {
                connection.Open();
                Scraper s = new Scraper("asangeethu@yahoo.com", "@nuk1978");
                var snapShot = s.Scrape();
                foreach (var item in snapShot)
                {
                    SqlCommand insCommand = new SqlCommand("INSERT INTO [Stock] (Symbol, Change, PercentChange, Currency, AverageVolume, MarketCap, Price, SnapshotTime) VALUES (@Symbol, @Change, @PercentChange, @Currency, @AverageVolume, @MarketCap, @Price, @SnapshotTime)", connection);
                    insCommand.Parameters.AddWithValue("@Symbol", item.Symbol.ToString());
                    insCommand.Parameters.AddWithValue("@Change", item.Change.ToString());
                    insCommand.Parameters.AddWithValue("@PercentChange", item.PercentChange.ToString());
                    insCommand.Parameters.AddWithValue("@Currency", item.Currency.ToString());
                    insCommand.Parameters.AddWithValue("@AverageVolume", item.AverageVolume.ToString());
                    insCommand.Parameters.AddWithValue("@MarketCap", item.MarketCap.ToString());
                    insCommand.Parameters.AddWithValue("@Price", item.Price.ToString());
                    insCommand.Parameters.AddWithValue("@SnapshotTime", item.SnapshotTime.ToString());

                    insCommand.ExecuteNonQuery();
                }

                Console.WriteLine("DB updated");
                connection.Close();


                return RedirectToAction(nameof(Index));
            }

        }


        // GET: Stocks/Edit/5
        public ActionResult Edit(string id)
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
        public ActionResult Edit([Bind(Include = "Symbol,Change,PercentChange,Currency,AverageVolume,MarketCap,Price,SnapshotTime")] Stock stock)
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
        public ActionResult Delete(string id)
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

        // POST: Stocks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            Stock stock = db.Stocks.Find(id);
            db.Stocks.Remove(stock);
            db.SaveChanges();
            return RedirectToAction("Index");
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
