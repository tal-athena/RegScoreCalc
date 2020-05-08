using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegScoreCalc.SVMClient
{
    class SVMClientCallback : SVMProcessDBService.ISVMServiceCallback
    {
        protected ViewSVM _viewSVM;
        public SVMClientCallback(ViewSVM viewSVM)
        {
            _viewSVM = viewSVM;
        }
        public void ProcessFinished(string processName)
        {
            _viewSVM.notificationTextBox.AppendText(Environment.NewLine + "-----------------------------------------" + Environment.NewLine);

            _viewSVM._paneSVM.EnableAllStartProcessButtons();
            _viewSVM._paneSVM.EnableReviewButton(processName[0].ToString());
        }

        public void NotifyClient(string message)
        {
            _viewSVM.notificationTextBox.AppendText(message);
        }


    }
}
