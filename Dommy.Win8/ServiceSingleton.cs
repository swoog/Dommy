//using Dommy.Win8.ViewModel;
//using System;
//using System.Collections.Generic;
//using System.Collections.ObjectModel;
//using System.Linq;
//using System.ServiceModel;
//using System.ServiceModel.Channels;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.ApplicationModel.Background;
//using Windows.UI.Xaml;

//namespace Dommy.Win8
//{
//    public class ServiceSingleton
//    {
//        private static DispatcherTimer timer;

//        static ServiceSingleton()
//        {
//            Current = new DommyService.ActionServiceClient();

//            timer = new DispatcherTimer();
//            timer.Tick += timer_Tick;
//            timer.Interval = new TimeSpan(00, 0, 1);
//            timer.Start();
//        }

//        private static async void timer_Tick(object sender, object e)
//        {
//            var actions = ActionViewModel.GetActions();
//            DommyService.ActionServiceClient action = new DommyService.ActionServiceClient();

//            var actionsReturn = await action.GetDatasAsync(new ObservableCollection<int>(actions.Select(a => a.ActionData.Id)));

//            for (int i = 0; i < actions.Count; i++)
//            {
//                var actionData = actionsReturn.FirstOrDefault(a => a.Id == actions[i].ActionData.Id);

//                if (actionData != null)
//                {
//                    actions[i] = ActionViewModel.GetAction(actionData);
//                }
//            }
//        }

//        public static DommyService.ActionServiceClient Current { get; set; }
//    }
//}
