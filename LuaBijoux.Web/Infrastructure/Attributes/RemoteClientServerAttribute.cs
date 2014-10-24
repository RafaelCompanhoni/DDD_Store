using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Web.Mvc;

/* Este atributo é utilizado para forçar as validações remotas a serem executas também pelo servidor, para o caso do JavaScript estar desativado */

namespace LuaBijoux.Web.Infrastructure.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RemoteClientServerAttribute : RemoteAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            // O comando abaixo obtém o controller que está executando o atributo de validação. Entenda em etapas:
            //      Assembly.GetCallingAssembly() : retorna o assembly onde está sendo utilizado o validador (ex: LuaBijoux.Web)
            //      .GetTypes() : retorna todos os tipos (classes, interfaces, structs, etc) definidos no assembly 
            //      .FirstOrDefault(type => type.Name.ToLower() == string.Format("{0}Controller", this.RouteData["controller"].ToString().ToLower())) : Esta filtragem faz o seguinte:
            //          1. Percorre todos os tipos do assembly 
            //          2. Busca o nome do controller que está disparando o atributo com RouteDate["controller"] -- MAS -- este nome vem sem o sufixo 'Controller'
            //          3. Anexa o sufixo e cria a string [NOME-CONTROLLER]Controller para então a utilizar para achar o controller com a query LINQ FirstOrDerfault
            Type controller =
                Assembly.GetExecutingAssembly()
                    .GetTypes()
                    .FirstOrDefault(type => type.Name.ToLower() == string.Format("{0}Controller", this.RouteData["controller"].ToString()).ToLower());
            
            if (controller != null)
            {
                // Obtém o action method responsável pela RemoteValidation que deve ser executada pelo servidor
                MethodInfo validationAction = controller.GetMethods().FirstOrDefault(method => method.Name.ToLower() == this.RouteData["action"].ToString().ToLower());

                if (validationAction != null)
                {
                    // obtém os parâmetros a serem passados para o action method (incluindo os additional fields)
                    List<object> propValues = new List<object> {value};

                    // instancia o controller
                    if (!(string.IsNullOrWhiteSpace(this.AdditionalFields) || string.IsNullOrEmpty(this.AdditionalFields)))
                    {
                        string[] additionalFields = this.AdditionalFields.Split(',');

                        foreach (string additionalField in additionalFields)
                        {
                            PropertyInfo prop = validationContext.ObjectType.GetProperty(additionalField);
                            if (prop != null)
                            {
                                object propValue = prop.GetValue(validationContext.ObjectInstance, null);
                                propValues.Add(propValue);
                            }
                        }
                    }

                    object controllerInstance = DependencyResolver.Current.GetService(controller);

                    // executa o action method que realiza a validação e passa os parâmetros que estão sendo validados
                    object result = validationAction.Invoke(controllerInstance, propValues.ToArray());

                    // a resposta do action method de validação deve ser um booleano no formato JsonResult (pois serve também para a validação client-side)
                    if (result is JsonResult)
                    {
                        object jsonData = ((JsonResult) result).Data;

                        if (jsonData is bool)
                        {
                            // caso a validação não seja 'aprovada' retorna a mensagem de erro definida no atributo
                            return (bool) jsonData ? ValidationResult.Success : new ValidationResult(this.ErrorMessage);
                        }
                    }
                }
            }

            return new ValidationResult(this.ErrorMessage);
        }

        public RemoteClientServerAttribute(string routeName) : base(routeName)
        {
            
        }

        public RemoteClientServerAttribute(string action, string controller) : base(action, controller)
        {

        }

        public RemoteClientServerAttribute(string action, string controller, string areaName) : base(action, controller, areaName)
        {

        }
    }
}