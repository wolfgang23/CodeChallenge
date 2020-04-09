using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DapperMockToken;
using DapperMockToken.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private readonly IProductRepository _productRepository;

    public ProductController(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    [HttpGet]
    [Route("{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public ActionResult<Product> GetById(int id)
    {
        var product = _productRepository.GetById(id);
        return Ok(product);
    }
    [HttpPost]
    public ActionResult AddProduct(Product entity)
    {
        _productRepository.AddProduct(entity);
        return Ok(entity);
    }
    [HttpPut("{id}")]
    public ActionResult<Product> Update(Product entity, int id)
    {
        _productRepository.UpdateProduct(entity, id);
        return Ok(entity);
    }
    [HttpDelete("{id}")]
    public ActionResult<Product> Delete(int id)
    {
        _productRepository.RemoveProduct(id);
        return Ok();
    }
}