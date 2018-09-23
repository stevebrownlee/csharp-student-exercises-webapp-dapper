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
    public class ExerciseController : GenericController {

        private string _simpleSelectColumns = "SELECT Id, Name, Language FROM Exercise";
        public ExerciseController (IConfiguration config) : base (config) { }

        [HttpGet]
        public async Task<IActionResult> Index () => await base.Index<Exercise> (_simpleSelectColumns);

        [HttpGet]
        public async Task<IActionResult> Edit (int? id) => await base.Edit<Exercise> (id, $"{_simpleSelectColumns} WHERE Id = {id}");

        [HttpGet]
        public async Task<IActionResult> DeleteConfirm (int? id) => await base.DeleteConfirm<Exercise>(id, _simpleSelectColumns);

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit ([FromRoute] int id, [Bind ("Id,Name,Language")] Exercise exercise) {
            if (id != exercise.Id) return NotFound ();

            if (ModelState.IsValid) {
                string sql = $@"
                    UPDATE Exercise
                    SET Name     = '{exercise.Name}',
                        Language = '{exercise.Language}'
                    WHERE Id = {id}";

                return await base.Edit(sql);
            } else {
                return new StatusCodeResult (StatusCodes.Status406NotAcceptable);
            }
        }

        [HttpPost, ActionName ("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed (int id) {
            return await base.DeleteConfirmed(id, $"DELETE FROM Student WHERE Id = {id}");
        }

    }
}