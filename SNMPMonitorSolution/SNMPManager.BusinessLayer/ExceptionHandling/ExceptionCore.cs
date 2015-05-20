using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SNMPManager.BusinessLayer.ExceptionHandling
{
    public class ExceptionCore
    {
        public static void HandleException(ExceptionCategory category, Exception exception)
        {
            switch (category)
            {
                case ExceptionCategory.Normal:
                    HandleNORMAL(category, exception);
                    break;
                case ExceptionCategory.Low:
                    break;
                case ExceptionCategory.Fatal:
                    HandleFATAL(category, exception);
                    break;
                case ExceptionCategory.High:
                    HandleHIGH(category, exception);
                    break;
                default:
                    break;
            }
        }

        private static void HandleFATAL(ExceptionCategory category, Exception exc)
        {
            DataLayer.ExceptionHandling.ExceptionLogger.LogException(category.ToString(), exc);
            //TODO: Stop Service
            throw exc;
        }

        private static void HandleNORMAL(ExceptionCategory category, Exception exc)
        {
            try
            {
                DataLayer.ExceptionHandling.ExceptionLogger.SaveExceptionToDB(category.ToString(), exc);
            }
            catch (Exception innerExc)
            {
                DataLayer.ExceptionHandling.ExceptionLogger.LogException(category.ToString(), exc);
                HandleException(ExceptionCategory.Fatal, innerExc);
            }
            
        }

        private static void HandleHIGH(ExceptionCategory category, Exception exc)
        {
            try
            {
                DataLayer.ExceptionHandling.ExceptionLogger.LogException(category.ToString(), exc);
                DataLayer.ExceptionHandling.ExceptionLogger.SaveExceptionToDB(category.ToString(), exc);
            }
            catch (Exception innerExc)
            {
                DataLayer.ExceptionHandling.ExceptionLogger.LogException(category.ToString(), exc);
                HandleException(ExceptionCategory.Fatal, innerExc);
            }
        }
    }
}
