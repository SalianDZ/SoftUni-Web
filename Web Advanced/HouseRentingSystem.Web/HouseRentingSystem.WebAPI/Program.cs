using HouseRentingSystem.Data;
using HouseRentingSystem.Services.Data;
using HouseRentingSystem.Services.Data.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace HouseRentingSystem.WebAPI
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);
			string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

			builder.Services.AddDbContext<HouseRentingDbContext>(opt => opt.UseSqlServer(connectionString));
			builder.Services.AddScoped<IHouseService, HouseService>();

			// Add services to the container.

			builder.Services.AddControllers();
			// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();
			builder.Services.AddCors(setup => 
			{
				setup.AddPolicy("HouseRentingSystem", policyBuilder =>
				{
					policyBuilder.WithOrigins("https://localhost:7010").AllowAnyHeader().AllowAnyMethod();
				});
			});

			var app = builder.Build();

			// Configure the HTTP request pipeline.
			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseHttpsRedirection();

			app.UseAuthorization();


			app.MapControllers();
			app.UseCors("HouseRentingSystem");

			app.Run();
		}
	}
}