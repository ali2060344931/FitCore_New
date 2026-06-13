using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Interfaces.ITrainingProgram;
using FitCore.Application.Services.TrainingProgramBuilder.Commands.AddTrainingDay;
using FitCore.Application.Services.TrainingProgramBuilder.Commands.AddTrainingExercise;
using FitCore.Application.Services.TrainingProgramBuilder.Commands.EditTrainingDay;
using FitCore.Application.Services.TrainingProgramBuilder.Commands.EditTrainingExercise;
using FitCore.Application.Services.TrainingProgramBuilder.Commands.RemoveTrainingDay;
using FitCore.Application.Services.TrainingProgramBuilder.Commands.RemoveTrainingExercise;

namespace FitCore.Application.Services.TrainingProgramBuilder.TrainingProgramBuilderFacad
{
    public class TrainingProgramBuilderFacad : ITrainingProgramBuilderFacad
    {
        private readonly IDataBaseContext _context;

        public TrainingProgramBuilderFacad(IDataBaseContext context)
        {
            _context = context;
        }

        //Day - Add
        private IAddTrainingDayService _addTrainingDayService;
        public IAddTrainingDayService AddTrainingDayService =>
            _addTrainingDayService ??=
            new AddTrainingDayService(_context);

        //Day - Edit
        private IEditTrainingDayService _editTrainingDayService;
        public IEditTrainingDayService EditTrainingDayService =>
            _editTrainingDayService ??=
            new EditTrainingDayService(_context);

        //Day - Remove
        private IRemoveTrainingDayService _removeTrainingDayService;
        public IRemoveTrainingDayService RemoveTrainingDayService =>
            _removeTrainingDayService ??=
            new RemoveTrainingDayService(_context);

        //Exercise - Add
        private IAddTrainingExerciseService _addTrainingExerciseService;
        public IAddTrainingExerciseService AddTrainingExerciseService =>
            _addTrainingExerciseService ??=
            new AddTrainingExerciseService(_context);

        //Exercise - Edit
        private IEditTrainingExerciseService _editTrainingExerciseService;
        public IEditTrainingExerciseService EditTrainingExerciseService =>
            _editTrainingExerciseService ??=
            new EditTrainingExerciseService(_context);

        //Exercise - Remove
        private IRemoveTrainingExerciseService _removeTrainingExerciseService;
        public IRemoveTrainingExerciseService RemoveTrainingExerciseService =>
            _removeTrainingExerciseService ??=
            new RemoveTrainingExerciseService(_context);
    }
}