using Dommy.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Dommy.Win8.ViewModel
{
    public class ActionViewModel : BaseViewModel
    {
        public ActionData ActionData { get; set; }

        static ActionViewModel()
        {
            LoadActions();
        }

        private static async void LoadActions()
        {
            var actionsData = await ServiceSingleton.Current.GetActionsAsync();

            var q = from a in actionsData select ActionViewModel.GetAction(a);

            foreach (var item in q)
            {
                if (item != null)
                {
                    actions.Add(item);
                }
            }
        }

        private static ObservableCollection<ActionViewModel> actions = new ObservableCollection<ActionViewModel>();

        public static ObservableCollection<ActionViewModel> GetActions()
        {
            return actions;
        }

        public static ActionViewModel GetAction(Model.ActionData actionData)
        {
            if (actionData is OnOffLightActionData)
            {
                return new OnOffLightActionViewModel() { Data = (OnOffLightActionData)actionData };
            }

            return null;
        }
    }

    public class ActionViewModel<T> : ActionViewModel
        where T : ActionData
    {
        public T Data
        {
            get { return (T)this.ActionData; }
            set { this.ActionData = value; }
        }
    }
}
