using System;
using DigitalStation.Contract.Dtos;

namespace DigitalStation.Contract.Interfaces;

public interface IProductServices
{
    Task<PaginationResponseDto<List<UpsertProductVariantDto>>>GetAllAsync(ProductQueryDto dto);
    Task<List<UpsertProductVariantDto>> GetRandomProductAsync(int rootCategoryId);
    Task<GetProductDetailsDto> GetByIdAsync(int productId);
    Task<int> UpsertAsync(UpsertProductDto dto);
    Task<bool> UpsertProductVariantAsync(List<UpsertProductVariantDto> dto, int productId);
    Task<GetProductVariantDto>GetDetailsByProductVarIdAsync( int productVariantId);
}
