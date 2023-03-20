using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using qNotifier.DAL;
using qNotifier.Models;
using qNotifier.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace qNotifier.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecordsController : ControllerBase
    {
        UserManager<User> _userManager;

        public RecordsController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        // GET: api/<RecordsController>
        [HttpGet]
        [Produces("application/json")]
        public async Task<IActionResult> Get(string selDate)
        {
            int year = DateTime.UtcNow.Year;
            int month = DateTime.UtcNow.Month;
            int day = DateTime.UtcNow.Day;

            var myDateArray = selDate.Split('.');

            bool correctInput = int.TryParse(myDateArray[0], out day);

            if (correctInput)
            {
                correctInput = int.TryParse(myDateArray[1], out month);
            }

            if (correctInput)
            {
                correctInput = int.TryParse(myDateArray[2], out year);
            }

            DateTime myDate = DateTime.UtcNow;

            if (correctInput)
            {
                try
                {
                    myDate = new(year, month, day);
                }
                catch (ArgumentOutOfRangeException e)
                {                    
                    correctInput = false;
                }
            }

            var user = await _userManager.GetUserAsync(User);

            var records = correctInput ? user?.Records?.Where(r => r.AppDateTime?.Date == myDate).OrderBy(r => r.AppDateTime)
                            .Select(r => new
                            {
                                id = r.Id,
                                myDateTime = r.AppDateTime,
                                title = r.Title,
                                status = r.Status.ToString()
                            }) : null;

            return new JsonResult(records);
        }
        
        

        // GET api/<RecordsController>/5
        [HttpGet("{recId}")]
        [Produces("application/json")]
        public async Task<IActionResult> Get(string mydate, string recId)
        {
            bool correctInput = int.TryParse(recId, out int id);
            UserRecord? record = null;

            User user = await _userManager.GetUserAsync(User);

            if (correctInput)
            {
                record = user.Records.Where(r => r.Id == id).FirstOrDefault();
            }

            return new JsonResult(new
            {
                id = record.Id,
                myDateTime = record.AppDateTime,
                title = record.Title,
                status = record.Status.ToString(),
                description = record.Description
            });
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserRecordViewModel myRecord)
        {
            User user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                UserRecord rec = new(user, myRecord.AppDateTime, myRecord.Title, myRecord.Status);

                if (user.Records == null)
                {
                    user.Records = new();
                }

                user.Records.Add(rec);

                var result = await _userManager.UpdateAsync(user);

                if (result.Succeeded)
                {                    
                    return Ok();
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return Ok();
        }

        // PUT api/<RecordsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserRecordViewModel myRecord)
        {

            User user = await _userManager.GetUserAsync(User);

            if (user != null)
            {
                user.Records.Find(r => r.Id == myRecord.Id).AppDateTime = myRecord.AppDateTime;
                user.Records.Find(r => r.Id == myRecord.Id).Title = myRecord.Title;
                user.Records.Find(r => r.Id == myRecord.Id).Status = myRecord.Status;
                user.Records.Find(r => r.Id == myRecord.Id).Description = myRecord.Description;

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok(); ;
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return Ok();
        }

        // DELETE api/<ValuesController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            User user = await _userManager.GetUserAsync(User);
            if (user != null)
            {
                var rec = user.Records.Remove(user.Records.Find(r => r.Id == id));

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok();
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return Ok();
        }
    }
}
