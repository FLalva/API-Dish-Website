using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Assignment4Final
{
    public class MealModel
    {
        public List<MealIndividualModel> meals { get; set; }
    }

    public class MealIndividualModel
    {
        public int idMeal { get; set; }
        public string strMeal { get; set; }
        public string strCategory { get; set; }
        public string strMealThumb { get; set; }
    }
}
