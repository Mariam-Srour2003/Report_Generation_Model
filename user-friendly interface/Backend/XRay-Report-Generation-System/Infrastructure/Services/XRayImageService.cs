using AutoMapper;
using Domain.Models;
using Infrastructure.DTO;
using Infrastructure.Enum;
using Infrastructure.Repository;
using Infrastructure.Repository.IRepository;
using Infrastructure.Services.IServices;
using Infrastructure.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Services
{
    public class XRayImageService : IXRayImageService
    {
        private readonly IXRayImageRepository _xRayImageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<XRayImageService> _logger;
        private readonly string _uploadsFolder = Path.Combine("Uploads");


        public XRayImageService(IXRayImageRepository xRayImageRepository, IUnitOfWork unitOfWork, IMapper mapper, ILogger<XRayImageService> logger)
        {
            _xRayImageRepository = xRayImageRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;

            if (!Directory.Exists(_uploadsFolder))
            {
                Directory.CreateDirectory(_uploadsFolder);
            }
        }

        public async Task<BaseResponseDTO<IEnumerable<XRayImageDTO>>> GetXRayImages()
        {
            try
            {
                var xRayImages = await _xRayImageRepository.GetAll();
                var xRayImageDTOs = _mapper.Map<IEnumerable<XRayImageDTO>>(xRayImages);

                return new BaseResponseDTO<IEnumerable<XRayImageDTO>>
                {
                    StatusCode = (int)StatusCode.Success,
                    Data = xRayImageDTOs
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in GetXRayImages");
                return new BaseResponseDTO<IEnumerable<XRayImageDTO>>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message.ToString(),
                    Data = null
                };
            }
        }

        public async Task<BaseResponseDTO<object>> DeleteXRayImage(long id)
        {
            try
            {
                var existingXRayImage = await _xRayImageRepository.GetById(id);

                if (existingXRayImage != null)
                {
                    _xRayImageRepository.Delete(existingXRayImage);
                    _unitOfWork.Save();

                    return new BaseResponseDTO<object>
                    {
                        StatusCode = (int)StatusCode.Success
                    };
                }

                return new BaseResponseDTO<object>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = "XRayImage not found"
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error in DeleteXRayImage for ID: {id}");
                return new BaseResponseDTO<object>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message.ToString()
                };
            }
        }

        public async Task<BaseResponseDTO<XRayImageDTO>> GetXRayImageById(long id)
        {
            BaseResponseDTO<XRayImageDTO> response = new();
            try
            {
                var xRayImage = await _xRayImageRepository.GetById(id);

                if (xRayImage == null)
                {
                    response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);
                }
                var xRayImageDto = new XRayImageDTO
                {
                    Id = xRayImage.Id,
                    FileName = xRayImage.FileName,
                    ImagePath = xRayImage.ImagePath,
                    UploadedDate = DateTime.Now,
                    UserId = xRayImage.UserId
                };
                response.StatusCode = Convert.ToInt32(StatusCode.Success);

                response.Data = xRayImageDto;
            }
            catch (Exception ex)
            {
                response.Message = ex.Message.ToString();
                response.StatusCode = Convert.ToInt32(StatusCode.BadRequest);

                _logger.LogError(ex.Message.ToString());
            }
            return response;
        }

        public async Task<BaseResponseDTO<XRayImageDTO>> UploadXRayImage(IFormFile file, long userId)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return new BaseResponseDTO<XRayImageDTO>
                    {
                        StatusCode = (int)StatusCode.BadRequest,
                        Message = "Invalid file."
                    };
                }

                // Generate a unique file name
                string fileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                string filePath = Path.Combine(_uploadsFolder, fileName);

                // Save the file to the server
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                // Create a new XRayImage entity
                var xRayImage = new XRayImage
                {
                    FileName = fileName,
                    ImagePath = filePath,
                    UploadedDate = DateTime.UtcNow,
                    UserId = userId
                };

                // Add the entity to the database
                await _xRayImageRepository.Add(xRayImage);
                _unitOfWork.Save(); 

                // Map the entity to DTO
                var xRayImageDTO = _mapper.Map<XRayImageDTO>(xRayImage);

                return new BaseResponseDTO<XRayImageDTO>
                {
                    StatusCode = (int)StatusCode.Success,
                    Data = xRayImageDTO
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in UploadImage");
                return new BaseResponseDTO<XRayImageDTO>
                {
                    StatusCode = (int)StatusCode.BadRequest,
                    Message = ex.Message.ToString()
                };
            }
        }
    }
}