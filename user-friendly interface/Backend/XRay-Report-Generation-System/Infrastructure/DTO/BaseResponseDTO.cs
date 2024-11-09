using System;

namespace Infrastructure.DTO
{
    public class BaseResponseDTO<T>
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
    }
}
