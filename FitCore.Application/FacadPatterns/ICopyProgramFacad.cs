using FitCore.Application.Contexts;
using FitCore.Application.Services.CopyPrograms;

namespace FitCore.Application.FacadPatterns
{
    //====================================================
    // Interface
    //====================================================
    public interface ICopyProgramFacad
    {
        ICopyNutritionProgramService CopyNutritionProgramService { get; }
        ICopyTrainingProgramService CopyTrainingProgramService { get; }
        IGetNutritionProgramsForCopyService GetNutritionProgramsForCopyService { get; }
        IGetTrainingProgramsForCopyService GetTrainingProgramsForCopyService { get; }
        IGetMembersForCopyService GetMembersForCopyService { get; }
    }

    //====================================================
    // Implementation
    //====================================================
    public class CopyProgramFacad : ICopyProgramFacad
    {
        private readonly IDataBaseContext _context;

        public CopyProgramFacad(IDataBaseContext context)
        {
            _context = context;
        }

        private ICopyNutritionProgramService _copyNutritionProgramService;
        public ICopyNutritionProgramService CopyNutritionProgramService =>
            _copyNutritionProgramService ??= new CopyNutritionProgramService(_context);

        private ICopyTrainingProgramService _copyTrainingProgramService;
        public ICopyTrainingProgramService CopyTrainingProgramService =>
            _copyTrainingProgramService ??= new CopyTrainingProgramService(_context);

        private IGetNutritionProgramsForCopyService _getNutritionProgramsForCopyService;
        public IGetNutritionProgramsForCopyService GetNutritionProgramsForCopyService =>
            _getNutritionProgramsForCopyService ??= new GetNutritionProgramsForCopyService(_context);

        private IGetTrainingProgramsForCopyService _getTrainingProgramsForCopyService;
        public IGetTrainingProgramsForCopyService GetTrainingProgramsForCopyService =>
            _getTrainingProgramsForCopyService ??= new GetTrainingProgramsForCopyService(_context);

        private IGetMembersForCopyService _getMembersForCopyService;
        public IGetMembersForCopyService GetMembersForCopyService =>
            _getMembersForCopyService ??= new GetMembersForCopyService(_context);
    }
}