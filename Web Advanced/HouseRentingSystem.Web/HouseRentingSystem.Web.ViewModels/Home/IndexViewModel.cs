﻿using HouseRentingSystem.Services.Mapping;
using HouseRentingSystem.Data.Models;

namespace HouseRentingSystem.Web.ViewModels.Home
{
    public class IndexViewModel : IMapFrom<Data.Models.House>
    {
        public string Id { get; set; } = null!;

        public string Title { get; set; } = null!;

        public string ImageUrl { get; set; } = null!;
    }
}
