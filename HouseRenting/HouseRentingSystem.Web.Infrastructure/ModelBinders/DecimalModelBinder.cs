using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HouseRentingSystem.Web.Infrastructure.ModelBinders
{
    internal class DecimalModelBinder : IModelBinder
    {
        public Task BindModelAsync(ModelBindingContext? bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }

            ValueProviderResult valueResult =
                bindingContext.ValueProvider
                    .GetValue(bindingContext.ModelName);
            if (valueResult != ValueProviderResult.None
                && !string.IsNullOrWhiteSpace(valueResult.FirstValue))
            {
                decimal parsedValue = 0m;
                bool binderSuccseded = false;

                try
                {
                    string formDecValue = valueResult.FirstValue;
                    formDecValue = formDecValue.Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
                    formDecValue = formDecValue.Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);

                    parsedValue = Convert.ToDecimal(formDecValue);
                    binderSuccseded = true;
                }
                catch (Exception e)
                {
                    bindingContext.ModelState.AddModelError(bindingContext.ModelName, e, bindingContext.ModelMetadata);
                }

                if (binderSuccseded)
                {
                    bindingContext.Result = ModelBindingResult.Success(parsedValue);
                }
            }
            return Task.CompletedTask;
        }
    }
}
