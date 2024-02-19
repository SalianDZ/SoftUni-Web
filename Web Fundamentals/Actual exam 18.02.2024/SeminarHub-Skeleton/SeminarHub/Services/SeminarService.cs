using Microsoft.EntityFrameworkCore;
using SeminarHub.Data;
using SeminarHub.Data.Models;
using SeminarHub.Models.Seminar;
using SeminarHub.Services.Contracts;
using static SeminarHub.Common.EntityValidations.SeminarValidation;

namespace SeminarHub.Services
{
    public class SeminarService : ISeminarService
    {
        private readonly SeminarHubDbContext context;

        public SeminarService(SeminarHubDbContext context)
        {
            this.context = context;
        }

        public async Task AddSeminarToUserCollectionByIdAsync(int seminarId, string userId)
        {
            SeminarParticipant seminarParticipant = new()
            {
                SeminarId = seminarId,
                ParticipantId = userId
            };

            await context.SeminarsParticipants.AddAsync(seminarParticipant);
            await context.SaveChangesAsync();
        }

        public async Task CreateSeminar(SeminarFormViewModel model, string userId)
        {
            Seminar seminar = new()
            {
                Topic = model.Topic,
                Lecturer = model.Lecturer,
                Details = model.Details,
                DateAndTime = DateTime.Parse(model.DateAndTime),
                Duration = model.Duration,
                CategoryId = model.CategoryId,
                OrganizerId = userId
            };   

            await context.Seminars.AddAsync(seminar);
            await context.SaveChangesAsync();
        }

        public async Task DeleteSeminarByIdAsync(int seminarId)
        {
            Seminar seminar = await context.Seminars.FirstAsync(s => s.Id == seminarId);
            context.Seminars.Remove(seminar);
            await context.SaveChangesAsync();
        }

        public async Task EditSeminarByIdAsync(int seminarId, SeminarFormViewModel model)
        {
            Seminar seminar = await context.Seminars.FirstAsync(s => s.Id == seminarId);
            seminar.Topic = model.Topic;
            seminar.Lecturer = model.Lecturer;
            seminar.Details = model.Details;
            seminar.DateAndTime = DateTime.Parse(model.DateAndTime);
            seminar.Duration = model.Duration;
            seminar.CategoryId = model.CategoryId;

            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<SeminarViewModel>> GetAllJoinedSeminarsByIdAsync(string userId)
        {
            IEnumerable<SeminarViewModel> model = 
                await context.SeminarsParticipants
                .Where(sp => sp.ParticipantId == userId)
                .Include(sp => sp.Seminar)
                .ThenInclude(sp => sp.Category)
                .Select(sp => new SeminarViewModel
            {
                Id = sp.Seminar.Id,
                Topic = sp.Seminar.Topic,
                Lecturer = sp.Seminar.Lecturer,
                DateAndTime = sp.Seminar.DateAndTime.ToString(DateFormat),
                Category = sp.Seminar.Category.Name,
                Organizer = sp.Seminar.Organizer.UserName
            }).ToArrayAsync();

            return model;
        }

        public async Task<IEnumerable<SeminarViewModel>> GetAllSeminarsAsync()
        {
            IEnumerable<SeminarViewModel> allSeminars = 
                await context.Seminars
                .Include(s => s.Organizer)
                .Include(s => s.Category)
                .Select(s => new SeminarViewModel
            {
                Id = s.Id,
                Topic = s.Topic,
                Lecturer = s.Lecturer,
                DateAndTime = s.DateAndTime.ToString(DateFormat),
                Category = s.Category.Name,
                Organizer = s.Organizer.UserName
            }).ToArrayAsync();

            return allSeminars;
        }

        public async Task<SeminarDeleteViewModel> GetSeminarDeleteViewModeForDeleteByAsync(int seminarId)
        {
            Seminar seminar = await context.Seminars.FirstAsync(s => s.Id == seminarId);
            SeminarDeleteViewModel model = new();
            model.Id = seminarId;
            model.Topic = seminar.Topic;
            model.DateAndTime = seminar.DateAndTime;
            return model;
        }

        public async Task<SeminarFormViewModel> GetSeminarFormModelByIdAsync(int seminarId)
        {
            Seminar model = await context
                .Seminars
                .FirstAsync(s => s.Id == seminarId);

            SeminarFormViewModel viewModel = new()
            {
                Topic = model.Topic,
                Lecturer = model.Lecturer,
                DateAndTime = model.DateAndTime.ToString(DateFormat),
                Details = model.Details,
                Duration = model.Duration,
                CategoryId = model.CategoryId
            };

            return viewModel;
        }

        public async Task<SeminarDetailsViewModel> GetSeminarViewModelForDetailsByIdAsync(int seminarId)
        {
            Seminar model = await context
                .Seminars
                .Include(s => s.Category)
                .Include(s => s.Organizer)
                .FirstAsync(s => s.Id == seminarId);

            SeminarDetailsViewModel viewModel = new()
            {
                Id = model.Id,
                Topic = model.Topic,
                Lecturer = model.Lecturer,
                DateAndTime = model.DateAndTime.ToString(DateFormat),
                Details = model.Details,
                Duration = model.Duration,
                Category = model.Category.Name,
                Organizer = model.Organizer.UserName
            };

            return viewModel;
        }

        public async Task<bool> IsSeminarAlreadyInCollectionByIdAsync(int seminarId, string userId)
        {
            bool result = await context.SeminarsParticipants.AnyAsync(sp => sp.SeminarId == seminarId && sp.ParticipantId == userId);
            return result;
        }

        public async Task<bool> IsSeminarValidByIdAsync(int id)
        {
            bool result = await context.Seminars.AnyAsync(s => s.Id == id);
            return result;
        }

        public async Task<bool> IsUserOwnerOfSeminarByIdAsync(int seminarId, string userId)
        {
            Seminar seminar = await context.Seminars.FirstAsync(s => s.Id == seminarId);
            return seminar.OrganizerId == userId;
        }

        public async Task RemoveSeminarFromUserCollectionByIdAsync(int seminarId, string userId)
        {
            SeminarParticipant seminarParticipant = await context.SeminarsParticipants.FirstAsync(sp => sp.SeminarId == seminarId && sp.ParticipantId == userId);
            context.SeminarsParticipants.Remove(seminarParticipant);
            await context.SaveChangesAsync();
        }
    }
}
