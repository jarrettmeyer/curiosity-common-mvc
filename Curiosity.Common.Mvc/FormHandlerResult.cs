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
                HandleOnInvalidModelState(context);
                return;
            }
            HandleForm(context);
        }

        private void HandleForm(ControllerContext context)
        {
            try
            {
                FormHandlerBus.Instance.SendForm(Form);
                ExecuteSuccessResult(context);
            }
            catch (ApplicationException ex)
            {
                HandleOnApplicationException(context, ex);
            }
            catch (Exception ex)
            {
                HandleOnException(context, ex);
            }
        }

        private void HandleOnApplicationException(ControllerContext context, ApplicationException exception)
        {
            if (NotifyOnApplicationException)
            {
                WriteFlashWarning(context.Controller.TempData, exception.Message);
            }
            ExecuteFailureResult(context);
        }

        private void HandleOnInvalidModelState(ControllerContext context)
        {
            if (NotifyOnInvalidModelState)
            {
                WriteFlashWarning(context.Controller.TempData, InvalidModelStateNotification);
            }
            ExecuteFailureResult(context);
        }

        private void HandleOnException(ControllerContext context, Exception exception)
        {
            if (NotifyOnError)
            {
                WriteFlashError(context.Controller.TempData, exception.Message);
            }
            ExecuteFailureResult(context);
        }

        private void ExecuteFailureResult(ControllerContext context)
        {
            ActionResult failureResult = Failure.Invoke() ?? new EmptyResult();
            failureResult.ExecuteResult(context);
        }

        private void ExecuteSuccessResult(ControllerContext context)
        {
            ActionResult successResult = Success.Invoke() ?? new EmptyResult();
            successResult.ExecuteResult(context);
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
