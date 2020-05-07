using Newtonsoft.Json;
using SWP.Classes.Game;
using SWP.Classes.Repository;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace SWP.Classes
{
    public class Core
    {
        public Core()
        {
            AutoSave();
            LoadTeams();
        }


        public static bool NeedSave { get; private set; }
        public List<ObservableCollection<Team>[]> teamsLists;


        public static void MarkTeamsAsChanged()
        {
            NeedSave = true;
        }


        private async void AutoSave()
        {
            while (true)
            {
                try
                {
                    if (NeedSave)
                    {
                        SaveTeams();
                        NeedSave = false;
                    }
                }
                catch (System.Exception e)
                {
                    Debug.WriteLine(e);
                }

                await Task.Delay(1000);
            }
        }

        public Team GetNewTeam(ContentType contentType)
        {
            int unitsCount;
            switch (contentType)
            {
                case ContentType.Arena:
                    unitsCount = 4;
                    break;

                case ContentType.GW:
                    unitsCount = 3;
                    break;

                case ContentType.Siege:
                    unitsCount = 3;
                    break;

                default:
                    unitsCount = 3;
                    break;
            }

            Team team = new Team()
            {
                ContentType = contentType
            };

            for (int i = 0; i < unitsCount; i++)
            {
                team.UnitsList.Add(new UnitItem());
            }

            return team;
        }

        public void SaveTeams()
        {
            FilesRepo.SaveObjToJson(
                FilesRepo.saveFolder + FilesRepo.teamsFileName,
                teamsLists);
        }

        public void LoadTeams()
        {
            teamsLists = FilesRepo.LoadJsonToObj(
                FilesRepo.saveFolder + FilesRepo.teamsFileName,
                () => {
                    return new List<ObservableCollection<Team>[]>()
                    {
                        new ObservableCollection<Team>[2]
                        {
                            new ObservableCollection<Team>(),
                            new ObservableCollection<Team>(),
                        },
                        new ObservableCollection<Team>[2]
                        {
                            new ObservableCollection<Team>(),
                            new ObservableCollection<Team>(),
                        },
                        new ObservableCollection<Team>[2]
                        {
                            new ObservableCollection<Team>(),
                            new ObservableCollection<Team>(),
                        }
                    };
                });
        }
    }
}
