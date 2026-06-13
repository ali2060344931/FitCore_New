using FitCore.Application.Interfaces.ITrainingProgram;

namespace FitCore.Application.FacadPatterns
{
    public interface ITrainingProgramBuilderFacad
    {
        IAddTrainingDayService AddTrainingDayService { get; }

        IEditTrainingDayService EditTrainingDayService { get; }

        IRemoveTrainingDayService RemoveTrainingDayService { get; }

        IAddTrainingExerciseService AddTrainingExerciseService { get; }

        IEditTrainingExerciseService EditTrainingExerciseService { get; }

        IRemoveTrainingExerciseService RemoveTrainingExerciseService { get; }
    }
}