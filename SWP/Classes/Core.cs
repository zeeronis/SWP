using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using SWP.Classes.Game;
using SWP.Classes.Repository;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace SWP.Classes
{
    public class Core
    {
        public Core()
        {
            AutoSave();
            LoadTeams();
        }

        public List<ObservableCollection<Team>[]> teamsLists;


        private async void AutoSave()
        {
            while (true)
            {
                try
                {
                    if (UnitsRepo.NeedSave)
                    {
                        SaveTeams();
                        UnitsRepo.SetNeedSave(false);
                    }
                }
                catch (System.Exception e)
                {
                    Debug.WriteLine(e);
                }

                await Task.Delay(1000);
            }
        }

        private void InitEmptyList()
        {
            teamsLists = new List<ObservableCollection<Team>[]>()
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
                    },
                };
        }

        private void CheckDirectory()
        {
            if (!Directory.Exists(UnitsRepo.dataFolder))
            {
                Directory.CreateDirectory(UnitsRepo.dataFolder);
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

            CheckDirectory();

            JsonSerializer serializer = new JsonSerializer();
            serializer.NullValueHandling = NullValueHandling.Ignore;

            using (StreamWriter sw = new StreamWriter(UnitsRepo.dataFolder + UnitsRepo.teamsFileName))
            {
                using (JsonWriter writer = new JsonTextWriter(sw))
                {
                    serializer.Serialize(writer, teamsLists);
                }
            }
        }

        public void LoadTeams()
        {
            try
            {
                CheckDirectory();

                string json = File.ReadAllText(UnitsRepo.dataFolder + UnitsRepo.teamsFileName);
                if(json != string.Empty)
                {
                    teamsLists = JsonConvert.DeserializeObject<List<ObservableCollection<Team>[]>>(json);
                }
                else
                {
                    InitEmptyList();
                }
            }
            catch (System.Exception e)
            {
                InitEmptyList();
            }
        }
    }
}
