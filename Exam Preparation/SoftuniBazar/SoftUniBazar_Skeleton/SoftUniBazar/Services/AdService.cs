﻿using Microsoft.EntityFrameworkCore;
using SoftUniBazar.Data;
using SoftUniBazar.Models.Ad;
using SoftUniBazar.Services.Interfaces;

namespace SoftUniBazar.Services
{
	public class AdService : IAdService
	{
		private readonly BazarDbContext context;
        public AdService(BazarDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<AllAdsViewModel>> AllAds()
		{
			return await context.Ads
				.Select(ad => new AllAdsViewModel
				{
					Id = ad.Id,
					Name = ad.Name,
					Description = ad.Description,
					ImageUrl = ad.ImageUrl,
					Category = ad.Category.Name,
					Price = ad.Price,
					Owner = ad.Owner.UserName,
					CreatedOn = ad.CreatedOn.ToString("yyyy-MM-dd H:mm")
				}).ToListAsync();
		}
	}
}
