using System.Collections.ObjectModel;
using System.Windows.Forms;
using System.IO;
using StopWatch.Model;
using StopWatch.Support;
using System;

namespace StopWatch.ViewModel
{
    class HistoryMemberVM : Notification
    {
        public ObservableCollection<Result> Results { get; set; }

        #region Clean DB

        private RelayCommand _Clean;
        public RelayCommand Clean
        {
            get
            {
                return _Clean ??
                  (_Clean = new RelayCommand(obj =>
                  {
                      if (Results.Count > 0)
                      {
                          int Succesful = 1;
                          System.Windows.MessageBoxResult result = System.Windows.MessageBox.Show("Хотите сохранить историю перед удалением?",
                                  "Question", System.Windows.MessageBoxButton.YesNo, System.Windows.MessageBoxImage.Question);
                          if (result == System.Windows.MessageBoxResult.Yes)
                          {                             
                              Logger.Log.Info("Сохранение истории");
                              FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
                              DialogResult Select = folderBrowser.ShowDialog();
                              if (!string.IsNullOrWhiteSpace(folderBrowser.SelectedPath))
                              {
                                  string Path = folderBrowser.SelectedPath + "\\";
                                  Succesful = SaveFile(Path);
                                  System.Diagnostics.Process.Start(Path);                                  
                              }
                          }
                          if (Succesful == 1)
                          {
                              Logger.Log.Info("Очистка базы данных");
                              using (ParticipantContainer pc = new ParticipantContainer())
                              {
                                  var Res = pc.Results;
                                  foreach (var Temp_Res in Res)
                                  {
                                      pc.Entry(Temp_Res).State = System.Data.Entity.EntityState.Deleted;
                                  }
                                  pc.Database.ExecuteSqlCommand("update sqlite_sequence set seq = 0 WHERE Name = 'Results'");
                                  pc.SaveChanges();
                              }
                              Results.Clear();
                          }                          
                      }
                  }));
            }
        }
        #endregion

        #region Save File History

        private int SaveFile(string Path)
        {
            Path += "История.txt";
            try
            {
                using (StreamWriter sw = new StreamWriter(Path))
                {
                    foreach (var Temp_Res in Results)
                    {
                        sw.WriteLine($"Забег {Temp_Res.Id}: ФИО 1 дорожка: {Temp_Res.Name_One} - Время 1 дорожка: {Temp_Res.Time_One} " +
                            $"ФИО 2 дорожка: {Temp_Res.Name_Two} - Время 2 дорожка: {Temp_Res.Time_Two}\n");
                    }
                }
                return 1;
            }
            catch(Exception ex)
            {
                Logger.Log.Info("Ошибка записи в файл!", ex);
                MessageBox.Show("Ошибка записи в файл!", "Ошибка");
                return -1;
            }
        }
        #endregion

        public void Update()
        {
            if(Results.Count > 0) { Results.Clear(); }
            using (ParticipantContainer pc = new ParticipantContainer())
            {
                var Res = pc.Results;
                foreach (var Temp_Res in Res)
                {
                    Results.Add(Temp_Res);
                }
            }
        }

        public HistoryMemberVM()
        {
            Results = new ObservableCollection<Result>();
            Update();
        }
    }
}
