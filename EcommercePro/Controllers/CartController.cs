using EcommercePro.DTO;
using EcommercePro.Models;
using EcommercePro.Repositiories;
using EcommercePro.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;


namespace EcommercePro.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ICart _cartRepository;

        public CartController(ICart cartRepository)
        {
            _cartRepository = cartRepository;
        }

        // GET: api/Cart
        [HttpGet]
        public ActionResult<IEnumerable<CartData>> GetCarts()
        {
            var carts = _cartRepository.GetAllCartsWithProductDetails()
                .Select(c => new CartData
                {
                    Id = c.Id,
                    Quentity = c.Quantity,
                    productId = c.productId,
                    UserId = c.UserId,
                    CreatedDate = c.CreatedDate,
                    ProductName = c.product.Name,
                    ProductImage = c.product.ImagePath,  // Assuming Product has an Image property
                    ProductPrice = c.product.Price   // Assuming Product has a Price property
                }).ToList();

            return Ok(carts);
        }

        // GET: api/Cart/5
        [HttpGet("{id}")]
        public ActionResult<CartData> GetCart(int id)
        {
            var cart = _cartRepository.GetCartWithProductDetails(id);

            if (cart == null)
            {
                return NotFound();
            }

            var cartData = new CartData
            {
                Id = cart.Id,
                Quentity = cart.Quantity,
                productId = cart.productId,
                UserId = cart.UserId,
                CreatedDate = cart.CreatedDate,
                ProductName = cart.product.Name,
                ProductImage = cart.product.ImagePath,
                ProductPrice = cart.product.Price
            };

            return Ok(cartData);
        }

        // POST: api/Cart
        [HttpPost]
        public ActionResult<CartData> PostCart(CartData cartData)
        {
            if (cartData == null || cartData.productId == 0 || cartData.UserId == " ")
            {
                return BadRequest("Invalid cart data");
            }

            var cart = new Cart
            {
                Quantity = cartData.Quentity,
                productId = cartData.productId,
                UserId = cartData.UserId,
                CreatedDate = System.DateOnly.FromDateTime(DateTime.UtcNow)
            };
            _cartRepository.Add(cart);

            cartData.Id = cart.Id;  // Update the DTO with the new ID

            // Retrieve the product details for the created cart item
            var createdCart = _cartRepository.GetCartWithProductDetails(cart.Id);
            cartData.ProductName = createdCart.product.Name;
            cartData.ProductImage = createdCart.product.ImagePath;
            cartData.ProductPrice = createdCart.product.Price;

            return CreatedAtAction("GetCart", new { id = cartData.Id }, cartData);
        }
        // PUT: api/Cart/5
        [HttpPut("{id}")]
        public IActionResult PutCart(int id, CartData cartData)
        {
            if (id != cartData.Id)
            {
                return BadRequest();
            }

            var cart = _cartRepository.Get(id);
            if (cart == null)
            {
                return NotFound();
            }

            cart.Quantity = cartData.Quentity;

            if (!_cartRepository.Update(id, cart))
            {
                return NotFound();
            }

            return NoContent();
        }

        // DELETE: api/Cart/5
        [HttpDelete("{id}")]
        public IActionResult DeleteCart(int id)
        {
            if (!_cartRepository.Delete(id))
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}