using FitCore.Application.Interfaces.ITrainingProgram;

namespace FitCore.Application.FacadPatterns
{
    public interface ITrainingProgramFacad
    {
        IAddTrainingProgramService AddTrainingProgramService { get; }

        IEditTrainingProgramService EditTrainingProgramService { get; }

        IDeleteTrainingProgramService DeleteTrainingProgramService { get; }

        IGetTrainingProgramsService GetTrainingProgramsService { get; }

        IGetTrainingProgramByIdService GetTrainingProgramByIdService { get; }
    }
}