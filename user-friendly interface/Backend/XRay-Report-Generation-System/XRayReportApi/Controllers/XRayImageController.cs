using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services.IServices;
using Infrastructure.DTO;
using Infrastructure.Services;

namespace XRayReportApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class XRayImageController : ControllerBase
    {
        private readonly IXRayImageService _xRayImageService;

        public XRayImageController(IXRayImageService xRayImageService)
        {
            _xRayImageService = xRayImageService;
        }

        [HttpGet]
        [Route("GetAllXRayImages")]
        public async Task<BaseResponseDTO<IEnumerable<XRayImageDTO>>> GetXRayImages()
        {
            return await _xRayImageService.GetXRayImages();
        }

        [HttpDelete("Delete/{id}")]
        public async Task<BaseResponseDTO<object>> DeleteXRayImage(long id)
        {
            return await _xRayImageService.DeleteXRayImage(id);
        }

        [HttpGet("GetXRayImageById/{id}")]
        public async Task<BaseResponseDTO<XRayImageDTO>> GetXRayImagebyId(long id)
        {
            return await _xRayImageService.GetXRayImageById(id);
        }

        [HttpPost("UploadXRayImage")]
        public async Task<BaseResponseDTO<XRayImageDTO>> UploadXRayImage(IFormFile file, long userId)
        {
            var response = await _xRayImageService.UploadXRayImage(file, userId);
            return response;
        }



        private async Task<string> RunModelInference(string filePath)
        {
            // Add logic to run the model inference
            // This is a placeholder for the actual model inference code
            return await Task.FromResult("Inference result");
        }
    }
}
