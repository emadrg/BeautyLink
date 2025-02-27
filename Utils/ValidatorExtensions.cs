using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;
using System.Reflection;

namespace Utils
{
    public class BaseValidator<TModel>
    {
        private List<PropToBeValidated<TModel>> PropertiesToBeValidated { get; set; }
        private List<Func<TModel, ModelStateDictionary, Task<bool>>> BaseValidationsAsync { get; set; }
        private List<Func<TModel, ModelStateDictionary, bool>> BaseValidations { get; set; }

        public BaseValidator()
        {
            PropertiesToBeValidated = new List<PropToBeValidated<TModel>>();
            BaseValidations = new List<Func<TModel, ModelStateDictionary, bool>>();
            BaseValidationsAsync = new List<Func<TModel, ModelStateDictionary, Task<bool>>>();
        }

        public List<PropToBeValidated<TModel>> GetPropertiesToBeValidated()
        {
            return PropertiesToBeValidated;
        }
        public PropToBeValidated<TModel> ForProperty<TProperty>(Expression<Func<TModel, TProperty>> expression)
        {
            var expressionProvider = new ModelExpressionProvider(new EmptyModelMetadataProvider());

            var nameOfProp = expressionProvider.GetExpressionText(expression);
            var newProp = new PropToBeValidated<TModel>(nameOfProp);
            PropertiesToBeValidated.Add(newProp);
            return newProp;
        }

        public async Task<bool> Validate(TModel model, ModelStateDictionary modelState)
        {
            foreach (var baseValidationAsyncFunc in BaseValidationsAsync)
            {
                await baseValidationAsyncFunc(model, modelState);
            }

            foreach (var baseValidationFunc in BaseValidations)
            {
                baseValidationFunc(model, modelState);
            }

            foreach (var prop in PropertiesToBeValidated)
            {
                await prop.ValidateProp(model, modelState);
            }
            return modelState.IsValid;
        }

        public void AddBaseValidation(Func<TModel, ModelStateDictionary, Task<bool>> baseValidation)
        {
            BaseValidationsAsync.Add(baseValidation);
        }

        public void AddBaseValidation(Func<TModel, ModelStateDictionary, bool> baseValidation)
        {
            BaseValidations.Add(baseValidation);
        }
    }

    public class PropToBeValidated<TModel>
    {
        private string NameOfProp { get; set; }

        private List<Tuple<Func<TModel, bool>, Func<TModel, string>>> Evaluations { get; set; }
        private List<Tuple<Func<TModel, Task<bool>>, Func<TModel, string>>> AsyncEvaluations { get; set; }

        public PropToBeValidated(string nameOfProp)
        {
            NameOfProp = nameOfProp;
            Evaluations = new List<Tuple<Func<TModel, bool>, Func<TModel, string>>>();
            AsyncEvaluations = new List<Tuple<Func<TModel, Task<bool>>, Func<TModel, string>>>();
        }

        public PropToBeValidated<TModel> Check(Func<TModel, bool> condToBeChecked, Func<TModel, string> errorMessageGetter)
        {
            Evaluations.Add(Tuple.Create(condToBeChecked, errorMessageGetter));
            return this;
        }
        public PropToBeValidated<TModel> Check(Func<TModel, Task<bool>> condToBeChecked, Func<TModel, string> errorMessageGetter)
        {
            AsyncEvaluations.Add(Tuple.Create(condToBeChecked, errorMessageGetter));
            return this;
        }

        public PropToBeValidated<TModel> Check(Func<TModel, bool> condToBeChecked, string errorMessage)
        {
            Check(condToBeChecked, new Func<TModel, string>(p => { return errorMessage; }));
            return this;
        }
        public PropToBeValidated<TModel> Check(Func<TModel, Task<bool>> condToBeChecked, string errorMessage)
        {
            Check(condToBeChecked, new Func<TModel, string>(p => { return errorMessage; }));
            return this;
        }

        public async Task ValidateProp(TModel model, ModelStateDictionary modelState)
        {
            Evaluations.ForEach(eval =>
            {
                if (!eval.Item1(model))
                {
                    modelState.TryAddModelError(NameOfProp, eval.Item2(model));
                }
            });
            foreach (var eval in AsyncEvaluations)
            {
                if (!await eval.Item1(model))
                {
                    modelState.TryAddModelError(NameOfProp, eval.Item2(model));
                }
            }
        }
    }

    public static class ValidationExtensionMethods
    {
        public static Dictionary<string, List<string>> GetErrors(this ModelStateDictionary modelState)
        {
            return modelState
                .Where(p => p.Value != null && p.Value.Errors.Any())
                .ToDictionary
                (
                    p => p.Key.Decapitalize(),
                    p => p.Value!.Errors
                        .Select(e => e.ErrorMessage)
                        .ToList()
                );
        }

        public static string GetModelKey<F, T>(Expression<Func<F, T>> expression)
        {
            var expressionProvider = new ModelExpressionProvider(new EmptyModelMetadataProvider());
            return expressionProvider.GetExpressionText(expression);
        }
        public static void AddModelError<TModel>(this ModelStateDictionary modelState, string message)
        {
            modelState.AddModelError<TModel, dynamic>(message);
        }
        public static void AddModelError<TModel, TProperty>(this ModelStateDictionary modelState, string message, Expression<Func<TModel, TProperty>>? expression = null)
        {
            var property = expression != null ? GetModelKey(expression) : "_error";
            modelState.TryAddModelError(property, message);
        }



    }
    public interface IValidate<T>
    {
        Task<bool> Validate(T model, ModelStateDictionary modelState);
    }
    public class ValidationResponse
    {
        public ValidationResponse()
        {
            IsValid = true;
            MainModelErrors = new List<string>();
            PropertiesErrors = new Dictionary<string, List<string>>();
        }
        public bool IsValid { get; set; }
        public List<string> MainModelErrors { get; set; }
        public Dictionary<string, List<string>> PropertiesErrors { get; set; }
    }
    public class ValidatorService
    {
        private IServiceProvider Services;

        public ValidatorService(IServiceProvider services)
        {
            Services = services;
        }

        public async Task<bool> Validate<T>(T model, ModelStateDictionary modelState)
        {
            var converter = Services.GetService<IValidate<T>>();

            return converter != null ? await converter.Validate(model, modelState) : true;
        }
    }
    public static class ValidatorServiceExtentionMethods
    {
        public static IServiceCollection AddValidator(this IServiceCollection services)
        {
            bool exprGoodInterfaces(Type i) =>
                i.Name.Contains(typeof(IValidate<int>).Name);

            var allAssemblies = new List<Assembly> { Assembly.GetCallingAssembly() }
                .Union(
                    Assembly.GetCallingAssembly()
                        .GetReferencedAssemblies()
                        .Select(assName => Assembly.Load(assName)))
                .ToList();

            var allTypes = allAssemblies.SelectMany(a => a.GetTypes()).ToList();

            var allInterfaces = allTypes
                .Where(t => t.GetInterfaces().Any(exprGoodInterfaces))
                .ToList();

            allInterfaces.ForEach(timpl =>
            {
                var allValidators = timpl.GetTypeInfo().GetInterfaces()
                                    .Where(exprGoodInterfaces)
                                    .ToList();

                allValidators.ForEach(m =>
                {
                    services.AddScoped(m, timpl);
                });
            });


            return services.AddScoped<ValidatorService>();
        }
    }
}
