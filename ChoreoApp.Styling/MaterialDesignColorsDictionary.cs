using MaterialColorUtilities;
using Microsoft.Maui.Graphics;

namespace ChoreoApp;

public sealed partial class MaterialDesignColorsDictionary : ResourceDictionary
{
    private MaterialDynamicColors DynamicColors { get; } = new();

    public MaterialDesignColorsDictionary()
    {
        SetMaterialDesignColors();

        var scheme = new SchemeContent(
            Hct.FromInt(Color.ArgbFromColor(Color.FromRgb(0x19, 0x76, 0xD2))),
            isDark: true,
            contrastLevel: 0.5,
            SpecVersion.Spec2025,
            Platform.Phone);

        SetScheme(scheme);
    }

    private void SetMaterialDesignColors()
    {
        this.SetColor(MaterialDesignColorKey.Red50,  Color.FromRgb(0xFF, 0xEB, 0xEE));
        this.SetColor(MaterialDesignColorKey.Red100, Color.FromRgb(0xFF, 0xCD, 0xD2));
        this.SetColor(MaterialDesignColorKey.Red200, Color.FromRgb(0xEF, 0x9A, 0x9A));
        this.SetColor(MaterialDesignColorKey.Red300, Color.FromRgb(0xE5, 0x73, 0x73));
        this.SetColor(MaterialDesignColorKey.Red400, Color.FromRgb(0xEF, 0x53, 0x50));
        this.SetColor(MaterialDesignColorKey.Red500, Color.FromRgb(0xF4, 0x43, 0x36));
        this.SetColor(MaterialDesignColorKey.Red600, Color.FromRgb(0xE5, 0x39, 0x35));
        this.SetColor(MaterialDesignColorKey.Red700, Color.FromRgb(0xD3, 0x2F, 0x2F));
        this.SetColor(MaterialDesignColorKey.Red800, Color.FromRgb(0xC6, 0x28, 0x28));
        this.SetColor(MaterialDesignColorKey.Red900, Color.FromRgb(0xB7, 0x1C, 0x1C));

        this.SetColor(MaterialDesignColorKey.Green50,  Color.FromRgb(0xE8, 0xF5, 0xE9));
        this.SetColor(MaterialDesignColorKey.Green100, Color.FromRgb(0xC8, 0xE6, 0xC9));
        this.SetColor(MaterialDesignColorKey.Green200, Color.FromRgb(0xA5, 0xD6, 0xA7));
        this.SetColor(MaterialDesignColorKey.Green300, Color.FromRgb(0x81, 0xC7, 0x84));
        this.SetColor(MaterialDesignColorKey.Green400, Color.FromRgb(0x66, 0xBB, 0x6A));
        this.SetColor(MaterialDesignColorKey.Green500, Color.FromRgb(0x4C, 0xAF, 0x50));
        this.SetColor(MaterialDesignColorKey.Green600, Color.FromRgb(0x43, 0xA0, 0x47));
        this.SetColor(MaterialDesignColorKey.Green700, Color.FromRgb(0x38, 0x8E, 0x3C));
        this.SetColor(MaterialDesignColorKey.Green800, Color.FromRgb(0x2E, 0x7D, 0x32));
        this.SetColor(MaterialDesignColorKey.Green900, Color.FromRgb(0x1B, 0x5E, 0x20));

        this.SetColor(MaterialDesignColorKey.Blue50,  Color.FromRgb(0xE3, 0xF2, 0xFD));
        this.SetColor(MaterialDesignColorKey.Blue100, Color.FromRgb(0xBB, 0xDE, 0xFB));
        this.SetColor(MaterialDesignColorKey.Blue200, Color.FromRgb(0x90, 0xCA, 0xF9));
        this.SetColor(MaterialDesignColorKey.Blue300, Color.FromRgb(0x64, 0xB5, 0xF6));
        this.SetColor(MaterialDesignColorKey.Blue400, Color.FromRgb(0x42, 0xA5, 0xF5));
        this.SetColor(MaterialDesignColorKey.Blue500, Color.FromRgb(0x21, 0x96, 0xF3));
        this.SetColor(MaterialDesignColorKey.Blue600, Color.FromRgb(0x1E, 0x88, 0xE5));
        this.SetColor(MaterialDesignColorKey.Blue700, Color.FromRgb(0x19, 0x76, 0xD2));
        this.SetColor(MaterialDesignColorKey.Blue800, Color.FromRgb(0x15, 0x65, 0xC0));
        this.SetColor(MaterialDesignColorKey.Blue900, Color.FromRgb(0x0D, 0x47, 0xA1));

        this.SetColor(MaterialDesignColorKey.Pink50,  Color.FromRgb(0xFC, 0xE4, 0xEC));
        this.SetColor(MaterialDesignColorKey.Pink100, Color.FromRgb(0xF8, 0xBB, 0xD0));
        this.SetColor(MaterialDesignColorKey.Pink200, Color.FromRgb(0xF4, 0x8F, 0xB1));
        this.SetColor(MaterialDesignColorKey.Pink300, Color.FromRgb(0xF0, 0x62, 0x92));
        this.SetColor(MaterialDesignColorKey.Pink400, Color.FromRgb(0xEC, 0x40, 0x7A));
        this.SetColor(MaterialDesignColorKey.Pink500, Color.FromRgb(0xE9, 0x1E, 0x63));
        this.SetColor(MaterialDesignColorKey.Pink600, Color.FromRgb(0xD8, 0x1B, 0x60));
        this.SetColor(MaterialDesignColorKey.Pink700, Color.FromRgb(0xC2, 0x18, 0x5B));
        this.SetColor(MaterialDesignColorKey.Pink800, Color.FromRgb(0xAD, 0x14, 0x57));
        this.SetColor(MaterialDesignColorKey.Pink900, Color.FromRgb(0x88, 0x0E, 0x4F));

        this.SetColor(MaterialDesignColorKey.Purple50,  Color.FromRgb(0xF3, 0xE5, 0xF5));
        this.SetColor(MaterialDesignColorKey.Purple100, Color.FromRgb(0xE1, 0xBE, 0xE7));
        this.SetColor(MaterialDesignColorKey.Purple200, Color.FromRgb(0xCE, 0x93, 0xD8));
        this.SetColor(MaterialDesignColorKey.Purple300, Color.FromRgb(0xBA, 0x68, 0xC8));
        this.SetColor(MaterialDesignColorKey.Purple400, Color.FromRgb(0xAB, 0x47, 0xBC));
        this.SetColor(MaterialDesignColorKey.Purple500, Color.FromRgb(0x9C, 0x27, 0xB0));
        this.SetColor(MaterialDesignColorKey.Purple600, Color.FromRgb(0x8E, 0x24, 0xAA));
        this.SetColor(MaterialDesignColorKey.Purple700, Color.FromRgb(0x7B, 0x1F, 0xA2));
        this.SetColor(MaterialDesignColorKey.Purple800, Color.FromRgb(0x6A, 0x1B, 0x9A));
        this.SetColor(MaterialDesignColorKey.Purple900, Color.FromRgb(0x4A, 0x14, 0x8C));

        this.SetColor(MaterialDesignColorKey.DeepPurple50,  Color.FromRgb(0xED, 0xE7, 0xF6));
        this.SetColor(MaterialDesignColorKey.DeepPurple100, Color.FromRgb(0xD1, 0xC4, 0xE9));
        this.SetColor(MaterialDesignColorKey.DeepPurple200, Color.FromRgb(0xB3, 0x9D, 0xDB));
        this.SetColor(MaterialDesignColorKey.DeepPurple300, Color.FromRgb(0x95, 0x75, 0xCD));
        this.SetColor(MaterialDesignColorKey.DeepPurple400, Color.FromRgb(0x7E, 0x57, 0xC2));
        this.SetColor(MaterialDesignColorKey.DeepPurple500, Color.FromRgb(0x67, 0x3A, 0xB7));
        this.SetColor(MaterialDesignColorKey.DeepPurple600, Color.FromRgb(0x5E, 0x35, 0xB1));
        this.SetColor(MaterialDesignColorKey.DeepPurple700, Color.FromRgb(0x51, 0x2D, 0xA8));
        this.SetColor(MaterialDesignColorKey.DeepPurple800, Color.FromRgb(0x45, 0x27, 0xA0));
        this.SetColor(MaterialDesignColorKey.DeepPurple900, Color.FromRgb(0x31, 0x1B, 0x92));

        this.SetColor(MaterialDesignColorKey.Indigo50,  Color.FromRgb(0xE8, 0xEA, 0xF6));
        this.SetColor(MaterialDesignColorKey.Indigo100, Color.FromRgb(0xC5, 0xCA, 0xE9));
        this.SetColor(MaterialDesignColorKey.Indigo200, Color.FromRgb(0x9F, 0xA8, 0xDA));
        this.SetColor(MaterialDesignColorKey.Indigo300, Color.FromRgb(0x79, 0x86, 0xCB));
        this.SetColor(MaterialDesignColorKey.Indigo400, Color.FromRgb(0x5C, 0x6B, 0xC0));
        this.SetColor(MaterialDesignColorKey.Indigo500, Color.FromRgb(0x3F, 0x51, 0xB5));
        this.SetColor(MaterialDesignColorKey.Indigo600, Color.FromRgb(0x39, 0x49, 0xAB));
        this.SetColor(MaterialDesignColorKey.Indigo700, Color.FromRgb(0x30, 0x3F, 0x9F));
        this.SetColor(MaterialDesignColorKey.Indigo800, Color.FromRgb(0x28, 0x35, 0x93));
        this.SetColor(MaterialDesignColorKey.Indigo900, Color.FromRgb(0x1A, 0x23, 0x7E));

        this.SetColor(MaterialDesignColorKey.BlueGrey50,  Color.FromRgb(0xEC, 0xEF, 0xF1));
        this.SetColor(MaterialDesignColorKey.BlueGrey100, Color.FromRgb(0xCF, 0xD8, 0xDC));
        this.SetColor(MaterialDesignColorKey.BlueGrey200, Color.FromRgb(0xB0, 0xBE, 0xC5));
        this.SetColor(MaterialDesignColorKey.BlueGrey300, Color.FromRgb(0x90, 0xA4, 0xAE));
        this.SetColor(MaterialDesignColorKey.BlueGrey400, Color.FromRgb(0x78, 0x90, 0x9C));
        this.SetColor(MaterialDesignColorKey.BlueGrey500, Color.FromRgb(0x60, 0x7D, 0x8B));
        this.SetColor(MaterialDesignColorKey.BlueGrey600, Color.FromRgb(0x54, 0x6E, 0x7A));
        this.SetColor(MaterialDesignColorKey.BlueGrey700, Color.FromRgb(0x45, 0x5A, 0x64));
        this.SetColor(MaterialDesignColorKey.BlueGrey800, Color.FromRgb(0x37, 0x47, 0x4F));
        this.SetColor(MaterialDesignColorKey.BlueGrey900, Color.FromRgb(0x26, 0x32, 0x38));

        this.SetColor(MaterialDesignColorKey.Cyan50,  Color.FromRgb(0xE0, 0xF7, 0xFA));
        this.SetColor(MaterialDesignColorKey.Cyan100, Color.FromRgb(0xB2, 0xEB, 0xF2));
        this.SetColor(MaterialDesignColorKey.Cyan200, Color.FromRgb(0x80, 0xDE, 0xEA));
        this.SetColor(MaterialDesignColorKey.Cyan300, Color.FromRgb(0x4D, 0xD0, 0xE1));
        this.SetColor(MaterialDesignColorKey.Cyan400, Color.FromRgb(0x26, 0xC6, 0xDA));
        this.SetColor(MaterialDesignColorKey.Cyan500, Color.FromRgb(0x00, 0xBC, 0xD4));
        this.SetColor(MaterialDesignColorKey.Cyan600, Color.FromRgb(0x00, 0xAC, 0xC1));
        this.SetColor(MaterialDesignColorKey.Cyan700, Color.FromRgb(0x00, 0x97, 0xA7));
        this.SetColor(MaterialDesignColorKey.Cyan800, Color.FromRgb(0x00, 0x83, 0x8F));
        this.SetColor(MaterialDesignColorKey.Cyan900, Color.FromRgb(0x00, 0x60, 0x64));

        this.SetColor(MaterialDesignColorKey.Teal50,  Color.FromRgb(0xE0, 0xF2, 0xF1));
        this.SetColor(MaterialDesignColorKey.Teal100, Color.FromRgb(0xB2, 0xDF, 0xDB));
        this.SetColor(MaterialDesignColorKey.Teal200, Color.FromRgb(0x80, 0xCB, 0xC4));
        this.SetColor(MaterialDesignColorKey.Teal300, Color.FromRgb(0x4D, 0xB6, 0xAC));
        this.SetColor(MaterialDesignColorKey.Teal400, Color.FromRgb(0x26, 0xA6, 0x9A));
        this.SetColor(MaterialDesignColorKey.Teal500, Color.FromRgb(0x00, 0x96, 0x88));
        this.SetColor(MaterialDesignColorKey.Teal600, Color.FromRgb(0x00, 0x89, 0x7B));
        this.SetColor(MaterialDesignColorKey.Teal700, Color.FromRgb(0x00, 0x79, 0x6B));
        this.SetColor(MaterialDesignColorKey.Teal800, Color.FromRgb(0x00, 0x69, 0x5C));
        this.SetColor(MaterialDesignColorKey.Teal900, Color.FromRgb(0x00, 0x4D, 0x40));

        this.SetColor(MaterialDesignColorKey.LightGreen50,  Color.FromRgb(0xF1, 0xF8, 0xE9));
        this.SetColor(MaterialDesignColorKey.LightGreen100, Color.FromRgb(0xDC, 0xED, 0xC8));
        this.SetColor(MaterialDesignColorKey.LightGreen200, Color.FromRgb(0xC5, 0xE1, 0xA5));
        this.SetColor(MaterialDesignColorKey.LightGreen300, Color.FromRgb(0xAE, 0xD5, 0x81));
        this.SetColor(MaterialDesignColorKey.LightGreen400, Color.FromRgb(0x9C, 0xCC, 0x65));
        this.SetColor(MaterialDesignColorKey.LightGreen500, Color.FromRgb(0x8B, 0xC3, 0x4A));
        this.SetColor(MaterialDesignColorKey.LightGreen600, Color.FromRgb(0x7C, 0xB3, 0x42));
        this.SetColor(MaterialDesignColorKey.LightGreen700, Color.FromRgb(0x68, 0x9F, 0x38));
        this.SetColor(MaterialDesignColorKey.LightGreen800, Color.FromRgb(0x55, 0x8B, 0x2F));
        this.SetColor(MaterialDesignColorKey.LightGreen900, Color.FromRgb(0x33, 0x69, 0x1E));

        this.SetColor(MaterialDesignColorKey.Lime50,  Color.FromRgb(0xF9, 0xFB, 0xE7));
        this.SetColor(MaterialDesignColorKey.Lime100, Color.FromRgb(0xF0, 0xF4, 0xC3));
        this.SetColor(MaterialDesignColorKey.Lime200, Color.FromRgb(0xE6, 0xEE, 0x9C));
        this.SetColor(MaterialDesignColorKey.Lime300, Color.FromRgb(0xDC, 0xE7, 0x75));
        this.SetColor(MaterialDesignColorKey.Lime400, Color.FromRgb(0xD4, 0xE1, 0x57));
        this.SetColor(MaterialDesignColorKey.Lime500, Color.FromRgb(0xCD, 0xDC, 0x39));
        this.SetColor(MaterialDesignColorKey.Lime600, Color.FromRgb(0xC0, 0xCA, 0x33));
        this.SetColor(MaterialDesignColorKey.Lime700, Color.FromRgb(0xAF, 0xB4, 0x2B));
        this.SetColor(MaterialDesignColorKey.Lime800, Color.FromRgb(0x9E, 0x9D, 0x24));
        this.SetColor(MaterialDesignColorKey.Lime900, Color.FromRgb(0x82, 0x77, 0x17));

        this.SetColor(MaterialDesignColorKey.Yellow50,  Color.FromRgb(0xFF, 0xFD, 0xE7));
        this.SetColor(MaterialDesignColorKey.Yellow100, Color.FromRgb(0xFF, 0xF9, 0xC4));
        this.SetColor(MaterialDesignColorKey.Yellow200, Color.FromRgb(0xFF, 0xF5, 0x9D));
        this.SetColor(MaterialDesignColorKey.Yellow300, Color.FromRgb(0xFF, 0xF1, 0x76));
        this.SetColor(MaterialDesignColorKey.Yellow400, Color.FromRgb(0xFF, 0xEE, 0x58));
        this.SetColor(MaterialDesignColorKey.Yellow500, Color.FromRgb(0xFF, 0xEB, 0x3B));
        this.SetColor(MaterialDesignColorKey.Yellow600, Color.FromRgb(0xFD, 0xD8, 0x35));
        this.SetColor(MaterialDesignColorKey.Yellow700, Color.FromRgb(0xFB, 0xC0, 0x2D));
        this.SetColor(MaterialDesignColorKey.Yellow800, Color.FromRgb(0xF9, 0xA8, 0x25));
        this.SetColor(MaterialDesignColorKey.Yellow900, Color.FromRgb(0xF5, 0x7F, 0x17));

        this.SetColor(MaterialDesignColorKey.Amber50,  Color.FromRgb(0xFF, 0xF8, 0xE1));
        this.SetColor(MaterialDesignColorKey.Amber100, Color.FromRgb(0xFF, 0xEC, 0xB3));
        this.SetColor(MaterialDesignColorKey.Amber200, Color.FromRgb(0xFF, 0xE0, 0x82));
        this.SetColor(MaterialDesignColorKey.Amber300, Color.FromRgb(0xFF, 0xD5, 0x4F));
        this.SetColor(MaterialDesignColorKey.Amber400, Color.FromRgb(0xFF, 0xCA, 0x28));
        this.SetColor(MaterialDesignColorKey.Amber500, Color.FromRgb(0xFF, 0xC1, 0x07));
        this.SetColor(MaterialDesignColorKey.Amber600, Color.FromRgb(0xFF, 0xB3, 0x00));
        this.SetColor(MaterialDesignColorKey.Amber700, Color.FromRgb(0xFF, 0xA0, 0x00));
        this.SetColor(MaterialDesignColorKey.Amber800, Color.FromRgb(0xFF, 0x8F, 0x00));
        this.SetColor(MaterialDesignColorKey.Amber900, Color.FromRgb(0xFF, 0x6F, 0x00));

        this.SetColor(MaterialDesignColorKey.Orange50,  Color.FromRgb(0xFF, 0xF3, 0xE0));
        this.SetColor(MaterialDesignColorKey.Orange100, Color.FromRgb(0xFF, 0xE0, 0xB2));
        this.SetColor(MaterialDesignColorKey.Orange200, Color.FromRgb(0xFF, 0xCC, 0x80));
        this.SetColor(MaterialDesignColorKey.Orange300, Color.FromRgb(0xFF, 0xB7, 0x4D));
        this.SetColor(MaterialDesignColorKey.Orange400, Color.FromRgb(0xFF, 0xA7, 0x26));
        this.SetColor(MaterialDesignColorKey.Orange500, Color.FromRgb(0xFF, 0x98, 0x00));
        this.SetColor(MaterialDesignColorKey.Orange600, Color.FromRgb(0xFB, 0x8C, 0x00));
        this.SetColor(MaterialDesignColorKey.Orange700, Color.FromRgb(0xF5, 0x7C, 0x00));
        this.SetColor(MaterialDesignColorKey.Orange800, Color.FromRgb(0xEF, 0x6C, 0x00));
        this.SetColor(MaterialDesignColorKey.Orange900, Color.FromRgb(0xE6, 0x51, 0x00));

        this.SetColor(MaterialDesignColorKey.DeepOrange50,  Color.FromRgb(0xFB, 0xE9, 0xE7));
        this.SetColor(MaterialDesignColorKey.DeepOrange100, Color.FromRgb(0xFF, 0xCC, 0xBC));
        this.SetColor(MaterialDesignColorKey.DeepOrange200, Color.FromRgb(0xFF, 0xAB, 0x91));
        this.SetColor(MaterialDesignColorKey.DeepOrange300, Color.FromRgb(0xFF, 0x8A, 0x65));
        this.SetColor(MaterialDesignColorKey.DeepOrange400, Color.FromRgb(0xFF, 0x70, 0x43));
        this.SetColor(MaterialDesignColorKey.DeepOrange500, Color.FromRgb(0xFF, 0x57, 0x22));
        this.SetColor(MaterialDesignColorKey.DeepOrange600, Color.FromRgb(0xF4, 0x51, 0x1E));
        this.SetColor(MaterialDesignColorKey.DeepOrange700, Color.FromRgb(0xE6, 0x4A, 0x19));
        this.SetColor(MaterialDesignColorKey.DeepOrange800, Color.FromRgb(0xD8, 0x43, 0x15));
        this.SetColor(MaterialDesignColorKey.DeepOrange900, Color.FromRgb(0xBF, 0x36, 0x0C));

        this.SetColor(MaterialDesignColorKey.Brown50,  Color.FromRgb(0xEF, 0xEB, 0xE9));
        this.SetColor(MaterialDesignColorKey.Brown100, Color.FromRgb(0xD7, 0xCC, 0xC8));
        this.SetColor(MaterialDesignColorKey.Brown200, Color.FromRgb(0xBC, 0xAA, 0xA4));
        this.SetColor(MaterialDesignColorKey.Brown300, Color.FromRgb(0xA1, 0x88, 0x7F));
        this.SetColor(MaterialDesignColorKey.Brown400, Color.FromRgb(0x8D, 0x6E, 0x63));
        this.SetColor(MaterialDesignColorKey.Brown500, Color.FromRgb(0x79, 0x55, 0x48));
        this.SetColor(MaterialDesignColorKey.Brown600, Color.FromRgb(0x6D, 0x4C, 0x41));
        this.SetColor(MaterialDesignColorKey.Brown700, Color.FromRgb(0x5D, 0x40, 0x37));
        this.SetColor(MaterialDesignColorKey.Brown800, Color.FromRgb(0x4E, 0x34, 0x2E));
        this.SetColor(MaterialDesignColorKey.Brown900, Color.FromRgb(0x3E, 0x27, 0x23));

        this.SetColor(MaterialDesignColorKey.Gray50,  Color.FromRgb(0xFA, 0xFA, 0xFA));
        this.SetColor(MaterialDesignColorKey.Gray100, Color.FromRgb(0xF5, 0xF5, 0xF5));
        this.SetColor(MaterialDesignColorKey.Gray200, Color.FromRgb(0xEE, 0xEE, 0xEE));
        this.SetColor(MaterialDesignColorKey.Gray300, Color.FromRgb(0xE0, 0xE0, 0xE0));
        this.SetColor(MaterialDesignColorKey.Gray400, Color.FromRgb(0xBD, 0xBD, 0xBD));
        this.SetColor(MaterialDesignColorKey.Gray500, Color.FromRgb(0x9E, 0x9E, 0x9E));
        this.SetColor(MaterialDesignColorKey.Gray600, Color.FromRgb(0x75, 0x75, 0x75));
        this.SetColor(MaterialDesignColorKey.Gray700, Color.FromRgb(0x61, 0x61, 0x61));
        this.SetColor(MaterialDesignColorKey.Gray800, Color.FromRgb(0x42, 0x42, 0x42));
        this.SetColor(MaterialDesignColorKey.Gray900, Color.FromRgb(0x21, 0x21, 0x21));

        this.SetColor(MaterialDesignColorKey.White, Color.FromRgb(0xFF, 0xFF, 0xFF));
        this.SetColor(MaterialDesignColorKey.Black, Color.FromRgb(0x00, 0x00, 0x00));
    }

