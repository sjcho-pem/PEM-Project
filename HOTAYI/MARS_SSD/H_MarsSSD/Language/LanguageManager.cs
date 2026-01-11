using System;
using System.Globalization;

namespace H_MarsSSD
{
    public class LanguageManager
    {
        public static CultureInfo GetLanguageCulture(int index)
        {
            switch (index)
            {
                case (int)LAN_MODE.ENG:
                    return new CultureInfo("en-US");
                case (int)LAN_MODE.KOR:
                    return new CultureInfo("ko-KR");
                case (int)LAN_MODE.CHA:
                    return new CultureInfo("zh-CN");
            }
            throw new InvalidOperationException();
        }

        public static void LanguageUpdate()
        {
			UI.PageSubItems.SetLanguageText();
			UI.PageManual.SetLanguageText();
			UI.PageData.SetLanguageText();
			UI.PageMotor.SetLanguageText();
			UI.PageIO.SetLanguageText();
			UI.PageError.SetLanguageText();
			UI.PageLog.SetLanguageText();
			UI.PageTenKey.SetLanguageText();
			UI.PageMainDisp.SetLanguageText();

			UI.ErrorRecipeDisp.SetLanguageText();
			
			UI.Barcode.SetLanguageText();
			UI.SubpageRecipe.SetLanguageText();
			UI.SubpageDm.SetLanguageText();
			UI.SubpageSDm.SetLanguageText();
			UI.SubpageBit.SetLanguageText();
			UI.SubpageUseskip.SetLanguageText();
        }
    }
}
