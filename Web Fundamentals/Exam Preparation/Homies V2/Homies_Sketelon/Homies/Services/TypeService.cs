using Homies.Data;
using Homies.Models.Type;
using Homies.Services.Contracts;
using Microsoft.EntityFrameworkCore;

namespace Homies.Services
{
    public class TypeService : ITypeService
    {
        private readonly HomiesDbContext context;

        public TypeService(HomiesDbContext context)
        {
            this.context = context;
        }

		public async Task<bool> DoesTypeExistByIdAsync(int id)
		{
			bool result = await context.Types.AnyAsync(t => t.Id == id);
            return result;
		}

		public async Task<IEnumerable<TypeViewModel>> GetAllTypesAsync()
        {
            IEnumerable<TypeViewModel> types = await context.Types.Select(t => new TypeViewModel
            {
                Id = t.Id,
                Name = t.Name
            }).ToListAsync();
            return types;
        }
    }
}
