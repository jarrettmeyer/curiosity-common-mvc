using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace Curiosity.Common.Mvc
{
    public class FormHandlerResult<FORM> : ActionResult
    {
        public FormHandlerResult(FORM form, Func<ActionResult> success, Func<ActionResult> failure)
        {
            if (Equals(form, default(FORM)))
                throw new ArgumentNullException("form");

            Form = form;
            Success = success ?? ReturnEmptyResult;
            Failure = failure ?? ReturnEmptyResult;

            NotifyOnApplicationException = true;
            NotifyOnError = true;
            NotifyOnInvalidModelState = true;

            InvalidModelStateNotification = "Please correct the errors with the form.";
        }

        public FORM Form { get; private set; }

        public Func<ActionResult> Failure { get; private set; }

        public Func<ActionResult> Success { get; private set; }

        public bool NotifyOnApplicationException { get; set; }

        public bool NotifyOnError { get; set; }

        public bool NotifyOnInvalidModelState { get; set; }

        public string InvalidModelStateNotification { get; set; }

        public override void ExecuteResult(ControllerContext context)
        {
            bool isModelStateValid = IsModelStateValid(context);
            if (!isModelStateValid)
            {
                if (NotifyOnInvalidModelState)
                {
                    WriteFlashWarning(context.Controller.TempData, "");
                }
                ExecuteFailureResult(context);
                return;
            }
            try
            {
                var formHandler = GetFormHandler();
                formHandler.Handle(Form);
                ExecuteSuccessResult(context);
            }
            catch (ApplicationException ex)
            {
                if (NotifyOnApplicationException)
                {
                    WriteFlashWarning(context.Controller.TempData, ex.Message);
                }
                ExecuteFailureResult(context);
            }
            catch (Exception ex)
            {
                if (NotifyOnError)
                {
                    WriteFlashError(context.Controller.TempData, ex.Message);
                }
                ExecuteFailureResult(context);
            }
        }

        private void ExecuteFailureResult(ControllerContext context)
        {
            Failure.Invoke().ExecuteResult(context);
        }

        private void ExecuteSuccessResult(ControllerContext context)
        {
            Success.Invoke().ExecuteResult(context);
        }

        private static IFormHandler GetFormHandler()
        {
            var factory = FormHandlerFactory.Instance;            
            var formHandler = factory.Create(typeof(FORM));
            return formHandler;
        }

        [DebuggerStepThrough]
        private static bool IsModelStateValid(ControllerContext context)
        {
            return context.Controller.ViewData.ModelState.IsValid;
        }

        private static ActionResult ReturnEmptyResult()
        {
            return new EmptyResult();
        }

        private static void WriteFlashError(TempDataDictionary tempData, string message)
        {
            tempData.Flash(new { error = message });
        }

        private static void WriteFlashWarning(TempDataDictionary tempData, string message)
        {
            tempData.Flash(new { warning = message });
        }
    }
}
