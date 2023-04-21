using AdvertisementAPI.Data;
using AdvertisementAPI.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AdvertisementAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    [EnableCors("AllowALl")]
    public class AdvertisementController : ControllerBase
    {
        private readonly ApplicationDbContext _adContext;
        public AdvertisementController(ApplicationDbContext adContext)
        {
            _adContext = adContext;
        }

        // READ ALL 
        /// <summary>
        /// Retrieve all advertisements from the database
        /// </summary>
        /// <returns>
        /// A list of all the advertisements
        /// </returns>
        /// <remarks>
        /// Example end point: GET /api/Advertisement
        /// </remarks>
        /// <response code="200">
        /// Succesfully returned all the advertisements 
        /// </response>
        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<List<AdvertisementDTO>>> GetAll()
        {
            return Ok(_adContext.Advertisements.Select(a => new AdvertisementDTO
            {
                CompanyName = a.CompanyName,
                Slogan = a.Slogan
            }).ToList());
        }


        // READ ONE 
        /// <summary>
        /// Retrieve one advertisement from the database
        /// </summary>
        /// <returns>
        /// An advertisement
        /// </returns>
        /// <remarks>
        /// Example end point: GET /api/Advertisement/{id}
        /// </remarks>
        /// <response code="200">
        /// Succesfully returned the advertisement 
        /// </response>
        /// <response code="400">
        /// Could not find the advertisement
        /// </response>
        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<AdvertisementDTO>> GetOne(int id)
        {
            var ad = _adContext.Advertisements.Find(id);
            if (ad == null) 
            {
                return BadRequest("Advertisement not found.");
            }
            return Ok(new AdvertisementDTO
            {
                CompanyName = ad.CompanyName,
                Slogan = ad.Slogan
            });
        }

        // POST
        /// <summary>
        /// Add an advertisement to the database
        /// </summary>
        /// <returns>
        /// A list of all advertisements
        /// </returns>
        /// <remarks>
        /// Example end point: POST /api/Advertisement/
        /// </remarks>
        /// <response code="200">
        /// Succesfully added the advertisement to the database
        /// </response>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AdvertisementDTO>> PostAdvertisement(AdvertisementDTO advertisement)
        {
            _adContext.Advertisements.Add(new Advertisement
            {
                CompanyName = advertisement.CompanyName,
                Slogan = advertisement.Slogan
            });
            await _adContext.SaveChangesAsync();
            return Ok(_adContext.Advertisements.Select(a => new AdvertisementDTO
            {
                CompanyName= a.CompanyName,
                Slogan = a.Slogan
            }).ToListAsync()) ;
        }

        // UPDATE ALL
        /// <summary>
        /// Updates all the properties of an advertisement
        /// </summary>
        /// <returns>
        /// The updated advertisement
        /// </returns>
        /// <remarks>
        /// Example end point: PUT /api/Advertisement/
        /// </remarks>
        /// <response code="200">
        /// Succesfully updated the advertisement 
        /// </response>
        /// <response code="400">
        /// Could not find the advertisement
        /// </response>
        [HttpPut]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AdvertisementDTO>> UpdateAdvertisement(Advertisement advertisement)
        {
            var adToUpdate = _adContext.Advertisements.Find(advertisement.Id);

            if (adToUpdate == null) return BadRequest("Advertisement not found.");

            adToUpdate.CompanyName = advertisement.CompanyName;
            adToUpdate.Slogan = advertisement.Slogan;

            await _adContext.SaveChangesAsync();
            var returnAd = new AdvertisementDTO 
            { 
                CompanyName = adToUpdate.CompanyName, 
                Slogan = adToUpdate.Slogan
            };
            return Ok(returnAd);
        }

        // DELETE 
        /// <summary>
        /// Delete an advertisement
        /// </summary>
        /// <returns>
        /// A list of all advertisements
        /// </returns>
        /// <remarks>
        /// Example end point: DELETE /api/Advertisement/{id}
        /// </remarks>
        /// <response code="200">
        /// Succesfully deleted the advertisement
        /// </response>
        /// <response code="400">
        /// Could not find the advertisement
        /// </response>
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AdvertisementDTO>> DeleteAdvertisement(int id)
        {
            var ad = await _adContext.Advertisements.FindAsync(id);
            if (ad == null) return BadRequest("Advertisement not found");

            _adContext.Advertisements.Remove(ad);
            await _adContext.SaveChangesAsync();

            return Ok(await _adContext.Advertisements.Select(a => new Advertisement
            {
                CompanyName = a.CompanyName,
                Slogan = a.Slogan
            }).ToListAsync());
        }

        // UPDATE ONE PROPERTY
        /// <summary>
        /// Updates one of the properties of an advertisement
        /// </summary>
        /// <returns>
        /// The updated advertisement
        /// </returns>
        /// <remarks>
        /// Example end point: PUT /api/Advertisement/{id}
        /// </remarks>
        /// <response code="200">
        /// Succesfully updated the advertisement 
        /// </response>
        /// <response code="400">
        /// Could not find the advertisement
        /// </response>
        [HttpPatch]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<AdvertisementDTO>> PatchAdvertisement(JsonPatchDocument ad, int id)
        {
            var adToUpdate = _adContext.Advertisements.Find(id);
            if (adToUpdate == null) return BadRequest("Advertisement not found.");

            ad.ApplyTo(adToUpdate);
            await _adContext.SaveChangesAsync();

            var returnAd = new AdvertisementDTO
            {
                CompanyName = adToUpdate.CompanyName,
                Slogan = adToUpdate.Slogan
            };

            return Ok(returnAd);
        }
    }
}
