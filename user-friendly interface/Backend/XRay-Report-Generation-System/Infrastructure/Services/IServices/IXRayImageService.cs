using Infrastructure.DTO;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services.IServices
{
    public interface IXRayImageService
    {
        Task<BaseResponseDTO<IEnumerable<XRayImageDTO>>> GetXRayImages();
        Task<BaseResponseDTO<object>> DeleteXRayImage(long id);
        Task<BaseResponseDTO<XRayImageDTO>> GetXRayImageById(long id);
        Task<BaseResponseDTO<XRayImageDTO>> UploadXRayImage(IFormFile file, long userId);
    }
}
