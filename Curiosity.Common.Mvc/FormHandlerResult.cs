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
            NotifyOnSuccess = true;

            InvalidModelStateNotification = () => "Please correct the errors with the form.";
            SuccessNotification = null;
        }

        public FORM Form { get; private set; }

        public Func<ActionResult> Failure { get; private set; }

        public Func<ActionResult> Success { get; private set; }

        /// <summary>
        /// Should a flash warning message be shown when an application exception is thrown?
        /// </summary>
        public bool NotifyOnApplicationException { get; set; }

        /// <summary>
        /// Should a flash error message be shown when an exception is thrown?
        /// </summary>
        public bool NotifyOnError { get; set; }

        /// <summary>
        /// Should a flash warning message be shown when the model state is invalid?
        /// </summary>
        public bool NotifyOnInvalidModelState { get; set; }

        /// <summary>
        /// Should a flash notification message be shown on success?
        /// </summary>
        public bool NotifyOnSuccess { get; set; }

        /// <summary>
        /// Notification message to show when the model state is invalid.
        /// </summary>
        public Func<string> InvalidModelStateNotification { get; set; }

        /// <summary>
        /// Success message to show when the form handler is successful.
        /// </summary>
        public Func<string> SuccessNotification { get; set; }

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
                HandleOnSuccess(context);
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
            if (NotifyOnInvalidModelState && InvalidModelStateNotification != null)
            {
                WriteFlashWarning(context.Controller.TempData, InvalidModelStateNotification());
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

        private void HandleOnSuccess(ControllerContext context)
        {
            if (NotifyOnSuccess && SuccessNotification != null)
            {
                WriteFlashSuccess(context.Controller.TempData, SuccessNotification());
            }
            ExecuteSuccessResult(context);
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

        private static void WriteFlashSuccess(TempDataDictionary tempData, string message)
        {
            tempData.Flash(new { success = message });
        }

        private static void WriteFlashWarning(TempDataDictionary tempData, string message)
        {
            tempData.Flash(new { warning = message });
        }
    }
}
