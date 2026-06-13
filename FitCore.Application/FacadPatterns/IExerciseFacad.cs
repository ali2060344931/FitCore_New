using FitCore.Application.Interfaces.ITrainingProgram;

namespace FitCore.Application.FacadPatterns
{
    public interface IExerciseFacad
    {
        IAddExerciseService AddExerciseService { get; }

        IEditExerciseService EditExerciseService { get; }

        IDeleteExerciseService DeleteExerciseService { get; }

        IGetExercisesService GetExercisesService { get; }
    }
}