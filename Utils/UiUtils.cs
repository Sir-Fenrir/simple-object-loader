using HarmonyLib;
using Microsoft.Xna.Framework;
using MLEM.Ui;
using MLEM.Ui.Elements;
using SimpleObjectLoader.Config;
using System.Collections.Generic;
using System.Linq;
using TinyLife.Uis;

namespace SimpleObjectLoader.Utils
{
    internal class UiUtils
    {
        /// <summary>
        /// Helper method for building the loaded mod list.
        /// </summary>
        /// <returns>An Element containing all the info about the loaded mods.</returns>
        public static Element ModInfoBox(List<ModConfig> modConfigs)
        {
            var cGroup = new CoveringGroup();

            var infoBox = new Panel(Anchor.Center, new Vector2(200, 1), Vector2.Zero, setHeightBasedOnChildren: true);
            cGroup.AddChild(infoBox);

            infoBox.AddChild(new Paragraph(Anchor.AutoCenter, 1, "<b>Loaded mods:</b>"));

            modConfigs
                .Select(mod => $"<i>{mod.ModName}</i> by {mod.Author}")
                .Select(text => new Paragraph(Anchor.AutoLeft, 1, text))
                .Do(p => infoBox.AddChild(p));

            if (SOL.Errors.Count > 0)
            {
                infoBox.AddChild(new Paragraph(Anchor.AutoCenter, 1, "<c Red>Errors:</c>"));

                SOL.Errors.Select(error => new Paragraph(Anchor.AutoCenter, 1, error))
                    .Do(err => infoBox.AddChild(err));
            }

            return cGroup;
        }
    }
}
