﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Presentation.Attributes;
using Services.Abstractions;
using Shared;
using Shared.ErrorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController(IServiceManager serviceManager) : ControllerBase
    {
        // Endpoint : public non-static method

        // sort : nameasc [default]
        // sort : namedesc
        // sort : pricedesc

        [HttpGet] // GET : /api/ products
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(ProductResultDto))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [Cache(100)]

        public async Task<ActionResult<PaginationResponse<ProductResultDto>>> GetAllProducts( [FromQuery] ProductSpecificationParamters specParams)
        {
            var result = await serviceManager.ProductService.GetAllProductsAsync(specParams);
            if (result is null) return BadRequest(); // 

            return Ok(result); // 200
        }

        [HttpGet("{id}")] // GET : /api/products/12
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(PaginationResponse<ProductResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(ErrorDetails))]

        public async Task<ActionResult<ProductResultDto>> GetProductById(int id)
        {
          var result = await  serviceManager.ProductService.GetProductByIdAsync(id);

            if(result is null) return NotFound(); // 404

            return Ok(result);
        }

        [HttpGet("brands")] // GET : /api/products/brands
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<BrandResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<BrandResultDto>> GetAllBrands()
        {
           var result = await serviceManager.ProductService.GetAllBrandsAsync();

            if (result is null) return BadRequest(); // 400

            return Ok(result); // 200
          
        }


        [HttpGet("types")] // GET : /api/products/types
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TypeResultDto>))]
        [ProducesResponseType(StatusCodes.Status500InternalServerError, Type = typeof(ErrorDetails))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ErrorDetails))]
        public async Task<ActionResult<TypeResultDto>> GetAllTypes()
        {
            var result = await serviceManager.ProductService.GetAllTypesAsync();

            if (result is null) return BadRequest(); // 400

            return Ok(result); // 200

        }

    }
}
