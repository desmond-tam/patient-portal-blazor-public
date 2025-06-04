using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace PatientPortalApp.Models
{
    public class MustHaveOneElementAttribute<T> : ValidationAttribute where T:ISelectable
    {
        public override bool IsValid(object? value)
        {
            var list = value as IList<T>;
            int count = list?.Count(x => x.Selected) ?? 0;
            return count > 0;
        }
    }
}