    public void SetScheme(DynamicScheme scheme)
    {
        ArgumentNullException.ThrowIfNull(scheme);
        SetBaseColors();
        SetPalettes();
        SetDynamicColors();
        return;

        void SetBaseColors()
        {
            this.SetColor(MaterialDesignColorKey.Primary, DynamicColors.Primary.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.Secondary, DynamicColors.Secondary.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.Tertiary, DynamicColors.Tertiary.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.Neutral, Color.FromArgb(scheme.NeutralPalette.GetKeyColor().Argb));
            this.SetColor(MaterialDesignColorKey.NeutralVariant, Color.FromArgb(scheme.NeutralVariantPalette.GetKeyColor().Argb));
            this.SetColor(MaterialDesignColorKey.Error, DynamicColors.Error.GetColor(scheme));
        }

        void SetPalettes()
        {
            int[] stops = [ 0, 5, 10, 15, 20, 25, 30, 35, 40, 50, 60, 70, 80, 90, 95, 98, 99, 100 ];
            foreach (var stop in stops)
            {
                this.SetColor($"Primary{stop}", Color.FromArgb(scheme.PrimaryPalette.Tone(stop)));
                this.SetColor($"Secondary{stop}", Color.FromArgb(scheme.SecondaryPalette.Tone(stop)));
                this.SetColor($"Tertiary{stop}", Color.FromArgb(scheme.TertiaryPalette.Tone(stop)));
                this.SetColor($"Neutral{stop}", Color.FromArgb(scheme.NeutralPalette.Tone(stop)));
                this.SetColor($"NeutralVariant{stop}", Color.FromArgb(scheme.NeutralVariantPalette.Tone(stop)));
                this.SetColor($"Error{stop}", Color.FromArgb(scheme.ErrorPalette.Tone(stop)));
            }
        }

        void SetDynamicColors()
        {
            this.SetColor(MaterialDesignColorKey.SurfaceTint, DynamicColors.SurfaceTint.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.OnPrimary, DynamicColors.OnPrimary.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.PrimaryContainer, DynamicColors.PrimaryContainer.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.OnPrimaryContainer, DynamicColors.OnPrimaryContainer.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.OnSecondary, DynamicColors.OnSecondary.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.SecondaryContainer, DynamicColors.SecondaryContainer.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.OnSecondaryContainer, DynamicColors.OnSecondaryContainer.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.OnTertiary, DynamicColors.OnTertiary.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.TertiaryContainer, DynamicColors.TertiaryContainer.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.OnTertiaryContainer, DynamicColors.OnTertiaryContainer.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.OnError, DynamicColors.OnError.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.ErrorContainer, DynamicColors.ErrorContainer.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.OnErrorContainer, DynamicColors.OnErrorContainer.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.Background, DynamicColors.Background.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.OnBackground, DynamicColors.OnBackground.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.Surface, DynamicColors.Surface.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.OnSurface, DynamicColors.OnSurface.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.SurfaceVariant, DynamicColors.SurfaceVariant.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.OnSurfaceVariant, DynamicColors.OnSurfaceVariant.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.Outline, DynamicColors.Outline.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.OutlineVariant, DynamicColors.OutlineVariant.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.Shadow, DynamicColors.Shadow.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.Scrim, DynamicColors.Scrim.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.InverseSurface, DynamicColors.InverseSurface.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.InverseOnSurface, DynamicColors.InverseOnSurface.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.InversePrimary, DynamicColors.InversePrimary.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.PrimaryFixed, DynamicColors.PrimaryFixed.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.OnPrimaryFixed, DynamicColors.OnPrimaryFixed.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.PrimaryFixedDim, DynamicColors.PrimaryFixedDim.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.OnPrimaryFixedVariant, DynamicColors.OnPrimaryFixedVariant.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.SecondaryFixed, DynamicColors.SecondaryFixed.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.OnSecondaryFixed, DynamicColors.OnSecondaryFixed.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.SecondaryFixedDim, DynamicColors.SecondaryFixedDim.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.OnSecondaryFixedVariant, DynamicColors.OnSecondaryFixedVariant.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.TertiaryFixed, DynamicColors.TertiaryFixed.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.OnTertiaryFixed, DynamicColors.OnTertiaryFixed.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.TertiaryFixedDim, DynamicColors.TertiaryFixedDim.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.OnTertiaryFixedVariant, DynamicColors.OnTertiaryFixedVariant.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.SurfaceDim, DynamicColors.SurfaceDim.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.SurfaceBright, DynamicColors.SurfaceBright.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.SurfaceContainerLowest, DynamicColors.SurfaceContainerLowest.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.SurfaceContainerLow, DynamicColors.SurfaceContainerLow.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.SurfaceContainer, DynamicColors.SurfaceContainer.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.SurfaceContainerHigh, DynamicColors.SurfaceContainerHigh.GetColor(scheme));
            this.SetColor(MaterialDesignColorKey.SurfaceContainerHighest, DynamicColors.SurfaceContainerHighest.GetColor(scheme));
        }
    }
}
