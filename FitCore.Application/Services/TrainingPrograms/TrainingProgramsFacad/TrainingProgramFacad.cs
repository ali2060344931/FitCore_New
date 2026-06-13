using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Interfaces.ITrainingProgram;
using FitCore.Application.Services.TrainingPrograms.Commands.AddTrainingProgram;
using FitCore.Application.Services.TrainingPrograms.Commands.DeleteTrainingProgram;
using FitCore.Application.Services.TrainingPrograms.Commands.EditTrainingProgram;
using FitCore.Application.Services.TrainingPrograms.Queries.GetTrainingProgramById;
using FitCore.Application.Services.TrainingPrograms.Queries.GetTrainingPrograms;

namespace FitCore.Application.Services.TrainingPrograms.TrainingProgramsFacad
{
    public class TrainingProgramFacad : ITrainingProgramFacad
    {
        private readonly IDataBaseContext _context;

        public TrainingProgramFacad(IDataBaseContext context)
        {
            _context = context;
        }

        //View
        private IGetTrainingProgramsService _getTrainingProgramsService;
        public IGetTrainingProgramsService GetTrainingProgramsService =>
            _getTrainingProgramsService ??=
            new GetTrainingProgramsService(_context);

        //ViewById
        private IGetTrainingProgramByIdService _getTrainingProgramByIdService;
        public IGetTrainingProgramByIdService GetTrainingProgramByIdService =>
            _getTrainingProgramByIdService ??=
            new GetTrainingProgramByIdService(_context);

        //Add
        private IAddTrainingProgramService _addTrainingProgramService;
        public IAddTrainingProgramService AddTrainingProgramService =>
            _addTrainingProgramService ??=
            new AddTrainingProgramService(_context);

        //Edit
        private IEditTrainingProgramService _editTrainingProgramService;
        public IEditTrainingProgramService EditTrainingProgramService =>
            _editTrainingProgramService ??=
            new EditTrainingProgramService(_context);

        //Delete
        private IDeleteTrainingProgramService _deleteTrainingProgramService;
        public IDeleteTrainingProgramService DeleteTrainingProgramService =>
            _deleteTrainingProgramService ??=
            new DeleteTrainingProgramService(_context);
    }
}