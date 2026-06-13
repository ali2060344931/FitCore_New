using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Interfaces.ITrainingProgram;
using FitCore.Application.Services.Exercises.Commands.AddExercise;
using FitCore.Application.Services.Exercises.Commands.DeleteExercise;
using FitCore.Application.Services.Exercises.Commands.EditExercise;
using FitCore.Application.Services.Exercises.Queries;

namespace FitCore.Application.Services.Exercises.ExerciseFacad
{
    public class ExerciseFacad : IExerciseFacad
    {
        private readonly IDataBaseContext _context;

        public ExerciseFacad(IDataBaseContext context)
        {
            _context = context;
        }

        //View
        private IGetExercisesService _getExercisesService;
        public IGetExercisesService GetExercisesService =>
            _getExercisesService ??=
            new GetExercisesService(_context);

        //Add
        private IAddExerciseService _addExerciseService;
        public IAddExerciseService AddExerciseService =>
            _addExerciseService ??=
            new AddExerciseService(_context);

        //Edit
        private IEditExerciseService _editExerciseService;
        public IEditExerciseService EditExerciseService =>
            _editExerciseService ??=
            new EditExerciseService(_context);

        //Delete
        private IDeleteExerciseService _deleteExerciseService;
        public IDeleteExerciseService DeleteExerciseService =>
            _deleteExerciseService ??=
            new DeleteExerciseService(_context);
    }
}