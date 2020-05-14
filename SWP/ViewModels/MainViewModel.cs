using GongSolutions.Wpf.DragDrop;
using SWP.Classes;
using SWP.Classes.Game;
using SWP.Classes.Infrastructure;
using SWP.Classes.Repository;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Data;

namespace SWP.ViewModels
{
    public class MainViewModel: ViewModelBase, IDropTarget
    {
        private Core core;
        private CollectionViewSource CvsStaff { get; set; }

        public MainViewModel()
        {
            core = new Core();
            UnitsRepo.LoadImagesData();
            SearchedUnitsList = new ObservableCollection<UnitItem>(UnitsRepo.GetAllUnits());

            CvsStaff = new CollectionViewSource();
            CvsStaff.Source = SearchedUnitsList;
            CvsStaff.Filter += ApplyFilter;
        }

        

        private DelegateCommand _addTeamCommand;
        private DelegateCommand _removeTeamCommand;

        private string _filter;
        private int _contentTabIndex;
        private int _arenaTeamsTypeIndex;
        private int _gwTeamsTypeIndex;
        private int _siegeTeamsTypeIndex;
        private int _teamIndex;

        #region propertyList

        public ObservableCollection<UnitItem> SearchedUnitsList { get; set; }

        public ObservableCollection<Team> ArenaOffence => core.teamsLists[0][0];
        public ObservableCollection<Team> ArenaDefence => core.teamsLists[0][1];

        public ObservableCollection<Team> GWO => core.teamsLists[1][0];
        public ObservableCollection<Team> GWD => core.teamsLists[1][1];

        public ObservableCollection<Team> SiegeOffence => core.teamsLists[2][0];
        public ObservableCollection<Team> SiegeDefence => core.teamsLists[2][1];


        public ICollectionView AllUnitsList
        {
            get { return CvsStaff.View; }
        }

        public DelegateCommand AddTeamCommand
        {
            get
            {
                return _addTeamCommand ??
                  (_addTeamCommand = new DelegateCommand(obj =>
                  {
                      GetCurrentCollection().Add(
                          core.GetNewTeam((ContentType)ContentTabIndex));
                      Core.MarkTeamsAsChanged();
                  }));
            }
        }

        public DelegateCommand RemoveTeamCommand
        {
            get
            {
                return _removeTeamCommand ??
                  (_removeTeamCommand = new DelegateCommand(obj =>
                  {
                      RemoveTeam(obj as Team);
                  }));
            }
        }

        public string Filter
        {
            get
            {
                return _filter;
            }
            set
            {
                _filter = value;
                OnFilterChanged();
            }
        }

        public int ContentTabIndex
        {
            get
            {
                return _contentTabIndex;
            }
            set
            {
                _contentTabIndex = value;
            }
        }
        public int ArenaTeamsTypeIndex
        {
            get
            {
                return _arenaTeamsTypeIndex;
            }
            set
            {
               _arenaTeamsTypeIndex = value;
            }
        }
        public int GWTeamsTypeIndex
        {
            get
            {
                return _gwTeamsTypeIndex;
            }
            set
            {
                _gwTeamsTypeIndex = value;
            }
        }
        public int SiegeTeamsTypeIndex
        {
            get
            {
                return _siegeTeamsTypeIndex;
            }
            set
            {
                _siegeTeamsTypeIndex = value;
            }
        }
        public int TeamIndex
        {
            get
            {
                return _teamIndex;
            }
            set
            {
                _teamIndex = value;
            }
        }

        #endregion


        private void OnFilterChanged()
        {
            CvsStaff.View.Refresh();
        }

        private void ApplyFilter(object sender, FilterEventArgs e)
        {
            var svm = (UnitItem)e.Item;

            if (string.IsNullOrWhiteSpace(this.Filter) || this.Filter.Length == 0)
            {
                e.Accepted = true;
            }
            else
            {
                e.Accepted = svm.Name.ToLower().Contains(Filter.ToLower());
            }
        }

        private ObservableCollection<Team> GetCurrentCollection()
        {
            int teamsTypeIndex = -1;

            switch ((ContentType)ContentTabIndex)
            {
                case ContentType.Arena:
                    teamsTypeIndex = ArenaTeamsTypeIndex;
                    break;

                case ContentType.GW:
                    teamsTypeIndex = GWTeamsTypeIndex;
                    break;

                case ContentType.Siege:
                    teamsTypeIndex = SiegeTeamsTypeIndex;
                    break;
            }

            return core.teamsLists[ContentTabIndex][teamsTypeIndex];
        }  
        
        private void RemoveTeam(Team team)
        {
            var result = MessageBox.Show("Do you really want delete this team?", "Allert",
                    MessageBoxButton.YesNo, MessageBoxImage.Question, MessageBoxResult.No);

            if (result != MessageBoxResult.Yes)
                return;


            var collection = GetCurrentCollection();
            if (collection.Count > TeamIndex)
            {
                GetCurrentCollection().RemoveAt(TeamIndex);
                Core.MarkTeamsAsChanged();
            }
        }

        public void DragOver(IDropInfo dropInfo)
        {
            if (dropInfo.DragInfo.SourceCollection as ObservableCollection<UnitItem> != null)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Scroll;
            }
            else if(dropInfo.DragInfo.SourceCollection as ObservableCollection<Team> != null)
            {
                dropInfo.DropTargetAdorner = DropTargetAdorners.Insert;
                dropInfo.Effects = DragDropEffects.Move;
            }

            var ss = dropInfo.DragInfo.SourceCollection.GetType();
        }

        public void Drop(IDropInfo dropInfo)
        {
            if(dropInfo.Effects == DragDropEffects.Scroll)
            {
                var sourceItem = dropInfo.Data as UnitItem;
                var sourceCollection = dropInfo.DragInfo.SourceCollection as ObservableCollection<UnitItem>;

                if (sourceItem != null && sourceCollection != null)
                {
                    sourceItem.SetID(-1);

                    Core.MarkTeamsAsChanged();
                    CollectionViewSource.GetDefaultView(sourceCollection).Refresh();
                }
            }
            else if (dropInfo.Effects == DragDropEffects.Move)
            {
                var sourceItem = dropInfo.Data as Team;
                var sourceCollection = dropInfo.DragInfo.SourceCollection as ObservableCollection<Team>;

                if (sourceItem != null && sourceCollection != null)
                {
                    RemoveTeam(sourceItem);
                }
            }
        }
    }
}
