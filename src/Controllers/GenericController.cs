using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using Workforce.Models;

namespace Workforce.Controllers {
    public class GenericController : Controller {
        internal readonly IConfiguration _config;

        public GenericController (IConfiguration config) {
            _config = config;
        }

        public IDbConnection Connection {
            get {
                return new SqliteConnection (_config.GetConnectionString ("DefaultConnection"));
            }
        }

        public async Task<IEnumerable<T>> GetAll<T> (string sql) {
            using (IDbConnection conn = Connection) {
                IEnumerable<T> querySet = await conn.QueryAsync<T> (sql);
                return querySet;
            }
        }

        public async Task<T> GetSingle<T> (string sql) {
            using (IDbConnection conn = Connection) {
                var querySet = await conn.QueryFirstAsync<T> (sql);
                return querySet;
            }
        }

        public async Task<IActionResult> Index<T> (string sql) => View (await GetAll<T> (sql));

        public async Task<IActionResult> Edit<T> (int? id, string sql) {
            if (id == null) return NotFound ();
            return View (await GetSingle<T> (sql));
        }

        public async Task<IActionResult> Edit (string sql) {
            using (IDbConnection conn = Connection) {
                int rowsAffected = await conn.ExecuteAsync (sql);
                if (rowsAffected > 0) {
                    return RedirectToAction (nameof (Index));
                }
                throw new Exception ("No rows affected");
            }
        }

        public async Task<IActionResult> DeleteConfirm<T> (int? id, string sql) {
            if (id == null) return NotFound ();
            return View (await GetSingle<T> (sql));
        }

        public async Task<IActionResult> DeleteConfirmed (int id, string sql) {
            using (IDbConnection conn = Connection) {
                int rowsAffected = await conn.ExecuteAsync (sql);
                if (rowsAffected > 0) {
                    return RedirectToAction (nameof (Index));
                }
                throw new Exception ("No rows affected");
            }
        }

    }
}