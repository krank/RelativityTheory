using Playnite.SDK;
using Playnite.SDK.Models;
using Playnite.SDK.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace RelativityTheory
{

    public class RelativityTheory : Plugin
    {
        private static readonly ILogger logger = LogManager.GetLogger();

        private RelativityTheorySettings settings { get; set; }

        public override Guid Id { get; } = Guid.Parse("5fbb60dc-4d72-46f5-bbb2-1bea69668f11");

        public RelativityTheory(IPlayniteAPI api) : base(api)
        {
            settings = new RelativityTheorySettings(this);
        }

        public override IEnumerable<ExtensionFunction> GetFunctions()
        {
            return new List<ExtensionFunction>
            {
                new ExtensionFunction(
                    "Relativize library",
                    () =>
                        {
                            IEnumerable<Game> localUnrelativizedGames = FilterUnrelativized(PlayniteApi.Database.Games);

                            RelativizeGames(localUnrelativizedGames);
                        }
                    ),
                new ExtensionFunction(
                    "Relativize selected games",
                    () =>
                        {
                            IEnumerable<Game> selectedGames = FilterUnrelativized(PlayniteApi.MainView.SelectedGames);

                            RelativizeGames(selectedGames);
                        }
                    )
            };
        }

        public IEnumerable<Game> FilterUnrelativized(IEnumerable<Game> gamesToFilter)
        {
            IEnumerable<Game> filteredGames = gamesToFilter.Where<Game>((g, b) => g.InstallDirectory.IndexOf(PlayniteApi.Paths.ApplicationPath) == 0);

            return filteredGames;
        }

        public void RelativizeGames(IEnumerable<Game> gamesToRelativize)
        {

            System.Windows.MessageBoxResult result = PlayniteApi.Dialogs.ShowMessage("This will relativize installation & image paths of " + gamesToRelativize.Count() + " games.", "Relativize all thesse games?", System.Windows.MessageBoxButton.OKCancel);

            if (result == System.Windows.MessageBoxResult.Cancel) return;

            foreach (Game game in gamesToRelativize)
            {
                RelativizeGame(game);
            }

            PlayniteApi.Database.Games.Update(gamesToRelativize);

            PlayniteApi.Dialogs.ShowMessage("Done.");
        }

        public void RelativizeGame(Game game)
        {
            game.InstallDirectory = game.InstallDirectory.Replace(PlayniteApi.Paths.ApplicationPath, "{PlayniteDir}");
            game.GameImagePath = game.GameImagePath.Replace(PlayniteApi.Paths.ApplicationPath, "{PlayniteDir}");
        }


        public override void OnGameInstalled(Game game)
        {
            // Add code to be executed when game is finished installing.
        }

        public override void OnGameStarted(Game game)
        {
            // Add code to be executed when game is started running.
        }

        public override void OnGameStarting(Game game)
        {
            // Add code to be executed when game is preparing to be started.
        }

        public override void OnGameStopped(Game game, long elapsedSeconds)
        {
            // Add code to be executed when game is preparing to be started.
        }

        public override void OnGameUninstalled(Game game)
        {
            // Add code to be executed when game is uninstalled.
        }

        public override void OnApplicationStarted()
        {
            // Add code to be executed when Playnite is initialized.
        }

        public override void OnApplicationStopped()
        {
            // Add code to be executed when Playnite is shutting down.
        }

        public override void OnLibraryUpdated()
        {
            // Add code to be executed when library is updated.
        }

        public override ISettings GetSettings(bool firstRunSettings)
        {
            return settings;
        }

        public override UserControl GetSettingsView(bool firstRunSettings)
        {
            return new RelativityTheorySettingsView();
        }
    }
}