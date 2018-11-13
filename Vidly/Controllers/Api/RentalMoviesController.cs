using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using Vidly.Dtos;
using Vidly.Models;
using System.Data.Entity;

namespace Vidly.Controllers.Api
{
    public class RentalMoviesController : ApiController
    {
        private ApplicationDbContext _context;
        public RentalMoviesController()
        {
            _context = new ApplicationDbContext();
        }


        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        //[Authorize(Roles = RoleName.CanManageMovies)]
        [HttpPost]
        public IHttpActionResult CreateNewRentals(NewRentalDto newRental)
        {


            if (newRental.MovieIds.Count == 0)
                return BadRequest("The given Movies count are invalid.");

            var customer = _context.Customers.Single(c => c.Id == newRental.CustomerId);

            if (customer == null)
                return BadRequest("The given Customer id is invalid");


            var movies = _context.Movies.Where(m => newRental.MovieIds.Contains(m.Id)).ToList();

            if (movies.Count != newRental.MovieIds.Count)
                return BadRequest("One or more movie are invalid");

            //var movies = _context.Movies.Where(m =>m.Id==newRental.MovieIds);

            foreach (var movieObj in movies)
            {
                if (movieObj.NumberAvailable == 0)
                   return BadRequest("Movie : " + movieObj.Name + " has no available copies");

                movieObj.NumberAvailable--;
                var rental = new Rental
                {
                    Customer =customer,
                    Movie = movieObj,
                    DateRented = DateTime.Now,
                    Active = true
                };
                _context.Rentals.Add(rental);
            }
            _context.SaveChanges();
            return Ok();
        }

        [HttpGet]
        public IHttpActionResult GetRentals()
        {
            return  Ok(_context.Rentals.Where(r=>r.Active).Include(r=>r.Customer).Include(r=>r.Movie).ToList().Select(Mapper.Map<Rental,RentalDto>));
        }

        [HttpPut]
        public IHttpActionResult ReturnRental(int id)
        {
            var rental = _context.Rentals.Where(r=>r.Active).Include(r=>r.Movie).SingleOrDefault(r => r.Id == id);

            if (rental == null)
                return NotFound();

            var movie = _context.Movies.Single(m => m.Id == rental.Movie.Id);

            rental.Active = false;
            movie.NumberAvailable++;

            _context.SaveChanges();

            return Ok();


        }
    }
}
    