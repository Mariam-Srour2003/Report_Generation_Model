using Domain.Models;
using Infrastructure.Data;
using Infrastructure.Repository.Base;
using Infrastructure.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Repository
{
    public class XRayImageRepository : Repository<XRayImage>, IXRayImageRepository
    {
        private readonly DBContext _context;

        public XRayImageRepository(DBContext context) : base(context)
        {
            _context = context;
        }

        public async Task<XRayImage> UploadXRayImage(XRayImage xRayImage)
        {
            await _context.XRayImages.AddAsync(xRayImage);
            await _context.SaveChangesAsync();
            return xRayImage;
        }
    }
}
