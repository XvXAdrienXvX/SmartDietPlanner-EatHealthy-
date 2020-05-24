using System;
using System.Collections.Generic;

namespace CoreDietPlan.Models
{
    public partial class ExercisesListTable
    {
        public Guid ExerciseId { get; set; }
        public string ExerciseList { get; set; }
        public int? Lb130 { get; set; }
        public int? Lb155 { get; set; }
        public int? Lb180 { get; set; }
        public int? Lb205 { get; set; }
    }
}
