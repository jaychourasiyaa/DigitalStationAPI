using System;
using System.Linq;
using System.Linq.Expressions;
using DigitalStation.Contract.Dtos;
using DigitalStation.Contract.Interfaces;
using DigitalStation.Contract.Models;
using DigitalStation.Provider.Database;
using Azure;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.ObjectPool;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Crypto.Engines;

namespace DigitalStation.Provider.Services;

public class ProductServices : IProductServices
{
    private readonly dbContext _dbContext;
    public ProductServices(dbContext _dbContext)
    {
        this._dbContext = _dbContext;
    }
    public async Task<GetProductDetailsDto> GetByIdAsync(int productId)
    {
        try
        {
            var productDetailsDto = await _dbContext.Products.Where(x => x.Id == productId)
            .Select(x => new GetProductDetailsDto
            {
                Name = x.Name,
                Description = x.Description!,
                Specifications = x.Specifications!,
                SubCategory = new IdAndNameDto
                {
                    Id = x.Category.Id,
                    Name = x.Category.Name
                },
                Category = new IdAndNameDto
                {
                    Id = x.Category.Parent.Id,
                    Name = x.Category.Parent.Name
                },
                Brand = new IdAndNameDto
                {
                    Id = x.Brand.Id,
                    Name = x.Brand.Name
                }
            }).FirstOrDefaultAsync();
            if (productDetailsDto == null)
            {
                throw new Exception(ResponseCode.InvalidProductId);
            }
            var variantDetails = await _dbContext.ProductVariants.Where(x => x.ProductId == productId)
            .Select(x => new UpsertProductVariantDto
            {
                Id = x.Id,
                Info = x.Info,
                DiscountedPrice = x.DiscountedPrice,
                ActualPrice = x.ActualPrice,
                Images = x.Images!,
                FlipkartUrl = x.FlipkartUrl,
                AmazonUrl = x.AmazonUrl,
                InStock = x.InStock,
                Highlights = x.Highlights
            }).ToListAsync();
            productDetailsDto.VariantDetails = variantDetails;
            return productDetailsDto;
        }
        catch
        {
            throw;
        }
    }
    public async Task<int> UpsertAsync(UpsertProductDto dto)
    {
        using var transaction = await _dbContext.Database.BeginTransactionAsync();
        var savepointCreated = false;

        try
        {
            await CheckProductValidations(dto);

            // 👉 Update flow
            if (dto.Id != null && dto.Id != 0)
            {
                var existingProduct = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == dto.Id);
                if (existingProduct == null)
                    throw new Exception(ResponseCode.InvalidProductId);

                existingProduct.Name = dto.Name;
                existingProduct.Description = dto.Description;
                existingProduct.Specifications = dto.Specifications;
                existingProduct.CategoryId = dto.CategoryId;
                existingProduct.BrandId = dto.BrandId;
                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();
                // if (dto.ProductVariantDto != null)
                // {
                // await UpsertProductVariantAsync(dto.ProductVariantDto, existingProduct.Id);
                // }
                return existingProduct.Id;
            }

            // 👉 Insert flow
            var upsertProduct = Product.Mapper(dto);
            await _dbContext.Products.AddAsync(upsertProduct);
            await _dbContext.SaveChangesAsync();

            // 🔖 Savepoint after base product added
            await transaction.CreateSavepointAsync("AfterProductAdd");
            savepointCreated = true;

            if (dto.ProductVariantDto != null)
            {
                await UpsertProductVariantAsync(dto.ProductVariantDto, upsertProduct.Id);
            }

            await transaction.CommitAsync();
            return upsertProduct.Id;
        }
        catch
        {
            if (dto.Id == null || dto.Id == 0)
            {
                if (savepointCreated)
                {
                    await transaction.RollbackToSavepointAsync("AfterProductAdd");
                }
                else
                {
                    await transaction.RollbackAsync(); // fallback full rollback
                }
            }
            else
            {
                await transaction.RollbackAsync();
            }

            throw;
        }
    }
    public async Task<bool> UpsertProductVariantAsync(List<UpsertProductVariantDto> dto, int productId)
    {
        try
        {
            if (productId <= 0)
            {
                throw new Exception(ResponseCode.IdOutOfRange);
            }
            var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == productId);
            if (product == null)
            {
                throw new Exception(ResponseCode.InvalidProductId);
            }
            foreach (var variantDto in dto)
            {
                if (variantDto.ActualPrice < variantDto.DiscountedPrice)
                {
                    throw new Exception(ResponseCode.PriceConfliction);
                }
                if (variantDto.Id != null && variantDto.Id != 0)
                {
                    var existingVariant = await _dbContext.ProductVariants.FirstOrDefaultAsync(x => x.Id == variantDto.Id);
                    if (existingVariant == null)
                    {
                        throw new Exception(ResponseCode.InvalidProductVariantId);
                    }
                    existingVariant.ActualPrice = variantDto.ActualPrice;
                    existingVariant.DiscountedPrice = variantDto.DiscountedPrice;
                    existingVariant.Images = variantDto.Images;
                    existingVariant.Info = variantDto.Info;
                    existingVariant.FlipkartUrl = variantDto.FlipkartUrl;
                    existingVariant.AmazonUrl = variantDto.AmazonUrl;
                    existingVariant.InStock = variantDto.InStock;
                    existingVariant.Highlights = variantDto.Highlights;
                }
                else
                {
                    var variant = ProductVariant.Mapper(variantDto);
                    variant.ProductId = product.Id;
                    await _dbContext.ProductVariants.AddAsync(variant);
                }
            }
            await _dbContext.SaveChangesAsync();
            return true;

        }
        catch
        {
            throw;
        }
    }
    public async Task<bool> CheckProductValidations(UpsertProductDto dto)
    {
        try
        {
            if (dto.CategoryId != null)
            {
                var category = await _dbContext.Categories.AnyAsync(x => x.Id == dto.CategoryId);
                if (!category)
                {
                    throw new Exception(ResponseCode.InvalidCategoryId);
                }
            }
            if (dto.BrandId != null)
            {
                var Brand = await _dbContext.Brands.AnyAsync(x => x.Id == dto.BrandId);
                if (!Brand)
                {
                    throw new Exception(ResponseCode.InvalidBrandId);
                }
            }
            return true;
        }
        catch
        {
            throw;
        }
    }

    public async Task<List<UpsertProductVariantDto>> GetRandomProductAsync(int rootCategoryId)
    {
        try
        {
            var subCategories = await _dbContext.Categories.Where(x => x.ParentId == rootCategoryId).Select(x => x.Id).ToListAsync();
            if (subCategories == null || subCategories.Count <= 0)
            {
                throw new Exception(ResponseCode.ZeroEntriesFound);
            }
            var products = await _dbContext.ProductVariants.Include(x => x.Product)
            .Where(x => subCategories.Contains(x.Product.CategoryId != null ? (int)x.Product.CategoryId : 0))
            .OrderBy(x => Guid.NewGuid())
            .Take(8)
            .Select(x => new UpsertProductVariantDto
            {
                Id = x.Id,
                Info = x.Product.Name +" "+ x.Info,
                DiscountedPrice = x.DiscountedPrice,
                ActualPrice = x.ActualPrice,
                Images = x.Images!,
                InStock = x.InStock,
                Highlights = x.Highlights,
                ProductId = x.ProductId
            })
            .ToListAsync();
            return products;
        }
        catch
        {
            throw;
        }
    }
    public async Task<PaginationResponseDto<List<UpsertProductVariantDto>>> GetAllAsync(ProductQueryDto dto)
    {
        try
        {
            IQueryable<ProductVariant> queryProducts = Enumerable.Empty<ProductVariant>().AsQueryable();
            var responseDto = new PaginationResponseDto<List<UpsertProductVariantDto>>();
            if (dto.CategoryId != null && dto.CategoryId != 0)
            {
                queryProducts = _dbContext.ProductVariants.Include(x => x.Product)
                .Where(x => x.Product.CategoryId == dto.CategoryId).AsQueryable();
            }
            else
            {
                queryProducts = _dbContext.ProductVariants.Include(x => x.Product).AsQueryable();
            }
            if (dto.BrandIds != null && dto.BrandIds.Count > 0)
            {
                queryProducts = queryProducts.Where(x => dto.BrandIds.Contains(x.Product.BrandId != null ? (int)x.Product.BrandId : 0));
            }
            if (dto.SortBy == "CreatedOn")
            {
                queryProducts = !dto.IsSortDescending ? queryProducts.OrderBy(x => x.CreatedOn) : queryProducts.OrderByDescending(x => x.CreatedOn);
            }
            if (dto.SortBy == "Price")
            {
                queryProducts = !dto.IsSortDescending ? queryProducts.OrderBy(x => x.DiscountedPrice) : queryProducts.OrderByDescending(x => x.DiscountedPrice);
            }
            var products = queryProducts.Select(x => new UpsertProductVariantDto
            {
                Id = x.Id,
                Info = x.Product.Name + " " + x.Info,
                ActualPrice = x.ActualPrice,
                DiscountedPrice = x.DiscountedPrice,
                Images = x.Images!,
                InStock = x.InStock,
                Highlights = x.Highlights,
                ProductId = x.ProductId
            }).AsQueryable();
            if (!string.IsNullOrEmpty(dto.SearchTerm))
            {
                products = products.Where(x => x.Info.Contains(dto.SearchTerm));
            }
            responseDto.Count = products.Count();
            responseDto.Pages = responseDto.Count/dto.PageSize;
            if(responseDto.Count%dto.PageSize != 0 )
            responseDto.Pages++;
            responseDto.Data = await products
            .Skip((dto.PageNumber - 1) * dto.PageSize)
            .Take(dto.PageSize).ToListAsync();
            return responseDto;
        }
        catch
        {
            throw;
        }
    }

    public async Task<GetProductVariantDto> GetDetailsByProductVarIdAsync(int productVariantId)
    {
        try
        {
            var productVariantDetail = await _dbContext.ProductVariants.Where(x => x.Id == productVariantId)
            .Select(x => new GetProductVariantDto
            {
                ProductId = x.Product.Id,
                ActualPrice = x.ActualPrice,
                DiscountedPrice = x.DiscountedPrice,
                ProductVariantName = x.Product.Name + " " + x.Info,
                Images = x.Images
            }).FirstOrDefaultAsync();
            if(productVariantDetail == null)
            {
                throw new Exception(ResponseCode.InvalidProductVariantId);
            }
            return productVariantDetail;
        }
        catch
        {
            throw;
        }
    }
}
