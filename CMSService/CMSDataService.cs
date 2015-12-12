﻿using CMSService.ServiceReference;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace CMSService
{
    static class CMSDataService
    {
        private static Timer _timer;
        private static double MILISECONDS_IN_TWO_HOURS = 7200000;
        private static int FIRST_RUN_DELAY = 60000; // one minute

        public static void InitService()
        {
            System.Threading.Thread.Sleep(FIRST_RUN_DELAY);

            CMSDatabaseCrossedDeathLineHelper.CrossedDeathLineEventHandler += CMSDatabaseCrossedDeathLineHelper_CrossedDeathLineEventHandler;
            CMSDatabaseCrossedDeathLineHelper.CheckCrossedDeathLine(null, null);

            _timer = new Timer(MILISECONDS_IN_TWO_HOURS);

            _timer.Enabled = true;            
            _timer.Elapsed -= new ElapsedEventHandler(CMSDatabaseCrossedDeathLineHelper.CheckCrossedDeathLine);
            _timer.Elapsed += new ElapsedEventHandler(CMSDatabaseCrossedDeathLineHelper.CheckCrossedDeathLine);
        }

        public static void StopService()
        {
            _timer = null;
        }

        private static void CMSDatabaseCrossedDeathLineHelper_CrossedDeathLineEventHandler()
        {            
            using (MessageMessengerClient service = new MessageMessengerClient())
            {
                service.ShowMessageOnServerSide("Mniej niż 2 tygodnia do przekroczenia terminu ważności!", "CMS");                
            }
        }
    }
}
